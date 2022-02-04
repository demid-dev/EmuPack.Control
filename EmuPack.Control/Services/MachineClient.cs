using EmuPack.Control.DTOs;
using EmuPack.Control.Models;
using EmuPack.Control.Models.Commands;
using EmuPack.Control.Models.Machine;
using EmuPack.Control.Models.Responses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmuPack.Control.Services
{
    public class MachineClient
    {
        private readonly ResponseProcessingService _responseProcessingService;
        private readonly NotificationService _notificationService;

        private TcpClient _tcpClient;
        private NetworkStream _stream;

        public MachineState MachineState { get; private set; }
        public bool ConnectedToMachine
        {
            get
            {
                if (_tcpClient == null)
                {
                    return false;
                }
                else
                {
                    return _tcpClient.Connected;
                }
            }
        }

        public MachineClient(ResponseProcessingService responseProcessingService,
            NotificationService notificationService)
        {
            _responseProcessingService = responseProcessingService;
            _notificationService = notificationService;

            MachineState = new MachineState();
        }

        public void Connect(string hostname, int port)
        {
            try
            {
                _tcpClient = new TcpClient();
                _tcpClient.Connect(hostname, port);
                _stream = _tcpClient.GetStream();
                Thread receiveResponsesThread = new Thread(new ThreadStart(ReceiveServerResponse));
                receiveResponsesThread.Start();
            }
            catch (Exception)
            {
                _notificationService.SendTcpErrorNotification(NotificationType.TcpConnectionError, 
                    hostname, port);
            }
            finally
            {
                if (_stream != null)
                    _stream.Close();
                if (_tcpClient != null)
                    _tcpClient.Close();
            }
        }

        public void SendCommand(Command command)
        {
            try
            {
                if(_tcpClient != null)
                {
                    byte[] data = Encoding.ASCII.GetBytes(command.CommandString);
                    _stream.Write(data, 0, data.Length);
                }
                else
                {
                    throw new NullReferenceException(nameof(_tcpClient));
                }
            }
            catch (Exception)
            {
                IPEndPoint ipEndpoint = _tcpClient.Client.RemoteEndPoint as IPEndPoint;
                _notificationService.SendTcpErrorNotification(NotificationType.TcpSendMessageError,
                    ipEndpoint.Address.ToString(), ipEndpoint.Port);
            }
            finally
            {
                if (_stream != null)
                    _stream.Close();
                if (_tcpClient != null)
                    _tcpClient.Close();
            }
        }

        private void ReceiveServerResponse()
        {
            byte[] data = new byte[99999];
            try
            {
                while (true)
                {
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = _stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.ASCII.GetString(data, 0, bytes));
                    }
                    while (_stream.DataAvailable);
                    string message = builder.ToString();

                    _responseProcessingService.ProcessResponse(message, MachineState);
                }
            }
            catch (Exception)
            {
                IPEndPoint ipEndpoint = _tcpClient.Client.RemoteEndPoint as IPEndPoint;
                _notificationService.SendTcpErrorNotification(NotificationType.TcpReceivingError,
                    ipEndpoint.Address.ToString(), ipEndpoint.Port);
            }
            finally
            {
                if (_stream != null)
                    _stream.Close();
                if (_tcpClient != null)
                    _tcpClient.Close();
            }
        }
    }
}

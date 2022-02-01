using EmuPack.Control.Models.Machine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EmuPack.Control.Services
{
    public class MachineClient
    {
        private readonly TcpClient _tcpClient;
        private NetworkStream _stream;

        public MachineState MachineState { get; private set; }
        public bool ConnectedToMachine
        {
            get
            {
                return _tcpClient.Connected;
            }
        }

        public MachineClient()
        {
            MachineState = new MachineState();

            _tcpClient = new TcpClient();
        }

        public void Connect(string hostname, int port)
        {
            try
            {
                _tcpClient.Connect(hostname, port);
                _stream = _tcpClient.GetStream();
            }
            catch(Exception ex)
            {

            }
        }

        public void SendMessage(string message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            _stream.Write(data, 0, data.Length);
        }

        public void ReceiveMessage(StringBuilder messageSpace)
        {
            byte[] data = new byte[99999];
            int bytes = 0;
            do
            {
                bytes = _stream.Read(data, 0, data.Length);
                messageSpace.Append(Encoding.ASCII.GetString(data, 0, bytes));
            }
            while (_stream.DataAvailable);
        }
    }
}

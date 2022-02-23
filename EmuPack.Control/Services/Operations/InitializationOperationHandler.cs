using EmuPack.Control.Models.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmuPack.Control.Services.Operations
{
    public class InitializationOperationHandler
    {
        private readonly MachineClient _machineClient;

        public InitializationOperationHandler(MachineClient machineClient)
        {
            _machineClient = machineClient;
        }

        public void InitializeMachine()
        {
            if (!_machineClient.ConnectedToMachine)
            {
                _machineClient.Connect(InitializationOperationHandlerValues.Hostname,
                    InitializationOperationHandlerValues.Port);
                Thread.Sleep(1000);
            }
            if (_machineClient.ConnectedToMachine)
            {
                InitializationCommand command = new InitializationCommand();
                _machineClient.SendCommand(command);
            }
        }
    }

    static public class InitializationOperationHandlerValues
    {
        static public string Hostname { get; private set; }
        static public int Port { get; private set; }

        static InitializationOperationHandlerValues()
        {
            Hostname = "127.0.0.1";
            Port = 8888;
        }
    }
}

using EmuPack.Control.Models.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

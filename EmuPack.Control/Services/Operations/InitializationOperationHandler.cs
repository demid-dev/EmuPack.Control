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
        public void InitializeMachine(MachineClient client)
        {
            if (!client.ConnectedToMachine)
            {
                client.Connect(InitializationOperationHandlerValues.Hostname,
                    InitializationOperationHandlerValues.Port);
            }
            if (client.ConnectedToMachine)
            {
                InitializationCommand command = new InitializationCommand();
                client.SendCommand(command);
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

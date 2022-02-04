using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmuPack.Control.Services.Operations
{
    public class OperationsHandler
    {
        public InitializationOperationHandler InitializationOperationHandler { get; private set; }
        
        public OperationsHandler()
        {
            InitializationOperationHandler = new InitializationOperationHandler();
        }
    }
}

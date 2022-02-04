using EmuPack.Control.DTOs;
using EmuPack.Control.Models.Commands;
using EmuPack.Control.Models.Responses;
using EmuPack.Control.Services;
using EmuPack.Control.Services.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EmuPack.Control.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommandsController : ControllerBase
    {
        private readonly MachineClient _machineClient;
        private readonly OperationsHandler _operationsHandler;

        public CommandsController(MachineClient machineClient,
            OperationsHandler operationsHandler)
        {
            _machineClient = machineClient;
            _operationsHandler = operationsHandler;
        }

        [HttpPost("fill")]
        public ActionResult Fill(FillCommandDTO dto)
        {
            return Ok(new FillCommand(dto).CommandString);
        }

        [HttpPost("connect")]
        public ActionResult ConnectToMachine()
        {
            _operationsHandler.InitializationOperationHandler.InitializeMachine(_machineClient);

            return Ok();
        }

        [HttpGet("connected-status")]
        public ActionResult GetMachineClientStatus()
        {
            return Ok(_machineClient.ConnectedToMachine);
        }

        [HttpGet("test-status-response")]
        public ActionResult GetStatusCommandResponseTest()
        {
            return Ok(new StatusCommandResponse("SRM1C1000090001023020"));
        }
    }
}

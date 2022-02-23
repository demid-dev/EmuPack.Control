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
        private readonly InitializationOperationHandler _initHandler;
        private readonly DispensingOperationHandler _dispenseHandler;
        private readonly StatusOperationHandler _statusHandler;

        public CommandsController(MachineClient machineClient,
            InitializationOperationHandler initHandler,
            DispensingOperationHandler dispenseHandler,
            StatusOperationHandler statusHandler)
        {
            _machineClient = machineClient;
            _initHandler = initHandler;
            _dispenseHandler = dispenseHandler;
            _statusHandler = statusHandler;
        }

        [HttpPost("fill")]
        public ActionResult Fill(FillCommandDTO dto)
        {
            return Ok(new FillCommand(dto).CommandString);
        }

        [HttpPost("connect")]
        public ActionResult ConnectToMachine()
        {
            _initHandler.InitializeMachine();

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
            _statusHandler.UpdateMachineState();
            return Ok(_machineClient.MachineState);
        }

        [HttpPost("test-mapping")]
        public ActionResult TestMapping(DispensingOperationDTO dto)
        {
            return Ok(_dispenseHandler.MapDispensingDtoToRegistrationDto(dto));
        }

        [HttpPost("test-mapping2")]
        public ActionResult TestMapping2(DispensingOperationDTO dto)
        {
            return Ok(_dispenseHandler.MapDispensingDtoToFillDtos(dto));
        }

        [HttpPost("dispense")]
        public ActionResult TestDispensing(DispensingOperationDTO dto)
        {
            _dispenseHandler.Dispense(dto);
            return Ok();
        }
    }
}

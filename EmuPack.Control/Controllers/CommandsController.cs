using EmuPack.Control.DTOs;
using EmuPack.Control.Models.Commands;
using EmuPack.Control.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmuPack.Control.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommandsController : ControllerBase
    {
        private readonly MachineClient _machineClient;

        public CommandsController(MachineClient machineClient)
        {
            _machineClient = machineClient;
        }

        [HttpPost("fill")]
        public ActionResult Fill(FillCommandDTO dto)
        {
            return Ok(new FillCommand(dto).CommandString);
        }

        [HttpPost("connect")]
        public ActionResult ConnectToMachine()
        {
            _machineClient.Connect("127.0.0.1", 30000);

            return Ok();
        }

        [HttpGet("connected-status")]
        public ActionResult GetMachineClientStatus()
        {
            return Ok(_machineClient.ConnectedToMachine);
        }
    }
}

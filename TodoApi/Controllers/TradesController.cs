using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TodoApi.Models;
using TodoApi.Service;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TradesController : ControllerBase
    {
        private readonly ILogger<TradesController> _logger;
        private readonly ITradesService _tradesService;

        public TradesController(ILogger<TradesController> logger, ITradesService tradesService)
        {
            _logger = logger;
            _tradesService = tradesService;
        }

        [HttpGet]
        public async Task<ActionResult> GetTrades([FromQuery] int? userID, [FromQuery] string type)
        {
            var result = await _tradesService.GetTrades(userID, type);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetTrade(int id)
        {
            var result = await _tradesService.GetTrade(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Trade>> CreateTrade(Trade trade)
        {
            if (trade.Shares < 1 || trade.Shares > 100 || !Enum.GetNames(typeof(TradeType)).Contains(trade.Type))
            {
                return BadRequest(trade);
            }

            var result = await _tradesService.Create(trade);
            return Created(nameof(CreateTrade), result);
        }

        [HttpDelete("{id}")]
        [HttpPut("{id}")]
        [HttpPatch("{id}")]
        public IActionResult NotAllowActionTrade(int id)
        {
            return StatusCode(StatusCodes.Status405MethodNotAllowed);
        }


    }
}

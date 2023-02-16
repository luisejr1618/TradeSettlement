using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Service;

namespace TodoApi.Controllers
{
    #region TodoController
    [Route("api/[controller]")]
    [ApiController]
    public class SettlementController : ControllerBase
    {
        private readonly ISettlementService _settlementService;

        public SettlementController(ISettlementService settlementService)
        {
            _settlementService = settlementService;
        }
        #endregion

        [HttpGet("ret-settlement-dates/{start_date}/{end_date}")]
        public ActionResult<IEnumerable<Settlement>> ret_settlement_dates(DateTime start_date, DateTime end_date, [FromQuery] string settlementName = null)
        {
            if (start_date == default || end_date == default)
            {
                return BadRequest("Dates Not Found");
            }
            return Ok(_settlementService.GetSettlementLocations(start_date,end_date,settlementName));
        }

        [HttpGet("agg-monthly")]
        public ActionResult<IEnumerable<Settlement>> agg_monthly([FromQuery] string settlementName = null)
        {
            return Ok(_settlementService.CalculateSettlementAggregation(settlementName));
        }
    }
}

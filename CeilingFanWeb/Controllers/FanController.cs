using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Services.FanService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CeilingFanWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FanController : ControllerBase
    {
        /// <summary>
        /// Get all fans
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Incorrect request</response>
        /// <response code="401">Not Authorized</response>
        /// <response code="404">Resource was not found</response>
        /// <response code="500">Internal error server</response>
        [HttpGet]
        public IActionResult GetAll([FromServices] FanService service)
        {
            List<Fan> fans = service.GetAll().ToList();  
            
            if (!fans.Any())
                return NoContent();

            return Ok(fans);
        }


        /// <summary>
        /// Get a fan by id
        /// </summary>
        /// <param name="id">Fan id</param>
        /// <response code="200">Success</response>
        /// <response code="400">Incorrect request</response>
        /// <response code="401">Not Authorized</response>
        /// <response code="404">Resource was not found</response>
        /// <response code="500">Internal error server</response>
        [HttpGet("{id}")]
        public IActionResult GetById([FromServices] FanService service, int id)
        {
            Fan fan = service.GetBy(f => f.FanId == id);

            if (fan == null)
                return NotFound();

            return Ok(fan);
        }


        /// <summary>
        /// Create a new fan
        /// </summary>
        /// <param name="fan"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Incorrect request</response>
        /// <response code="401">Not Authorized</response>
        /// <response code="404">Resource was not found</response>
        /// <response code="500">Internal error server</response>
        [HttpPost]
        public IActionResult Create([FromServices] FanService service, [FromBody] Fan fan)
        {
            fan.UpdatedAt = null;
            Fan newFan = service.Insert<FanValidator>(fan);

            if (newFan != null && newFan.FanId > 0)
            {
                return CreatedAtAction ("GetById", new { id = newFan.FanId},  newFan);
            }
            else
            {
                return BadRequest();
            }
        }


        /// <summary>
        /// Update a fan
        /// </summary>
        /// <param name="fan"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Incorrect request</response>
        /// <response code="401">Not Authorized</response>
        /// <response code="404">Resource was not found</response>
        /// <response code="500">Internal error server</response>
        [HttpPut("{id}")]
        public IActionResult Update([FromServices] FanService service, int id, [FromBody] Fan fan)
        {
            if (id != fan.FanId)
            {
                return BadRequest();
            }

            Fan udpatedFan = service.Update<FanValidator>(fan);

            if (udpatedFan != null && udpatedFan.FanId > 0)
            {
                return Ok(udpatedFan);
            }
            else
            {
                return BadRequest();
            }

        }


        /// <summary>
        /// Delete a fan
        /// </summary>
        /// <param name="fanDTO"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Incorrect request</response>
        /// <response code="401">Not Authorized</response>
        /// <response code="404">Resource was not found</response>
        /// <response code="500">Internal error server</response>
        [HttpDelete]
        public IActionResult Delete([FromServices] FanService service, int id)
        {

            Fan fan = service.GetBy(f => f.FanId == id);

            if (fan == null)
                return NotFound();

            service.Delete(fan);

            return Ok();

        }
    }
}

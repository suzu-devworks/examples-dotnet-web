using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Examples.WebApi.Applications.Serialization.Models;

namespace Examples.WebApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SerializationController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            await Task.Delay(100);

            return Ok(new SerializableData
            {
                SearchId = id,
                Id = Guid.NewGuid(),
                Date = DateTimeOffset.Now,
                Elaps = DateTimeOffset.Now.TimeOfDay,
                Grobal = GrobalStatus.InActive,
                Local = LocalStatus.Activate,
                OffsetRange = 4..^12,
            });

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SerializableData data)
        {
            await Task.Delay(100);

            return Ok(data);
        }

    }
}
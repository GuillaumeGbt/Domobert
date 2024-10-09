using BLL.Services;
using Common;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_Domobert.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MqttGlobalController : ControllerBase
    {
        private HistoryTempService _historyTempService { get; set; }
        public MqttGlobalController(HistoryTempService historyTempService)
        {
            _historyTempService = historyTempService;
        }


        // GET: api/<MqttGlobalController>
        [HttpGet]
        public IActionResult Get()
        {
            return BadRequest(new NotImplementedException("Nothing to see here"));
        }

        // GET api/<MqttGlobalController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                return Ok(_historyTempService.GetAll(id));
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        // POST api/<MqttGlobalController>
        [HttpPost]
        public IActionResult Post([FromBody] string value)
        {
            return BadRequest(new NotImplementedException("Nothing to see here"));
        }

        // PUT api/<MqttGlobalController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] string value)
        {
            return BadRequest(new NotImplementedException("Nothing to see here"));
        }

        // DELETE api/<MqttGlobalController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return BadRequest(new NotImplementedException("Nothing to see here"));
        }
    }
}

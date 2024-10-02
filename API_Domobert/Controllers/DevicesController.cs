using Common;
using Bll = BLL.Models;
using Microsoft.AspNetCore.Mvc;
using API_Domobert.Models;
using API_Domobert.Mapper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_Domobert.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private IDeviceRepository<Bll.Device> _repository { get; set; }
        public DevicesController(IDeviceRepository<Bll.Device> repository)
        {
            _repository = repository;
        }

        // GET: api/<DevicesController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_repository.GetAll());
        }

        // GET api/<DevicesController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_repository.GetById(id));
        }

        // POST api/<DevicesController>
        [HttpPost]
        public IActionResult Post([FromBody] AddDevice value)
        {
            return Ok(_repository.Add(value.toBll()));
        }

        // PUT api/<DevicesController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] AddDevice value)
        {
            return Ok(_repository.Update(id,value.toBll())) ;

        }

        // DELETE api/<DevicesController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok(_repository.Delete(id));
        }
    }
}

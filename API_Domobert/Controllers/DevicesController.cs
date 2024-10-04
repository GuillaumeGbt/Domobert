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
            try
            {
                return Ok(_repository.GetAll());
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        // GET api/<DevicesController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                return Ok(_repository.GetById(id));
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        // POST api/<DevicesController>
        [HttpPost]
        public IActionResult Post([FromBody] AddDevice value)
        {
            try
            {
                return Ok(_repository.Add(value.toBll()));
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        // PUT api/<DevicesController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] AddDevice value)
        {
            try
            {
                return Ok(_repository.Update(id, value.toBll()));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        // DELETE api/<DevicesController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                return Ok(_repository.Delete(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}

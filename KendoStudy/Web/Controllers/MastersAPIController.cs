using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Domain.VMModels;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MastersAPIController : ControllerBase
    {
        private readonly IMasterRepository _masterRepository;

        public MastersAPIController(IMasterRepository masterRepository)
        {
            this._masterRepository = masterRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var masters  = _masterRepository.Get();
            return Ok(masters);
        }

        [HttpPost]
        public IActionResult Create([FromBody] VMMaster master)
        {
          string status =  _masterRepository.Post(master);

            if (!status.Equals("success"))
            {
                return BadRequest(status);
            } 

            return Ok(status);
        }

        [HttpPut]
        public IActionResult Update([FromBody] VMMaster master)
        {
           var status =  _masterRepository.Put(master);

            if (!status.Equals("success"))
            {
                return BadRequest(status);
            } 

            return Ok(status);
            
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var status = (_masterRepository.Delete(id));

            
            return Ok(status);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var master = _masterRepository.GetById(id);

            if (master == null)
                return BadRequest("Not Found");

            return Ok(master);
        }

    }
}

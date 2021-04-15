using Microsoft.AspNetCore.Mvc;
using UwpBackend.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UwpBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CamerasController : ControllerBase
    {
        private readonly ICameraRepository cameraRepository;

        public CamerasController(ICameraRepository repository)
        {
            cameraRepository = repository;
        }

        // GET: api/<CamerasController>
        [HttpGet]
        public JsonResult Get()
        {
            var cameras = cameraRepository.GetAll();

            var result = new JsonResult(cameras);

            return result;
        }

        // GET api/<CamerasController>/5
        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            var camera = cameraRepository.GetById(id);

            var result = new JsonResult(camera);

            return result;
        }
    }
}

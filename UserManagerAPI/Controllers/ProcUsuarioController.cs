using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagerAPI.Repositories.Implementation;
using UserManagerAPI.Repositories;
using UserManagerAPI.Models;

namespace UserManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcUsuarioController : ControllerBase
    {
        private IUsuarioRepository _repository;
        public ProcUsuarioController()
        {
            _repository = new ProcUsuarioRepository();
        }

        [HttpGet]
        public IActionResult FindAll()
        {
            var usuarios = _repository.GetUsers();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public IActionResult FindById(int id)
        {
            var usuario = _repository.GetById(id);
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Usuario user)
        {
            _repository.InsertUser(user);
            return Ok(user);
        }

        [HttpPut]
        public IActionResult Update([FromBody] Usuario user)
        {
            _repository.UpdateUser(user);
            return Ok(user);
        }

        [HttpDelete]
        public IActionResult DeleteById(int id)
        {
            var usuario = _repository.GetById(id);
            if (usuario == null) return NotFound();
            _repository.DeleteUser(id);
            return NoContent();
        }


    }
}


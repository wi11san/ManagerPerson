using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagerAPI.Models;
using UserManagerAPI.Repositories;
using UserManagerAPI.Repositories.Implementation;

namespace UserManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private IUsuarioRepository _repository;
        public UsuarioController()
        {
            _repository = new UsuarioRepository();
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_repository.GetUsers());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = _repository.GetById(id);
            if (user == null) return NotFound("Usuário não encontrado!");
            return Ok(user);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Usuario user)
        {
            _repository.InsertUser(user);
            return Ok(user);
        }

        [HttpPut]
        public IActionResult Put([FromBody]Usuario user)
        {
            _repository.UpdateUser(user);
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _repository.DeleteUser(id);
            return NoContent();
        }

    }
}

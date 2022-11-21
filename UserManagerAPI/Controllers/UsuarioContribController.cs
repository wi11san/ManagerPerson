using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagerAPI.Models;
using UserManagerAPI.Repositories.Implementation;
using UserManagerAPI.Repositories;

namespace UserManagerAPI.Controllers
{
    [Route("api/Contrib/Usuarios")]
    [ApiController]
    public class UsuarioContribController : ControllerBase
    {

        private IUsuarioRepository _repository;
        public UsuarioContribController()
        {
            _repository = new ContribUsuarioRepository();
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
        public IActionResult Put([FromBody] Usuario user)
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

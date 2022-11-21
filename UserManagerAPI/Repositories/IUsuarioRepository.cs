using System.Collections.Generic;
using UserManagerAPI.Models;

namespace UserManagerAPI.Repositories
{
    public interface IUsuarioRepository
    {
        public List<Usuario> GetUsers();
        public Usuario GetById(int id);
        public void InsertUser(Usuario user);
        public void UpdateUser(Usuario user);
        public void DeleteUser(int id);
    }
}

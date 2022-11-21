using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using UserManagerAPI.Models;
using Dapper.Contrib.Extensions;
using System.Linq;
using System;

namespace UserManagerAPI.Repositories.Implementation
{
    public class ContribUsuarioRepository : IUsuarioRepository
    {
        private IDbConnection _connection;

        public ContribUsuarioRepository()
        {
            _connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=UserManagerDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        public Usuario GetById(int id)
        {
            return _connection.Get<Usuario>(id);
        }

        public List<Usuario> GetUsers()
        {
            return _connection.GetAll<Usuario>().ToList();
        }

        public void InsertUser(Usuario user)
        {
            user.Id = Convert.ToInt32(_connection.Insert(user));
        }

        public void UpdateUser(Usuario user)
        {
            _connection.Update(user);
        }

        public void DeleteUser(int id)
        {
            _connection.Delete(GetById(id));
        }
    }
}

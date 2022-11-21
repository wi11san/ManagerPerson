using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using UserManagerAPI.Models;
    using Dapper;
    using System.Linq;

namespace UserManagerAPI.Repositories.Implementation
{
    public class ProcUsuarioRepository : IUsuarioRepository
    {
        private IDbConnection _connection;

        public ProcUsuarioRepository()
        {
            _connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=UserManagerDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        public Usuario GetById(int id)
        {
            var usuario = _connection.Query<Usuario>("BuscarUsuarioPorId", new { Id = id }, commandType: CommandType.StoredProcedure);
            return usuario.SingleOrDefault();
        }

        public List<Usuario> GetUsers()
        {
            var usuario = _connection.Query<Usuario>("BuscarUsuarios", commandType: CommandType.StoredProcedure);
            return usuario.ToList();
        }

        public void InsertUser(Usuario user)
        {
            var procedure = "[CadastrarUsuario]";
            var values = new {nome = user.Nome, email = user.Email, sexo = user.Sexo, rg = user.RG, cpf = user.CPF, nomeMae = user.NomeMae, situacaoCadastro = user.SituacaoCadastro, dataCadastro = user.DataCadastro};
            _connection.Query<Usuario>(procedure, values, commandType: CommandType.StoredProcedure);
        }

        public void UpdateUser(Usuario user)
        {
            var procedure = "[AtualizarUsuario]";
            var values = new { nome = user.Nome, email = user.Email, sexo = user.Sexo, rg = user.RG, cpf = user.CPF, nomeMae = user.NomeMae, situacaoCadastro = user.SituacaoCadastro, dataCadastro = user.DataCadastro };
            _connection.Query<Usuario>(procedure, values, commandType: CommandType.StoredProcedure);
        }

        public void DeleteUser(int id)
        {
            _connection.Query<Usuario>("DeletarUsuario", new { Id = id }, commandType: CommandType.StoredProcedure);
        }
    }
}

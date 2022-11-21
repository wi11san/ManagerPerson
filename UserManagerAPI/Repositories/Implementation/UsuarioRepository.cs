using System.Data;
using System.Data.SqlClient;
using UserManagerAPI.Models;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System;

namespace UserManagerAPI.Repositories.Implementation
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private IDbConnection _connection;

        public UsuarioRepository()
        {
            _connection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=UserManagerDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        public Usuario GetById(int id)
        {

            List<Usuario> usuarios = new List<Usuario>();

            string sql = "SELECT U.*, C.*, EE.*, D.* FROM Usuarios as U " +
                "LEFT JOIN Contatos C ON C.UsuarioId = U.Id " +
                "LEFT JOIN EnderecosEntrega EE ON EE.UsuarioId = U.Id " +
                "LEFT JOIN UsuariosDepartamentos UD ON UD.UsuarioId = U.Id " +
                "LEFT JOIN Departamentos D ON UD.DepartamentoId = D.Id " +
                "WHERE U.Id = @Id;";

            _connection.Query<Usuario, Contato, EnderecoEntrega, Departamento, Usuario>(sql,
                (usuario, contato, enderecoEntrega, departamento) =>
                {
                    //Verificação do Usuário
                    if (usuarios.SingleOrDefault(a => a.Id == usuario.Id) == null)
                    {
                        usuario.Departamentos = new List<Departamento>();
                        usuario.EnderecosEntrega = new List<EnderecoEntrega>();
                        usuario.Contato = contato;
                        usuarios.Add(usuario);
                    }
                    else
                    {
                        usuario = usuarios.SingleOrDefault(a => a.Id == usuario.Id);
                    }

                    //Verificação do Endereço para evitar duplicidade
                    if (enderecoEntrega != null)
                    {
                        if (usuario.EnderecosEntrega.SingleOrDefault(a => a.Id == enderecoEntrega.Id) == null)
                        {
                            usuario.EnderecosEntrega.Add(enderecoEntrega);
                        }
                    }

                    //Verificação do Departemento para evitar duplicidade
                    if (departamento != null)
                    {
                        if (usuario.Departamentos.SingleOrDefault(a => a.Id == departamento.Id) == null)
                        {
                            usuario.Departamentos.Add(departamento);
                        }
                    }

                    return usuario;
                }, new {Id = id});
            return usuarios.SingleOrDefault();
        }

        public List<Usuario> GetUsers()
        {
            //return _connection.Query<User>("SELECT * FROM Usuarios").ToList();

            List<Usuario> usuarios = new List<Usuario>();

            string sql = "SELECT U.*, C.*, EE.*, D.* FROM Usuarios as U " +
                "LEFT JOIN Contatos C ON C.UsuarioId = U.Id " +
                "LEFT JOIN EnderecosEntrega EE ON EE.UsuarioId = U.Id " +
                "LEFT JOIN UsuariosDepartamentos UD ON UD.UsuarioId = U.Id " +
                "LEFT JOIN Departamentos D ON UD.DepartamentoId = D.Id;";

            _connection.Query<Usuario, Contato, EnderecoEntrega, Departamento, Usuario>(sql,
                (usuario, contato, enderecoEntrega, departamento) =>
                {
                    //Verificação do Usuário
                    if (usuarios.SingleOrDefault(a => a.Id == usuario.Id) == null)
                    {
                        usuario.Departamentos = new List<Departamento>();
                        usuario.EnderecosEntrega = new List<EnderecoEntrega>();
                        usuario.Contato = contato;
                        usuarios.Add(usuario);
                    }
                    else
                    {
                        usuario = usuarios.SingleOrDefault(a => a.Id == usuario.Id);
                    }

                    //Verificação do Endereço para evitar duplicidade
                    if (enderecoEntrega != null)
                    {
                        if (usuario.EnderecosEntrega.SingleOrDefault(a => a.Id == enderecoEntrega.Id) == null)
                        {
                            usuario.EnderecosEntrega.Add(enderecoEntrega);
                        }
                    }

                    //Verificação do Departemento para evitar duplicidade
                    if (departamento != null)
                    {
                        if (usuario.Departamentos.SingleOrDefault(a => a.Id == departamento.Id) == null)
                        {
                            usuario.Departamentos.Add(departamento);
                        }
                    }

                    return usuario;
                });
            return usuarios;
        }

        public void InsertUser(Usuario usuario)
        {
            _connection.Open();
            var transaction = _connection.BeginTransaction();
            try
            {
                string sql = "INSERT INTO Usuarios(Nome, Email, Sexo, RG, CPF, NomeMae, SituacaoCadastro, DataCadastro) VALUES (@Nome, @Email, @Sexo, @RG, @CPF, @NomeMae, @SituacaoCadastro, @DataCadastro); SELECT CAST (SCOPE_IDENTITY() AS INT);";
                usuario.Id = _connection.Query<int>(sql, usuario, transaction).Single();
                if (usuario.Contato != null)
                {
                    usuario.Contato.UsuarioId = usuario.Id;
                    string sqlContato = "INSERT INTO Contatos(UsuarioId, Telefone, Celular) VALUES (@UsuarioId, @Telefone, @Celular); SELECT CAST (SCOPE_IDENTITY() AS INT);";
                    usuario.Contato.Id = _connection.Query<int>(sqlContato, usuario.Contato, transaction).Single();
                }

                if (usuario.EnderecosEntrega != null && usuario.EnderecosEntrega.Count > 0)
                {
                    foreach (var enderecoEntrega in usuario.EnderecosEntrega)
                    {
                        enderecoEntrega.UsuarioId = usuario.Id;
                        string sqlEE = "INSERT INTO EnderecosEntrega (UsuarioId, NomeEndereco, CEP, Estado, Cidade, Bairro, Endereco, Numero, Complemento) VALUES (@UsuarioId, @NomeEndereco, @CEP, @Estado, @Cidade, @Bairro, @Endereco, @Numero, @Complemento); SELECT CAST (SCOPE_IDENTITY() AS INT);";
                        enderecoEntrega.Id = _connection.Query<int>(sqlEE, enderecoEntrega, transaction).Single();
                    }
                }

                if (usuario.Departamentos != null && usuario.Departamentos.Count > 0)
                {
                    foreach (var departamento in usuario.Departamentos)
                    {
                        string SqlUsuariosDepartamentos = "INSERT INTO UsuariosDepartamentos (UsuarioId, DepartamentoId) VALUES (@UsuarioId, @DepartamentoId);";
                        _connection.Execute(SqlUsuariosDepartamentos, new {UsuarioId = usuario.Id, DepartamentoId = departamento.Id}, transaction);
                    }
                }
                transaction.Commit();
            }
            catch (Exception e)
            {
                Console.WriteLine("Falha na operação!");
                try
                {
                    transaction.Rollback();

                }
                catch (Exception)
                {
                    throw;
                }
            }
            finally
            {
                _connection.Close();
            }
        }

        public void UpdateUser(Usuario usuario)
        {
            _connection.Open();
            var transaction = _connection.BeginTransaction();
            try
            {
                string sql = "UPDATE Usuarios SET Nome = @Nome, Email = @Email, Sexo = @Sexo, RG = @RG, CPF = @CPF, NomeMae = @NomeMae, SituacaoCadastro = @SituacaoCadastro, DataCadastro = @DataCadastro WHERE Id = @Id;";
                _connection.Execute(sql, usuario, transaction);

                if (usuario.Contato != null)
                {
                    usuario.Contato.UsuarioId = usuario.Id;
                    string sqlContato = "UPDATE Contatos SET UsuarioId = @UsuarioId, Telefone = @Telefone, Celular = @Celular WHERE Id = @Id; ";
                    _connection.Execute(sqlContato, usuario.Contato, transaction);
                }

                string sqlDeletarEnderecoEntrega = "DELETE FROM EnderecosEntrega WHERE UsuarioId = @Id";
                _connection.Execute(sqlDeletarEnderecoEntrega, usuario, transaction);

                if (usuario.EnderecosEntrega != null && usuario.EnderecosEntrega.Count > 0)
                {
                    foreach (var enderecoEntrega in usuario.EnderecosEntrega)
                    {
                        enderecoEntrega.UsuarioId = usuario.Id;
                        string sqlEE = "INSERT INTO EnderecosEntrega (UsuarioId, NomeEndereco, CEP, Estado, Cidade, Bairro, Endereco, Numero, Complemento) VALUES (@UsuarioId, @NomeEndereco, @CEP, @Estado, @Cidade, @Bairro, @Endereco, @Numero, @Complemento); SELECT CAST (SCOPE_IDENTITY() AS INT);";
                        enderecoEntrega.Id = _connection.Query<int>(sqlEE, enderecoEntrega, transaction).Single();
                    }
                }

                string sqlDeletarDepartamento = "DELETE FROM UsuariosDepartamentos WHERE UsuarioId = @Id";
                _connection.Execute(sqlDeletarEnderecoEntrega, usuario, transaction);

                if (usuario.Departamentos != null && usuario.Departamentos.Count > 0)
                {
                    foreach (var departamento in usuario.Departamentos)
                    {
                        string SqlUsuariosDepartamentos = "INSERT INTO UsuariosDepartamentos (UsuarioId, DepartamentoId) VALUES (@UsuarioId, @DepartamentoId);";
                        _connection.Execute(SqlUsuariosDepartamentos, new { UsuarioId = usuario.Id, DepartamentoId = departamento.Id }, transaction);
                    }
                }

                transaction.Commit();
            }
            catch (Exception)
            {
                try
                {
                    transaction.Rollback();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            finally
            {
                _connection.Close();
            }
        }

        public void DeleteUser(int id)
        {
            _connection.Execute("DELETE FROM Usuarios WHERE Id = @Id", new { Id = id });

        }
    }
}
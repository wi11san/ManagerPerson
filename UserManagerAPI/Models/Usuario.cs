using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;

namespace UserManagerAPI.Models
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        
        public string Nome { get; set; }
        
        public string Email { get; set; }
        
        public string Sexo { get; set; }

        public string RG { get; set; }

        public string CPF { get; set; }

        public string NomeMae { get; set; }

        public string SituacaoCadastro { get; set; }

        public DateTimeOffset DataCadastro { get; set; }

        //Relacionamento 1 para 1 (um usuario possui um contato)
        [Write(false)]
        public Contato? Contato { get; set; }

        //Relacionamento 1 para muitos (um usuarios possui varios enderecos)
        [Write(false)]
        public ICollection<EnderecoEntrega>? EnderecosEntrega { get; set; }

        //Relacionamento muitos pra muutos (vários usuários podem estar em varios departamentos)
        [Write(false)]
        public ICollection<Departamento>? Departamentos { get; set; }
    }
}

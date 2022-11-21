using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagerAPI.Models
{
    [Table("EnderecosEntrega")]
    public class EnderecoEntrega
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        
        public string NomeEndereco { get; set; }
        
        public string CEP { get; set; }
        
        public string Estado { get; set; }

        public string Cidade { get; set; }

        public string Bairro{ get; set; }
        
        public string Endereco { get; set; }
        
        public string Numero { get; set; }
        
        public string Complemento { get; set; }

        //relacionamento muitos para 1 (muitos endereços são de 1 usuario)
        public Usuario? User { get; set; }
    }
}

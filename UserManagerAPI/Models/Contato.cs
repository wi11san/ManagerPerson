using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagerAPI.Models
{
    [Table("Contatos")]
    public class Contato
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        
        public string Telefone { get; set; }
        
        public string Celular { get; set; }

        //Relacionamento 1 pra 1
        public Usuario? Usuario { get; set; }
    }
}

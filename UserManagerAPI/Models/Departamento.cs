using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagerAPI.Models
{
    public class Departamento
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        //Relacionamento muitos pra muutos (vários departamentos podem conter varios usuarios)
        public ICollection<Usuario>? User { get; set; }
    }
}

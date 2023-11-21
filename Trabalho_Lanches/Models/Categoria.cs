using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KF_Lanches.Models
{
    public class Categoria
    {
        public int id { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(50, ErrorMessage = "A descrição deve ter no máximo 50 caracteres.")]
        [DisplayName("Descrição")]
        public string descricao { get; set; }
    }
}

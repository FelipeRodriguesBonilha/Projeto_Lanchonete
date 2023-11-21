using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace KF_Lanches.Models
{
    public class Consumivel
    {
        [Key]
        public int id { get; set; }

        public int categoriaID { get; set; }

        [ForeignKey("categoriaID")]
        [DisplayName("Categoria")]
        public Categoria categoria { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(50, ErrorMessage = "A descrição deve ter no máximo 50 caracteres.")]
        [DisplayName("Descrição")]
        public string descricao { get; set; }

        [StringLength(50, ErrorMessage = "A descrição deve ter no máximo 50 caracteres.")]
        [DisplayName("Composição")]
        public string composicao { get; set; }

        [Required(ErrorMessage = "O valor é obrigatório.")]
        [Range(0, float.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
        [DisplayName("Valor")]
        public float valor { get; set; }
    }
}

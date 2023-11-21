using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KF_Lanches.Models
{
    public class Pedido
    {
        public Pedido()
        { 
            this.DataPedido = DateTime.Now;
        }

        [Key]
        public int id { get; set; }
        public DateTime DataPedido { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [DisplayName("Nome")]
        public string nome { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve ter 11 dígitos.")]
        [DisplayName("CPF")]
        public string cpf { get; set; }

        [Required(ErrorMessage = "O endereço é obrigatório.")]
        [DisplayName("Endereço")]
        public string endereco { get; set; }

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [RegularExpression(@"^\(18\)[2-9][0-9]{4,5}\-[0-9]{4}$", ErrorMessage = "O telefone não está em um formato válido.")]

        [DisplayName("Telefone")]
        public string telefone { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KF_Lanches.Models
{
    public class ItemPedido
    {
        [Key]
        public int id { get; set; }
        public string cliID { get; set; }
        public int pedID { get; set; }
        public int consumivelID { get; set; }
        [ForeignKey("consumivelID")]
        public Consumivel consumivel { get; set; }
        public int quantidade { get; set; }
        public bool virouPedido { get; set; }
        public float QtdValor()
        {
            return this.quantidade * this.consumivel.valor;
        }
    }
}

using Microsoft.EntityFrameworkCore;

namespace KF_Lanches.Models
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options) {}

        public DbSet<Categoria> categorias { get; set; }
        public DbSet<Consumivel> consumiveis { get; set; }
        public DbSet<ItemPedido> itensPedidos { get; set; }
        public DbSet<Pedido> pedidos { get; set; }
    }
}

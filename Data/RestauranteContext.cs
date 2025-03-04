using Microsoft.EntityFrameworkCore;
using RestauranteMvc.Models;

namespace RestauranteMvc.Data
{
    public class RestauranteContext : DbContext
    {
        public RestauranteContext(DbContextOptions<RestauranteContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Prato> Pratos { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ItemPedido> ItensPedido { get; set; }
        public DbSet<Reserva> Reservas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // Configurar precisão decimal para evitar os avisos
    modelBuilder.Entity<ItemPedido>()
        .Property(i => i.PrecoUnitario)
        .HasColumnType("decimal(18,2)");

    modelBuilder.Entity<Pedido>()
        .Property(p => p.ValorTotal)
        .HasColumnType("decimal(18,2)");

    modelBuilder.Entity<Prato>()
        .Property(p => p.Preco)
        .HasColumnType("decimal(18,2)");

    // Modificar a relação entre Pedidos e Usuarios para não usar CASCADE
    modelBuilder.Entity<Pedido>()
        .HasOne(p => p.Usuario)
        .WithMany(u => u.Pedidos)
        .HasForeignKey(p => p.UsuarioId)
        .OnDelete(DeleteBehavior.NoAction);

    // Ou modificar a relação entre Pedidos e Enderecos para não usar CASCADE
    modelBuilder.Entity<Pedido>()
        .HasOne(p => p.Endereco)
        .WithMany()
        .HasForeignKey(p => p.EnderecoId)
        .OnDelete(DeleteBehavior.NoAction);
}

    }
}

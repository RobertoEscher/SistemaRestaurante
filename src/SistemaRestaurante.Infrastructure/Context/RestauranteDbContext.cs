using Microsoft.EntityFrameworkCore;
using SistemaRestaurante.Domain.Entities;

namespace SistemaRestaurante.Infrastructure.Context
{
    public class RestauranteDbContext : DbContext
    {
        public DbSet<Produto> Produtos { get; set; } = null!;
        public DbSet<Prato> Pratos { get; set; } = null!;
        public DbSet<ItemReceita> ItensReceita { get; set; } = null!;
        public DbSet<VendaPrato> Vendas { get; set; } = null!;

        public RestauranteDbContext(DbContextOptions<RestauranteDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Mapeamento da entidade Produto
            modelBuilder.Entity<Produto>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
                entity.Property(e => e.UnidadeMedida).IsRequired().HasMaxLength(10);
            });

            // Mapeamento da entidade Prato
            modelBuilder.Entity<Prato>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nome).IsRequired().HasMaxLength(100);
                
                // Configura o campo privado '_receita' para ser usado pelo EF Core
                entity.Metadata.FindNavigation(nameof(Prato.Receita))
                    ?.SetPropertyAccessMode(PropertyAccessMode.Field);
            });

            // Mapeamento da entidade ItemReceita
            modelBuilder.Entity<ItemReceita>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.HasOne(d => d.Prato)
                    .WithMany(p => p.Receita)
                    .HasForeignKey(d => d.PratoId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Produto)
                    .WithMany()
                    .HasForeignKey(d => d.ProdutoId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Mapeamento da entidade VendaPrato
            modelBuilder.Entity<VendaPrato>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.HasOne(d => d.Prato)
                    .WithMany()
                    .HasForeignKey(d => d.PratoId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
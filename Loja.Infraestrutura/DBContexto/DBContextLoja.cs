
using Loja.Dominio.Entidades;
using Loja.Infraestrutura.Mapeamentos;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Loja.Infraestrutura.DBContexto
{
    public class DBContextLoja : IdentityDbContext<ApplicationUser> //DbContext 
    {
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DBContextLoja(DbContextOptions<DBContextLoja> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoriaMap());
            modelBuilder.ApplyConfiguration(new ProdutoMap());
            modelBuilder.ApplyConfiguration(new ApplicationUserMap());

            modelBuilder.Entity<ApplicationUser>().ToTable("ApsNetUsers").HasKey(t => t.Id); 
            base.OnModelCreating(modelBuilder);
        }
    }
}

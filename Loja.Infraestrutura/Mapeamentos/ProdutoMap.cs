using Loja.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Loja.Infraestrutura.Mapeamentos
{
    public class ProdutoMap : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("Produtos");

            builder.HasIndex(t => t.Titulo).IsUnique().HasDatabaseName("Indice_Produtos");

            builder.HasKey(k => k.Codigo);

            builder.Property(c => c.Codigo).HasColumnName("prod_codigo");
            builder.Property(d => d.Titulo).HasColumnName("prod_titulo").HasMaxLength(100).IsRequired();
            builder.Property(p => p.Preco).HasColumnName("prod_preco").HasColumnType("decimal(5,2)");

            //mapear o relacionamento entre as tabelas: 
            //builder.HasOne(p => p.Categoria).WithMany(c => c.ListaProdutos)
              //  .HasForeignKey(p => p.CodigoCategoria);
           
        }
    }
}

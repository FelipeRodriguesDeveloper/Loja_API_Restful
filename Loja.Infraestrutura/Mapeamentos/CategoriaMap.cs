
using Loja.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Loja.Infraestrutura.Mapeamentos
{
    public class CategoriaMap : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.ToTable("Categorias");

            builder.HasIndex(t => t.Titulo).IsUnique().HasDatabaseName("Indice_Categorias");

            builder.HasKey(k => k.Codigo);

            builder.Property(c => c.Codigo).HasColumnName("cat_codigo");
            builder.Property(d => d.Titulo).HasColumnName("cat_titulo").HasMaxLength(100).IsRequired();
        }
    }
}

using Loja.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Loja.Infraestrutura.Mapeamentos
{
    public class ApplicationUserMap : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("ApsNetUsers");
            builder.HasKey(k => k.Id);
        }
    }
}

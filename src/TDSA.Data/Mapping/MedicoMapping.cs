
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDSA.Business.Models;

namespace TDSA.Data.Mapping
{
    public class MedicoMapping : IEntityTypeConfiguration<Medico>
    {
        public void Configure(EntityTypeBuilder<Medico> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.DataCadastro)
                .IsRequired();

            builder.Property(x => x.Nome)
                .IsRequired()
                .HasColumnType("varchar(255)");

            builder.Property(x => x.CRM)
                .IsRequired();

            builder.Property(x => x.CPF)
                .IsRequired();

            builder.HasMany(x => x.Especialidades)
                .WithOne(e => e.Medico)
                .HasForeignKey(e => e.MedicoId);

            builder.ToTable("Medicos");
        }
    }
}

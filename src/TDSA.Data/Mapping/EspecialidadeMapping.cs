using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TDSA.Business.Models;

namespace TDSA.Data.Mapping
{
    public class EspecialidadeMapping : IEntityTypeConfiguration<Especialidade>
    {
        public void Configure(EntityTypeBuilder<Especialidade> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.DataCadastro)
                .IsRequired();

            builder.Property(x => x.MedicoId)
                .IsRequired();

            builder.Property(x => x.Nome)
                .IsRequired()
                .HasColumnType("varchar(255)");

            builder.ToTable("Especialidades");
        }
    }
}

using Entidades.DulceriaEntidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Datos.Mapeo
{
    public class AjusteMap : IEntityTypeConfiguration<Ajuste>
    {
        public void Configure(EntityTypeBuilder<Ajuste> builder)
        {
            builder.ToTable("TblAjuste")
                .HasKey(c => c.IdAjuste);
        }
    }
}

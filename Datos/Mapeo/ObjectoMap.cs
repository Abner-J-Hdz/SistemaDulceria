using Entidades.DulceriaEntidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Datos.Mapeo
{
    public class ObjectoMap : IEntityTypeConfiguration<Objecto>
    {
        public void Configure(EntityTypeBuilder<Objecto> builder)
        {
            builder.ToTable("TblObjeto")
                    .HasKey(c => c.IdObjeto);
        }
    }
}

using Entidades.DulceriaEntidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Datos.Mapeo
{
    public class DetalleProductoTerminadoMap : IEntityTypeConfiguration<DetalleProductoTerminado>
    {
        public void Configure(EntityTypeBuilder<DetalleProductoTerminado> builder)
        {
            builder.ToTable("TblDetalleProductoTerminado")
                .HasKey(x => x.IdArticuloTerminado);

        }
    }
}

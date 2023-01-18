using Entidades.DulceriaEntidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Datos.Mapeo
{
    public class VentaMap : IEntityTypeConfiguration<Venta>
    {
        public void Configure(EntityTypeBuilder<Venta> builder)
        {
            builder.ToTable("TblVentas")
                .HasKey(c => c.IdVenta);
        }
    }
}

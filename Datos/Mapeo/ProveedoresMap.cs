using Entidades.DulceriaEntidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Datos.Mapeo
{
    public class ProveedoresMap : IEntityTypeConfiguration<Proveedores>
    {
        public void Configure(EntityTypeBuilder<Proveedores> builder)
        {
            builder.ToTable("TblProveedores")
                .HasKey(c => c.IdProveedor);
        }
    }
}

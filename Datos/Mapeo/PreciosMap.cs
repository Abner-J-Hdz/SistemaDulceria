using Entidades.DulceriaEntidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Datos.Mapeo
{
    public class PreciosMap : IEntityTypeConfiguration<Precios>
    {
        public void Configure(EntityTypeBuilder<Precios> builder)
        {
            builder.ToTable("TblPrecios")
                .HasKey(x => x.IdArticuloPrecio);

            builder
                .HasOne(p => p.Articulo)
                .WithMany(a => a.Precios)
                .HasForeignKey(da => da.IdArticulo);
        }
    }
}

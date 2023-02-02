using Entidades.DulceriaEntidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Datos.Mapeo
{
    public class TipoObjetoMap : IEntityTypeConfiguration<TipoObjecto>
    {
        public void Configure(EntityTypeBuilder<TipoObjecto> builder)
        {
            builder.ToTable("TblTipoObjeto")
                    .HasKey(c => c.IdTipoObjeto);
        }
    }
}

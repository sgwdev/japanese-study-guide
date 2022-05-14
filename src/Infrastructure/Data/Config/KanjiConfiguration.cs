using Core.Entities.KanjiAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Config
{
    class KanjiConfiguration : IEntityTypeConfiguration<Kanji>
    {
        public void Configure(EntityTypeBuilder<Kanji> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Character).IsRequired();
            builder.Property(p => p.Character).HasColumnType("char(1)");
            builder.HasIndex(p => p.Character).IsUnique();
        }
    }
}

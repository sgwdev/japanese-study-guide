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
    public class ReadingConfiguration : IEntityTypeConfiguration<Reading>
    {
        public void Configure(EntityTypeBuilder<Reading> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.TypeId).IsRequired();
            builder.Property(p => p.Label).HasColumnType("varchar(15)").IsRequired();
        }
    }
}

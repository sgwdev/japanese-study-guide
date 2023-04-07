using Core.Entities.VocabularyAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Config
{
    public class WordReadingConfiguration : IEntityTypeConfiguration<WordReading>
    {
        public void Configure(EntityTypeBuilder<WordReading> builder)
        {
            builder.Property(p => p.WordId).IsRequired();
            builder.Property(p => p.ReadingId).IsRequired();
            builder.Property(p => p.Order).IsRequired();
            builder.HasKey(wr => new { wr.WordId, wr.ReadingId, wr.Order });
        }
    }
}

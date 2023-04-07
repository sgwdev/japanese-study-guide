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
    public class WordConfiguration : IEntityTypeConfiguration<Word>
    {
        public void Configure(EntityTypeBuilder<Word> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Label).IsRequired();
            builder.Property(p => p.Label).HasColumnType("varchar(10)");
            builder.Property(p => p.Translation).HasColumnType("varchar(100)");
            builder.Property(p => p.Pronunciation).HasColumnType("varchar(50)");
        }
    }
}

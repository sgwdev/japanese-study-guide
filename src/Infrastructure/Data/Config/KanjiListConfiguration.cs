using Core.Entities.KanjiAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq;

namespace Infrastructure.Data.Config
{
    public class KanjiListConfiguration : IEntityTypeConfiguration<KanjiList>
    {
        public void Configure(EntityTypeBuilder<KanjiList> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.Name).HasColumnType("varchar(100)");
            builder.HasMany(p => p.Kanji).WithMany(p => p.KanjiLists).UsingEntity<KanjiListMapping>().ToTable("kanji_list_kanji");
        }
    }
}

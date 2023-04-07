using Core.Entities.VocabularyAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.KanjiAggregate
{
    public class Reading : BaseEntity
    {
        public int KanjiId { get; set; }
        public int TypeId { get; set; }
        public string Label { get; set; }

        public ReadingType Type { get; set; }
        public Kanji Kanji { get; set; }
        public List<WordReading> Words { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Reading))
                return false;

            Reading reading = (Reading)obj;
            return (this.KanjiId, this.TypeId, this.Label).Equals((reading.KanjiId, reading.TypeId, reading.Label));
        }

        public override int GetHashCode()
        {
            return  (this.KanjiId, this.TypeId, this.Label).GetHashCode();
        }
    }
}

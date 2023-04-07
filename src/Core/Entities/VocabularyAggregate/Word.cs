using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.VocabularyAggregate
{
    public class Word : BaseEntity
    {
        public string Label { get; set; }
        public string Translation { get; set; }
        public string Pronunciation { get; set; }

        public List<WordReading> Readings { get; set; }
    }
}

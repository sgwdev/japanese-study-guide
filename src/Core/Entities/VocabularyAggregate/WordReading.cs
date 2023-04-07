using Core.Entities.KanjiAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.VocabularyAggregate
{
    public class WordReading
    {
        public int WordId { get; set; }
        public int ReadingId { get; set; }
        public int Order { get; set; }

        public Word Word { get; set; }
        public Reading Reading { get; set; } 
    }
}

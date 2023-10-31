using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.VocabularyAggregate
{
    public class WordList : BaseEntity
    {
        public string Name { get; set; }

        public List<Word> Words { get; set; }
    }
}

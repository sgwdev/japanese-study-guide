using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.KanjiAggregate
{
    public class Kanji : BaseEntity
    {
        public string Character { get; set; }
        public List<Reading> Readings { get; set; }

        public List<KanjiList> KanjiLists { get; set; }
    }
}

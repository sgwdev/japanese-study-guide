using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.KanjiAggregate
{
    public class KanjiList : BaseEntity
    {
        public string Name { get; set; }

        public List<Kanji> Kanji { get; set; }
    }
}

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
    }
}

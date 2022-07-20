using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities.KanjiAggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Specifications
{
    public class KanjiWithReadingsSpecification : BaseSpecification<Kanji>
    {
        public KanjiWithReadingsSpecification()
        {
            AddInclude(x => x.Include(k => k.Readings).ThenInclude(r => r.Type));
        }

        public KanjiWithReadingsSpecification(int kanjiId) : this()
        {
            Criteria = (k => k.Id == kanjiId);
        }

        public KanjiWithReadingsSpecification(string character) : this()
        {
            Criteria = (k => k.Character == character);
        }
    }
}

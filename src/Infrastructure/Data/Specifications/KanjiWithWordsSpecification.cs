using Core.Entities.KanjiAggregate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Specifications
{
    public class KanjiWithWordsSpecification : BaseSpecification<Kanji>
    {
        public KanjiWithWordsSpecification()
        {
            AddInclude(x => x.Include(k => k.Readings).ThenInclude(r => r.Type));
            AddInclude(x => x.Include(k => k.Readings).ThenInclude(r => r.Words).ThenInclude(wr => wr.Word));
        }

        public KanjiWithWordsSpecification(int kanjiId) : this()
        {
            Criteria = (k => k.Id == kanjiId);
        }

        public KanjiWithWordsSpecification(string character) : this()
        {
            Criteria = (k => k.Character == character);
        }
    }
}

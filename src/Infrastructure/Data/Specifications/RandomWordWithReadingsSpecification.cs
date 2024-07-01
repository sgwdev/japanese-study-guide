using Core.Entities.VocabularyAggregate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Specifications
{
    public class RandomWordWithReadingsSpecification : BaseSpecification<Word>
    {
        public RandomWordWithReadingsSpecification()
        {
            AddInclude(x => x.Include(w => w.Readings).ThenInclude(wr => wr.Reading).ThenInclude(r => r.Type));
            AddInclude(x => x.Include(w => w.Readings).ThenInclude(wr => wr.Reading).ThenInclude(r => r.Kanji));
            IsRandomized = true;
        }

        public RandomWordWithReadingsSpecification(int wordCount) : this()
        {
            Take = wordCount;
        }

        public RandomWordWithReadingsSpecification(int wordListId, int wordCount) : this()
        {
            Criteria = (x => x.WordLists.Any(wl => wl.Id == wordListId));
            Take = wordCount;
        }
    }
}

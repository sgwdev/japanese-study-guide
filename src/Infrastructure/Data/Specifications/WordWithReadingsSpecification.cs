using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities.VocabularyAggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Specifications
{
    public class WordWithReadingsSpecification : BaseSpecification<Word>
    {
        public WordWithReadingsSpecification()
        {
            AddInclude(x => x.Include(w => w.Readings).ThenInclude(wr => wr.Reading).ThenInclude(r => r.Type));
            AddInclude(x => x.Include(w => w.Readings).ThenInclude(wr => wr.Reading).ThenInclude(r => r.Kanji));
        }

        public WordWithReadingsSpecification(int wordId) : this()
        {
            Criteria = (w => w.Id == wordId);
        }

        public WordWithReadingsSpecification(string label) : this()
        {
            Criteria = (w => w.Label == label);
        }

        public WordWithReadingsSpecification(List<int> wordsId) :this()
        {
            Criteria = (w => wordsId.Contains(w.Id));
        }
    }
}

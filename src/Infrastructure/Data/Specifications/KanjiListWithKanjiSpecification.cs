using Core.Entities.KanjiAggregate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Specifications
{
    public class KanjiListWithKanjiSpecification : BaseSpecification<KanjiList>
    {
        public KanjiListWithKanjiSpecification()
        {
            AddInclude(x => x.Include(kl => kl.Kanji));
        }

        public KanjiListWithKanjiSpecification(int kanjiListId) : this()
        {
            Criteria = (kl => kl.Id == kanjiListId);
        }
    }
}

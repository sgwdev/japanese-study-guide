using Core.Entities.VocabularyAggregate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Specifications
{
    public class WordListWithWordsSpecification : BaseSpecification<WordList>
    {
        public WordListWithWordsSpecification()
        {
            AddInclude(x => x.Include(wl => wl.Words)); 
        }

        public WordListWithWordsSpecification(int wordListId) : this()
        {
            Criteria = (wl => wl.Id == wordListId);
        }
    }
}

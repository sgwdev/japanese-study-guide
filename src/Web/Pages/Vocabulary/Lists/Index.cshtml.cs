using Core.Entities.VocabularyAggregate;
using Core.Interfaces;
using Infrastructure.Data.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Web.Pages.Vocabulary.Lists
{
    public class IndexModel : PageModel
    {
        private IRepository<WordList> _repository { get; set; }

        public List<WordList> WordLists { get; set; }

        public IndexModel(IRepository<WordList> repository)
        {
            _repository = repository;
        }

        public void OnGet()
        {
            WordLists = _repository.GetList(new WordListWithWordsSpecification());
        }
    }
}

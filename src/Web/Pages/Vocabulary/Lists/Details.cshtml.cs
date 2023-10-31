using Core.Entities.VocabularyAggregate;
using Core.Interfaces;
using Infrastructure.Data.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Vocabulary.Lists
{
    public class DetailsModel : PageModel
    {
        private IRepository<WordList> _repository { get; set; }

        public WordList WordList { get; set; }

        public DetailsModel(IRepository<WordList> repository)
        {
            _repository = repository;
        }

        public IActionResult OnGet(int id)
        {
            if (id > 0)
            {
                WordList = _repository.Get(new WordListWithWordsSpecification(id));
                if (WordList != null)
                {
                    return Page();
                }
            }

            return RedirectToPage("Index");
        }
    }
}

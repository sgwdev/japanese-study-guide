using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.Entities.VocabularyAggregate;
using Core.Interfaces;
using Infrastructure.Data.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Vocabulary
{
    public class DetailsModel : PageModel
    {
        private IRepository<Word> _repository { get; set; }
        public Word Word { get; set; }

        public DetailsModel(IRepository<Word> wordRepository)
        {
            this._repository = wordRepository;
        }

        public IActionResult OnGet(int id)
        {
            if(id > 0)
            {
                Word = _repository.Get(new WordWithReadingsSpecification(id));

                if(Word != null)
                {
                    return Page();
                }
            }

            return RedirectToPage("Index");
        }
    }
}

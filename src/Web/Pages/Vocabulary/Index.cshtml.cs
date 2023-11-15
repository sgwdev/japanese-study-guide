using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.VocabularyAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Vocabulary
{
    public class IndexModel : PageModel
    {
        private IRepository<Word> _repository { get; set; }
        public List<Word> Words { get; set; }

        public IndexModel(IRepository<Word> wordRepository)
        {
            this._repository = wordRepository;
        }

        public void OnGet()
        {
            Words = _repository.GetList();
        }
    }
}

using Core;
using Core.Entities.VocabularyAggregate;
using Core.Interfaces;
using Infrastructure.Data.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace Web.Pages.Vocabulary.Lists
{
    public class AddModel : PageModel
    {
        private IRepository<WordList> _repository { get; set; }
        private IRepository<Word> _wordRepository { get; set; }

        [BindProperty]
        public WordList WordList { get; set; }

        [BindProperty]
        public List<int> SelectedWordsId { get; set; }

        public List<Word> Words { get; set; }

        public AddModel(IRepository<WordList> repository, IRepository<Word> wordRepository)
        {
            this._repository = repository;
            this._wordRepository = wordRepository;
        }

        public void OnGet()
        {
            Words = _wordRepository.GetList();
        }

        public IActionResult OnPost()
        {
            WordList.Words = (SelectedWordsId.Count > 0) ? _wordRepository.GetList(new WordWithReadingsSpecification(SelectedWordsId)) : new List<Word>();

            ValidateModel();

            if (ModelState.IsValid)
            {
                _repository.Add(WordList);
                _repository.Save();

                return RedirectToPage("Index");
            }

            Words = _wordRepository.GetList();
            return Page();
        }

        public void ValidateModel()
        {
            if (string.IsNullOrEmpty(WordList.Name))
            {
                ModelState.AddModelError("WordList.Name", "A name is required.");
            }

            if (WordList.Words.Count == 0)
            {
                ModelState.AddModelError("WordList.Words", "At least one word must be selected.");
            }
        }
    }
}

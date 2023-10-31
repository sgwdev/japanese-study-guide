using Core.Entities.VocabularyAggregate;
using Core.Interfaces;
using Infrastructure.Data.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Web.Pages.Vocabulary.Lists
{
    public class EditModel : PageModel
    {
        private IRepository<WordList> _repository { get; set; }
        private IRepository<Word> _wordRepository { get; set; }

        [BindProperty]     
        public WordList WordList { get; set; }

        [BindProperty]
        public List<int> SelectedWordsId { get; set; }

        public List<Word> Words { get; set; }

        public EditModel(IRepository<WordList> repository, IRepository<Word> wordRepository)
        {
            this._repository = repository;
            this._wordRepository = wordRepository;
        }

        public IActionResult OnGet(int id)
        {
            if(id > 0)
            {
                WordList = _repository.Get(new WordListWithWordsSpecification(id));

                if(WordList != null)
                {
                    SelectedWordsId = WordList.Words.Select(w => w.Id).ToList();
                    Words = _wordRepository.GetList();

                    return Page();
                }
            }

            return RedirectToPage("Index");
        }

        public IActionResult OnPost(int id)
        {
            WordList.Words = (SelectedWordsId.Count > 0) ? _wordRepository.GetList(new WordWithReadingsSpecification(SelectedWordsId)) : new List<Word>();

            ValidateModel();

            if (ModelState.IsValid)
            {
                WordList savedWordList = _repository.Get(new WordListWithWordsSpecification(id));

                if(savedWordList != null)
                {
                    savedWordList.Name = WordList.Name;
                    savedWordList.Words = WordList.Words;
                    _repository.Save();

                    return RedirectToPage("Details", new { id = savedWordList.Id });
                }
            }

            Words = _wordRepository.GetList();

            return Page();
        }

        private void ValidateModel()
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

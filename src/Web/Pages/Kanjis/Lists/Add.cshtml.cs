using Core.Entities.KanjiAggregate;
using Core.Entities.VocabularyAggregate;
using Core.Interfaces;
using Infrastructure.Data.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Web.Pages.Kanjis.Lists
{
    public class AddModel : PageModel
    {
        private IRepository<KanjiList> _repository { get; set; }
        private IRepository<Kanji> _kanjiRepository { get; set; }

        [BindProperty]
        public KanjiList KanjiList { get; set; }

        [BindProperty]
        public List<int> SelectedKanjiId { get; set; }

        public List<Kanji> Kanji { get; set; }

        public AddModel(IRepository<KanjiList> repository, IRepository<Kanji> kanjiRepository)
        {
            this._repository = repository;
            this._kanjiRepository = kanjiRepository;
        }

        public void OnGet()
        {
            Kanji = _kanjiRepository.GetList();
        }

        public IActionResult OnPost()
        {
            KanjiList.Kanji = (SelectedKanjiId.Count > 0) ? _kanjiRepository.GetList(new KanjiWithReadingsSpecification(SelectedKanjiId)) : new List<Kanji>();

            ValidateModel();

            if (ModelState.IsValid)
            {
                _repository.Add(KanjiList);
                _repository.Save();

                return RedirectToPage("Index");
            }

            Kanji = _kanjiRepository.GetList();
            return Page();
        }

        public void ValidateModel()
        {
            if (string.IsNullOrEmpty(KanjiList.Name))
            {
                ModelState.AddModelError("KanjiList.Name", "A name is required.");
            }

            if (KanjiList.Kanji.Count == 0)
            {
                ModelState.AddModelError("KanjiList.Kanji", "At least one kanji must be selected.");
            }
        }
    }
}

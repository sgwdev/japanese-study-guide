using Core.Entities.KanjiAggregate;
using Core.Entities.VocabularyAggregate;
using Core.Interfaces;
using Infrastructure.Data.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace Web.Pages.Kanjis.Lists
{
    public class EditModel : PageModel
    {
        private IRepository<KanjiList> _repository { get; set; }
        private IRepository<Kanji> _kanjiRepository { get; set; }

        [BindProperty]
        public KanjiList KanjiList { get; set; }

        [BindProperty]
        public List<int> SelectedKanjiId { get; set; }

        public List<Kanji> Kanji { get; set; }

        public EditModel(IRepository<KanjiList> repository, IRepository<Kanji> kanjiRepository)
        {
            _repository = repository;
            _kanjiRepository = kanjiRepository;
        }

        public IActionResult OnGet(int id)
        {
            if (id > 0)
            {
                KanjiList = _repository.Get(new KanjiListWithKanjiSpecification(id));

                if (KanjiList != null)
                {
                    SelectedKanjiId = KanjiList.Kanji.Select(k => k.Id).ToList();
                    Kanji = _kanjiRepository.GetList();

                    return Page();
                }
            }

            return RedirectToPage("Index");
        }

        public IActionResult OnPost(int id)
        {
            KanjiList.Kanji = (SelectedKanjiId.Count > 0) ? _kanjiRepository.GetList(new KanjiWithReadingsSpecification(SelectedKanjiId)) : new List<Kanji>();

            ValidateModel();

            if (ModelState.IsValid)
            {
                KanjiList savedKanjiList = _repository.Get(new KanjiListWithKanjiSpecification(id));

                if (savedKanjiList != null)
                {
                    savedKanjiList.Name = KanjiList.Name;
                    savedKanjiList.Kanji = KanjiList.Kanji;
                    _repository.Save();

                    return RedirectToPage("Details", new { id = savedKanjiList.Id });
                }
            }

            Kanji = _kanjiRepository.GetList();

            return Page();
        }

        private void ValidateModel()
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

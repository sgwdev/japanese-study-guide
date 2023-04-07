using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Entities.KanjiAggregate;
using Core.Entities.VocabularyAggregate;
using Core.Interfaces;
using Infrastructure.Data.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Vocabulary
{
    public class AddModel : PageModel
    {
        private IRepository<Word> _repository { get; set; }
        private IRepository<Kanji> _kanjiRepository { get; set; }

        [BindProperty]
        public Word Word { get; set; }
        
        [BindProperty]
        public bool IsSpecialReading { get; set; }

        public List<Kanji> WordKanji { get; private set; }

        public AddModel(IRepository<Word> wordRepository, IRepository<Kanji> kanjiRepository)
        {
            this._repository = wordRepository;
            this._kanjiRepository = kanjiRepository;
            this.WordKanji = new List<Kanji>();
        }

        public IActionResult OnPost()
        {
            ValidateModel();

            if (!ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(Word.Label) && Word.Readings.Count > 0)
                {

                    // Prepare data for readings SelectLists
                    for (int i = 0; i < Word.Label.Length; i++)
                    {
                        if (Word.Label[i] == Constants.Noma)
                            continue;

                        Kanji k = _kanjiRepository.Get(new KanjiWithReadingsSpecification(Word.Label[i].ToString()));
                        k.Readings = k.Readings.Where(r => r.TypeId == Constants.ReadingTypes.On || r.TypeId == Constants.ReadingTypes.Kun).ToList();
                        WordKanji.Add(k);
                    }
                }

                return Page();
            }

            if (IsSpecialReading)
            {
                Word.Readings = new List<WordReading>();

                // Set special readings for each kanji, reuse existing ones if already in database
                for(int i=0; i<Word.Label.Length; i++)
                {
                    Kanji kanji = _kanjiRepository.Get(new KanjiWithReadingsSpecification(Word.Label[i].ToString()));

                    Reading reading = new Reading();
                    if(kanji.Readings.Where(r => r.Label == Word.Pronunciation && r.TypeId == Constants.ReadingTypes.Special).Count() > 0)
                    {
                        reading = kanji.Readings.Where(r => r.Label == Word.Pronunciation && r.TypeId == Constants.ReadingTypes.Special).SingleOrDefault();
                    }
                    else
                    {
                        reading = new Reading { Label = Word.Pronunciation, TypeId = Constants.ReadingTypes.Special };
                        kanji.Readings.Add(reading);
                    }

                    WordReading wordReading = new WordReading() { Reading = reading, Order = i + 1 };
                    Word.Readings.Add(wordReading);
                }
            }

            _repository.Add(Word);
            _repository.Save();

            return RedirectToPage("Details", new { id = Word.Id });
        }

        public void ValidateModel()
        {
            if (string.IsNullOrEmpty(Word.Label))
            {
                ModelState.AddModelError("Word.Label", "A label is required.");
            }

            if (string.IsNullOrEmpty(Word.Pronunciation))
            {
                ModelState.AddModelError("Word.Pronunciation", "A label is required.");
            }
            else if (!Tools.IsInHiragana(Word.Pronunciation))
            {
                ModelState.AddModelError("Word.Pronunciation", "Reading must be written in hiragana.");
            }
            else if(!string.IsNullOrEmpty(Word.Label))
            {
                List<Word> sameWords = _repository.GetList(new WordWithReadingsSpecification(Word.Label)).Where(w => w.Pronunciation == Word.Pronunciation).ToList();
                if (sameWords != null && sameWords.Count > 0)
                {
                    ModelState.AddModelError("Word.Label", "A word with the same label and pronunciation already exists.");
                }
            }
        }

        public JsonResult OnGetKanjiSearch(string character)
        {
            
            Kanji kanji = _kanjiRepository.Get(new KanjiWithReadingsSpecification(character));
            KanjiDTO kanjiDTO = new KanjiDTO()
            {
                Id = kanji.Id,
                Character = kanji.Character,
                Readings = new List<ReadingDTO>()
            };

            foreach(Reading r in kanji.Readings.Where(r => r.TypeId == Constants.ReadingTypes.On || r.TypeId == Constants.ReadingTypes.Kun))
            {
                kanjiDTO.Readings.Add(new ReadingDTO
                {
                    Id = r.Id,
                    TypeId = r.TypeId,
                    Label = r.Label
                });
            }

            return new JsonResult(kanjiDTO);
        }

        private class KanjiDTO
        {
            public int Id { get; set; }
            public string Character { get; set; }
            public List<ReadingDTO> Readings { get; set; }
        }

        private class ReadingDTO
        {
            public int Id { get; set; }
            public int TypeId { get; set; }
            public string Label { get; set; }
        }
    }
}

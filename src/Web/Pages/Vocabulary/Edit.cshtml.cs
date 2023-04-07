using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.Entities.KanjiAggregate;
using Core.Entities.VocabularyAggregate;
using Core.Interfaces;
using Infrastructure.Data.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Vocabulary
{
    public class EditModel : PageModel
    {
        private IRepository<Word> _repository { get; set; }
        private IRepository<Kanji> _kanjiRepository { get; set; }

        [BindProperty]
        public Word Word { get; set; }

        [BindProperty]
        public bool IsSpecialReading { get; set; }

        public List<Kanji> WordKanji { get; private set; }

        public EditModel(IRepository<Word> wordRepository, IRepository<Kanji> kanjiRepository)
        {
            _repository = wordRepository;
            _kanjiRepository = kanjiRepository;
            WordKanji = new List<Kanji>();
        }

        public IActionResult OnGet(int id)
        {
            if(id > 0)
            {
                Word = _repository.Get(new WordWithReadingsSpecification(id));

                if (Word != null)
                {
                    if (Word.Readings[0].Reading.TypeId == Constants.ReadingTypes.Special)
                        IsSpecialReading = true;

                    for(int i=0; i<Word.Label.Length; i++)
                    {
                        if (Word.Label[i] == Constants.Noma)
                            continue;

                        Kanji k = _kanjiRepository.Get(new KanjiWithReadingsSpecification(Word.Label[i].ToString()));
                        k.Readings = k.Readings.Where(r => r.TypeId == Constants.ReadingTypes.On || r.TypeId == Constants.ReadingTypes.Kun).ToList();
                        WordKanji.Add(k);
                    }

                    return Page();
                }
            }

            return RedirectToPage("Index");
        }

        public IActionResult OnPost(int id)
        {
            ValidateModel();

            Word SavedWord = _repository.Get(new WordWithReadingsSpecification(id));
            SavedWord.Readings.Sort((x, y) => x.Order.CompareTo(y.Order));

            if (!ModelState.IsValid)
            {
                Word.Id = SavedWord.Id;
                Word.Label = SavedWord.Label;

                // Prepare data for readings SelectLists
                for (int i = 0; i < Word.Label.Length; i++)
                {
                    if (Word.Label[i] == Constants.Noma)
                        continue;

                    Kanji k = _kanjiRepository.Get(new KanjiWithReadingsSpecification(Word.Label[i].ToString()));
                    k.Readings = k.Readings.Where(r => r.TypeId == Constants.ReadingTypes.On || r.TypeId == Constants.ReadingTypes.Kun).ToList();
                    WordKanji.Add(k);
                }

                return Page();
            }

            if(IsSpecialReading)
                Word.Readings = new List<WordReading>();

            bool wasSpecialReading = (SavedWord.Readings[0].Reading.TypeId == Constants.ReadingTypes.Special);

            // Standard reading => Special reading or Special reading => Standard reading or Special reading => Special reading
            if ((!wasSpecialReading && IsSpecialReading) ||
                (wasSpecialReading && !IsSpecialReading) ||
                (wasSpecialReading && IsSpecialReading && SavedWord.Pronunciation != Word.Pronunciation))
            {
                for(int i=0; i<Word.Label.Length; i++)
                {
                    if (Word.Label[i] == Constants.Noma)
                        continue;

                    Kanji kanji = _kanjiRepository.Get(new KanjiWithWordsSpecification(Word.Label[i].ToString()));
                    Reading reading = kanji.Readings.Where(r => r.Id == SavedWord.Readings[i].ReadingId).SingleOrDefault();

                    if (wasSpecialReading && reading.Words.Count == 1)
                    {
                        kanji.Readings.Remove(reading);
                    }

                    if (IsSpecialReading)
                    {
                        // Set special reading, reuse existing one if already in database
                        Reading newSpecialReading = new Reading();
                        if (kanji.Readings.Where(r => r.Label == Word.Pronunciation && r.TypeId == Constants.ReadingTypes.Special).Count() > 0)
                        {
                            newSpecialReading = kanji.Readings.Where(r => r.Label == Word.Pronunciation && r.TypeId == Constants.ReadingTypes.Special).SingleOrDefault();
                        }
                        else
                        {
                            newSpecialReading = new Reading { Label = Word.Pronunciation, TypeId = Constants.ReadingTypes.Special };
                            kanji.Readings.Add(newSpecialReading);
                        }

                        WordReading wordReading = new WordReading() { Reading = newSpecialReading, WordId = Word.Id, Order = i + 1 };
                        Word.Readings.Add(wordReading);
                    }
                }
            }
            
            SavedWord.Pronunciation = Word.Pronunciation;
            SavedWord.Translation = Word.Translation;
            SavedWord.Readings = Word.Readings;

            _repository.Save();

            return RedirectToPage("Details", new { id = SavedWord.Id });
        }

        private void ValidateModel()
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
            else
            {
                List<Word> sameWords = _repository.GetList(new WordWithReadingsSpecification(Word.Label)).Where(w => w.Pronunciation == Word.Pronunciation && w.Id != Word.Id).ToList();
                if (sameWords != null && sameWords.Count > 0)
                {
                    ModelState.AddModelError("Word.Pronunciation", "A word with the same label and pronunciation already exists.");
                }
            }
        }
    }
}

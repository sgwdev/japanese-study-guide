using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.Entities.KanjiAggregate;
using Core.Interfaces;
using Infrastructure.Data.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Kanjis
{
    public class EditModel : PageModel
    {
        private IRepository<Kanji> _repository { get; set; }
        private IRepository<ReadingType> _readingTypeRepository { get; set; }

        [BindProperty]
        public Kanji Kanji { get; set; }
        public List<ReadingType> ReadingTypes { get; set; }

        public EditModel(IRepository<Kanji> repository, IRepository<ReadingType> readingTypeRepository)
        {
            _repository = repository;
            _readingTypeRepository = readingTypeRepository;
        }

        public IActionResult OnGet(int id)
        {
            if (id > 0)
            {
                Kanji = _repository.Get(new KanjiWithReadingsSpecification(id));
                
                if(Kanji != null)
                {
                    Kanji.Readings = Kanji.Readings.OrderBy(r => r.TypeId).ToList();
                    ReadingTypes = _readingTypeRepository.GetList(new OnlyStandardReadingsSpecification());
                    return Page();
                }             
            }

            return RedirectToPage("Index");
        }

        public IActionResult OnPost(int id)
        {
            foreach (string key in ModelState.Keys.Where(x => x.StartsWith("Kanji.Readings")).ToList())
            {
                ModelState.Remove(key);
            }

            Kanji.Readings = FilterInvalidReadings(Kanji.Readings);
            ValidateModel();

            Kanji SavedKanji = _repository.Get(new KanjiWithReadingsSpecification(id));

            if (!ModelState.IsValid)
            {
                Kanji.Id = SavedKanji.Id;
                Kanji.Character = SavedKanji.Character;

                ReadingTypes = _readingTypeRepository.GetList(new OnlyStandardReadingsSpecification());
                return Page();
            }

            SavedKanji.Readings = Kanji.Readings;
            _repository.Save();

            return RedirectToPage("Details", new { id = SavedKanji.Id });
        }

        private List<Reading> FilterInvalidReadings(List<Reading> readings)
        {
            HashSet<Reading> readingSet = new HashSet<Reading>();
            List<Reading> filteredReadings = new List<Reading>();

            for (int i = 0; i < readings.Count; i++)
            {
                if (!string.IsNullOrEmpty(readings[i].Label) && !readingSet.Contains(readings[i]))
                {
                    readingSet.Add(Kanji.Readings[i]);
                    filteredReadings.Add(readings[i]);
                }
            }

            return filteredReadings;
        }

        private void ValidateModel()
        {
            if (Kanji.Readings.Count == 0)
            {
                ModelState.AddModelError("Kanji.Readings[0].Label", "At least one reading is required.");
                Kanji.Readings.Add(new Reading());
            }
            else
            {
                for (int i = 0; i < Kanji.Readings.Count; i++)
                {
                    if (Kanji.Readings[i].TypeId != Constants.ReadingTypes.On && Kanji.Readings[i].TypeId != Constants.ReadingTypes.Kun)
                    {
                        ModelState.AddModelError($"Kanji.Readings[{i}].TypeId", $"A reading type is required.");
                    }

                    if (string.IsNullOrEmpty(Kanji.Readings[i].Label))
                    {
                        ModelState.AddModelError($"Kanji.Readings[{i}].Label", $"A label is required.");
                    }

                    if (!Tools.IsInHiragana(Kanji.Readings[i].Label))
                    {
                        ModelState.AddModelError($"Kanji.Readings[{i}].Label", $"Label must be written in hiragana.");
                    }
                }
            }
        }
    }
}

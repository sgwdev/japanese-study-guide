using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class AddModel : PageModel
    {
        [BindProperty]
        public Kanji Kanji { get; set; } = new Kanji() { Readings = new List<Reading>() { } };

        public List<ReadingType> ReadingTypes { get; set; }
        private IRepository<Kanji> _repository { get; set; }
        private IRepository<ReadingType> _readingTypeRepository { get; set; }

        public AddModel(IRepository<Kanji> repository, IRepository<ReadingType> readingTypeRepository)
        {
            this._repository = repository;
            this._readingTypeRepository = readingTypeRepository;
            Kanji.Readings.Add(new Reading());
        }

        public void OnGet()
        {
            ReadingTypes = _readingTypeRepository.GetList(new OnlyStandardReadingsSpecification());
        }

        public IActionResult OnPost()
        {
            // Avoid mismatches between input fields and error messages after filtering invalid readings
            foreach (string key in ModelState.Keys.Where(x => x.StartsWith("Kanji.Readings")).ToList())
            {
                ModelState.Remove(key);
            }

            Kanji.Readings = FilterInvalidReadings(Kanji.Readings);
            ValidateModel();

            if (!ModelState.IsValid)
            {
                ReadingTypes = _readingTypeRepository.GetList(new OnlyStandardReadingsSpecification());
                return Page();
            }

            _repository.Add(Kanji);
            _repository.Save();

            return RedirectToPage("index", new { id = Kanji.Id });
        }

        private List<Reading> FilterInvalidReadings(List<Reading> readings)
        {
            HashSet<Reading> readingSet = new HashSet<Reading>();
            List<Reading> filteredReadings = new List<Reading>();

            for (int i = 0; i < readings.Count; i++)
            {
                if(!string.IsNullOrEmpty(readings[i].Label) && !readingSet.Contains(readings[i]))
                {
                    readingSet.Add(Kanji.Readings[i]);
                    filteredReadings.Add(readings[i]);
                }
            }

            return filteredReadings;
        }

        private void ValidateModel()
        {
            if (string.IsNullOrEmpty(Kanji.Character))
            {
                ModelState.AddModelError("Kanji.Character", "A character is required.");
            }

            Kanji sameKanji = _repository.Get(new KanjiWithReadingsSpecification(Kanji.Character));
            if (sameKanji != null)
            {
                ModelState.AddModelError("Kanji.Character", "A Kanji with the same character already exists.");
            }

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
                        ModelState.AddModelError($"Kanji.Readings[{i}].TypeId", "A reading type is required.");
                    }

                    if (string.IsNullOrEmpty(Kanji.Readings[i].Label))
                    {
                        ModelState.AddModelError($"Kanji.Readings[{i}].Label", "A label is required.");
                    }

                    if (!Tools.IsInHiragana(Kanji.Readings[i].Label))
                    {
                        ModelState.AddModelError($"Kanji.Readings[{i}].Label", "Label must be written in hiragana.");
                    }
                }
            }
        }
    }
}

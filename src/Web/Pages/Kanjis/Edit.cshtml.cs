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
        public List<bool> LockedReadings { get; set; }

        public EditModel(IRepository<Kanji> repository, IRepository<ReadingType> readingTypeRepository)
        {
            _repository = repository;
            _readingTypeRepository = readingTypeRepository;
            LockedReadings = new List<bool>();
        }

        public IActionResult OnGet(int id)
        {
            if (id > 0)
            {
                Kanji = _repository.Get(new KanjiWithWordsSpecification(id));
                
                if(Kanji != null)
                {
                    Kanji.Readings = Kanji.Readings.Where(r => r.TypeId == Constants.ReadingTypes.On || r.TypeId == Constants.ReadingTypes.Kun)
                                                   .OrderBy(r => r.TypeId).ToList();

                    for(int i=0; i<Kanji.Readings.Count; i++)
                    {
                        LockedReadings.Add(Kanji.Readings[i].Words.Count > 0 ? true : false);
                    }

                    ReadingTypes = _readingTypeRepository.GetList(new OnlyStandardReadingsSpecification());

                    return Page();
                }             
            }

            return RedirectToPage("Index");
        }

        public IActionResult OnPost(int id)
        {
            // Avoid mismatches between input fields and error messages after filtering invalid readings
            foreach (string key in ModelState.Keys.Where(x => x.StartsWith("Kanji.Readings")).ToList())
            {
                ModelState.Remove(key);
            }

            Kanji.Readings = FilterInvalidReadings(Kanji.Readings);

            Kanji savedKanji = _repository.Get(new KanjiWithWordsSpecification(id));
            Kanji.Readings = MergeReadings(savedKanji.Readings, Kanji.Readings);

            ValidateModel();

            if (!ModelState.IsValid)
            {
                Kanji.Id = savedKanji.Id;
                Kanji.Character = savedKanji.Character;
                
                ReadingTypes = _readingTypeRepository.GetList(new OnlyStandardReadingsSpecification());

                LockedReadings = new List<bool>();
                foreach (Reading reading in Kanji.Readings)
                    LockedReadings.Add((reading.Words is null || reading.Words.Count == 0) ? false : true);

                return Page();
            }

            savedKanji.Readings = savedKanji.Readings.Where(r => r.TypeId == Constants.ReadingTypes.Special).ToList();
            savedKanji.Readings.AddRange(Kanji.Readings);
            _repository.Save();

            return RedirectToPage("Details", new { id = savedKanji.Id });
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

        private List<Reading> MergeReadings(List<Reading> previousReadings, List<Reading> newReadings)
        {
            previousReadings = previousReadings.OrderBy(r => r.TypeId).ToList();
            List<Reading> mergedReadings = new List<Reading>();
            int j = 0;
            
            for (int i = 0; i < previousReadings.Count; i++)
            {
                if (previousReadings[i].TypeId == Constants.ReadingTypes.Special)
                    break;

                if (previousReadings[i].Words.Count > 0)
                {
                    mergedReadings.Add(previousReadings[i]);
                }
                else if (j < newReadings.Count && previousReadings[i].Id == newReadings[j].Id)
                {
                    mergedReadings.Add(newReadings[j]);
                    j++;
                }
            }

            for (int i = j; i < newReadings.Count; i++)
            {
                mergedReadings.Add(newReadings[i]);
            }

            return mergedReadings;
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

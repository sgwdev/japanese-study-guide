using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.KanjiAggregate;
using Core.Interfaces;
using Infrastructure.Data.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Kanjis
{
    public class DetailsModel : PageModel
    {
        private IRepository<Kanji> _repository { get; set; }
        public Kanji Kanji { get; set; }

        public DetailsModel(IRepository<Kanji> repository)
        {
            _repository = repository;
        }

        public IActionResult OnGet(int id)
        {
            if (id > 0)
            {
                Kanji = _repository.Get(new KanjiWithWordsSpecification(id));
                Kanji.Readings = Kanji.Readings.OrderBy(k => k.TypeId).ToList();

                if(Kanji != null)
                {
                    return Page();
                }
            }

            return RedirectToPage("Index");
        }
    }
}

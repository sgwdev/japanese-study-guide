using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Core.Interfaces;
using Core.Entities.KanjiAggregate;
using Infrastructure.Data.Specifications;

namespace Web.Pages.Kanjis
{
    public class IndexModel : PageModel
    {
        private IRepository<Kanji> _kanjiRepository { get; }
        public List<Kanji> KanjiList { get; set; }

        public IndexModel(IRepository<Kanji> kanjiRepository)
        {
            this._kanjiRepository = kanjiRepository;
        }

        public void OnGet()
        {
            KanjiList = _kanjiRepository.GetList(new KanjiWithWordsSpecification());
        }
    }
}

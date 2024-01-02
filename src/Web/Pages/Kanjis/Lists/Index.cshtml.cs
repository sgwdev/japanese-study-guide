using Core.Entities.KanjiAggregate;
using Core.Interfaces;
using Infrastructure.Data.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Web.Pages.Kanjis.Lists
{
    public class IndexModel : PageModel
    {
        private IRepository<KanjiList> _repository { get; set; }

        public List<KanjiList> KanjiLists { get; set; }

        public IndexModel(IRepository<KanjiList> repository)
        {
            this._repository = repository;
        }

        public void OnGet()
        {
            this.KanjiLists = _repository.GetList(new KanjiListWithKanjiSpecification());
        }
    }
}

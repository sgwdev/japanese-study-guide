using Core.Entities.KanjiAggregate;
using Core.Interfaces;
using Infrastructure.Data.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Kanjis.Lists
{
    public class DetailsModel : PageModel
    {
        private IRepository<KanjiList> _repository { get; set; }

        public KanjiList KanjiList { get; set; }

        public DetailsModel(IRepository<KanjiList> repository)
        {
            _repository = repository;
        }

        public void OnGet(int id)
        {
            this.KanjiList = _repository.Get(new KanjiListWithKanjiSpecification(id));
        }
    }
}

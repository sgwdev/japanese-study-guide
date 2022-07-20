using Core;
using Core.Entities.KanjiAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Specifications
{
    public class OnlyStandardReadingsSpecification : BaseSpecification<ReadingType>
    {
        public OnlyStandardReadingsSpecification()
        {
            Criteria = (readingType => readingType.Id == Constants.ReadingTypes.On || readingType.Id == Constants.ReadingTypes.Kun);
        }
    }
}

using Core;
using Core.Entities.KanjiAggregate;
using Infrastructure.Data.Extensions;
using Infrastructure.Data.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class SpecificationTests
    {
        [Fact]
        public void Specification_filters_only_standard_reading_types()
        {
            var onReadingType = new ReadingType() { Id = Constants.ReadingTypes.On };
            var kunReadingType = new ReadingType() { Id = Constants.ReadingTypes.Kun };
            var specialReadingType = new ReadingType() { Id = Constants.ReadingTypes.Special };

            var readingTypes = new List<ReadingType>() { onReadingType, kunReadingType, specialReadingType };

            var spec = new OnlyStandardReadingsSpecification();
            List<ReadingType> filteredTypes = readingTypes.AsQueryable().ApplySpecification(spec).ToList();

            Assert.Contains(onReadingType, filteredTypes);
            Assert.Contains(kunReadingType, filteredTypes);
            Assert.DoesNotContain(specialReadingType, filteredTypes);
        }
    }
}

using Core.Entities.KanjiAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTests.Builders;
using Xunit;

namespace UnitTests
{
    public class ReadingTests
    {
        [Fact]
        public void Same_readings_are_equal()
        {
            Reading a = new Reading() { KanjiId = 1, TypeId = 2, Label = "A" };
            Reading b = new Reading() { KanjiId = 1, TypeId = 2, Label = "A" };

            Reading x = new Reading() { KanjiId = 7, TypeId = 3, Label = "B" };
            Reading y = new Reading() { KanjiId = 7, TypeId = 3, Label = "B" };

            Assert.True(a.Equals(b) && b.Equals(a));
            Assert.True(a.GetHashCode() == b.GetHashCode());

            Assert.True(x.Equals(y) && y.Equals(x));
            Assert.True(x.GetHashCode() == y.GetHashCode());
        }

        [Fact]
        public void Different_readings_are_not_equal()
        {
            Reading x = new Reading() { KanjiId = 1, TypeId = 3, Label = "A" };
            Reading y = new Reading() { KanjiId = 2, TypeId = 3, Label = "A" };
            Assert.False(x.Equals(y) && y.Equals(x));
            Assert.False(x.GetHashCode() == y.GetHashCode());

            x = new Reading() { KanjiId = 1, TypeId = 2, Label = "A" };
            y = new Reading() { KanjiId = 1, TypeId = 3, Label = "A" };
            Assert.False(x.Equals(y) && y.Equals(x));
            Assert.False(x.GetHashCode() == y.GetHashCode());

            x = new Reading() { KanjiId = 1, TypeId = 2, Label = "A" };
            y = new Reading() { KanjiId = 1, TypeId = 2, Label = "B" };
            Assert.False(x.Equals(y) && y.Equals(x));
            Assert.False(x.GetHashCode() == y.GetHashCode());

            y = null;
            Assert.False(x.Equals(y) && y.Equals(x));
        }
    }
}

using Core.Entities.KanjiAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Builders
{
    public class KanjiBuilder
    {
        private int _id { get; set; }
        private string _character { get; set; }
        private List<Reading> _readings { get; set; } = new List<Reading>();

        public KanjiBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public KanjiBuilder WithCharacter(string character)
        {
            _character = character;
            return this;
        }

        public KanjiBuilder WithReading(Reading reading)
        {
            Reading newReading = new Reading()
            {
                Id = reading.Id,
                TypeId = reading.TypeId,
                Label = reading.Label
            };

            _readings.Add(newReading);
            return this;
        }

        public KanjiBuilder WithTestValues()
        {
            _id = 0;
            _character = "A";
            _readings = new List<Reading>()
            {
                new Reading() { TypeId = 2, Label = "reading 1" },
                new Reading() { TypeId = 3, Label = "reading 2" }
            };

            return this;
        }

        public Kanji Build()
        {
            Kanji newKanji = new Kanji()
            {
                Id = _id,
                Character = _character,
                Readings = _readings
            };

            Init();
            return newKanji;
        }

        private void Init()
        {
            _id = 0;
            _character = string.Empty;
            _readings = new List<Reading>();
        }
    }
}

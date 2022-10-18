using Core.Entities.KanjiAggregate;
using Infrastructure.Data.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using UnitTests.Builders;
using Core.Interfaces;

namespace IntegrationTests.Data
{
    public class KanjiRepositoryTests : BaseRepositoryTestFixture
    {
        private KanjiBuilder _kanjiBuilder { get; }
        private IRepository<Kanji> _repository { get; }

        public KanjiRepositoryTests()
        {
            _kanjiBuilder = new KanjiBuilder();
            _repository = GetRepository<Kanji>();
        }

        [Fact]
        public void Kanji_is_added_to_database()
        {
            var kanji = _kanjiBuilder.WithTestValues().Build();
            _repository.Add(kanji);
            _repository.Save();

            // detach the item so we get a different instance
            _appDbContext.Entry(kanji).State = EntityState.Detached;

            var newKanji = _repository.GetList(new KanjiWithReadingsSpecification()).FirstOrDefault();

            Assert.NotNull(newKanji);
            Assert.True(newKanji?.Id > 0);
            Assert.Equal(kanji.Character, newKanji.Character);
            Assert.Contains(kanji.Readings, r => r.TypeId == kanji.Readings[0].TypeId && r.Label == kanji.Readings[0].Label);
            Assert.Contains(kanji.Readings, r => r.TypeId == kanji.Readings[1].TypeId && r.Label == kanji.Readings[1].Label);
            Assert.Equal(2, newKanji.Readings.Count);
            Assert.True(newKanji.Readings.All(r => r.Id > 0));
        }

        [Fact]
        public void Kanji_is_updated_into_database()
        {
            var kanji = _kanjiBuilder.WithTestValues().Build();
            _repository.Add(kanji);
            _repository.Save();

            _appDbContext.Entry(kanji).State = EntityState.Detached;

            var newKanji = _repository.GetList().FirstOrDefault();

            string character = "B";            
            newKanji.Character = character;
            _repository.Update(newKanji);
            _repository.Save();

            _appDbContext.Entry(newKanji).State = EntityState.Detached;

            var updatedKanji = _repository.GetList().FirstOrDefault();

            Assert.NotNull(updatedKanji);
            Assert.Equal(character, updatedKanji.Character);
            Assert.NotEqual(kanji.Character, updatedKanji.Character);
            Assert.Equal(kanji.Id, updatedKanji.Id);
        }

        [Fact]
        public void Kanji_is_retrieved_by_id()
        {
            var kanjiId = 25;
            var kanji = _kanjiBuilder.WithTestValues().WithId(kanjiId).Build();
            _repository.Add(kanji);
            _repository.Save();

            _appDbContext.Entry(kanji).State = EntityState.Detached;

            var newKanji = _repository.GetById(kanjiId);

            Assert.NotNull(newKanji);
            Assert.Equal(kanjiId, newKanji.Id);
            Assert.Equal(kanji.Character, newKanji.Character);
        }

        [Fact]
        public void Kanji_is_retrieved_by_specification()
        {
            var kanjiId = 25;
            var kanji = _kanjiBuilder.WithTestValues().WithId(kanjiId).Build();
            _repository.Add(kanji);
            _repository.Save();

            _appDbContext.Entry(kanji).State = EntityState.Detached;

            var newKanji = _repository.Get(new KanjiWithReadingsSpecification(kanjiId));

            Assert.NotNull(newKanji);
            Assert.Equal(kanjiId, newKanji.Id);
            Assert.Equal(kanji.Character, newKanji.Character);
            Assert.NotNull(newKanji.Readings);
            Assert.Equal(kanji.Readings[0].TypeId, newKanji.Readings[0].TypeId);
            Assert.Equal(kanji.Readings[0].Label, newKanji.Readings[0].Label);
            Assert.NotNull(newKanji.Readings[0].Type);
        }
        
        [Fact]
        public void Kanji_list_is_retrieved_from_database()
        {
            var kanji = _kanjiBuilder.WithTestValues().Build();
            var kanji2 = _kanjiBuilder.WithCharacter("B").Build();
            _repository.Add(kanji);
            _repository.Add(kanji2);
            _repository.Save();

            _appDbContext.Entry(kanji).State = EntityState.Detached;
            _appDbContext.Entry(kanji2).State = EntityState.Detached;

            var kanjiList = _repository.GetList();

            Assert.NotNull(kanjiList);
            Assert.Equal(2, kanjiList.Count);
            Assert.Contains(kanjiList, k => k.Character == kanji.Character);
            Assert.Contains(kanjiList, k => k.Character == kanji2.Character);
            Assert.True(kanjiList.All(k => k.Id > 0));
        }
        
        [Fact]
        public void Kanji_list_with_reading_type_is_retrieved_from_database()
        {
            var kanji = _kanjiBuilder.WithTestValues().Build();
            var reading = new Reading() { TypeId = 3, Label = "test" };
            var kanji2 = _kanjiBuilder.WithCharacter("B").WithReading(reading).Build();
            _repository.Add(kanji);
            _repository.Add(kanji2);
            _repository.Save();

            _appDbContext.Entry(kanji).State = EntityState.Detached;
            _appDbContext.Entry(kanji2).State = EntityState.Detached;

            var kanjiList = _repository.GetList(new KanjiWithReadingsSpecification());

            Assert.NotNull(kanjiList);
            
            Assert.Contains(kanjiList,
                k => k.Character == kanji.Character &&
                k.Readings[0].TypeId == kanji.Readings[0].TypeId &&
                k.Readings[0].Label == kanji.Readings[0].Label
            );
            
            Assert.Contains(kanjiList,
                k => k.Character == kanji2.Character &&
                k.Readings[0].TypeId == kanji2.Readings[0].TypeId &&
                k.Readings[0].Label == kanji2.Readings[0].Label
            );

            Assert.Equal(2, kanjiList.Count);
            Assert.True(kanjiList[0].Readings.All(k => k.Type.Id == k.TypeId));
            Assert.True(kanjiList[1].Readings.All(k => k.Type.Id == k.TypeId));
            Assert.True(kanjiList.All(k => k.Id > 0));
        }
    }
}

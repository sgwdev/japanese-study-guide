using Core.Entities.VocabularyAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using Core;
using Infrastructure.Data.Specifications;
using System.Linq;
using System;
using Web.Models;

namespace Web.Pages.Vocabulary.Tests
{
    public class QuickTestModel : PageModel
    {
        // Kanji -> Hiragana

        [BindProperty]
        public List<WordToFill> WordsToFillInHiragana { get; set; }

        [BindProperty]
        public List<string> UserHiraganaAnswers { get; set; }

        public List<string> KanjiToHiraganaFullAnswers { get; set; }

        // Hiragana -> Kanji

        [BindProperty]
        public List<WordToFill> WordsToFillInKanji { get; set; }

        [BindProperty]
        public List<string> UserKanjiAnswers { get; set; }

        public List<string> HiraganaToKanjiFullAnswers { get; set; }


        private IRepository<Word> _repository { get; set; }
        private int _wordsPerExercice { get; set; }

        public QuickTestModel(IRepository<Word> wordRepository)
        {
            this._repository = wordRepository;
            this._wordsPerExercice = 5;

            WordsToFillInHiragana = new List<WordToFill>();
            WordsToFillInKanji = new List<WordToFill>();
        }

        public void OnGet()
        {
            List<Word> words = _repository.GetList(new RandomWordWithReadingsSpecification(_wordsPerExercice*2));
            WordsToFillInHiragana = GetKanjiToHiraganaExercise(words.Take(_wordsPerExercice).ToList());
            WordsToFillInKanji = GetHiraganaToKanjiExercise(words.Skip(_wordsPerExercice).ToList());
        }

        public IActionResult OnPost()
        {
            List<int> hiraganaWordsId = WordsToFillInHiragana.Select(w => w.WordId).ToList();
            List<int> hiraganaWordsSplitIndex = WordsToFillInHiragana.Select(w => w.SplitIndex).ToList();

            List<Word> hiraganaWords = _repository.GetList(new WordWithReadingsSpecification(hiraganaWordsId)).OrderBy(w => hiraganaWordsId.IndexOf(w.Id)).ToList();
            WordsToFillInHiragana = GetKanjiToHiraganaExercise(hiraganaWords, hiraganaWordsSplitIndex);

            List<int> kanjiWordsId = WordsToFillInKanji.Select(w => w.WordId).ToList();
            List<int> kanjiWordsSplitIndex = WordsToFillInKanji.Select(w => w.SplitIndex).ToList();

            List<Word> kanjiWords = _repository.GetList(new WordWithReadingsSpecification(kanjiWordsId)).OrderBy(w => kanjiWordsId.IndexOf(w.Id)).ToList();
            WordsToFillInKanji = GetHiraganaToKanjiExercise(kanjiWords, kanjiWordsSplitIndex);

            KanjiToHiraganaFullAnswers = hiraganaWords.Select(w => w.Pronunciation).ToList();
            HiraganaToKanjiFullAnswers = kanjiWords.Select(w => w.Label).ToList();

            return Page();
        }

        public List<WordToFill> GetKanjiToHiraganaExercise(List<Word> words, List<int> wordsSplitIndex = null, List<string> answers = null)
        {
            List<WordToFill> wordsToFill = new List<WordToFill>();
            WordToFill wordToFill;
            int i = 0;

            foreach (Word word in words)
            {
                if (word.Readings.First().Reading.TypeId == 3 || word.Label.Length == 1)
                {
                    string label = string.IsNullOrEmpty(word.Translation) ? word.Label : $"{word.Label} ({word.Translation})";
                    wordToFill = new WordToFill(word.Id, label, word.Pronunciation);
                }
                else
                {
                    Random rnd = new Random();
                    int index = (wordsSplitIndex != null && wordsSplitIndex.Count > 0) ? wordsSplitIndex[i] : rnd.Next(0, word.Label.Length);
                    string[] splittedWord = GetHiraganaParts(word, index);
                    string answer = splittedWord[2];

                    wordToFill = new WordToFill(word.Id, word.Label, index, splittedWord[0..2], answer);
                }

                wordsToFill.Add(wordToFill);
                i++;
            }

            return wordsToFill;
        }

        public List<WordToFill> GetHiraganaToKanjiExercise(List<Word> words, List<int> wordsSplitIndex = null)
        {
            List<WordToFill> wordsToFill = new List<WordToFill>();
            WordToFill wordToFill;
            int i = 0;
            
            foreach (Word word in words)
            {
                if (word.Readings.First().Reading.TypeId == 3 || word.Label.Length == 1)
                {
                    string label = string.IsNullOrEmpty(word.Translation) ? word.Pronunciation : $"{word.Pronunciation} ({word.Translation})";
                    wordToFill = new WordToFill(word.Id, label, word.Label);
                }
                else
                {
                    Random rnd = new Random();
                    int index = (wordsSplitIndex != null && wordsSplitIndex.Count > 0) ? wordsSplitIndex[i] : rnd.Next(0, word.Label.Length);
                    string[] splittedWord = GetKanjiParts(word, index);
                    string answer = splittedWord[2];

                    wordToFill = new WordToFill(word.Id, word.Pronunciation, index, splittedWord[0..2], answer);
                }
                wordsToFill.Add(wordToFill);
                i++;
            }

            return wordsToFill;
        }

        // Splitting for Kanji -> Hiragana
        public string[] GetHiraganaParts(Word word, int index)
        {
            if (word.Label[index] == Constants.Noma)
                index--;

            string[] parts = word.Readings.OrderBy(r => r.Order).Select(wr => wr.Reading.Label).ToArray();

            int leftLength = 0;
            for (int i = 0; i < index; i++)
            {
                leftLength += parts[i].Length;
            }

            string left = word.Pronunciation.Substring(0, leftLength);
            string right = word.Pronunciation.Substring(leftLength + parts[index].Length);
            string answer = word.Pronunciation.Substring(leftLength, parts[index].Length);
            return new string[] { left, right, answer };
        }

        // Splitting for Hiragana -> Kanji
        private string[] GetKanjiParts(Word word, int index)
        {
            if (word.Label[index] == Constants.Noma)
            {
                index--;
            }

            string left = word.Label.Substring(0, index);
            string right = word.Label.Substring(index + 1);
            string answer = word.Label[index].ToString();
            return new string[] { left, right, answer };
        }    
    }
}

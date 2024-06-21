namespace Web.Models
{
    public class WordToFill
    {
        public int WordId { get; set; }
        public string Label { get; }
        public int SplitIndex { get; set; }
        public string[] SplittedWord { get; set; }
        public string Answer { get; }

        public WordToFill()
        {

        }

        public WordToFill(int wordId, string label, int splitIndex, string[] splittedWord, string answer)
        {
            this.WordId = wordId;
            this.Label = label;
            this.SplitIndex = splitIndex;
            this.SplittedWord = splittedWord;
            this.Answer = answer;
        }

        public WordToFill(int wordId, string label, string answer)
        {
            this.WordId = wordId;
            this.Label = label;
            this.Answer = answer;
            this.SplittedWord = new string[2] { "", "" };
        }
    }
}

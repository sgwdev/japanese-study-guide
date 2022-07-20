using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public static class Tools
    {
        private static Dictionary<char, char> HiraganaToKatakana = new Dictionary<char, char>
        {
            { 'ぁ' , 'ァ' },
            { 'あ' , 'ア' },
            { 'ぃ' , 'ィ' },
            { 'い' , 'イ' },
            { 'ぅ' , 'ゥ' },
            { 'う' , 'ウ' },
            { 'ぇ' , 'ェ' },
            { 'え' , 'エ' },
            { 'ぉ' , 'ォ' },
            { 'お' , 'オ' },
            { 'か' , 'カ' },
            { 'が' , 'ガ' },
            { 'き' , 'キ' },
            { 'ぎ' , 'ギ' },
            { 'く' , 'ク' },
            { 'ぐ' , 'グ' },
            { 'け' , 'ケ' },
            { 'げ' , 'ゲ' },
            { 'こ' , 'コ' },
            { 'ご' , 'ゴ' },
            { 'さ' , 'サ' },
            { 'ざ' , 'ザ' },
            { 'し' , 'シ' },
            { 'じ' , 'ジ' },
            { 'す' , 'ス' },
            { 'ず' , 'ズ' },
            { 'せ' , 'セ' },
            { 'ぜ' , 'ゼ' },
            { 'そ' , 'ソ' },
            { 'ぞ' , 'ゾ' },
            { 'た' , 'タ' },
            { 'だ' , 'ダ' },
            { 'ち' , 'チ' },
            { 'ぢ' , 'ヂ' },
            { 'っ' , 'ッ' },
            { 'つ' , 'ツ' },
            { 'づ' , 'ヅ' },
            { 'て' , 'テ' },
            { 'で' , 'デ' },
            { 'と' , 'ト' },
            { 'ど' , 'ド' },
            { 'な' , 'ナ' },
            { 'に' , 'ニ' },
            { 'ぬ' , 'ヌ' },
            { 'ね' , 'ネ' },
            { 'の' , 'ノ' },
            { 'は' , 'ハ' },
            { 'ば' , 'バ' },
            { 'ぱ' , 'パ' },
            { 'ひ' , 'ヒ' },
            { 'び' , 'ビ' },
            { 'ぴ' , 'ピ' },
            { 'ふ' , 'フ' },
            { 'ぶ' , 'ブ' },
            { 'ぷ' , 'プ' },
            { 'へ' , 'ヘ' },
            { 'べ' , 'ベ' },
            { 'ぺ' , 'ペ' },
            { 'ほ' , 'ホ' },
            { 'ぼ' , 'ボ' },
            { 'ぽ' , 'ポ' },
            { 'ま' , 'マ' },
            { 'み' , 'ミ' },
            { 'む' , 'ム' },
            { 'め' , 'メ' },
            { 'も' , 'モ' },
            { 'ゃ' , 'ャ' },
            { 'や' , 'ヤ' },
            { 'ゅ' , 'ュ' },
            { 'ゆ' , 'ユ' },
            { 'ょ' , 'ョ' },
            { 'よ' , 'ヨ' },
            { 'ら' , 'ラ' },
            { 'り' , 'リ' },
            { 'る' , 'ル' },
            { 'れ' , 'レ' },
            { 'ろ' , 'ロ' },
            { 'ゎ' , 'ヮ' },
            { 'わ' , 'ワ' },
            { 'ゐ' , 'ヰ' },
            { 'ゑ' , 'ヱ' },
            { 'を' , 'ヲ' },
            { 'ん' , 'ン' },
            { 'ゔ' , 'ヴ' },
            { 'ゕ' , 'ヵ' },
            { 'ゖ' , 'ヶ' }
        };

        public static char ToKatakana(char c)
        {
            if (HiraganaToKatakana.ContainsKey(c))
                return HiraganaToKatakana[c];
            else
                return '?';
        }

        public static string ToKatakana(string s)
        {
            StringBuilder sb = new StringBuilder(s);
            for (int i = 0; i < s.Length; i++)
            {
                sb[i] = ToKatakana(sb[i]);
            }
            return sb.ToString();
        }

        public static bool GetCharsInRange(string text, int min, int max)
        {
            return text.All(e => e >= min && e <= max);
        }

        public static bool IsInHiragana(string text)
        {
            return GetCharsInRange(text, 0x3041, 0x3096);
        }
    }
}

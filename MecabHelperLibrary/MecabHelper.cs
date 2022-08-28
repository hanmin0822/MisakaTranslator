using MeCab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MecabHelperLibrary
{
    public struct MecabWordInfo {

        /// <summary>
        /// 单词
        /// </summary>
        public string Word;

        /// <summary>
        /// 词性
        /// </summary>
        public string PartOfSpeech;

        /// <summary>
        /// 词性说明
        /// </summary>
        public string Description;

        /// <summary>
        /// 片假名
        /// </summary>
        public string Kana;

        /// <summary>
        /// 平假名
        /// </summary>
        public string HiraKana;

        /// <summary>
        /// 罗马音
        /// </summary>
        public string Romaji;

        /// <summary>
        /// Mecab能提供的关于这个词的详细信息 CSV表示
        /// </summary>
        public string Feature;
    }


    public class MecabHelper:IDisposable
    {
        private MeCabParam Parameter;
        private MeCabTagger Tagger;

        public MecabHelper() {
            Parameter = new MeCabParam();
            //Parameter.UserDic.Append("");
            Tagger = MeCabTagger.Create(Parameter);
        }

        public void Dispose()
        {
            Tagger.Dispose();
        }




        /// <summary>
        /// 处理句子，对句子进行分词，得到结果
        /// </summary>
        /// <param name="sentence"></param>
        /// <returns></returns>
        public List<MecabWordInfo> SentenceHandle(string sentence) {

            List<MecabWordInfo> ret = new List<MecabWordInfo>();

            foreach (var node in Tagger.ParseToNodes(sentence))
            {
                if (node.CharType > 0)
                {
                    var features = node.Feature.Split(',');

                    
                    MecabWordInfo mwi = new MecabWordInfo {
                        Word = node.Surface,
                        PartOfSpeech = features[0],
                        Description = features[1],
                        Feature =  node.Feature
                    };

                    //加这一步是为了防止乱码进入分词导致无法读取假名
                    //以及助词一般不需要注音
                    if (features.Length >= 8 & mwi.PartOfSpeech != "助詞")
                    {
                        mwi.HiraKana = KataganaToHiragana(features[7]);
                        mwi.Romaji = HiraganaToAlphabet(mwi.HiraKana);
                        mwi.Kana = features[7];
                    }
                    
                    ret.Add(mwi);
                }
            }

            return ret;
        }

        static string KataganaToHiragana(string s)
        {
            StringBuilder sb = new StringBuilder();
            char[] target = s.ToCharArray();
            char c;
            for (int i = 0; i < target.Length; i++)
            {
                c = target[i];
                if (c >= 'ァ' && c <= 'ヴ')
                { // 筛选平假名范围内的字符
                    c = (char)(c - 0x0060);  // 平假名转换为片假名
                }
                sb.Append(c);
            }
            return sb.ToString();
        }

        static string HiraganaToAlphabet1(string s)
        {
            switch (s)
            {
                case "あ": return "a";
                case "い": return "i";
                case "う": return "u";
                case "え": return "e";
                case "お": return "o";
                case "か": return "ka";
                case "き": return "ki";
                case "く": return "ku";
                case "け": return "ke";
                case "こ": return "ko";
                case "さ": return "sa";
                case "し": return "shi";
                case "す": return "su";
                case "せ": return "se";
                case "そ": return "so";
                case "た": return "ta";
                case "ち": return "chi";
                case "つ": return "tsu";
                case "て": return "te";
                case "と": return "to";
                case "な": return "na";
                case "に": return "ni";
                case "ぬ": return "nu";
                case "ね": return "ne";
                case "の": return "no";
                case "は": return "ha";
                case "ひ": return "hi";
                case "ふ": return "hu";
                case "へ": return "he";
                case "ほ": return "ho";
                case "ま": return "ma";
                case "み": return "mi";
                case "む": return "mu";
                case "め": return "me";
                case "も": return "mo";
                case "や": return "ya";
                case "ゆ": return "yu";
                case "よ": return "yo";
                case "ら": return "ra";
                case "り": return "ri";
                case "る": return "ru";
                case "れ": return "re";
                case "ろ": return "ro";
                case "わ": return "wa";
                case "を": return "wo";
                case "ん": return "n";
                case "が": return "ga";
                case "ぎ": return "gi";
                case "ぐ": return "gu";
                case "げ": return "ge";
                case "ご": return "go";
                case "ざ": return "za";
                case "じ": return "ji";
                case "ず": return "zu";
                case "ぜ": return "ze";
                case "ぞ": return "zo";
                case "だ": return "da";
                case "ぢ": return "ji";
                case "づ": return "du";
                case "で": return "de";
                case "ど": return "do";
                case "ば": return "ba";
                case "び": return "bi";
                case "ぶ": return "bu";
                case "べ": return "be";
                case "ぼ": return "bo";
                case "ぱ": return "pa";
                case "ぴ": return "pi";
                case "ぷ": return "pu";
                case "ぺ": return "pe";
                case "ぽ": return "po";
                case "きゃ": return "kya";
                case "きぃ": return "kyi";
                case "きゅ": return "kyu";
                case "きぇ": return "kye";
                case "きょ": return "kyo";
                case "しゃ": return "sha";
                case "しぃ": return "syi";
                case "しゅ": return "shu";
                case "しぇ": return "she";
                case "しょ": return "sho";
                case "ちゃ": return "cha";
                case "ちぃ": return "cyi";
                case "ちゅ": return "chu";
                case "ちぇ": return "che";
                case "ちょ": return "cho";
                case "にゃ": return "nya";
                case "にぃ": return "nyi";
                case "にゅ": return "nyu";
                case "にぇ": return "nye";
                case "にょ": return "nyo";
                case "ひゃ": return "hya";
                case "ひぃ": return "hyi";
                case "ひゅ": return "hyu";
                case "ひぇ": return "hye";
                case "ひょ": return "hyo";
                case "みゃ": return "mya";
                case "みぃ": return "myi";
                case "みゅ": return "myu";
                case "みぇ": return "mye";
                case "みょ": return "myo";
                case "りゃ": return "rya";
                case "りぃ": return "ryi";
                case "りゅ": return "ryu";
                case "りぇ": return "rye";
                case "りょ": return "ryo";
                case "ぎゃ": return "gya";
                case "ぎぃ": return "gyi";
                case "ぎゅ": return "gyu";
                case "ぎぇ": return "gye";
                case "ぎょ": return "gyo";
                case "じゃ": return "ja";
                case "じぃ": return "ji";
                case "じゅ": return "ju";
                case "じぇ": return "je";
                case "じょ": return "jo";
                case "ぢゃ": return "dya";
                case "ぢぃ": return "dyi";
                case "ぢゅ": return "dyu";
                case "ぢぇ": return "dye";
                case "ぢょ": return "dyo";
                case "びゃ": return "bya";
                case "びぃ": return "byi";
                case "びゅ": return "byu";
                case "びぇ": return "bye";
                case "びょ": return "byo";
                case "ぴゃ": return "pya";
                case "ぴぃ": return "pyi";
                case "ぴゅ": return "pyu";
                case "ぴぇ": return "pye";
                case "ぴょ": return "pyo";
                case "ぐぁ": return "gwa";
                case "ぐぃ": return "gwi";
                case "ぐぅ": return "gwu";
                case "ぐぇ": return "gwe";
                case "ぐぉ": return "gwo";
                case "つぁ": return "tsa";
                case "つぃ": return "tsi";
                case "つぇ": return "tse";
                case "つぉ": return "tso";
                case "ふぁ": return "fa";
                case "ふぃ": return "fi";
                case "ふぇ": return "fe";
                case "ふぉ": return "fo";
                case "うぁ": return "wha";
                case "うぃ": return "whi";
                case "うぅ": return "whu";
                case "うぇ": return "whe";
                case "うぉ": return "who";
                case "ヴぁ": return "va";
                case "ヴぃ": return "vi";
                case "ヴ": return "vu";
                case "ヴぇ": return "ve";
                case "ヴぉ": return "vo";
                case "でゃ": return "dha";
                case "でぃ": return "dhi";
                case "でゅ": return "dhu";
                case "でぇ": return "dhe";
                case "でょ": return "dho";
                case "てゃ": return "tha";
                case "てぃ": return "thi";
                case "てゅ": return "thu";
                case "てぇ": return "the";
                case "てょ": return "tho";
                default: return "";
            }
        }

        static string HiraganaToAlphabet(string s1)
        {
            string s2 = "";
            for (int i = 0; i < s1.Length; i++)
            {
               
                if (i + 1 < s1.Length)
                {
                    // 有「っ」的情况下
                    if (s1.Substring(i, 1).CompareTo("っ") == 0)
                    {
                        s2 += HiraganaToAlphabet1(s1.Substring(i + 1, 1)).Substring(0, 1);
                        continue;
                    }


                    // 出现其他小假名的情况
                    string s3 = HiraganaToAlphabet1(s1.Substring(i, 2));
                    if (s3.CompareTo("*") != 0)
                    {
                        s2 += s3;
                        i++;
                        continue;
                    }
                }
                s2 += HiraganaToAlphabet1(s1.Substring(i, 1));
            }
            return s2;
        }
    }

}

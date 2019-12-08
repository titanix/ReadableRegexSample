using System;
using System.Collections.Generic;
using System.Linq;

namespace SyllabicZhuyinParser
{
    class SyllableParser
    {
        Dictionary<string, string> consonants = new Dictionary<string, string>()
        {
            ["ㄅ"] = "p",
            ["ㄆ"] = "pʰ",
            ["ㄇ"] = "m",
            ["ㄈ"] = "f",
            ["ㄉ"] = "t",
            ["ㄊ"] = "tʰ",
            ["ㄋ"] = "n",
            ["ㄌ"] = "l",
            ["ㄍ"] = "k",
            ["ㄎ"] = "kʰ",
            ["ㄏ"] = "x",
            ["ㄐ"] = "tɕ",  // 家 ts profonde, tç surface
            ["ㄑ"] = "tɕʰ", // 去 tsh prof, tçh surface
            ["ㄒ"] = "ɕ",   // 西 s prof, ç surface
            ["ㄓ"] = "ʈʂ",  // OK
            ["ㄔ"] = "ʈʂʰ", // 吃 OK
            ["ㄕ"] = "ʂ",   // ok
            ["ㄖ"] = "ʐ",   // choisir l'un duanmu: r // ʐ
            ["ㄗ"] = "ts",  // 坐 OK
            ["ㄘ"] = "tsʰ", // 村 OK
            ["ㄙ"] = "s",   // ok
        };

        // warning, those are also contained in 'consonants'
        Dictionary<string, string> syllabic_consonants = new Dictionary<string, string>()
        {
            ["ㄓ"] = "ʈʂ",
            ["ㄔ"] = "ʈʂʰ",
            ["ㄕ"] = "ʂ",
            ["ㄙ"] = "s",
            ["ㄖ"] = "ʐ",
        };

        Dictionary<string, string> medials = new Dictionary<string, string>()
        {
            ["ㄧ"] = "i", // j
            ["ㄨ"] = "u", // w
            ["ㄩ"] = "y", // ɥ
        };

        Dictionary<string, string> vowels = new Dictionary<string, string>()
        {
            ["ㄚ"] = "a",
            ["ㄛ"] = "o",
            ["ㄜ"] = "ɤ",
            ["ㄝ"] = "e",
        };

        Dictionary<string, string> rimes = new Dictionary<string, string>()
        {
            ["ㄞ"] = "ai",
            ["ㄟ"] = "ei",
            ["ㄠ"] = "au",
            ["ㄠ"] = "ou",
            ["ㄢ"] = "an", // peut changer en ɛn après ㄧ, ㄩ
            ["ㄣ"] = "ən",
            ["ㄤ"] = "aŋ",
            ["ㄥ"] = "əŋ", // à vérifer mais faire attention à fong
            ["ㄦ"] = "aɚ",
        };

        Dictionary<string, string> tones = new Dictionary<string, string>()
        {
            ["ˉ"] = "1",
            ["ˊ"] = "2",
            ["ˇ"] = "3",
            ["ˋ"] = "4",
            ["˙"] = "5"
        };
        
        private string SingleSyllableRegex()
        {
            IRegexBuilder regex = new RegexBuilder()
            .Char('˙').AtMost(1)
            .Disjunction(
                // voyelle seule, ex: 阿 ㄚ
                new RegexBuilder()
                    .StartGroup("vowel_only")
                        .Disjunction(vowels.KeysArray())
                    .CloseGroup(),

                // médiale seule, ex: 五 ㄨ
                new RegexBuilder()
                    .StartGroup("medial_only")
                        .Disjunction(medials.KeysArray())
                    .CloseGroup(),

                // finale seule, ex: 案 ㄢ
                new RegexBuilder()
                    .StartGroup("rime_only")
                        .Disjunction(rimes.KeysArray())
                    .CloseGroup(),

                // consonne syllabique seule (seulement ㄓ, ㄔ, ㄕ, ㄖ, ㄙ)
                new RegexBuilder()
                    .StartGroup("syllabic_consonant_only")
                        .Disjunction(syllabic_consonants.KeysArray())
                    .CloseGroup(),

                // médiale + rime, ex: 王 ㄨㄤ
                new RegexBuilder()
                    .StartGroup()
                        .Disjunction(medials.KeysArray())
                        .Disjunction(rimes.KeysArray())
                    .CloseGroup(),

                // consonne + voyelle, ex: 怕 ㄆㄚ
                new RegexBuilder()
                    .StartGroup("consonant_and_vowel")
                        .Disjunction(consonants.KeysArray())
                        .Disjunction(vowels.KeysArray())
                    .CloseGroup(),

                // consonne + médiale, ex: 不 ㄅㄨ
                new RegexBuilder()
                    .StartGroup("consonant_and_medial")
                        .Disjunction(consonants.KeysArray())
                        .Disjunction(medials.KeysArray())
                    .CloseGroup(),

                // consonne + rime
                new RegexBuilder()
                    .StartGroup("consonant_and_rime")
                        .Disjunction(consonants.KeysArray())
                        .Disjunction(rimes.KeysArray())
                    .CloseGroup(),
                
                // consonne + médiale + vowel, ex: 別 ㄅㄧㄝ
                new RegexBuilder()
                    .StartGroup("consonant_and_medial_and_vowel")
                        .Disjunction(consonants.KeysArray())
                        .Disjunction(medials.KeysArray())
                        .Disjunction(vowels.KeysArray())
                    .CloseGroup(),

                // consonne + médiale + rime, ex: 囧 ㄐㄩㄥ (jiong)
                new RegexBuilder()
                    .StartGroup("consonant_and_medial_and_rime")
                        .Disjunction(consonants.KeysArray())
                        .Disjunction(medials.KeysArray())
                        .Disjunction(rimes.KeysArray())
                    .CloseGroup()

            ).Disjunction(tones.KeysArray().Take(4).ToArray()).AtMost(1);

            return regex.Regex();
        }

        public string WordReadingWithSpacesRegex()
        {
            IRegexBuilder regex = new RegexBuilder()
                .Raw(SingleSyllableRegex())
                .StartGroup()
                    .Char('　') // chinese space
                    .Raw(SingleSyllableRegex())
                .CloseGroup()
                .KleenStar();
            
            return $"^{regex.Regex()}$";
        }
    }
}

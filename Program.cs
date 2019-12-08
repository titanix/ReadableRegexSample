using System;

namespace SyllabicZhuyinParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Example_1());

            Console.WriteLine();

            SyllableParser sp = new SyllableParser();
            Console.WriteLine(sp.WordReadingWithSpacesRegex());
        }

        static string Example_1()
        {
            var letters = new string[] { "a", "b", "c" };

            IRegexBuilder regex = new RegexBuilder()
                .StartGroup("first_letter") // on crée un group de capture nommé
                    .Disjunction(letters)
                    .AtLeast(1)
                .CloseGroup();

            return regex.Regex();
        }
    }
}

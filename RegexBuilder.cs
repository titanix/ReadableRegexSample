using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SyllabicZhuyinParser
{
    class RegexBuilder : IRegexBuilder
    {
        List<string> tokens = new List<string>();

        public IRegexBuilder AnyChar(int n = 1)
        {
            for (int i = 0; i < n; i++)
            {
                tokens.Add(".");
            }

            return this;
        }

        public IRegexBuilder Char(char c)
        {
            tokens.Add(c.ToString());

            return this;
        }

        public IRegexBuilder Raw(string s)
        {
            tokens.Add(s);

            return this;
        }

        public IRegexBuilder KleenStar()
        {
            return Raw("*");
        }

        public IRegexBuilder AtLeast(int n = 1)
        {
            if (n == 1)
            {
                tokens.Add("+");
            }
            else
            {
                tokens.Add($"{{{n},}}");
            }

            return this;
        }

        public IRegexBuilder AtMost(int n = 1)
        {
            if (n == 1)
            {
                tokens.Add("?");
            }
            else
            {
                tokens.Add($"{{0,{n}}}");
            }

            return this;
        }

        public IRegexBuilder Disjunction(params string[] ss)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("(?:");
            builder.Append(string.Join("|", ss));
            builder.Append(")");
            tokens.Add(builder.ToString());

            return this;
        }

        public IRegexBuilder Disjunction(params IRegexBuilder[] ts)
        {
            return  Disjunction(ts.Select(s => s.Regex()).ToArray());
        }

        public IRegexBuilder Exactly(int n)
        {
            tokens.Add($"{{{n}}}");

            return this;
        }

        public string Regex()
        {
            return String.Join("", tokens);
        }

        public IRegexBuilder StartGroup(string name = "")
        {
            if (String.IsNullOrEmpty(name))
            {
                tokens.Add("(");
            }
            else
            {
                tokens.Add($"(?<{name}>");
            }

            return this;
        }

        public IRegexBuilder CloseGroup()
        {
            tokens.Add(")");

            return this;
        }
    }
}


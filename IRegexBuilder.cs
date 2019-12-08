using System;
using System.Collections.Generic;

namespace SyllabicZhuyinParser
{
    interface IRegexBuilder
    {
        IRegexBuilder AtLeast(int n = 1);   // by default 1, which translate to +
        IRegexBuilder AtMost(int n = 1);    // by default 1, which translate to ?
        IRegexBuilder Exactly(int n);
        IRegexBuilder KleenStar();          // translate to *

        IRegexBuilder AnyChar(int n = 1);   // translate to .
        IRegexBuilder Char(char c);
        IRegexBuilder Raw(string s);

        IRegexBuilder StartGroup(string name = "");
        IRegexBuilder CloseGroup();

        IRegexBuilder Disjunction(params string[] ss);
        IRegexBuilder Disjunction(params IRegexBuilder[] ts);

        string Regex();
    }
}

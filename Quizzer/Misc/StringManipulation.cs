using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Diagnostics;
namespace Quizzer
{
    public static class StringFunctions
    {
        public static readonly string[] SuperscriptDigits =
        new string[]{"\u2070","\u00b9","\u00b2","\u00b3","\u2074","\u2075","\u2076","\u2077","\u2078","\u2079"};

        public static string NormalizeSuperScript(string word)
        {
            string stringFormKd = word.Normalize(NormalizationForm.FormKD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char character in stringFormKd)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(character);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(character);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormKC);
        }
    

        public static string ConvertToSuperscript(decimal value, bool units = false)
        {
            string sNumber = Math.Abs(value).ToString();
            string output = "";
            if (value < 0) { output += "⁻"; }
            if (units)
            {
                if (value == 1 || value == 0) { return ""; }
            }
            for (int i = 0; i < sNumber.Length; i++)
            {
                int val = Convert.ToInt32(sNumber[i].ToString());
                output += SuperscriptDigits[Math.Abs(val)];
            }
            return output;
        }
        public static readonly string[] SubscriptDigits = new string[] {"\u2080","\u2081","\u2082","\u2083","\u2084","\u2085","\u2086","\u2087","\u2088","\u2089"};
        public static string ConvertToSubscript(decimal value, bool units = false)
        {
            string sNumber = Math.Abs(value).ToString();
            string output = "";
            if (value < 0) { output += "⁻"; }
            if (units)
            {
                if (value == 1 || value == 0) { return ""; }
            }
            for (int i = 0; i < sNumber.Length; i++)
            {
                int val = Convert.ToInt32(sNumber[i].ToString());
                output += SubscriptDigits[Math.Abs(val)];
            }
            return output;
        }
        public static string AddSpaces(string sentence)
        {
            for(int i =1; i < sentence.Count();i++)
            {
                if(char.IsUpper(sentence[i]) && !char.IsWhiteSpace(sentence[i-1])){sentence = sentence.Insert(i," ");}
            }
            return sentence;
        }
        public static bool IsNumeric(string Text)
        {
            int test;
            return int.TryParse(Text, out test);
        }
    }
}

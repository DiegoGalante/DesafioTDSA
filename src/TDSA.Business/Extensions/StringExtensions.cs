using System.Globalization;
using System.Linq;
using System.Text;

namespace TDSA.Business.Extensions
{
    public static class StringExtensions
    {
        public static string FormataStringComLetrasMinusculas(this string valor)
        {
            return valor.Trim().ToLower();
        }

        public static string RemoveDiacritics(this string text)
        {
            return string.Concat(text.Normalize(NormalizationForm.FormD)
                                     .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark))
                         .Normalize(NormalizationForm.FormC);
        }
    }
}

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

        public static bool EhVazio(this string valor)
        {
            return string.IsNullOrEmpty(valor?.Trim());
        }
    }
}

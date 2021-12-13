using System.Globalization;
using System.Linq;

namespace MD2Word.Word.Extensions
{
    public static class FontStylesExt
    {
        public static int GetMaxLevel(this FontStyles fontStyle)
        {
            var memInfo = typeof(FontStyles).GetMember(fontStyle.ToString(CultureInfo.InvariantCulture));
            var description = ((NesstingStyleAttribute) memInfo[0].
                GetCustomAttributes(typeof(NesstingStyleAttribute), false).
                First()).MaxLevel;
            return description;
        }
    }
}
using System.Globalization;

namespace FileHelpers.NetCore.Converters
{
    public class TitleCaseConverter : ConverterBase
    {
        public override object StringToField(string from)
        {
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(from.ToLower());
        }

        public override string FieldToString(object from)
        {
            return (string)from;
        }
    }
}

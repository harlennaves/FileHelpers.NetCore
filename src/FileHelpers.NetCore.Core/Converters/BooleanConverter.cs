namespace FileHelpers.Core.Converters
{
    public class BooleanConverter : ConverterBase
    {
        public override object StringToField(string @from)
        {
            if (string.IsNullOrWhiteSpace(from))
                return false;

            string lowerFrom = from.ToLowerInvariant();

            switch (lowerFrom)
            {
                case "true":
                case "1":
                case "v":
                case "y":
                case "t":
                case "s":
                case "yes":
                case "sim":
                case "si":
                    return true;
                default:
                    return false;
            }
        }
    }
}

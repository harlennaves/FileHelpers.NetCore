using FileHelpers.Fluent.Exceptions;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace FileHelpers.Core.Converters
{
    public class BooleanConverter : ConverterBase
    {
        private string trueValue;
        private string falseValue;
        private bool useDefaults;

        public BooleanConverter() 
        {
            trueValue = "Y";
            falseValue = "N";
            useDefaults = true;
        }

        public BooleanConverter(string pattern) : this()
        {
            Discover(pattern);
        }

        public override object StringToField(string @from)
        {
            if (string.IsNullOrWhiteSpace(from))
                return false;

            if (!useDefaults)
                return trueValue.Equals(from);
            
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

        public override string FieldToString(object from)
        {
            return (bool)from ? trueValue : falseValue;
        }

        private void Discover(string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern))
                return;
            
            var matches = Regex.Matches(pattern, "^1:|0:");
            
            if (matches.Count == 0)
                return;

            if (matches.Count > 2 || (matches.Count == 2 && pattern.IndexOf(',') == -1))
                throw new BadFluentConfigurationException("Invalid pattern!");

            var splitted = pattern.Split(',', StringSplitOptions.RemoveEmptyEntries);

            foreach (Match match in matches)
            {
                var matchItem = splitted.FirstOrDefault(x => x.StartsWith(match.Value));
                if (string.IsNullOrWhiteSpace(matchItem))
                    continue;
                
                if (match.Value == "1:")
                { 
                    trueValue = matchItem.Replace("1:", string.Empty);
                    useDefaults = false;
                }
                else if (match.Value == "0:")
                {   
                    falseValue = matchItem.Replace("0:", string.Empty);
                    useDefaults = false;
                }
            }
        }
    }
}

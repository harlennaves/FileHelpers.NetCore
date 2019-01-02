namespace FileHelpers.NetCore.Converters
{
    public class BoolConverter : ConverterBase
    {
        private string _trueCondition { get; set; }
        private string _falseCondition { get; set; }

        public BoolConverter(string trueCondition)
        {
            _trueCondition = trueCondition;
        }

        public BoolConverter(string trueCondition, string falseCondition)
        {
            _trueCondition = trueCondition;
            _falseCondition = falseCondition;
        }

        public override object StringToField(string from)
        {
            return from.Equals(_trueCondition);
        }

        public override string FieldToString(object from)
        {
            return (bool)from ? _trueCondition : _falseCondition;
        }
    }
}
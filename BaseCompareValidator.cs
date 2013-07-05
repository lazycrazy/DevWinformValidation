using System.ComponentModel;
using System.Text.RegularExpressions;

namespace DevWinformValidation
{
    public abstract class BaseCompareValidator : BaseValidator
    {
        private readonly string[] _typeTable = new[] {"System.Decimal", 
            "System.DateTime",
            "System.Double",
            "System.Int32",
            "System.String"};

        public BaseCompareValidator()
        {
            Type = ValidationDataType.String;
        }

        [Category("Behavior"), Description("Sets or returns the data type that specifies how to interpret the values being compared."), DefaultValue(ValidationDataType.String)]
        public ValidationDataType Type { get; set; }

        protected TypeConverter TypeConverter
        {
            get { return TypeDescriptor.GetConverter(System.Type.GetType(_typeTable[(int)Type])); }
        }

        protected bool CanConvert(string value)
        {
            try
            {
                if (_typeTable != null)
                {
                    var converter = TypeDescriptor.GetConverter(System.Type.GetType(_typeTable[(int)Type]));
                    converter.ConvertFrom(value);
                }
                return true;
            }
            catch { return false; }
        }

        protected string Format(string value)
        {
            // If currency
            if (Type == ValidationDataType.Currency)
            {
                // Convert to decimal format ie remove currency formatting characters
                return Regex.Replace(value, "[$ .]", "");
            }
            return value;
        }
    }
}
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace DevWinformValidation
{
    [ToolboxBitmap(typeof(RangeValidator), "RangeValidator.ico")]
    public class RangeValidator : BaseCompareValidator
    {
        [Category("Behavior"), Description("Sets or returns the value of the control that you are validating, which must be greater than or equal to the value of this property. The default value is an empty string (\"\")."), DefaultValue("")]
        public string MinimumValue { get; set; }

        [Category("Behavior"), Description("Sets or returns the value of the control that you are validating, which must be less than or equal to the value of this property. The default value is an empty string (\"\")."), DefaultValue("")]
        public string MaximumValue { get; set; }

        protected override bool EvaluateIsValid()
        {
            // Don't validate if empty, unless required
            if (ControlToValidate.Text.Trim().Length == 0) return true;

            // Validate and convert Minimum
            if (MinimumValue.Trim().Length == 0) throw new Exception("MinimumValue must be provided.");
            string formattedMinimumValue = Format(MinimumValue.Trim());
            if (!CanConvert(formattedMinimumValue)) throw new Exception("MinimumValue cannot be converted to the specified Type.");
            object minimum = TypeConverter.ConvertFrom(formattedMinimumValue);

            // Validate and convert Maximum
            if (MaximumValue.Trim().Length == 0) throw new Exception("MaximumValue must be provided.");
            string formattedMaximumValue = Format(MaximumValue.Trim());
            if (!CanConvert(formattedMaximumValue)) throw new Exception("MaximumValue cannot be converted to the specified Type.");
            object maximum = TypeConverter.ConvertFrom(formattedMaximumValue);

            // Check minimum <= maximum
            if (Comparer.Default.Compare(minimum, maximum) > 0) throw new Exception("MinimumValue cannot be greater than MaximumValue.");

            // Check and convert ControlToValue
            string formattedValue = Format(ControlToValidate.Text.Trim());
            if (!CanConvert(formattedValue)) return false;
            object value = TypeConverter.ConvertFrom(formattedValue);

            // Validate value's range (minimum <= value <= maximum)
            return ((Comparer.Default.Compare(minimum, value) <= 0) &&
                    (Comparer.Default.Compare(value, maximum) <= 0));
        }
    }
}
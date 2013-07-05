using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DevWinformValidation
{
    [ToolboxBitmap(typeof(CompareValidator), "CompareValidator.ico")]
    public class CompareValidator : BaseCompareValidator
    {
        public CompareValidator()
        {
            ValueToCompare = string.Empty;
            Operator = ValidationCompareOperator.Equal;
            ControlToCompare = null;
        }

        [TypeConverter(typeof(ValidatableControlConverter)), Category("Behavior"), Description("Sets or returns the input control to compare with the input control being validated."), DefaultValue(null)]
        public Control ControlToCompare { get; set; }

        [Category("Behavior"), Description("Sets or returns the comparison operation to perform."), DefaultValue(null)]
        public ValidationCompareOperator Operator { get; set; }

        [Category("Behavior"), Description("Sets or returns a constant value to compare with the value entered by the user into the input control being validated."), DefaultValue("")]
        public string ValueToCompare { get; set; }

        protected override bool EvaluateIsValid()
        {
            // Don't validate if empty, unless required
            if (ControlToValidate.Text.Trim().Length == 0) return true;

            // Can't evaluate if missing ControlToCompare and ValueToCompare
            if ((ControlToCompare == null) && (string.IsNullOrWhiteSpace(ValueToCompare))) throw new Exception("The ControlToCompare property cannot be blank.");

            // Validate and convert CompareFrom
            string formattedCompareFrom = Format(ControlToValidate.Text.Trim());
            bool canConvertFrom = CanConvert(formattedCompareFrom);
            if (canConvertFrom)
            {
                if (Operator == ValidationCompareOperator.DataTypeCheck) return true;
            }
            else return false;
            var compareFrom = TypeConverter.ConvertFrom(formattedCompareFrom);

            // Validate and convert CompareTo
            string formattedCompareTo = Format(((ControlToCompare != null) ? ControlToCompare.Text : ValueToCompare));
            if (!CanConvert(formattedCompareTo)) throw new Exception("The value you are comparing to cannot be converted to the specified Type.");
            object compareTo = TypeConverter.ConvertFrom(formattedCompareTo);

            // Perform comparison eg ==, >, >=, <, <=, !=
            int result = Comparer.Default.Compare(compareFrom, compareTo);
            switch (Operator)
            {
                case ValidationCompareOperator.Equal:
                    return (result == 0);
                case ValidationCompareOperator.GreaterThan:
                    return (result > 0);
                case ValidationCompareOperator.GreaterThanEqual:
                    return (result >= 0);
                case ValidationCompareOperator.LessThan:
                    return (result < 0);
                case ValidationCompareOperator.LessThanEqual:
                    return (result <= 0);
                case ValidationCompareOperator.NotEqual:
                    return ((result != 0));
                default:
                    return false;
            }
        }
    }
}
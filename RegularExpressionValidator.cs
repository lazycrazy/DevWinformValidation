using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;

namespace DevWinformValidation
{
    [ToolboxBitmap(typeof(RegularExpressionValidator), "RegularExpressionValidator.ico")]
    public class RegularExpressionValidator : BaseValidator
    {
        [Category("Behavior"), Description("Sets or returns the regular expression that determines the pattern used to validate a field."), DefaultValue("")]
        public string ValidationExpression { get; set; }

        protected override bool EvaluateIsValid()
        {
            // Don't validate if empty
            if (ControlToValidate.Text.Length == 0) return true;
            // Successful if match matches the entire text of ControlToValidate
            string input = ControlToValidate.Text.Trim();
            return Regex.IsMatch(input, ValidationExpression.Trim());
        }
    }
}
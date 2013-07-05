using System.ComponentModel;
using System.Drawing;

namespace DevWinformValidation
{
    [ToolboxBitmap(typeof(RequiredFieldValidator), "RequiredFieldValidator.ico")]
    public class RequiredFieldValidator : BaseValidator
    {
        [Category("Behavior"), DefaultValue(null), Description("Sets or returns the base value for the validator. The default value is null.")]
        public string InitialValue { get; set; }

        protected override bool EvaluateIsValid()
        {
            //string controlValue = ControlToValidate.Text.Trim();
            object controlValue = null;
            System.Reflection.PropertyInfo propertyInfo = ControlToValidate.GetType().GetProperty("EditValue");
            if (propertyInfo != null)
                controlValue = propertyInfo.GetValue(ControlToValidate, null);
            if (controlValue == null || string.IsNullOrWhiteSpace(controlValue.ToString()))
                return false;
            if (controlValue.ToString() == "-1") return false;
            var initialValue = InitialValue ?? string.Empty;
            return (controlValue.ToString().Trim() != initialValue.Trim());
        }
    }
}
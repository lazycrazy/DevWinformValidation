using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DevWinformValidation
{
    [ToolboxBitmap(typeof(CustomValidator), "CustomValidator.ico")]
    [DefaultEvent("Validating")]
    public class CustomValidator : BaseValidator
    {
        public class ValidatingCancelEventArgs
        {
            public ValidatingCancelEventArgs(bool valid, Control controlToValidate)
            {
                Valid = valid;
                ControlToValidate = controlToValidate;
            }

            public bool Valid { get; set; }

            public Control ControlToValidate { get; set; }
        }

        [Category("Action")]
        [Description("Occurs when the CustomValidator validates the value of the ControlToValidate property.")]
        public event Action<object, ValidatingCancelEventArgs> Validating;
        public void OnValidating(ValidatingCancelEventArgs e)
        {
            if (Validating != null) Validating(this, e);
        }

        protected override bool EvaluateIsValid()
        {
            // Pass validation processing to event handler and wait for response
            var args = new ValidatingCancelEventArgs(false, ControlToValidate);
            OnValidating(args);
            return args.Valid;
        }
    }
}
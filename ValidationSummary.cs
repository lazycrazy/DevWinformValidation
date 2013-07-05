using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DevWinformValidation
{
    [ToolboxBitmap(typeof(ValidationSummary), "ValidationSummary.ico")]
    [ProvideProperty("DisplayMode", typeof(BaseContainerValidator))]
    public class ValidationSummary : BaseValidationSummary
    {

        private readonly IDictionary<BaseContainerValidator, ValidationSummaryDisplayMode> _displayModes = new Dictionary<BaseContainerValidator, ValidationSummaryDisplayMode>();

        [Category("Validation Summary")]
        [Description("Sets or returns how the validation summary will be displayed.")]
        [DefaultValue(ValidationSummaryDisplayMode.Simple)]
        public ValidationSummaryDisplayMode GetDisplayMode(BaseContainerValidator extendee)
        {
            return _displayModes.ContainsKey(extendee) ? _displayModes[extendee] : ValidationSummaryDisplayMode.Simple;
        }

        public void SetDisplayMode(BaseContainerValidator extendee, ValidationSummaryDisplayMode value)
        {
            _displayModes[extendee] = value;
        }

        protected override void Summarize(object sender, SummarizeEventArgs e)
        {
            if ((e.Validators == null) || (e.Validators.Count == 0)) return;

            var extendee = (BaseContainerValidator)sender;
            var displayMode = GetDisplayMode(extendee);

            var errorMessage = GetErrorMessage(extendee) ?? string.Empty;
            var errorCaption = GetErrorCaption(extendee) ?? string.Empty;

            // Build summary message body
            string errors = string.Empty;
            if (displayMode == ValidationSummaryDisplayMode.Simple)
            {
                // Build Simple message
                errors = errorMessage;
            }
            else
            {
                // Build List, BulletList or SingleParagraph 
                foreach (var validator in e.Validators.OrderBy(v => v.FlattenedTabIndex))
                {
                    BaseValidator current = validator;
                    if (!current.Valid)
                    {
                        switch (displayMode)
                        {
                            case ValidationSummaryDisplayMode.List:
                                errors += string.Format("{0}\n", current.ErrorMessage);
                                break;
                            case ValidationSummaryDisplayMode.BulletList:
                                errors += string.Format("- {0}\n", current.ErrorMessage);
                                break;
                            case ValidationSummaryDisplayMode.SingleParagraph:
                                errors += string.Format("{0}. ", current.ErrorMessage);
                                break;
                        }
                    }
                }
                // Prepend error message, if provided
                if (!(string.IsNullOrWhiteSpace(errors)) && !string.IsNullOrWhiteSpace(errorMessage))
                {
                    errors = string.Format("{0}\n\n{1}", errorMessage.Trim(), errors);
                }
            }

            // Display summary message
            MessageBox.Show(errors, errorCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
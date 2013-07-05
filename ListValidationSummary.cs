using System;
using System.Drawing;
using System.Linq;


namespace DevWinformValidation
{
    #region ListValidationSummary
    [ToolboxBitmap(typeof(ListValidationSummary), "ListValidationSummary.ico")]
    public class ListValidationSummary : BaseValidationSummary
    {

        ValidationSummaryForm _dlg;
        BaseContainerValidator _currentExtendee;

        protected override void Summarize(object sender, SummarizeEventArgs e)
        {
            // Close form if open and nothing invalid
            if (e.Validators.Count == 0)
            {
                if (_dlg != null)
                {
                    _dlg.Close();
                    _dlg = null;
                    _currentExtendee = null;
                }
                return;
            }

            var extendee = (BaseContainerValidator)sender;

            // If the ValidationSummaryForm is open, but refers to a different extendee
            // (BaseContainerValidator), get rid of it
            if ((_dlg != null) && (_currentExtendee != null) && (extendee != _currentExtendee))
            {
                _dlg.Close();
                _dlg = null;
                _currentExtendee = extendee;
            }

            // Open ValidationSummaryForm if it hasn't been opened,
            // or has been closed since Summarize was last called
            if (_dlg == null)
            {
                _dlg = new ValidationSummaryForm
                    {
                        ErrorCaption = GetErrorCaption(extendee),
                        ErrorMessage = GetErrorMessage(extendee),
                        Owner = extendee.HostingForm
                    };

                // Register Disposed to handle clean up when user closes form
                _dlg.Disposed += ValidationSummaryForm_Disposed;
            }

            // Get complete set of Validators under the jurisdiction
            // of the BaseContainerValidator
            _dlg.LoadValidators(extendee.GetValidators().OrderBy(v => v.FlattenedTabIndex).ToList());

            // Show dialog if not already visible
            if (!_dlg.Visible) _dlg.Show();
        }

        private void ValidationSummaryForm_Disposed(object sender, EventArgs e)
        {
            // Clean up if user closes form
            _dlg.Disposed -= ValidationSummaryForm_Disposed;
            _dlg = null;
        }
    }
    #endregion
}
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DevWinformValidation
{
    [ToolboxBitmap(typeof(FormValidator), "FormValidator.ico")]
    public class FormValidator : BaseContainerValidator, ISupportInitialize
    {
        public FormValidator()
        {
            ValidateOnAccept = true;
        }

        #region ISupportInitialize

        public void BeginInit() { }

        public void EndInit()
        {
            // Handle AcceptButton click if requested
            if ((HostingForm != null) && ValidateOnAccept)
            {
                var btn = HostingForm.AcceptButton as Button;

                if (btn != null)
                    btn.Click += AcceptButton_Click;

                var sb = HostingForm.AcceptButton as DevExpress.XtraEditors.SimpleButton;
                if (sb != null)
                    sb.Click += AcceptButton_Click;
            }
        }

        #endregion

        [Category("Behavior"), Description("If the host form's AcceptButton property is set, automatically validate when it is clicked. The AcceptButton's DialogResult property must be set to 'OK'."), DefaultValue(true)]
        public bool ValidateOnAccept { get; set; }

        public override ICollection<BaseValidator> GetValidators()
        {
            return ValidatorManager.GetValidators(HostingForm);
        }

        private void AcceptButton_Click(object sender, System.EventArgs e)
        {
            // If DialogResult is OK, that means we need to return None
            if (HostingForm.DialogResult == DialogResult.OK)
            {
                //Validate();
                if (!Valid)
                {
                    HostingForm.DialogResult = DialogResult.None;
                }
            }
        }
    }
}
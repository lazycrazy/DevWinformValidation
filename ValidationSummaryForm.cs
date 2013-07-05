using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace DevWinformValidation
{
    /// <summary>
    /// Summary description for ValidationSummaryForm.
    /// </summary>
    public class ValidationSummaryForm : Form
    {
        private Button _closeButton;
        private ListBox _validationErrorsList;
        private Label _lblErrorMessage;
        private Label _label1;
        private readonly IContainer components;

        public ValidationSummaryForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._closeButton = new System.Windows.Forms.Button();
            this._validationErrorsList = new System.Windows.Forms.ListBox();
            this._lblErrorMessage = new System.Windows.Forms.Label();
            this._label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            this._closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._closeButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this._closeButton.Location = new System.Drawing.Point(238, 176);
            this._closeButton.Name = "_closeButton";
            this._closeButton.Size = new System.Drawing.Size(90, 24);
            this._closeButton.TabIndex = 0;
            this._closeButton.Text = "关闭(&C)";
            this._closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // validationErrorsList
            // 
            this._validationErrorsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._validationErrorsList.IntegralHeight = false;
            this._validationErrorsList.ItemHeight = 12;
            this._validationErrorsList.Location = new System.Drawing.Point(10, 25);
            this._validationErrorsList.Name = "_validationErrorsList";
            this._validationErrorsList.Size = new System.Drawing.Size(318, 148);
            this._validationErrorsList.TabIndex = 1;
            this._validationErrorsList.DoubleClick += new System.EventHandler(this.validationErrorsList_DoubleClick);
            // 
            // lblErrorMessage
            // 
            this._lblErrorMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._lblErrorMessage.Location = new System.Drawing.Point(10, 9);
            this._lblErrorMessage.Name = "_lblErrorMessage";
            this._lblErrorMessage.Size = new System.Drawing.Size(315, 13);
            this._lblErrorMessage.TabIndex = 2;
            this._lblErrorMessage.Text = "[ErrorMessage Goes Here]";
            // 
            // label1
            // 
            this._label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this._label1.Location = new System.Drawing.Point(8, 180);
            this._label1.Name = "_label1";
            this._label1.Size = new System.Drawing.Size(192, 20);
            this._label1.TabIndex = 3;
            this._label1.Text = "双击错误信息到相关的编辑控件";
            // 
            // ValidationSummaryForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.CancelButton = this._closeButton;
            this.ClientSize = new System.Drawing.Size(337, 205);
            this.Controls.Add(this._label1);
            this.Controls.Add(this._validationErrorsList);
            this.Controls.Add(this._lblErrorMessage);
            this.Controls.Add(this._closeButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ValidationSummaryForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "[ErrorCaption Goes Here]";
            this.Load += new System.EventHandler(this.ValidationSummaryForm_Load);
            this.ResumeLayout(false);

        }
        #endregion

        public string ErrorCaption
        {
            get { return Text; }
            set { Text = value; }
        }

        public string ErrorMessage
        {
            get { return _lblErrorMessage.Text; }
            set { _lblErrorMessage.Text = value; }
        }

        public void LoadValidators(ICollection validators)
        {

            // Note: validators should contain all validators under
            // the jurisdiction of the BaseContainerValidator so
            // we can handle both where a control becomes valid and
            // where a control becomes invalid. If we only get 
            // the invalid controls, we can't display controls that were
            // valid at the time but become invalid

            // If list currently has items then deregister the 
            // Validate event handler

            foreach (BaseValidator validator in _validationErrorsList.Items)
            {
                validator.Validated -= BaseValidator_Validated;
            }

            // Clear the list
            _validationErrorsList.Items.Clear();

            // Add new validators and register the Validate 
            // event handler
            foreach (BaseValidator validator in validators)
            {
                if (!validator.Valid)
                {
                    _validationErrorsList.Items.Add(validator);
                }
                validator.Validated += BaseValidator_Validated;
            }
        }

        private void validationErrorsList_DoubleClick(object sender, EventArgs e)
        {
            // Set focus on the selected BaseValidator's ControlToValidate ie
            // in the owner form
            if (_validationErrorsList.Items.Count == 0) return;
            var selected = _validationErrorsList.SelectedItem as BaseValidator ??
                         _validationErrorsList.Items[0] as BaseValidator;
            if (selected != null && !selected.ControlToValidate.Focused)
                FocusHelper.FocusFirstErrorEditor(selected.ControlToValidate);
        }

        public void BaseValidator_Validated(object sender, EventArgs e)
        {
            var validator = sender as BaseValidator;
            if (validator == null || validator.ControlToValidate == null) return;
            // If validator is valid, remove from list
            if (validator.Valid)
            {
                _validationErrorsList.Items.Remove(validator);
                if (_validationErrorsList.Items.Count == 0)
                    this.Close();
            }
            else
            {
                var removeObjs = _validationErrorsList.Items.OfType<BaseValidator>().Where(i => (i != validator && i.ControlToValidate == validator.ControlToValidate));
                foreach (var obj in removeObjs)
                    _validationErrorsList.Items.Remove(obj);

                if (!_validationErrorsList.Items.Contains(validator))
                {
                    decimal tabIndex = validator.FlattenedTabIndex;
                    for (int i = 0; i < _validationErrorsList.Items.Count; i++)
                    {
                        var currentValidator = (BaseValidator)_validationErrorsList.Items[i];
                        if (tabIndex < currentValidator.FlattenedTabIndex)
                        {
                            _validationErrorsList.Items.Insert(i, validator);
                            return;
                        }
                    }
                    _validationErrorsList.Items.Add(validator);
                }
            }

        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ValidationSummaryForm_Load(object sender, EventArgs e)
        {
            // Show form to the right of the owner form
            if (Owner != null)
            {
                //this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;               
                const int padding = 10;
                if (Owner.MdiParent != null)
                {
                    Top = Owner.MdiParent.Bottom - Height - padding;// -25;
                    Left = Owner.MdiParent.Right - Width - padding;
                }
                else
                {
                    Top = Owner.Bottom - Height - padding;
                    Left = Owner.Right - Width - padding;
                }
                TopMost = true;
            }
        }
    }
}

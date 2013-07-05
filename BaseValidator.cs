using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DevWinformValidation
{
    public abstract class BaseValidator : Component, ISupportInitialize
    {

        private Control _controlToValidate;
        //private ErrorProvider _errorProvider = new ErrorProvider();
        private string _flattenedTabIndex;

        protected BaseValidator()
        {
            ValidateOnLoad = false;
            Icon = new Icon(typeof(ErrorProvider), "Error.ico");
            ErrorMessage = string.Empty;
            Valid = false;
        }

        #region ISupportInitialize

        public void BeginInit() { }
        public void EndInit()
        {
            // Hook up ControlToValidate's parent form's Load and Closed events 
            // to register and unregister with the ValidationManager
            // ONLY if _controlToValidate exists at run-time and has a parent form
            // ie has been added to a Form's Controls collection
            // NOTE: if there is no form, we don't add this instance to the ValidatorManager
            // so it is not available for form-wide validation which makes sense
            // since there is no form and therefore no form scope.

            if (DesignMode) return;

            Control topMostParent = _controlToValidate;
            while (topMostParent.Parent != null)
            {
                topMostParent = topMostParent.Parent;
            }
            var form = topMostParent as Form;
            if (form != null)
            {
                form.Load += Host_Load;
                return;
            }
            var parent = topMostParent as UserControl;
            if (parent != null)
            {
                parent.Load += Host_Load;
            }
            //      if( _controlToValidate.Parent is UserControl ) {
            //        UserControl userControl = _controlToValidate.Parent as UserControl;
            //        userControl.Load += new EventHandler(Form_Load);
            //        userControl.Disposed += new EventHandler(Form_Closed);
            //        return;
            //      }
            //
            //      Form host = _controlToValidate.FindForm();
            //      if( (_controlToValidate != null) && (!DesignMode) && (host != null) ) {
            //        host.Load += new EventHandler(Form_Load);
            //        host.Closed += new EventHandler(Form_Closed);
            //      }
        }

        #endregion

        [Category("Appearance"), Description("Sets or returns the text for the error message."), DefaultValue("")]
        public string ErrorMessage { get; set; }

        [Category("Appearance"), Description("Sets or returns the Icon to display ErrorMessage.")]
        public Icon Icon { get; set; }

        [Category("Behavior")]
        [Description("Sets or returns the input control to validate.")]
        [DefaultValue(null)]
        [TypeConverter(typeof(ValidatableControlConverter))]
        public Control ControlToValidate
        {
            get { return _controlToValidate; }
            set
            {
                _controlToValidate = value;

                // Hook up ControlToValidate’s Validating event at run-time ie not from VS.NET
                if ((_controlToValidate != null) && (!DesignMode))
                {
                    _controlToValidate.Validating += ControlToValidate_Validating;
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Valid { get; set; }

        [Category("Behavior"), Description("Sets or returns whether this validator will validate itself when its host form loads."), DefaultValue(false)]
        public bool ValidateOnLoad { get; set; }

        public void Validate(Form hostingForm)
        {
            foreach (BaseValidator validator in ValidatorManager.GetValidators(hostingForm))
                if (ControlToValidate == validator.ControlToValidate)
                    validator.Valid = true;
            foreach (BaseValidator validator in ValidatorManager.GetValidators(hostingForm))
            {
                if (ControlToValidate == validator.ControlToValidate)
                {
                    // Validate control
                    validator.Validate();

                    // Set focus on the control it its invalid and the earliest invalid
                    // control in the tab order
                    if (!validator.Valid)
                    {
                        //validator.ControlToValidate.Focus();
                        break;
                    }
                }
            }
        }

        public void Validate()
        {

            // Validate control
            Valid = EvaluateIsValid();

            // Display an error if ControlToValidate is invalid
            string errorMessage = string.Empty;
            if (!Valid)
            {
                errorMessage = ErrorMessage;
                //_errorProvider.Icon = _icon;
            }
            //_errorProvider.SetError(_controlToValidate, errorMessage);
            //(_controlToValidate as DevExpress.XtraEditors.BaseEdit).ErrorText = errorMessage;
            System.Reflection.PropertyInfo propertyInfo = _controlToValidate.GetType().GetProperty("ErrorText");
            if (propertyInfo != null)
                propertyInfo.SetValue(_controlToValidate, errorMessage, null);

            OnValidated(new EventArgs());
        }

        public override string ToString()
        {
            return ErrorMessage;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public decimal FlattenedTabIndex
        {
            get
            {
                // Generate unique tab index and store it if 
                // not already generated
                if (_flattenedTabIndex == null)
                {
                    var sb = new StringBuilder();
                    Control current = _controlToValidate;
                    while (current != null)
                    {
                        string tabIndex = current.TabIndex.ToString();
                        sb.Insert(0, tabIndex);
                        current = current.Parent;
                    }
                    sb.Insert(0, "0.");
                    _flattenedTabIndex = sb.ToString();
                }
                // Return unique tab index
                return decimal.Parse(_flattenedTabIndex);
            }
        }

        public event EventHandler Validated;
        protected void OnValidated(EventArgs e)
        {
            if (Validated != null)
            {
                Validated(this, e);
            }
        }

        protected abstract bool EvaluateIsValid();

        private void ControlToValidate_Validating(object sender, CancelEventArgs e)
        {
            // We don't cancel if invalid since we don't want to force
            // the focus to remain on ControlToValidate if invalid
            var control = sender as Control;
            if (control != null)
            {
                Form form = control.FindForm();
                Validate(form);
            }
            //if (!Valid) e.Cancel = true;
            //Validate();
        }

        private void Host_Load(object sender, EventArgs e)
        {
            // Register with ValidatorManager
            Form hostingForm = ((Control)sender).FindForm();
            ValidatorManager.Register(this, hostingForm);
            if (ValidateOnLoad) Validate();
            if (hostingForm != null) hostingForm.Closed += HostingForm_Closed;
        }
        private void HostingForm_Closed(object sender, EventArgs e)
        {
            // DeRegister from ValidatorCollection
            ValidatorManager.DeRegister(this, (Form)sender);
        }
    }


}
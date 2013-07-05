using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Windows.Forms;

namespace DevWinformValidation
{
    public abstract class BaseContainerValidator : Component
    {

        private Form _hostingForm;

        [Browsable(false)]
        [DefaultValue(null)]
        public Form HostingForm
        {
            get
            {
                if ((_hostingForm == null) && DesignMode)
                {
                    // See if we're being hosted in VS.NET (or something similar)
                    // Cheers Ian Griffiths/Chris Sells for this code
                    var designer = GetService(typeof(IDesignerHost)) as IDesignerHost;
                    if (designer != null) _hostingForm = designer.RootComponent as Form;
                }
                return _hostingForm;
            }
            set
            {
                if (!DesignMode)
                {
                    // Only allow this property to be set if:
                    //    a) it is being set for the first time
                    //    b) it is the same Form as the original form
                    if ((_hostingForm != null) && (_hostingForm != value))
                    {
                        throw new Exception("Can't change HostingForm at runtime.");
                    }
                    _hostingForm = value;
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Valid
        {
            get
            {
                Validate();
                return GetValidators().All(validator => validator.Valid);
            }
        }



        private void Validate()
        {
            // Validate
            BaseValidator firstInTabOrder = null;
            foreach (BaseValidator validator in GetValidators())
                validator.Valid = true;
            foreach (BaseValidator validator in GetValidators())
            {

                // Validate control
                validator.Validate(HostingForm);

                // Set focus on the control it its invalid and the earliest invalid
                // control in the tab order
                if (!validator.Valid)
                {
                    if ((firstInTabOrder == null) ||
                        (firstInTabOrder.FlattenedTabIndex > validator.FlattenedTabIndex))
                    {
                        firstInTabOrder = validator;
                    }
                }
            }



            OnSummarize(new SummarizeEventArgs(GetValidators(), HostingForm));

            // Select first invalid control in tab order, if any
            if (firstInTabOrder != null)
                FocusHelper.FocusFirstErrorEditor(firstInTabOrder.ControlToValidate);
        }





        public abstract ICollection<BaseValidator> GetValidators();

        public event Action<object, SummarizeEventArgs> Summarize;
        protected void OnSummarize(SummarizeEventArgs e)
        {
            if (Summarize != null)
            {
                Summarize(this, e);
            }
        }

    }
}
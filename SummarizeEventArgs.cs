using System.Collections.Generic;
using System.Windows.Forms;

namespace DevWinformValidation
{
    public class SummarizeEventArgs
    {
        public ICollection<BaseValidator> Validators;
        public Form HostingForm;
        public SummarizeEventArgs(ICollection<BaseValidator> validators, Form hostingForm)
        {
            Validators = validators;
            HostingForm = hostingForm;
        }
    }
}
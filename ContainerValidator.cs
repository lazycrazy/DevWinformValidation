using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DevWinformValidation
{
    [ToolboxBitmap(typeof(ContainerValidator), "ContainerValidator.ico")]
    public class ContainerValidator : BaseContainerValidator
    {
        public ContainerValidator()
        {
            ValidationDepth = ValidationDepth.All;
            ContainerToValidate = null;
        }

        [Category("Behavior"), Description("Sets or returns the input control to validate."), DefaultValue(null), TypeConverter(typeof(ContainerControlConverter))]
        public Control ContainerToValidate { get; set; }

        [Category("Behavior"), DefaultValue(ValidationDepth.All), Description(@"Sets or returns the level of validation applied to ContainerToValidate using the ValidationDepth enumeration.")]
        public ValidationDepth ValidationDepth { get; set; }

        public override ICollection<BaseValidator> GetValidators()
        {
            return ValidatorManager.GetValidators(HostingForm, ContainerToValidate, ValidationDepth);
        }
    }
}
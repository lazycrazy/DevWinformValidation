using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DevWinformValidation
{
    public class ValidatorManager
    {
        private static readonly IDictionary<Form, ICollection<BaseValidator>> FormValidators = new Dictionary<Form, ICollection<BaseValidator>>();

        public static void Register(BaseValidator validator, Form hostingForm)
        {
            if (!FormValidators.ContainsKey(hostingForm))
                FormValidators[hostingForm] = new List<BaseValidator>();
            FormValidators[hostingForm].Add(validator);
        }

        public static ICollection<BaseValidator> GetValidators(Form hostingForm, Control container = null, ValidationDepth validationDepth = ValidationDepth.All)
        {
            if (container == null)
                return FormValidators[hostingForm];
            return FormValidators[hostingForm].Where(v =>
                IsParent(container, v.ControlToValidate, validationDepth)
                ).ToList();
        }

        public static void DeRegister(BaseValidator validator, Form hostingForm)
        {
            // Remove this validator from the list of registered validators
            FormValidators[hostingForm].Remove(validator);
            // Remove form bucket if all validators on the form are de-registered
            if (FormValidators[hostingForm].Count == 0) FormValidators.Remove(hostingForm);
        }

        private static bool IsParent(Control parent, Control child, ValidationDepth validationDepth)
        {
            if (validationDepth == ValidationDepth.ContainerOnly)
            {
                return (child.Parent == parent);
            }
            Control current = child;
            while (current != null)
            {
                if (current == parent) return true;
                current = current.Parent;
            }
            return false;
        }
    }
}
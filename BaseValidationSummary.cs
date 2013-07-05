using System.Collections.Generic;
using System.ComponentModel;

namespace DevWinformValidation
{
    [ProvideProperty("ShowSummary", typeof(BaseContainerValidator))]
    [ProvideProperty("ErrorMessage", typeof(BaseContainerValidator))]
    [ProvideProperty("ErrorCaption", typeof(BaseContainerValidator))]
    public abstract class BaseValidationSummary : Component, IExtenderProvider
    {

        private readonly IDictionary<BaseContainerValidator, bool> _showSummaries = new Dictionary<BaseContainerValidator, bool>();
        private readonly IDictionary<BaseContainerValidator, string> _errorMessages = new Dictionary<BaseContainerValidator, string>();
        private readonly IDictionary<BaseContainerValidator, string> _errorCaptions = new Dictionary<BaseContainerValidator, string>();

        #region IExtenderProvider
        bool IExtenderProvider.CanExtend(object extendee)
        {
            return true;
        }
        #endregion

        [Category("Validation Summary")]
        [Description("Sets or returns whether BaseContainerValidator uses the ValidationSummary component.")]
        [DefaultValue(false)]
        public bool GetShowSummary(BaseContainerValidator extendee)
        {
            if (_showSummaries.ContainsKey(extendee))
            {
                return _showSummaries[extendee];
            }
            return false;
        }

        public void SetShowSummary(BaseContainerValidator extendee, bool value)
        {
            if (value)
            {
                _showSummaries[extendee] = true;
                extendee.Summarize += Summarize;
            }
            else
            {
                _showSummaries.Remove(extendee);
            }
        }

        [Category("Validation Summary")]
        [Description("Sets or returns the message to display with the validation summary.")]
        [DefaultValue("")]
        public string GetErrorMessage(BaseContainerValidator extendee)
        {
            if (_errorMessages.ContainsKey(extendee))
            {
                return _errorMessages[extendee];
            }
            return string.Empty;
        }

        public void SetErrorMessage(BaseContainerValidator extendee, string value)
        {
            if (value != null)
            {
                _errorMessages[extendee] = value;
            }
            else
            {
                _errorMessages.Remove(extendee);
            }
        }

        [Category("Validation Summary")]
        [Description("Sets or returns the caption to display with the validation summary.")]
        [DefaultValue("")]
        public string GetErrorCaption(BaseContainerValidator extendee)
        {
            if (_errorCaptions.ContainsKey(extendee))
            {
                return _errorCaptions[extendee];
            }
            return string.Empty;
        }

        public void SetErrorCaption(BaseContainerValidator extendee, string value)
        {
            if (value != null)
            {
                _errorCaptions[extendee] = value;
            }
            else
            {
                _errorCaptions.Remove(extendee);
            }
        }


        protected abstract void Summarize(object sender, SummarizeEventArgs e);

        // Support validation in flattened tab index order
        /*protected ValidatorCollection Sort(ValidatorCollection validators)
        {
             

            // Sort validators into flattened tab index order
            // using the BaseValidatorComparer
            ArrayList sorted = new ArrayList();
            foreach (BaseValidator validator in validators)
            {
                sorted.Add(validator);
            }
            sorted.Sort(new BaseValidatorComparer());
            ValidatorCollection sortedValidators = new ValidatorCollection();
            foreach (BaseValidator validator in sorted)
            {
                sortedValidators.Add(validator);
            }

            return sortedValidators;
        }*/
    }

    /*#region BaseValidatorComparer
   public class BaseValidatorComparer : IComparer<BaseValidator>
   {
       public int Compare(BaseValidator x, BaseValidator y)
       {
           if (x.FlattenedTabIndex < y.FlattenedTabIndex) return -1;
           if (x.FlattenedTabIndex > y.FlattenedTabIndex) return 1;
           return 0;
       }
   }
   #endregion*/
}
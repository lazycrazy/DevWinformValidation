using System;
using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraLayout;
using DevExpress.XtraTab;

namespace DevWinformValidation
{
    public class ContainerControlConverter : ReferenceConverter
    {

        public ContainerControlConverter(Type type) : base(type) { }

        protected override bool IsValueAllowed(ITypeDescriptorContext context, object value)
        {
            return ((value is GroupBox) ||
                    (value is TabControl) ||
                    (value is Panel) ||
                    (value is Form) ||
                    (value is UserControl) ||
                    (value is TabbedControlGroup) ||
                    (value is XtraTabPage)
                   );
        }
    }
}
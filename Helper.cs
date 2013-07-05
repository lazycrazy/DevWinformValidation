using System.Windows.Forms;
using DevExpress.XtraLayout;
using DevExpress.XtraTab;

namespace DevWinformValidation
{
    public class FocusHelper
    {
        public static void FocusFirstErrorEditor(Control ctrl)
        {
            if (ctrl == null) return;

            if (ctrl.Parent != null)
            {
                var lc = ctrl.Parent as LayoutControl;
                if (lc != null)
                {
                    var layoutItem = lc.GetItemByControl(ctrl);
                    SelectLayoutGroup(layoutItem);
                }
            }
            SelectTabPage(ctrl);
            ctrl.Focus();
        }
        private static void SelectTabPage(Control ctrl)
        {
            if (ctrl != null && ctrl.Parent != null)
            {
                var xtp = ctrl.Parent as XtraTabPage;
                if (xtp != null)
                    xtp.TabControl.SelectedTabPage = xtp;
                var tp = ctrl.Parent as TabPage;
                if (tp != null)
                {
                    var tabControl = tp.Parent as TabControl;
                    if (tabControl != null) tabControl.SelectedTab = tp;
                }
                SelectTabPage(ctrl.Parent);
            }
        }

        private static void SelectLayoutGroup(BaseLayoutItem item)
        {
            if (item != null)
            {
                var a = (item as LayoutGroup);
                if (a != null && a.ParentTabbedGroup != null)
                {
                    a.ParentTabbedGroup.SelectedTabPage = a;
                }
                SelectLayoutGroup(item.Parent);
            }
        }
    }
}

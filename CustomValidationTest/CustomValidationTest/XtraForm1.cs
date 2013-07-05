using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.Utils.Controls;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraTab;

namespace DevWinformValidationTest
{
    public partial class XtraForm1 : DevExpress.XtraEditors.XtraForm
    {
        public XtraForm1()
        {
            InitializeComponent();
        }

        private void rangeValidator1_Validated(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //containerValidator1.Valid
            if (!containerValidator1.Valid)
            {
                return;
            }
        }



        private void customValidator1_Validating(object sender, DevWinformValidation.CustomValidator.ValidatingCancelEventArgs e)
        {
            e.Valid = e.ControlToValidate.Text.Trim().Length > 0;

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            var v = containerValidator2.Valid;
        }
    }
}
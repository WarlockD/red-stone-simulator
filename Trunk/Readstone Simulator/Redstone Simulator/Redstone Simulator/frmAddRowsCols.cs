using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Redstone_Simulator
{
    public partial class frmAddRowsCols : Form
    {
        public addRowsColumnsResult Result;
        public Button BtnCancel
        {
            get { return this.btnCancel; }
        }

        public Button BtnOk
        {
            get { return this.btnOK; }
        }
        public frmAddRowsCols()
        {
            InitializeComponent();
            Result = null;
        }

        private void frmAddRowsCols_Load(object sender, EventArgs e)
        {
            this.tbBottom.Text = "0";
            this.tbTop.Text = "0";
            this.tbLeft.Text = "0";
            this.tbRight.Text = "0";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Result = new addRowsColumnsResult();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if ((this.tbBottom.Text.Trim() == String.Empty || this.tbBottom.Text.Trim() == "0")
                && (this.tbTop.Text.Trim() == String.Empty || this.tbTop.Text.Trim() == "0")
                && (this.tbLeft.Text.Trim() == String.Empty || this.tbLeft.Text.Trim() == "0")
                && (this.tbRight.Text.Trim() == String.Empty || this.tbRight.Text.Trim() == "0"))
            {
                this.Result = new addRowsColumnsResult();
            }
            else
            {
                int bottomResult;
                int topResult;
                int leftResult;
                int rightResult;

                if ((int.TryParse(tbBottom.Text.Trim(), out bottomResult))
                && (int.TryParse(tbTop.Text.Trim(), out topResult))
                && (int.TryParse(tbLeft.Text.Trim(), out leftResult))
                && (int.TryParse(tbRight.Text.Trim(), out rightResult)))
                {
                    if (bottomResult > 0 || topResult > 0 || leftResult > 0 || rightResult > 0)
                    {
                        Result = new addRowsColumnsResult(topResult, bottomResult, leftResult, rightResult);
                    }
                    else
                    {
                        Result = new addRowsColumnsResult();
                    }
                }
                else
                {
                    Result = new addRowsColumnsResult();
                }
            }
        }

    }
}

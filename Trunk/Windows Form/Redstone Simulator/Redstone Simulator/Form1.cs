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
    public partial class Form1 : Form
    {
     

        public Form1()
        {
            InitializeComponent();
            this.blockView.ChangeStrip += new BlockView.ChangeStripHandler(blockView_ChangeStrip);
            this.blockView.select = blockSelect;
            
        }

        void  blockView_ChangeStrip(object s, myStatusStripEventArgs e)
        {
 	        if (e.Update.HasFlag(eStatusStripUdate.XYZ))
            {
                mainStatusStrip.setCord(e.X, e.Y, e.Z);
             //   this.cCord.Text = e.X + "x" + e.Y + "y" + e.Z + "z";
            //    this.cCord.Invalidate();
            }
        }

    

        void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            this.MouseWheel += new MouseEventHandler(Form1_MouseWheel);
            
            this.blockView.select = blockSelect;
            if (e.Delta > 0 | e.Delta < 0)
                blockSelect.moveSelect(e.Delta > 0 ? 1 : -1);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            blockSelect.Left = (this.Width - blockSelect.Width) / 2;
            if (blockSelect.Left < 0) blockSelect.Left = 0;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0 | e.Delta < 0)
                blockSelect.moveSelect(e.Delta);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void blockView1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            blockSelect.Left = (this.Width - blockSelect.Width) / 2;
            if (blockSelect.Left < 0) blockSelect.Left = 0;
        }
    }
}

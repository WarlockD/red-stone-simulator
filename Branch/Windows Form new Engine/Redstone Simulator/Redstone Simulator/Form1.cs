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
        Image goImage;
        Image stopImage;
        bool running = false;
        int  ticks = 0;
        void MakeIcons()
        {
            goImage = new Bitmap(16, 16);
            stopImage = new Bitmap(16, 16);
            Graphics g = Graphics.FromImage(goImage);
            g.Clear(Color.LightGreen);
            g.Dispose();
            g = Graphics.FromImage(stopImage);
            g.Clear(Color.Red);
            g.Dispose();
        }

        public Form1()
        {
            InitializeComponent();
            MakeIcons();
            this.blockView.StatusStrip = this.mainStatusStrip;
            this.blockView.select = blockSelect;
            this.toolStripButton1.Image = goImage;
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

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (running)
            {
                running = false;
                toolStripButton1.Image = goImage;
            }
            else
            {
                running = true;
                toolStripButton1.Image = stopImage;
                ticks = 0;
                mainStatusStrip.setTicks(ticks);
            }

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ticks++;
          //  blockView.currentSim.tick();
          //  blockView.currentSim.noTick();
            blockView.currentSim.newTick();
            mainStatusStrip.setTicks(ticks);
            blockView.Invalidate();
        }
    }
}

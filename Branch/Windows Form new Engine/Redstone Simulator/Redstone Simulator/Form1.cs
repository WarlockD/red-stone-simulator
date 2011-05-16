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
        Timer time;
        frmAddRowsCols _addRowsCols;
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
            time = new Timer();
            time.Tick += new EventHandler(time_Tick);
            time.Interval = 500;
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
                time.Stop();
            }
            else
            {
                running = true;
                toolStripButton1.Image = stopImage;
                ticks = 0;
                mainStatusStrip.setTicks(ticks);
                time.Start();
            }

        }

        void time_Tick(object sender, EventArgs e)
        {
            ticks++;
            blockView.currentSim.newTick();
            mainStatusStrip.setTicks(ticks);
            blockView.Refresh();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ticks++;
          //  blockView.currentSim.tick();
          //  blockView.currentSim.noTick();
            blockView.currentSim.newTick();
           // blockView.currentSim.tick();
           // blockView.currentSim.noTick();
            mainStatusStrip.setTicks(ticks);
            blockView.Refresh();
        }


        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }


        private void tsbZoomIn_Click(object sender, EventArgs e)
        {
            this.blockView.zoomIn();
        }

        private void tsbZoomOut_Click(object sender, EventArgs e)
        {
            this.blockView.zoomOut();
        }

        private void tsbAddTop_Click(object sender, EventArgs e)
        {
            this.blockView.addTopRow();
        }

        private void tsbAddBottom_Click(object sender, EventArgs e)
        {
            this.blockView.addBottomRow();
        }

        private void tsbAddLeft_Click(object sender, EventArgs e)
        {
            this.blockView.addLeftColumn();
        }

        private void tsbAddRight_Click(object sender, EventArgs e)
        {
            this.blockView.addRightColumn();
        }

        private void tsbAddMultiDimension_Click(object sender, EventArgs e)
        {
            _addRowsCols = new frmAddRowsCols();
            _addRowsCols.BtnCancel.Click += handleAddRowCol_Result;
            _addRowsCols.BtnOk.Click += handleAddRowCol_Result;
            _addRowsCols.Show();
        }


        public void handleAddRowCol_Result(object sender, EventArgs e)
        {
            _addRowsCols.Visible = false;
            _addRowsCols.Hide();
            addRowsColumnsResult result = _addRowsCols.Result;
            if (result.ResultOK)
            {
                if (result.LeftColumns > 0)
                    this.blockView.addNColumnToLeft(result.LeftColumns);
                if (result.RightColumns > 0)
                    this.blockView.addNColumnToRight(result.RightColumns);
                if (result.BottomRows > 0)
                    this.blockView.addNRowToBottom(result.BottomRows);
                if (result.TopRows > 0)
                    this.blockView.addNRowToTop(result.TopRows);
            }
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

namespace Redstone_Simulator
{
    public partial class myStatusStrip : StatusStrip
    {
        int x=0;
        int y=0;
        int z=0;
        int wires, torches,redstone = 0;
        int layer = 1;
        ToolStripLabel cLayer;
        ToolStripLabel cCord;
        ToolStripLabel cTorches;
        ToolStripLabel cWires;
        ToolStripLabel cRedstone;

        private void CreateStrip()
        {
            this.SuspendLayout();
            this.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.cLayer = new ToolStripLabel();
            this.cLayer.ForeColor = Color.DarkBlue;
            this.cLayer.Name = "cLayer";
            this.cLayer.Text = "Layer 1";
            this.cLayer.Alignment = ToolStripItemAlignment.Left;
            this.Items.Add(cLayer);

            this.Items.Add(new ToolStripSeparator());

            this.cCord = new ToolStripLabel();
            this.cCord.ForeColor = Color.Black;
            this.cCord.Name = "cCord";
            this.cCord.Text = "0x0y0z";
            this.cCord.Alignment = ToolStripItemAlignment.Left;
            this.Items.Add(cCord);

            this.Items.Add(new ToolStripSeparator());

            this.cRedstone = new ToolStripLabel();
            this.cRedstone.ForeColor = Color.Black;
            this.cRedstone.Image = Redstone_Simulator.Properties.Resources.Ore16;
            this.cRedstone.Name = "cRedstone";
            this.cRedstone.Text = "0";
            this.cRedstone.Alignment = ToolStripItemAlignment.Right;
            this.Items.Add(cRedstone);

            this.Items.Add(new ToolStripSeparator() { Alignment = ToolStripItemAlignment.Right });

            this.cTorches = new ToolStripLabel();
            this.cTorches.ForeColor = Color.Black;
            this.cTorches.Image = Redstone_Simulator.Properties.Resources.Torch16;
            this.cTorches.ImageAlign = ContentAlignment.MiddleLeft;
            this.cTorches.Name = "cTorches";
            this.cTorches.Text = "0";
            this.cTorches.Alignment = ToolStripItemAlignment.Right;
            this.Items.Add(cTorches);

            this.Items.Add(new ToolStripSeparator() { Alignment = ToolStripItemAlignment.Right });

            this.cWires = new ToolStripLabel();
            this.cWires.ForeColor = Color.Black;
            this.cWires.Image = Redstone_Simulator.Properties.Resources.Redstone16;
            this.cWires.Name = "cWires";
            this.cWires.Text = "0";
            this.cWires.Alignment = ToolStripItemAlignment.Right;
            this.Items.Add(cWires);

            this.Items.Add(new ToolStripSeparator() { Alignment = ToolStripItemAlignment.Right });

            this.ResumeLayout(false);

        }
        public myStatusStrip()
        {
            this.BackColor = Color.White;
            CreateStrip();
            ChangeText();
        }

        void ChangeText()
        {
            cLayer.Text = String.Format("Layer {0,3:d}" , layer);
            cTorches.Text = String.Format("{0,3:d}", torches);
            cWires.Text = String.Format("{0,3:d}", wires);
            cRedstone.Text = String.Format("{0,3:d}", redstone);
            cCord.Text = x.ToString("d") + "x" +
                y.ToString("d") + "y" +
                z.ToString("d") + "z";
            this.Invalidate(true);
        }

        public void setCord(int X, int Y, int Z)
        {
            x = X; y = Y; z = Z;
            ChangeText();
        }
        public void setWire(int Wires)
        {
            wires = Wires;
            ChangeText();
        }
        public void setRedstone(int Redstone)
        {
            redstone = Redstone;
            ChangeText();
        }
        public void setTorches(int Torches)
        {
            torches = Torches;
            ChangeText();
        }
            
        public void setLayer(int Layer)
        {
            layer = Layer;
            ChangeText();
        }


      
    }
}

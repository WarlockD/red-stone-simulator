using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Redstone_Simulator
{
    public partial class BlockSelect : UserControl
    {
        Bitmap bar;
        int selected = 0; public int Selected { get { return selected; } }
        public Blocks SelectedBlock { get { return sArray[selected][0]; } }
        float scale = 5;
        public float BlockScale { get { return scale; } set { scale = value; } }
        Blocks[][] sArray = { 
                               new Blocks[] { Blocks.sAIR,Blocks.AIR,Blocks.AIR },
                                new Blocks[]  { Blocks.sBLOCK,Blocks.AIR,Blocks.AIR },
                                new Blocks[]  { Blocks.sBLOCK,Blocks.sBLOCK,Blocks.AIR },
                                new Blocks[]  { Blocks.sWIRE,Blocks.AIR,Blocks.AIR },
                                new Blocks[]  { Blocks.sTORCH,Blocks.AIR,Blocks.AIR },
                                new Blocks[]  { Blocks.sBLOCK,Blocks.sWIRE,Blocks.AIR },
                               new Blocks[]   { Blocks.sBLOCK,Blocks.sTORCH,Blocks.AIR },
                               new Blocks[]   { Blocks.sWIRE,Blocks.sBLOCK,Blocks.AIR },
                                new Blocks[]  { Blocks.sTORCH,Blocks.sBLOCK,Blocks.AIR },
                                new Blocks[]  { Blocks.sWIRE,Blocks.sTORCH,Blocks.AIR },
                                new Blocks[]  { Blocks.sWIRE,Blocks.sBLOCK,Blocks.sWIRE },
                               new Blocks[]   { Blocks.sLEVER,Blocks.AIR,Blocks.AIR },
                                new Blocks[]  { Blocks.sBUTTON,Blocks.AIR,Blocks.AIR },
                                new Blocks[]  { Blocks.sPRESS,Blocks.AIR,Blocks.AIR },
                                new Blocks[]  { Blocks.sDOORA,Blocks.AIR,Blocks.AIR }  ,
                                 new Blocks[]  { Blocks.REPEATER,Blocks.AIR,Blocks.AIR }  
                            };
      
        public BlockSelect()
        {
            makeBar();      
            this.DoubleBuffered = true;
            InitializeComponent();
        }
        void makeBar()
        {
            bar = new Bitmap((int)((sArray.Length * 9 + 1) * scale), (int)(scale * 10));
            Graphics g = Graphics.FromImage(bar);
            
            g.Clear(BlockColors.cGrid);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.ScaleTransform(scale, scale);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.FillRectangle(BlockColors.bHilite, (selected * 9), 0, 10, 10);
 
            for (int i = 0; i < sArray.Length; i++)
            {
                Rectangle r = new Rectangle(i * 9 + 1, 1, 8, 8);
                BlockImages.gDrawBlockStack(g, r, sArray[i]);
            }
            g.Dispose();
            this.MinimumSize = bar.Size;
            this.MaximumSize = bar.Size;
        }
        private void BlockSelect_Load(object sender, EventArgs e)
        {
            
        }

        public  void moveSelect(int select)
        {
            selected += select;
            if (selected  > sArray.Length-1)
                selected = sArray.Length-1;
            if(selected < 0)
                selected = 0;
            makeBar();
            this.Refresh();
            this.Invalidate();
        }



        private void BlockSelect_Paint(object sender, PaintEventArgs e)
        {
      
            Graphics g = e.Graphics;
            g.DrawImage(bar, 0, 0);
        }
       

        private void BlockSelect_MouseClick(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case System.Windows.Forms.MouseButtons.Left:
                   // int pX = (e.X-center) / (int)scale;
                    int pX = (e.X) / (int)scale;
                    if (pX < 0) return;
                    if (pX % 9 == 0) return;
                    pX /= 9;
                    if (pX >= sArray.Length) return;
                    else
                    {
                        selected = pX;
                        makeBar();
                        this.Refresh();
                    }
                    break;

            }
        }

        private void BlockSelect_MouseEnter(object sender, EventArgs e)
        {
        //    if (!this.Focused) this.Focus();
        }

      
    }
}

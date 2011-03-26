using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Redstone_Simulator
{
    public partial class BlockSelect : UserControl
    {
        Bitmap bar;
        int selected = 0;
        int scale = 5;
        Size maxSize;
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
            maxSize = new Size((sArray.Length * 9 + 1) * scale, scale * 10);
            this.MinimumSize = maxSize;
            this.MinimumSize = maxSize;
            makeBar();
            this.DoubleBuffered = true;
            InitializeComponent();
        }
        void makeBar()
        {
            Rectangle r;
            bar = new Bitmap(maxSize.Width, maxSize.Height);
            Graphics g = Graphics.FromImage(bar);
            
            g.Clear(BlockColors.cGrid);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.ScaleTransform(scale, scale);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.FillRectangle(BlockColors.bHilite, (selected * 9), 0, 10, 10);
            // g.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);
            //g.setColor(Colors.hilite);
            
            for (int i = 0; i < sArray.Length; i++)
            {
                r = new Rectangle(i * 9 + 1, 1, 8, 8);
                BlockImages.gDrawBlockStack(g, r, sArray[i]);
            }
            g.Dispose();
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
            g.Clear(BlockColors.cGrid);
            
            g.DrawImage(bar, center, 0);
            //g.ScaleTransform(scale, scale);
            //g.DrawRectangle(new Pen(BlockColors.bHilite), (selected * 9) +center, 0, 10, 10);
           

        }
        int center = 0;
        private void BlockSelect_Resize(object sender, EventArgs e)
        {
            center = (Width -bar.Width) / 2; 
            if (center < 0) center = 0;
            this.Refresh();
        //    this.Invalidate();
          //  scale = this.Width / (sArray.Length * 9 + 1);
          //  if (scale > 5)
           //     scale = 5;
           // //if (scale < 2)
            //    scale = 2;
            ////maxSize = new Size((sArray.Length * 9 + 1) * scale, scale * 10);
           // this.MinimumSize = maxSize;
           // this.MinimumSize = maxSize;
           // Dimension d = new Dimension((palArr.length * 9 + 1) * pScale, 10 * pScale);
          //  pView.setPreferredSize(d);
          //  pView.setMaximumSize(d);
           // pView.revalidate();
        }


        private void BlockSelect_MouseClick(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case System.Windows.Forms.MouseButtons.Left:
                    int pX = (e.X-center) / scale;
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
            if (!this.Focused) this.Focus();
        }

      
    }
}

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
    public partial class BlockView : UserControl
    {
        Timer simTime;
        Bitmap bmp;
        private PictureBox Display;
        public BlockSelect select;
        public BlockSim currentSim;
        Size DisplaySize;
        float scale = 3;
        bool isDragging = false;
        bool isCloneing = false;
        bool ctrlDown = true;
        bool cChanged = false;
        Point lMouse = new Point(0, 0);
        int cX = -1, cY = -1, cZ = 0;
        bool isMouseHere = false;
        bool playing = false;

        public delegate void ChangeStripHandler(object s, myStatusStripEventArgs e);
        public event ChangeStripHandler ChangeStrip;


        void ToggleTimer()
        {
            if (simTime == null)
            {
                simTime = new Timer();
                simTime.Tick += new EventHandler(simTime_Tick);
                simTime.Interval = 100;

            }
            if (!playing) simTime.Start();
            if (playing) simTime.Stop();
            playing = !playing;
        }

        void simTime_Tick(object sender, EventArgs e)
        {
            currentSim.tick();
        }



        protected void OnChangeStrip(object s, myStatusStripEventArgs e) { }// nothing to as we are changing the strip }

        private void SetUpInternalDisplay()
        {
            this.Display = new System.Windows.Forms.PictureBox();
  
            this.SuspendLayout();
            // 
            // PicBox
            // 
            this.Display.Location = new System.Drawing.Point(0, 0);
            this.Display.Name = "Display";
            this.Display.Size = DisplaySize;
            this.Display.TabIndex = 3;
            this.Display.TabStop = false;
            this.Display.Paint += new PaintEventHandler(Display_Paint);
            this.Display.MouseEnter += new EventHandler(Display_MouseEnter);
            this.Display.MouseMove += new MouseEventHandler(Display_MouseMove);
            this.Display.MouseDown += new MouseEventHandler(Display_MouseDown);
            this.Display.MouseUp += new MouseEventHandler(Display_MouseUp);
            this.Display.MouseLeave += new EventHandler(Display_MouseLeave);

            this.Display.MouseClick += new MouseEventHandler(Display_MouseClick);

           // this.Display
            // 
            // OuterPanel
            // 
          
            this.Controls.Add(this.Display);
            
            this.ResumeLayout(false);

        }

        void Display_MouseClick(object sender, MouseEventArgs e)
        {
            //place(cX, cY, cZ, true);
        }

        void Display_MouseLeave(object sender, EventArgs e)
        {
           isDragging = false;
        }

        void Display_MouseEnter(object sender, EventArgs e)
        {
            Focus();
            isDragging = false;
        }

        void Display_MouseUp(object sender, MouseEventArgs e)
        {
            this.isDragging = false;
            if (!cChanged)
                place(cX, cY, cZ, true);
            switch (e.Button)
            {
                case System.Windows.Forms.MouseButtons.Left:
                    if (isDragging)
                        isDragging = false;
                    if(!cChanged)
                        place(cX, cY, cZ, true);
                    break;
                
                case System.Windows.Forms.MouseButtons.Right:
                    Blocks b = currentSim[cX, cY, cZ];
                    if (b.isControl)
                    {
                        currentSim.SetPower(cX, cY, cZ, 16);
                        Display.Invalidate();
                    }
                    break;
            }
        }

        void Display_MouseDown(object sender, MouseEventArgs e)
        {
            switch(e.Button)
            {
                case System.Windows.Forms.MouseButtons.Left:
                    lMouse = e.Location;
                    isDragging = true;
                    cChanged = false;
                    break;
                


            }
        }

        void Display_MouseMove(object sender, MouseEventArgs e)
        {
            lMouse = e.Location;
            updateTooltip();
            if (isDragging && cChanged)
            {
                place(cX, cY, cZ,false);
            }
         
        }


       



        public BlockView()
        {
            currentSim = new BlockSim(20, 20, 5);
            DisplaySize = new Size((int)((currentSim.X * 9 + 1) * scale), (int)((currentSim.Y * 9 + 1) * scale));
            //this.DoubleBuffered = true;
           // this.AutoScroll = true;
            InitializeComponent();
            SetUpInternalDisplay();
            
        }
        private void BlockView_Load(object sender, EventArgs e)
        {

            bmp = new Bitmap(DisplaySize.Height,DisplaySize.Width);
        }
        public BlockView(BlockSim sim)
        {
            InitializeComponent();
            currentSim = sim;
        }
        public void drawWire(Graphics g, Rectangle r,int x, int y, int z)
        {
            WireMask m = WireMask.None;

            if (currentSim.canConnect(x, y, x - 1, y, z)) m |= WireMask.West;
            if (currentSim.canConnect(x, y, x + 1, y, z)) m |= WireMask.East;
            if (currentSim.canConnect(x, y, x, y - 1, z)) m |= WireMask.North;
            if (currentSim.canConnect(x, y, x, y + 1, z)) m |= WireMask.South;

            BlockImages.gDrawBlock(g, r, new BlockDrawSettings(currentSim.GetBlock(x, y, z), m));
    

        }

        public void updateTooltip()
        {
            Point p = new Point(lMouse.X / (int)scale, lMouse.Y / (int)scale);
            //ifoutofbounds
            if (lMouse.X < 0 || lMouse.Y < 0 || p.X < 0 || p.Y < 0 || p.X > currentSim.X * 9 || p.Y > currentSim.Y * 9)
            { 
                cX = cY = -1;
                toolTip.Active = false;
                return;
            }
            if (p.X % 9 == 0 || p.Y % 9 == 0)
                return;
            if (cX != p.X / 9 || cY != p.Y)
                cChanged = true;
            else
                cChanged = false;
            cX = p.X / 9; cY = p.Y / 9; 
            
            myStatusStripEventArgs e = new myStatusStripEventArgs(eStatusStripUdate.XYZ);
            e.X = cX;
            e.Y = cY;
            e.Z = cZ;
            ChangeStrip(this,e);
            // lLoc.setText("X=" + lastX +"Y=" + lastY + "Z=" +lyr);
            //  lLoc.setVisible(true);
            //   sLoc.setVisible(true);

        }

        private void place(int x, int y, int z, bool rotate)
        {
            Blocks b = select.SelectedBlock;
            Blocks t = currentSim[x, y, z];
            if (t.Type == b.Type && t.canRotate && rotate)
            { currentSim.RotateMount(x, y, z); }
            else
                currentSim.SetBlock(x,y,z, b.Type);
            Display.Refresh();
        }


        private void Display_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(BlockColors.cGrid);
            // g.PageScale
            g.ScaleTransform(scale, scale);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
           
            
            for (int x = 0; x < currentSim.X; x++)
                for (int y = 0; y < currentSim.Y; y++)
                {
                    //int ba = r.x / scale / 9; (i * 9 + 1) * scale < r.x + r.width;
                    // g.PageScale
                    Rectangle r = new Rectangle(x * 9 + 1, y * 9 + 1, 8, 8);
                    BlockDrawSettings b = new BlockDrawSettings(currentSim.GetBlock(x,y,cZ));
                    if (b.Block.Type == eBlock.WIRE)
                        drawWire(g, r, x, y, cZ);
                    else
                        BlockImages.gDrawBlock(g, r, b);

                }
        }

        bool tick = false;
        private void BlockView_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.T:
                    if (!tick)
                    {
                        tick = true;
                        ToggleTimer();
                        Display.Invalidate();
                    }
                    break;

            }
        }

        private void BlockView_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.T:
                    if (tick)
                    {
                        tick = false;
                        Display.Invalidate();
                    }
                    break;

            }
        }
       

       
    }
}

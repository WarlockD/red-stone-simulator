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
        private BlockStatusStrip statusStrip = null;
        public BlockStatusStrip StatusStrip { get { return statusStrip; } set { statusStrip = value; } }
        private PictureBox Display;
        public BlockSelect select;
        public BlockSim currentSim;
        Size DisplaySize;
        float scale = 3;

        Point startMouse = new Point(-1, -1);
        Point nextMouse = new Point(-1, -1);
        BlockVector currentLoc;
        BlockVector startLoc;
        int floor = 0;
        bool mouseLeftDown = false;
        bool playing = false;
        bool locValid = false;
        bool isDragging = false;
        void UpdateLoc(Point p, int cFloor)
        {
            
            if (p.X < 0 || p.Y < 0 || p.X > Display.Width|| p.Y > Display.Height)
            {
                UpdateLoc(false);
            }
            else
            {
                currentLoc = new BlockVector((int)(p.X / scale / 9), (int)(p.Y / scale / 9), floor);
                UpdateLoc(true);
            }
        }
        void UpdateLoc(bool valid)
        {
            if (!valid)
            {
                locValid = false;
                if (statusStrip != null) statusStrip.setCord(0, 0, 0);
            }
            else
            {
                locValid = true;
                if (statusStrip != null) statusStrip.setCord(currentLoc.X, currentLoc.Y, floor);
            }

        }

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
           mouseLeftDown = false;
           UpdateLoc(false);
        }

        void Display_MouseEnter(object sender, EventArgs e)
        {
            Focus();
            isDragging = false;
            mouseLeftDown = false;
        }

        void Display_MouseUp(object sender, MouseEventArgs e)
        {
            if(mouseLeftDown)
                switch (e.Button)
                {
                    case System.Windows.Forms.MouseButtons.Left:
                        break;
                
                    case System.Windows.Forms.MouseButtons.Right:
                        if (currentSim[currentLoc].isControl)
                        {
                            currentSim[currentLoc].Charge = 16;
                            Display.Invalidate();
                        }
                        break;
                }
            mouseLeftDown = false;
            isDragging = false;
        }

        void Display_MouseDown(object sender, MouseEventArgs e)
        {
            if(!mouseLeftDown)
                switch(e.Button)
                {
                    case System.Windows.Forms.MouseButtons.Left:

                        startMouse = e.Location;
                        startLoc = currentLoc;
                        place(currentLoc, false);
                        isDragging = false;
                        mouseLeftDown = true;

                        break;
                    case System.Windows.Forms.MouseButtons.Right:
                        if (currentSim[currentLoc].isControl)
                        {
                            currentSim[currentLoc].Charge = 16;
                            Display.Invalidate();
                        }
                        break;
            }
            
        }

        void Display_MouseMove(object sender, MouseEventArgs e)
        {
            UpdateLoc(e.Location, floor);
            if (mouseLeftDown)
                if (startLoc != currentLoc)
                {
                    startLoc = currentLoc;
                    place(currentLoc, false);
                }
        }
         


        BlockVector getTileLoc(Point p)
        {
            if (p.X < 0 || p.Y < 0 || p.X > currentSim.X * 9 || p.Y > currentSim.Y * 9)
            {
                return new BlockVector(-1, -1,currentLoc.Z);
            } else
                return new BlockVector((int)(startMouse.X / scale), (int)(startMouse.Y / scale),currentLoc.Z);
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

        }
        public BlockView(BlockSim sim)
        {
            InitializeComponent();
            currentSim = sim;

        }
      

       

        private void place(BlockVector v, bool rotate)
        {
            Block b = select.SelectedBlock;
            if (currentSim[v].ID == b.ID)
            { 
                currentSim[v].Rotate(); 
            }
            else
            {
                Block b = new Block(b.ID);
                if(b.ID == BlockType.BUTTON)
                currentSim[v].ID = b.ID;
                currentSim.setConnections(v);
            }
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
                    BlockDrawSettings b = new BlockDrawSettings(currentSim[currentLoc.ChangeXY(x,y)]);
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

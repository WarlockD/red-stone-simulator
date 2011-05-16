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
        public int Floor { get { return floor; } set { if (floor > 0 || floor < currentSim.Z) floor = value; currentLoc = currentLoc.ChangeZ(floor); } }
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
                            currentSim[currentLoc].Powered = !currentSim[currentLoc].Powered;
                            currentSim.updateT();
                            Display.Invalidate();

                        }
                        if (currentSim[currentLoc].isRepeater)
                        {
                            currentSim[currentLoc].increaseTick();
                            currentSim.updateT();
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
            currentSim = new BlockSim(@"C:\Users\Paul Bruner\Documents\MC14500bv6.schematic");
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
                Block t = new Block(b);
     
                currentSim[v].ID = t.ID;
                currentSim.setConnections(v);
            }
            currentSim.updateT();
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

        private void BlockView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.W)
                this.Floor++;
            if (e.KeyData == Keys.S)
                this.Floor--;
            Display.Refresh();

        }

      



        public void addTopRow()
        { 
            //create new Simulation with Y+1 rows
            BlockSim _newSim = new BlockSim(this.currentSim.X, this.currentSim.Y + 1, this.currentSim.Z);
            for (int i = 0; i < currentSim.X; i++)
            {
                for (int j = 0; j < currentSim.Y; j++)
                {
                    for (int k = 0; k < currentSim.Z; k++)
                    {
                        if (currentSim.GetBlockType(i,j,k) == BlockType.AIR)
                            continue;
                        _newSim.SetBlock(i, j + 1, k, currentSim.GetBlockType(i,j,k));
                    }
                }
            }
            this.Controls.Clear();
            this.currentSim = _newSim;

            DisplaySize = new Size((int)((currentSim.X * 9 + 1) * scale), (int)((currentSim.Y * 9 + 1) * scale));
            SetUpInternalDisplay();   
        }

        public void addRightColumn()
        {
            //create new Simulation with Y+1 rows
            BlockSim _newSim = new BlockSim(this.currentSim.X + 1, this.currentSim.Y, this.currentSim.Z);
            for (int i = 0; i < currentSim.X; i++)
            {
                for (int j = 0; j < currentSim.Y; j++)
                {
                    for (int k = 0; k < currentSim.Z; k++)
                    {
                        if (currentSim.GetBlockType(i, j, k) == BlockType.AIR)
                            continue;
                        _newSim.SetBlock(i, j, k, currentSim.GetBlockType(i, j, k));
                    }
                }
            }
            this.Controls.Clear();
            this.currentSim = _newSim;

            DisplaySize = new Size((int)((currentSim.X * 9 + 1) * scale), (int)((currentSim.Y * 9 + 1) * scale));
            SetUpInternalDisplay();
        }

        public void addBottomRow()
        {
            //create new Simulation with Y+1 rows
            BlockSim _newSim = new BlockSim(this.currentSim.X, this.currentSim.Y + 1, this.currentSim.Z);
            for (int i = 0; i < currentSim.X; i++)
            {
                for (int j = 0; j < currentSim.Y; j++)
                {
                    for (int k = 0; k < currentSim.Z; k++)
                    {
                        if (currentSim.GetBlockType(i, j, k) == BlockType.AIR)
                            continue;
                        _newSim.SetBlock(i, j, k, currentSim.GetBlockType(i, j, k));
                    }
                }
            }
            this.Controls.Clear();
            this.currentSim = _newSim;

            DisplaySize = new Size((int)((currentSim.X * 9 + 1) * scale), (int)((currentSim.Y * 9 + 1) * scale));
            SetUpInternalDisplay();
        }

        public void addLeftColumn()
        {
            //create new Simulation with Y+1 rows
            BlockSim _newSim = new BlockSim(this.currentSim.X + 1, this.currentSim.Y, this.currentSim.Z);
            for (int i = 0; i < currentSim.X; i++)
            {
                for (int j = 0; j < currentSim.Y; j++)
                {
                    for (int k = 0; k < currentSim.Z; k++)
                    {
                        if (currentSim.GetBlockType(i, j, k) == BlockType.AIR)
                            continue;
                        _newSim.SetBlock(i + 1, j, k, currentSim.GetBlockType(i, j, k));

                    }
                }
            }
            this.Controls.Clear();
            this.currentSim = _newSim;

            DisplaySize = new Size((int)((currentSim.X * 9 + 1) * scale), (int)((currentSim.Y * 9 + 1) * scale));
            SetUpInternalDisplay();
        }
      
        public void addNRowToTop(int n)
        {
            for (int i = 0; i < n; i++)
                addTopRow();
        }

        public void addNRowToBottom(int n)
        {
            for (int i = 0; i < n; i++)
                addBottomRow();
        }

        public void addNColumnToLeft(int n)
        {
            for (int i = 0; i < n; i++)
                addLeftColumn();
        }

        public void addNColumnToRight( int n)
        {
            for (int i = 0; i < n; i++)
                addRightColumn();
        }

        public void zoomOut()
        {
            if (this.scale <= 1)
                return;
            this.scale -= 1.0F;
            DisplaySize = new Size((int)((currentSim.X * 9 + 1) * scale), (int)((currentSim.Y * 9 + 1) * scale));
            this.Controls.Clear();
            SetUpInternalDisplay();
        }

        public void zoomIn()
        {
            if (this.scale >= 5)
                return;
            this.scale += 1.0F;
            DisplaySize = new Size((int)((currentSim.X * 9 + 1) * scale), (int)((currentSim.Y * 9 + 1) * scale));
            this.Controls.Clear();
            SetUpInternalDisplay();
        }

    }
}

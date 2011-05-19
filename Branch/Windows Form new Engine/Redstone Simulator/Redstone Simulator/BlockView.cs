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
        public BlockSim currentSim;
        public BlockType selectedBlock { get; set; }
        bool stopPaint = false;
        Size DisplaySize;
        float scale = 3;

        Point startMouse = new Point(-1, -1);
        Point nextMouse = new Point(-1, -1);
        BlockVector currentLoc;
        BlockVector startLoc;
        int floor = 0;
        bool mouseLeftDown = false;
        bool controlUp = true;
        bool playing = false;
        bool locValid = false;
        bool isDragging = false;



        void UpdateLoc(Point p, int cFloor)
        {
            
            if (p.X < 0 || p.Y < 0 || p.X > this.Width|| p.Y > this.Height)
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
            currentSim.newTick();
        }



        private void SetUpInternalDisplay()
        {
            stopPaint = true;
            DisplaySize = new Size((int)((currentSim.X * 9 + 1) * scale), (int)((currentSim.Y * 9 + 1) * scale));
            this.Size = DisplaySize;
            MinimumSize = DisplaySize;
            stopPaint = false;
            this.Refresh();

        }
        private void SetUpInternalDisplay(BlockSim newSim)
        {
            stopPaint = true;
            currentSim = null;
            currentSim = newSim;
            DisplaySize = new Size((int)((currentSim.X * 9 + 1) * scale), (int)((currentSim.Y * 9 + 1) * scale));
            this.Size = DisplaySize;
            this.MinimumSize = DisplaySize;
            stopPaint = false;
            this.Refresh();

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
                            this.Invalidate();
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
                            this.Invalidate();

                        }
                        if (currentSim[currentLoc].isRepeater)
                        {
                            currentSim[currentLoc].increaseTick();
                            currentSim.updateT();
                            this.Invalidate();                

                        }
                        break;
            }
            
        }

        void Display_MouseMove(object sender, MouseEventArgs e)
        {
            UpdateLoc(e.Location, floor);
            if (mouseLeftDown)
            {
                if (startLoc != currentLoc)
                {
                    startLoc = currentLoc;
                    place(currentLoc, false);
                }
            }
            else
            {
              //  Rectangle g = new Rectangle(0, 0, this.Width, blockSelect.Height);
              //  if (g.Contains(e.Location))
             //   { blockSelect.Visible = true; blockSelect.Focus(); }
             //   else
              //  { blockSelect.Visible = false; }

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
            InitializeComponent();
           // currentSim = new BlockSim(@"C:\Users\Paul Bruner\Documents\MC14500bv6.schematic");
            currentSim = new BlockSim(30, 30, 5);
            DisplaySize = new Size((int)((currentSim.X * 9 + 1) * scale), (int)((currentSim.Y * 9 + 1) * scale));
            SetUpInternalDisplay();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            this.AutoScrollOffset = new Point(10, 10);
            this.Size = DisplaySize;
            this.Paint += this.Display_Paint;
            this.MouseDown += this.Display_MouseDown;
            this.MouseUp += this.Display_MouseUp;
            this.MouseMove += this.Display_MouseMove;
            this.MouseEnter += this.Display_MouseEnter;
            this.MouseLeave += this.Display_MouseLeave;
 
            
          
            
        }
        private void BlockView_Load(object sender, EventArgs e)
        {
         //   blockSelect.Left = (this.Width - blockSelect.Width) / 2;
         //   if (blockSelect.Left < 0) blockSelect.Left = 0;
         //   blockSelect.Visible = false; 

            // Test load
           // this.SetUpInternalDisplay(new BlockSim("MC14500bv6.schematic"));
        }
        public BlockView(BlockSim sim)
        {
            
            currentSim = sim;
            DisplaySize = new Size((int)((currentSim.X * 9 + 1) * scale), (int)((currentSim.Y * 9 + 1) * scale));
           // SetUpInternalDisplay();

        }
      
        
       

        private void place(BlockVector v, bool rotate)
        {
            if (currentSim[v].ID == selectedBlock)
            {
                Block b = currentSim[v];
                Direction old = b.Place;
                switch (selectedBlock)
                {
                    case BlockType.TORCH:
                    case BlockType.LEVER:
                         b.Rotate();
                        if (!currentSim[v.Dir(b.Place)].isBlock && b.Place != old ||  (b.Place != Direction.DOWN  && !currentSim[v.Down].isBlock))
                            goto case BlockType.LEVER;
                        break;
                    case BlockType.BUTTON:
                        b.Rotate();
                        if (!currentSim[v.Flip(b.Place)].isBlock || b.Place != old)
                            goto case BlockType.BUTTON;
                        break;
                    case BlockType.REPEATER:
                        b.Rotate();
                        break;
                }
            }
            else
            {
                Block b = Block.New(selectedBlock);
                Direction old = b.Place;
                switch (selectedBlock)
                {
                    case BlockType.TORCH:
                    case BlockType.LEVER:
                    b.Rotate();
                        if (!currentSim[v.Dir(b.Place)].isBlock && b.Place != old ||  (b.Place != Direction.DOWN  && !currentSim[v.Down].isBlock))
                            goto case BlockType.LEVER;
                           if(!currentSim[v.Down].isBlock && b.Place == old)
                               return;
                        break;
                    case BlockType.BUTTON:
                        b.Rotate();
                        if (!currentSim[v.Flip(b.Place)].isBlock || b.Place != old)
                            goto case BlockType.BUTTON;
                        if(!currentSim[v.Flip(b.Place)].isBlock && b.Place == old)
                                return;
                        break;
                    case BlockType.REPEATER:
                        b.Rotate();
                        break;
                }
                currentSim[v] = b;
                currentSim.setConnections(v);

            }
            currentSim.updateT();
            this.Refresh();
        }


        private void Display_Paint(object sender, PaintEventArgs e)
        {
            if (!stopPaint)
            {
                Graphics g = e.Graphics;
                g.Clear(BlockColors.cGrid);
                // g.PageScale
                g.ScaleTransform(scale, scale);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;


                for (int x = 0; x < currentSim.X; x++)
                    for (int y = 0; y < currentSim.Y; y++)
                    {
                        BlockDrawSettings b;
                        BlockVector v = new BlockVector(x, y, floor);
                        if (currentSim[v].isBlock && (currentSim[v.Up].isWire || currentSim[v.Up].isPreasurePad ||
                            ((currentSim[v.Up].isTorch || currentSim[v.Up].isLeaver || currentSim[v.Up].isTorch) && currentSim[v.Up].Place == Direction.DOWN)))
                        {
                            b = new BlockDrawSettings(currentSim[v.Up]);
                            b.OnBlock = true;
                        }
                        else
                        {
                            b = new BlockDrawSettings(currentSim[v]);
                        }

                        Rectangle r = new Rectangle(x * 9 + 1, y * 9 + 1, 8, 8);
         
            
                        BlockImages.gDrawBlock(g, r, b);

                    }
            }
           
        }

        private void BlockView_KeyUp(object sender, KeyEventArgs e)
        {
            

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
     

            DisplaySize = new Size((int)((currentSim.X * 9 + 1) * scale), (int)((currentSim.Y * 9 + 1) * scale));
            SetUpInternalDisplay(_newSim);   
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
       

            SetUpInternalDisplay(_newSim);
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
     
          
            SetUpInternalDisplay(_newSim);
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
    

            SetUpInternalDisplay(_newSim);
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
     
         
            SetUpInternalDisplay();
        }

        public void zoomIn()
        {
            if (this.scale >= 5)
                return;
            this.scale += 1.0F;
          
         
            SetUpInternalDisplay();
        }

        private void BlockView_Resize(object sender, EventArgs e)
        {

             
        }

        private void BlockView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.W)
                this.Floor++;
            if (e.KeyData == Keys.S)
                this.Floor--;
            this.Refresh();
        }

        private void BlockView_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // BlockView
            // 
            this.Name = "BlockView";
            this.Size = new System.Drawing.Size(445, 385);
            this.ResumeLayout(false);

        }


    }
}

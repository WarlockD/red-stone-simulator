using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

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

        #region WndProc Override to pass mouse to form
        [FlagsAttribute]
        enum DownKeys : int
        {
            MK_CONTROL = 0x008,
            MK_LBUTTON = 0x001,
            MK_MBUTTON = 0x010,
            MK_RBUTTON = 0x002,
            MK_SHIFT = 0x004,
            MK_XBUTTON1 = 0x0020,
            MK_XBUTTON2 = 0x0040
        }
        const int WM_MOUSEWHEEL = 0x020A;
        const int WM_KEYUP = 0x0101;
        const int WM_KEYDOWN = 0x0100;
        bool imInWinProc = false;
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr wnd, int msg, IntPtr wp, IntPtr lp);
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_MOUSEWHEEL:
                    if (!imInWinProc)
                    {
                        // http://msdn.microsoft.com/en-us/library/ms645614%28v=VS.85%29.aspx
                        // wParm high: Whiel delta, short in 120 marks.  - left, + right
                        // wParm low: mask with DownKeys for keys pressed
                        // lowParm, High is x, low is y.  Eveything is short
                        imInWinProc = true;
                        SendMessage(this.ParentForm.Handle, m.Msg, m.WParam, m.LParam);
                        imInWinProc = false;
                        // Return zero if its processed, we are just passing it to the form though.
                    }
                    break;
               /*case WM_KEYUP:
                case WM_KEYDOWN:
                    if (!imInWinProc)
                    {
                        imInWinProc = true;
                        SendMessage(this.ParentForm.Handle, m.Msg, m.WParam, m.LParam);
                        imInWinProc = false;
                    }
                 break;
                    */
                default:
                    base.WndProc(ref m);
                    break;
            }

        }
        #endregion
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

      //  delegate void AutoScrollPositionDelegate(ScrollableControl sender, Point p);
        void Display_MouseEnter(object sender, EventArgs e)
        {
            // Sigh, more hacks,  Java is starting to look alot better.
       /*     if (this.Parent is Panel)
            {
                Panel panel = this.Parent as Panel;
                Point p = panel.AutoScrollPosition;
                Console.WriteLine("Before: " + p.ToString());
                AutoScrollPositionDelegate d = new AutoScrollPositionDelegate(AutoScrollScrewup);
                BeginInvoke(d, new Object[] { panel, p });

            }*/
            isDragging = false;
            mouseLeftDown = false;
        }
      /*  void AutoScrollScrewup(ScrollableControl sender, Point p)
        {
            Console.WriteLine("After: " + sender.AutoScrollPosition.ToString());
           p.X = Math.Abs(p.X);
           p.Y = Math.Abs(p.Y);
            sender.AutoScrollPosition = p;
        }*/
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
        public void LoadSim(string filename)
        {
           BlockSim sim = new BlockSim(filename);
           SetUpInternalDisplay(sim);
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
            this.MouseWheel += new MouseEventHandler(BlockView_MouseWheel);
            this.KeyDown += new KeyEventHandler(BlockView_KeyDown);
            
            
          
            
        }
        //const int WM_MOUSEWHEEL = 0x020A;
        void BlockView_MouseWheel(object sender, MouseEventArgs e)
        {
          //  HandledMouseEventArgs ee =  (HandledMouseEventArgs)e;
          //  ee.Handled = false;
          //  Message m = Message.Create(this.Parent.Handle,WM_MOUSEWHEEL,
            //Form f = this.FindForm();
           // f.PreProcessMessage(e.
           // throw new NotImplementedException();
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
                        if (b.Place == Direction.WEST) { b.Place = Direction.DOWN; } else { b.Place++; }
                        if (currentSim[v.South].isBlock && b.Place == Direction.NORTH) break; 
                        if (currentSim[v.East].isBlock && b.Place == Direction.WEST) break; 
                        if (currentSim[v.North].isBlock && b.Place == Direction.SOUTH) break; 
                        if (currentSim[v.West].isBlock && b.Place == Direction.EAST) break;
                        if (currentSim[v.Down].isBlock && b.Place == Direction.DOWN) break;
                        if (b.Place != old) goto case BlockType.LEVER;
                        b.Place = old;
                        break;
                    case BlockType.BUTTON:
                         if (b.Place == Direction.WEST) { b.Place = Direction.DOWN; } else { b.Place++; }
                        if (currentSim[v.South].isBlock && b.Place == Direction.NORTH) break; 
                        if (currentSim[v.East].isBlock && b.Place == Direction.WEST) break; 
                        if (currentSim[v.North].isBlock && b.Place == Direction.SOUTH) break; 
                        if (currentSim[v.West].isBlock && b.Place == Direction.EAST) break;
                        if (b.Place != old) goto case BlockType.LEVER;
                        b.Place = old;
                        break;
                    case BlockType.REPEATER:
                        if (b.Place == Direction.WEST) { b.Place = Direction.NORTH; } else { b.Place++; }
                        break;
                }
            }
            else
            {

                Block oldBlock = currentSim[v];
                Block b = Block.New(selectedBlock);
                Direction old = b.Place;
                
                switch (selectedBlock)
                {
                    case BlockType.TORCH:
                    case BlockType.LEVER:
                        if (b.Place == Direction.WEST) { b.Place = Direction.DOWN; } else { b.Place++; }
                        if (currentSim[v.South].isBlock && b.Place == Direction.NORTH) { currentSim[v] = b; break; }
                        if (currentSim[v.East].isBlock && b.Place == Direction.WEST) { currentSim[v] = b; break; }
                        if (currentSim[v.North].isBlock && b.Place == Direction.SOUTH) { currentSim[v] = b; break; }
                        if (currentSim[v.West].isBlock && b.Place == Direction.EAST) { currentSim[v] = b; break; }
                        if (currentSim[v.Down].isBlock && b.Place == Direction.DOWN) { currentSim[v] = b; break; }
                        if (b.Place != old) goto case BlockType.LEVER;
                        return;
                    case BlockType.BUTTON:
                        if (b.Place == Direction.WEST) { b.Place = Direction.DOWN; } else { b.Place++; }
                        if (currentSim[v.South].isBlock && b.Place == Direction.NORTH) { currentSim[v] = b; break; }
                        if (currentSim[v.East].isBlock && b.Place == Direction.WEST) { currentSim[v] = b; break; }
                        if (currentSim[v.North].isBlock && b.Place == Direction.SOUTH) { currentSim[v] = b; break; }
                        if (currentSim[v.West].isBlock && b.Place == Direction.EAST) { currentSim[v] = b; break; }
                        if (b.Place != old) goto case BlockType.LEVER;
                        return;
                    case BlockType.REPEATER:
                        if (b.Place == Direction.WEST) { b.Place = Direction.NORTH; } else { b.Place++; }
                        currentSim[v] = b;
                        break;
                    case BlockType.WIRE:
                        currentSim[v] = b;
                        currentSim.setConnections(v);
                        break;
                    default:
                        currentSim[v] = b;
                        //if the old block was a wire, we need to recheck the connections 
                        //so that the previously wired block does not hold the wire connections
                        //that it did previously hold
                        //  Was:        |   Now Is:
                        //  ┌┐ -> ┌┐    |   ┌┐ ->  ┌─
                        //  └┘ -> └     |   └┘ ->  │
                        //
                        if (oldBlock.ID == BlockType.WIRE) {  currentSim.setConnections(v); }
                        break;
                }

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
                            v = v.Up;
                            b = new BlockDrawSettings(currentSim[v]);
                            b.OnBlock = true;
                        }
                        else
                        {
                            b = new BlockDrawSettings(currentSim[v]);
                        }
                        
                        if (currentSim[v.Up].isBlock) b.Fog = true;
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
            if (Floor < 0) Floor = 0;
            if (Floor > currentSim.Z) Floor = currentSim.Z;
            UpdateLoc(true);
            this.Invalidate();
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
            this.AutoScroll = true;
            this.AutoSize = true;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.Name = "BlockView";
            this.Size = new System.Drawing.Size(445, 385);
            this.Load += new System.EventHandler(this.BlockView_Load_1);
            this.ResumeLayout(false);

        }

        private void BlockView_Load_1(object sender, EventArgs e)
        {

        }


    }
}

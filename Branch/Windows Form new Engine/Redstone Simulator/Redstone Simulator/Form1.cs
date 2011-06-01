using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Redstone_Simulator
{
    public partial class frmMain : Form
    {
        
        const int ConstScale = 10;
        Image goImage;
        Image stopImage;
        bool running = false;
        int  ticks = 0;
        Timer time;
        private BlockStatusStrip blockStatusStrip;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem loadItem;
        private ToolStripMenuItem saveItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem quitItem;
        private ToolStrip toolStrip;
        private PanelScrollFix outerPanel;
        private BlockView blockView;
        private ToolStrip toolStrip1;
        private ToolStripButton tsbZoomIn;
        private ToolStripButton tsbZoomOut;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton tsbAddRowTop;
        private ToolStripButton tsbAddRowBottom;
        private ToolStripButton tsbAddColLeft;
        private ToolStripButton tsbAddColRight;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton tsbModifyExtents;
        private ToolStripSeparator toolStripSeparator3;
        frmAddRowsCols _addRowsCols;
        int currentSelectedBlock = 0;
        ToolStripButton selectedButton;
        Bitmap[] blockImages;
        private ToolStripButton tsbUpOneLevel;
        private ToolStripButton tsbDownOneLevel;
        Block[] selectArray;
        void MakeSelectArray()
        {
            selectArray = new Block[8];
            selectArray[0] = new Block(BlockType.AIR, Direction.DOWN, 0, 0, 0);
            selectArray[1] = new Block(BlockType.BLOCK, Direction.DOWN, 0, 0, 0);
            selectArray[2] = new Block(BlockType.WIRE, Direction.DOWN, 16, 0, 0); selectArray[2].Mask = WireMask.AllDir;
            selectArray[3] = new Block(BlockType.TORCH, Direction.NORTH, 16, 0, 0);
            selectArray[4] = new Block(BlockType.LEVER, Direction.NORTH, 0, 0, 0);
            selectArray[5] = new Block(BlockType.BUTTON, Direction.NORTH, 0, 0, 0);
            selectArray[6] = new Block(BlockType.PREASUREPAD, Direction.DOWN, 0, 0, 0);
            selectArray[7] = new Block(BlockType.REPEATER, Direction.NORTH, 0, 0, 0);
        }
        void SelectBlock(int i)
        {
            currentSelectedBlock = i;
            blockView.selectedBlock = selectArray[i].ID;
            selectedButton.Image = blockImages[i];


        }
        void SelectBlock()
        {
            blockView.selectedBlock = selectArray[currentSelectedBlock].ID;
            selectedButton.Image = blockImages[currentSelectedBlock];
        }
        void makeCustomControls()
        {
            blockView = new BlockView();
            blockStatusStrip = new BlockStatusStrip();
            this.SuspendLayout();
            this.Controls.Add(blockView);
            this.Controls.Add(blockStatusStrip);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
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
        public frmMain()
        {
            MakeIcons();
            InitializeComponent();
            this.MouseWheel += new MouseEventHandler(Form1_MouseWheel);
            MakeNewToolStipButtons();
           // this.AutoScroll = true;
           // makeCustomControls();
            MakeIcons();

          
            this.blockView.StatusStrip = this.blockStatusStrip;
           // this.toolStripButton1.Image = goImage;
            time = new Timer();
            time.Tick += new EventHandler(time_Tick);
            time.Interval = 500;
        }

        private void toolStrip_SelectBlock(object sender, EventArgs e)
        {
            if (sender is ToolStripButton)
            {
                ToolStripButton b = (ToolStripButton)sender;
                blockView.selectedBlock = (BlockType)b.Tag;
            }
        }

        void MakeNewToolStipButtons()
        {
            MakeSelectArray();
            Bitmap[] pics = getFoorOneTiles();
            blockImages = pics;
            ToolStripButton[] b = new ToolStripButton[selectArray.Length];
            toolStrip.SuspendLayout();
            b[0] = new ToolStripButton();
            b[0].DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            b[0].Image = global::Redstone_Simulator.Properties.Resources.Play24;
            b[0].Name = "StartClock";
            b[0].Size = new System.Drawing.Size(23, 29);
            b[0].Text = "Start Clock";
            b[0].Click += new System.EventHandler(this.startRunning);
            toolStrip.Items.Add(b[0]);
            toolStrip.Items.Add(new ToolStripSeparator());
            

            selectedButton = new ToolStripButton(pics[0]);
           
            toolStrip.Items.Add(selectedButton);
            toolStrip.Items.Add(new ToolStripSeparator());
            for (int i = 0; i < b.Length; i++)
            {
                
                b[i] = new ToolStripButton();
                b[i].AutoSize = true;
                b[i].DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                b[i].Image = pics[i];
                b[i].Name = "BlockSelect_" + i;
                b[i].Size = pics[i].Size;
                b[i].Text = "BlockSelect_" + i;
                b[i].Tag = i;
                b[i].Click += new System.EventHandler(delegate(object o, EventArgs e) { 
                    SelectBlock((int)((ToolStripButton)o).Tag); });
            }
            toolStrip.Items.AddRange(b);
            b[0].Select();
            toolStrip.ResumeLayout(false);


        
        }

        void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            //HandledMouseEventArgs me = (HandledMouseEventArgs)e;
           // me.Handled = true;
             if (e.Delta == 0)
                return;

             currentSelectedBlock +=e.Delta > 0 ? 1 : -1;
             if (currentSelectedBlock < 0) currentSelectedBlock = selectArray.Length - 1;
             if (currentSelectedBlock > selectArray.Length - 1) currentSelectedBlock = 0;
             SelectBlock();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
         //   blockSelect.Left = (this.Width - blockSelect.Width) / 2;
        //    if (blockSelect.Left < 0) blockSelect.Left = 0;
        }

     
        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void startRunning(object sender, EventArgs e)
        {
            ToolStripButton o = (ToolStripButton)sender;
            if (running)
            {
                o.Image = global::Redstone_Simulator.Properties.Resources.Play24;
                running = false;
               // toolStripButton1.Image = goImage;
                time.Stop();
            }
            else
            {
                o.Image = global::Redstone_Simulator.Properties.Resources.Stop24;
                running = true;
               // toolStripButton1.Image = stopImage;
                ticks = 0;
                blockStatusStrip.setTicks(ticks);

                time.Start();
            }

        }

        void time_Tick(object sender, EventArgs e)
        {
            ticks++;
            blockView.currentSim.newTick();
            blockStatusStrip.setTicks(ticks);
            blockView.Refresh();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ticks++;
       
            blockView.currentSim.newTick();

            blockStatusStrip.setTicks(ticks);
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

        public static Graphics setGraphics(Bitmap b)
        {
            Graphics g = Graphics.FromImage(b);
            g.Clear(BlockColors.cGrid);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.ScaleTransform(ConstScale, ConstScale);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            return g;
        }
        public Bitmap[] getFoorOneTiles()
        {
            // make them 50x50
            BlockDrawSettings bd;
            Size s = new Size(10 * ConstScale, 10 * ConstScale);
            Point p = new Point(0, 0);
            Graphics g;
            Bitmap[] bArray = new Bitmap[selectArray.Length];

            for (int i = 0; i < bArray.Length; i++)
            {
                bArray[i] = new Bitmap(s.Width, s.Height);
                g = setGraphics(bArray[i]);
                bd = new BlockDrawSettings(selectArray[i]);
                BlockImages.gDrawBlock(g, new Rectangle(p, s), bd);
                g.Dispose();
            }
            
            return bArray;

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        void SetUpTheView()
        {
            this.blockView.AutoScrollOffset = new System.Drawing.Point(10, 10);
            this.blockView.Floor = 0;
            this.blockView.Location = new System.Drawing.Point(0, 0);
            this.blockView.MinimumSize = new System.Drawing.Size(813, 813);
            this.blockView.Name = "blockView";
            this.blockView.selectedBlock = Redstone_Simulator.BlockType.AIR;
            this.blockView.Size = new System.Drawing.Size(813, 816);
            this.blockView.StatusStrip = null;
            this.blockView.TabIndex = 0;

            this.outerPanel.AutoScroll = true;
            this.outerPanel.AutoSize = true;
            this.outerPanel.Controls.Add(this.blockView);
            this.outerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outerPanel.Location = new System.Drawing.Point(0, 74);
            this.outerPanel.Name = "outerPanel";
            this.outerPanel.Size = new System.Drawing.Size(888, 547);
            this.outerPanel.TabIndex = 3;
        }
        private void InitializeComponent()
        {
            System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
            this.loadItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.quitItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbZoomIn = new System.Windows.Forms.ToolStripButton();
            this.tsbZoomOut = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbAddRowTop = new System.Windows.Forms.ToolStripButton();
            this.tsbAddRowBottom = new System.Windows.Forms.ToolStripButton();
            this.tsbAddColLeft = new System.Windows.Forms.ToolStripButton();
            this.tsbAddColRight = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbModifyExtents = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.outerPanel = new Redstone_Simulator.PanelScrollFix();
            this.blockView = new Redstone_Simulator.BlockView();
            this.blockStatusStrip = new Redstone_Simulator.BlockStatusStrip();
            this.tsbUpOneLevel = new System.Windows.Forms.ToolStripButton();
            this.tsbDownOneLevel = new System.Windows.Forms.ToolStripButton();
            fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.outerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadItem,
            this.saveItem,
            this.toolStripMenuItem1,
            this.quitItem});
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // loadItem
            // 
            this.loadItem.Name = "loadItem";
            this.loadItem.Size = new System.Drawing.Size(100, 22);
            this.loadItem.Text = "Load";
            this.loadItem.Click += new System.EventHandler(this.loadItem_Click);
            // 
            // saveItem
            // 
            this.saveItem.Name = "saveItem";
            this.saveItem.Size = new System.Drawing.Size(100, 22);
            this.saveItem.Text = "Save";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(97, 6);
            // 
            // quitItem
            // 
            this.quitItem.Name = "quitItem";
            this.quitItem.Size = new System.Drawing.Size(100, 22);
            this.quitItem.Text = "Quit";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(888, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStrip
            // 
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip.Location = new System.Drawing.Point(0, 24);
            this.toolStrip.MinimumSize = new System.Drawing.Size(0, 50);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(888, 50);
            this.toolStrip.TabIndex = 2;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbZoomIn,
            this.tsbZoomOut,
            this.toolStripSeparator1,
            this.tsbAddRowTop,
            this.tsbAddRowBottom,
            this.tsbAddColLeft,
            this.tsbAddColRight,
            this.toolStripSeparator2,
            this.tsbModifyExtents,
            this.toolStripSeparator3,
            this.tsbUpOneLevel,
            this.tsbDownOneLevel});
            this.toolStrip1.Location = new System.Drawing.Point(0, 74);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(888, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbZoomIn
            // 
            this.tsbZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbZoomIn.Image = global::Redstone_Simulator.Properties.Resources.ZoomIn16;
            this.tsbZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbZoomIn.Name = "tsbZoomIn";
            this.tsbZoomIn.Size = new System.Drawing.Size(23, 22);
            this.tsbZoomIn.Text = "Zoom in";
            this.tsbZoomIn.Click += new System.EventHandler(this.tsbZoomIn_Click);
            // 
            // tsbZoomOut
            // 
            this.tsbZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbZoomOut.Image = global::Redstone_Simulator.Properties.Resources.ZoomOut16;
            this.tsbZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbZoomOut.Name = "tsbZoomOut";
            this.tsbZoomOut.Size = new System.Drawing.Size(23, 22);
            this.tsbZoomOut.Text = "Zoom out";
            this.tsbZoomOut.Click += new System.EventHandler(this.tsbZoomOut_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbAddRowTop
            // 
            this.tsbAddRowTop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAddRowTop.Image = global::Redstone_Simulator.Properties.Resources.GrowTop16;
            this.tsbAddRowTop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddRowTop.Name = "tsbAddRowTop";
            this.tsbAddRowTop.Size = new System.Drawing.Size(23, 22);
            this.tsbAddRowTop.Text = "Add row to top";
            this.tsbAddRowTop.Click += new System.EventHandler(this.tsbAddTop_Click);
            // 
            // tsbAddRowBottom
            // 
            this.tsbAddRowBottom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAddRowBottom.Image = global::Redstone_Simulator.Properties.Resources.GrowBottom16;
            this.tsbAddRowBottom.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddRowBottom.Name = "tsbAddRowBottom";
            this.tsbAddRowBottom.Size = new System.Drawing.Size(23, 22);
            this.tsbAddRowBottom.Text = "Add row to bottom";
            this.tsbAddRowBottom.Click += new System.EventHandler(this.tsbAddBottom_Click);
            // 
            // tsbAddColLeft
            // 
            this.tsbAddColLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAddColLeft.Image = global::Redstone_Simulator.Properties.Resources.GrowLeft16;
            this.tsbAddColLeft.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddColLeft.Name = "tsbAddColLeft";
            this.tsbAddColLeft.Size = new System.Drawing.Size(23, 22);
            this.tsbAddColLeft.Text = "Add column to left";
            this.tsbAddColLeft.Click += new System.EventHandler(this.tsbAddLeft_Click);
            // 
            // tsbAddColRight
            // 
            this.tsbAddColRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAddColRight.Image = global::Redstone_Simulator.Properties.Resources.GrowRight16;
            this.tsbAddColRight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddColRight.Name = "tsbAddColRight";
            this.tsbAddColRight.Size = new System.Drawing.Size(23, 22);
            this.tsbAddColRight.Text = "Add column to right";
            this.tsbAddColRight.Click += new System.EventHandler(this.tsbAddRight_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbModifyExtents
            // 
            this.tsbModifyExtents.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbModifyExtents.Image = global::Redstone_Simulator.Properties.Resources.GrowBack16;
            this.tsbModifyExtents.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbModifyExtents.Name = "tsbModifyExtents";
            this.tsbModifyExtents.Size = new System.Drawing.Size(23, 22);
            this.tsbModifyExtents.Text = "Modify extents";
            this.tsbModifyExtents.Click += new System.EventHandler(this.tsbAddMultiDimension_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // outerPanel
            // 
            this.outerPanel.AutoScroll = true;
            this.outerPanel.AutoSize = true;
            this.outerPanel.Controls.Add(this.blockView);
            this.outerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outerPanel.Location = new System.Drawing.Point(0, 74);
            this.outerPanel.Name = "outerPanel";
            this.outerPanel.Size = new System.Drawing.Size(888, 547);
            this.outerPanel.TabIndex = 3;
            // 
            // blockView
            // 
            this.blockView.AutoScroll = true;
            this.blockView.AutoScrollOffset = new System.Drawing.Point(10, 10);
            this.blockView.AutoSize = true;
            this.blockView.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.blockView.Floor = 0;
            this.blockView.Location = new System.Drawing.Point(0, 3);
            this.blockView.MinimumSize = new System.Drawing.Size(813, 813);
            this.blockView.Name = "blockView";
            this.blockView.selectedBlock = Redstone_Simulator.BlockType.AIR;
            this.blockView.Size = new System.Drawing.Size(813, 813);
            this.blockView.StatusStrip = null;
            this.blockView.TabIndex = 0;
            // 
            // blockStatusStrip
            // 
            this.blockStatusStrip.BackColor = System.Drawing.Color.White;
            this.blockStatusStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.blockStatusStrip.Location = new System.Drawing.Point(0, 621);
            this.blockStatusStrip.Name = "blockStatusStrip";
            this.blockStatusStrip.Size = new System.Drawing.Size(888, 23);
            this.blockStatusStrip.TabIndex = 0;
            this.blockStatusStrip.Text = "blockStatusStrip1";
            // 
            // tsbUpOneLevel
            // 
            this.tsbUpOneLevel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbUpOneLevel.Image = global::Redstone_Simulator.Properties.Resources.Up16;
            this.tsbUpOneLevel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbUpOneLevel.Name = "tsbUpOneLevel";
            this.tsbUpOneLevel.Size = new System.Drawing.Size(23, 22);
            this.tsbUpOneLevel.Text = "Up One Level";
            this.tsbUpOneLevel.Click += new System.EventHandler(this.upOneLevel_Click);
            // 
            // tsbDownOneLevel
            // 
            this.tsbDownOneLevel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDownOneLevel.Image = global::Redstone_Simulator.Properties.Resources.Down16;
            this.tsbDownOneLevel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDownOneLevel.Name = "tsbDownOneLevel";
            this.tsbDownOneLevel.Size = new System.Drawing.Size(23, 22);
            this.tsbDownOneLevel.Text = "Down One Level";
            this.tsbDownOneLevel.Click += new System.EventHandler(this.downOneLevel_Click);
            // 
            // frmMain
            // 
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(888, 644);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.outerPanel);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.blockStatusStrip);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.Text = "Redston Simulator";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.outerPanel.ResumeLayout(false);
            this.outerPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void loadItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog diag = new OpenFileDialog();
            if (diag.ShowDialog() == DialogResult.OK)
            {
                blockView.LoadSim(diag.FileName);
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.outerPanel.Size = new Size(this.outerPanel.Width, this.outerPanel.Height - this.toolStrip1.Size.Height);
        }

        private void upOneLevel_Click(object sender, EventArgs e)
        {
            if (this.blockView.Floor < this.blockView.currentSim.Z - 1)
            {
                this.blockView.AddLayer();
            }
            this.blockView.Floor += 1;
            this.blockStatusStrip.setLayer(this.blockView.Floor);
            this.Refresh();
        }

        private void downOneLevel_Click(object sender, EventArgs e)
        {
            if (this.blockView.Floor > 1)
            {
                this.blockView.Floor -= 1;
            }
            this.blockStatusStrip.setLayer(this.blockView.Floor);
            this.Refresh();
        }


    }
}

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
        private Panel outerPanel;
        private BlockView blockView;
        frmAddRowsCols _addRowsCols;

        // Hack till I can fix designer
      
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
        /*void customInitalize()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbZoomIn = new System.Windows.Forms.ToolStripButton();
            this.tsbZoomOut = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbAddTop = new System.Windows.Forms.ToolStripButton();
            this.tsbAddBottom = new System.Windows.Forms.ToolStripButton();
            this.tsbAddLeft = new System.Windows.Forms.ToolStripButton();
            this.tsbAddRight = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbAddMultiDimension = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1156, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            this.menuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exportToolStripMenuItem,
            this.toolStripMenuItem2,
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.newToolStripMenuItem.Text = "New";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(104, 6);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.exportToolStripMenuItem.Text = "Export";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(104, 6);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripSeparator1,
            this.tsbZoomIn,
            this.tsbZoomOut,
            this.toolStripSeparator2,
            this.tsbAddTop,
            this.tsbAddBottom,
            this.tsbAddLeft,
            this.tsbAddRight,
            this.toolStripSeparator3,
            this.tsbAddMultiDimension,
            this.toolStripSeparator4});
            this.toolStrip.Location = new System.Drawing.Point(0, 24);
            this.toolStrip.MinimumSize = new System.Drawing.Size(0, 32);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(1156, 32);
            this.toolStrip.TabIndex = 2;
            this.toolStrip.Text = "toolStrip1";
            this.toolStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 29);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 29);
            this.toolStripButton2.Text = "Tick";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 32);
            // 
            // tsbZoomIn
            // 
            this.tsbZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbZoomIn.Image = global::Redstone_Simulator.Properties.Resources.ZoomIn16;
            this.tsbZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbZoomIn.Name = "tsbZoomIn";
            this.tsbZoomIn.Size = new System.Drawing.Size(23, 29);
            this.tsbZoomIn.Text = "Zoom In";
            this.tsbZoomIn.Click += new System.EventHandler(this.tsbZoomIn_Click);
            // 
            // tsbZoomOut
            // 
            this.tsbZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbZoomOut.Image = global::Redstone_Simulator.Properties.Resources.ZoomOut16;
            this.tsbZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbZoomOut.Name = "tsbZoomOut";
            this.tsbZoomOut.Size = new System.Drawing.Size(23, 29);
            this.tsbZoomOut.Text = "Zoom Out";
            this.tsbZoomOut.Click += new System.EventHandler(this.tsbZoomOut_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 32);
            // 
            // tsbAddTop
            // 
            this.tsbAddTop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAddTop.Image = global::Redstone_Simulator.Properties.Resources.GrowTop16;
            this.tsbAddTop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddTop.Name = "tsbAddTop";
            this.tsbAddTop.Size = new System.Drawing.Size(23, 29);
            this.tsbAddTop.Text = "Add row to top";
            this.tsbAddTop.Click += new System.EventHandler(this.tsbAddTop_Click);
            // 
            // tsbAddBottom
            // 
            this.tsbAddBottom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAddBottom.Image = global::Redstone_Simulator.Properties.Resources.GrowBottom16;
            this.tsbAddBottom.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddBottom.Name = "tsbAddBottom";
            this.tsbAddBottom.Size = new System.Drawing.Size(23, 29);
            this.tsbAddBottom.Text = "Add row to bottom";
            this.tsbAddBottom.Click += new System.EventHandler(this.tsbAddBottom_Click);
            // 
            // tsbAddLeft
            // 
            this.tsbAddLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAddLeft.Image = global::Redstone_Simulator.Properties.Resources.GrowLeft16;
            this.tsbAddLeft.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddLeft.Name = "tsbAddLeft";
            this.tsbAddLeft.Size = new System.Drawing.Size(23, 29);
            this.tsbAddLeft.Text = "Add column to left";
            this.tsbAddLeft.Click += new System.EventHandler(this.tsbAddLeft_Click);
            // 
            // tsbAddRight
            // 
            this.tsbAddRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAddRight.Image = global::Redstone_Simulator.Properties.Resources.GrowRight16;
            this.tsbAddRight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddRight.Name = "tsbAddRight";
            this.tsbAddRight.Size = new System.Drawing.Size(23, 29);
            this.tsbAddRight.Text = "Add column to right";
            this.tsbAddRight.Click += new System.EventHandler(this.tsbAddRight_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 32);
            // 
            // tsbAddMultiDimension
            // 
            this.tsbAddMultiDimension.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAddMultiDimension.Image = global::Redstone_Simulator.Properties.Resources.GrowFront16;
            this.tsbAddMultiDimension.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddMultiDimension.Name = "tsbAddMultiDimension";
            this.tsbAddMultiDimension.Size = new System.Drawing.Size(23, 29);
            this.tsbAddMultiDimension.Text = "Add Multiple Dimensions";
            this.tsbAddMultiDimension.Click += new System.EventHandler(this.tsbAddMultiDimension_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 32);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1156, 629);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseClick);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);

        }*/
        public frmMain()
        {
            InitializeComponent();
            MakeNewToolStipButtons();
           // this.AutoScroll = true;
           // makeCustomControls();
            MakeIcons();
            // Fucking hate 2010 designer

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
            Bitmap[] pics = getFoorOneTiles();
            ToolStripButton[] b = new ToolStripButton[pics.Length];

            for (int i = 0; i < b.Length; i++)
            {
                b[i] = new ToolStripButton();
                b[i].DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                b[i].Image = pics[i];
                b[i].Name = "BlockSelect_" + i;
                b[i].Size = new System.Drawing.Size(23, 29);
                b[i].Text = "BlockSelect_" + i;
                b[i].Tag = (BlockType)i;
                b[i].Click += new System.EventHandler(this.toolStrip_SelectBlock);
            }
            toolStrip.Items.AddRange(b);


        
        }

        void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            this.MouseWheel += new MouseEventHandler(Form1_MouseWheel);
            
       //     this.blockView.blockSelect = blockSelect;
          //  if (e.Delta > 0 | e.Delta < 0)
            //    blockSelect.moveSelect(e.Delta > 0 ? 1 : -1);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
         //   blockSelect.Left = (this.Width - blockSelect.Width) / 2;
        //    if (blockSelect.Left < 0) blockSelect.Left = 0;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
         //   if (e.Delta > 0 | e.Delta < 0)
          //      blockSelect.moveSelect(e.Delta);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void blockView1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
        //    blockSelect.Left = (this.Width - blockSelect.Width) / 2;
        //    if (blockSelect.Left < 0) blockSelect.Left = 0;
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (running)
            {
                running = false;
               // toolStripButton1.Image = goImage;
                time.Stop();
            }
            else
            {
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
        public static Bitmap[] getFoorOneTiles()
        {
            // make them 50x50
            BlockDrawSettings bd;
            Size s = new Size(10 * ConstScale, 10 * ConstScale);
            Point p = new Point(0, 0);
            Graphics g;
            Bitmap[] bArray = new Bitmap[8];
            
   
            // Air
            bArray[0] = new Bitmap(s.Width, s.Height);
            g = setGraphics(bArray[0]);
            bd = BlockDrawSettings.New(BlockType.AIR);
            BlockImages.gDrawBlock(g, new Rectangle(p, s), bd);

            // Block
            bArray[1] = new Bitmap(s.Width, s.Height);
            g = setGraphics(bArray[1]);
            bd = BlockDrawSettings.New(BlockType.BLOCK);
            BlockImages.gDrawBlock(g, new Rectangle(p, s), bd);

            // Wire 
            bArray[2] = new Bitmap(s.Width, s.Height);
            g = setGraphics(bArray[2]);
            bd = BlockDrawSettings.New(BlockType.WIRE, 16);
            bd.Mask = WireMask.AllDir;
            BlockImages.gDrawBlock(g, new Rectangle(p, s), bd);

            // Torch 
            bArray[3] = new Bitmap(s.Width, s.Height);
            g = setGraphics(bArray[3]);
            bd = BlockDrawSettings.New(BlockType.TORCH, 16);
            BlockImages.gDrawBlock(g, new Rectangle(p, s), bd);

        // Repeater 
            bArray[7] = new Bitmap(s.Width, s.Height);
            g = setGraphics(bArray[7]);
            bd = BlockDrawSettings.New(BlockType.REPEATER, 0, Direction.NORTH, 0);
            BlockImages.gDrawBlock(g, new Rectangle(p, s), bd);

        // Button 
            bArray[5] = new Bitmap(s.Width, s.Height);
            g = setGraphics(bArray[5]);
            bd = BlockDrawSettings.New(BlockType.BUTTON, 0, Direction.NORTH, 0);
            BlockImages.gDrawBlock(g, new Rectangle(p, s), bd);

            // Lever 
            bArray[4] = new Bitmap(s.Width, s.Height);
            g = setGraphics(bArray[4]);
            bd = BlockDrawSettings.New(BlockType.LEVER, 0, Direction.NORTH, 0);
            BlockImages.gDrawBlock(g, new Rectangle(p, s), bd);

            

            // Plate 
            bArray[6] = new Bitmap(s.Width, s.Height);
            g = setGraphics(bArray[6]);
            bd = BlockDrawSettings.New(BlockType.PREASUREPAD);
            BlockImages.gDrawBlock(g, new Rectangle(p, s), bd);

            

            return bArray;

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void InitializeComponent()
        {
            System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.loadItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.quitItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.outerPanel = new System.Windows.Forms.Panel();
            this.blockView = new Redstone_Simulator.BlockView();
            this.blockStatusStrip = new Redstone_Simulator.BlockStatusStrip();
            fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.outerPanel.SuspendLayout();
            this.SuspendLayout();
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
            // toolStrip
            // 
            this.toolStrip.Location = new System.Drawing.Point(0, 24);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(888, 25);
            this.toolStrip.TabIndex = 2;
            this.toolStrip.Text = "toolStrip1";
            // 
            // outerPanel
            // 
            this.outerPanel.AutoScroll = true;
            this.outerPanel.AutoSize = true;
            this.outerPanel.Controls.Add(this.blockView);
            this.outerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outerPanel.Location = new System.Drawing.Point(0, 49);
            this.outerPanel.Name = "outerPanel";
            this.outerPanel.Size = new System.Drawing.Size(888, 572);
            this.outerPanel.TabIndex = 3;
            // 
            // blockView
            // 
            this.blockView.AutoScrollOffset = new System.Drawing.Point(10, 10);
            this.blockView.Floor = 0;
            this.blockView.Location = new System.Drawing.Point(0, 0);
            this.blockView.Name = "blockView";
            this.blockView.selectedBlock = Redstone_Simulator.BlockType.AIR;
            this.blockView.Size = new System.Drawing.Size(742, 816);
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
            // frmMain
            // 
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(888, 644);
            this.Controls.Add(this.outerPanel);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.blockStatusStrip);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.Text = "Redston Simulator";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.outerPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        // Backup before stupid designer screw up
       /* private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbZoomIn = new System.Windows.Forms.ToolStripButton();
            this.tsbZoomOut = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbAddTop = new System.Windows.Forms.ToolStripButton();
            this.tsbAddBottom = new System.Windows.Forms.ToolStripButton();
            this.tsbAddLeft = new System.Windows.Forms.ToolStripButton();
            this.tsbAddRight = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbAddMultiDimension = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1156, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exportToolStripMenuItem,
            this.toolStripMenuItem2,
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.newToolStripMenuItem.Text = "New";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(104, 6);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.exportToolStripMenuItem.Text = "Export";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(104, 6);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripSeparator1,
            this.tsbZoomIn,
            this.tsbZoomOut,
            this.toolStripSeparator2,
            this.tsbAddTop,
            this.tsbAddBottom,
            this.tsbAddLeft,
            this.tsbAddRight,
            this.toolStripSeparator3,
            this.tsbAddMultiDimension,
            this.toolStripSeparator4});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.MinimumSize = new System.Drawing.Size(0, 32);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1156, 32);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 29);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 29);
            this.toolStripButton2.Text = "Tick";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 32);
            // 
            // tsbZoomIn
            // 
            this.tsbZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbZoomIn.Image = global::Redstone_Simulator.Properties.Resources.ZoomIn16;
            this.tsbZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbZoomIn.Name = "tsbZoomIn";
            this.tsbZoomIn.Size = new System.Drawing.Size(23, 29);
            this.tsbZoomIn.Text = "Zoom In";
            this.tsbZoomIn.Click += new System.EventHandler(this.tsbZoomIn_Click);
            // 
            // tsbZoomOut
            // 
            this.tsbZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbZoomOut.Image = global::Redstone_Simulator.Properties.Resources.ZoomOut16;
            this.tsbZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbZoomOut.Name = "tsbZoomOut";
            this.tsbZoomOut.Size = new System.Drawing.Size(23, 29);
            this.tsbZoomOut.Text = "Zoom Out";
            this.tsbZoomOut.Click += new System.EventHandler(this.tsbZoomOut_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 32);
            // 
            // tsbAddTop
            // 
            this.tsbAddTop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAddTop.Image = global::Redstone_Simulator.Properties.Resources.GrowTop16;
            this.tsbAddTop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddTop.Name = "tsbAddTop";
            this.tsbAddTop.Size = new System.Drawing.Size(23, 29);
            this.tsbAddTop.Text = "Add row to top";
            this.tsbAddTop.Click += new System.EventHandler(this.tsbAddTop_Click);
            // 
            // tsbAddBottom
            // 
            this.tsbAddBottom.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAddBottom.Image = global::Redstone_Simulator.Properties.Resources.GrowBottom16;
            this.tsbAddBottom.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddBottom.Name = "tsbAddBottom";
            this.tsbAddBottom.Size = new System.Drawing.Size(23, 29);
            this.tsbAddBottom.Text = "Add row to bottom";
            this.tsbAddBottom.Click += new System.EventHandler(this.tsbAddBottom_Click);
            // 
            // tsbAddLeft
            // 
            this.tsbAddLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAddLeft.Image = global::Redstone_Simulator.Properties.Resources.GrowLeft16;
            this.tsbAddLeft.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddLeft.Name = "tsbAddLeft";
            this.tsbAddLeft.Size = new System.Drawing.Size(23, 29);
            this.tsbAddLeft.Text = "Add column to left";
            this.tsbAddLeft.Click += new System.EventHandler(this.tsbAddLeft_Click);
            // 
            // tsbAddRight
            // 
            this.tsbAddRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAddRight.Image = global::Redstone_Simulator.Properties.Resources.GrowRight16;
            this.tsbAddRight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddRight.Name = "tsbAddRight";
            this.tsbAddRight.Size = new System.Drawing.Size(23, 29);
            this.tsbAddRight.Text = "Add column to right";
            this.tsbAddRight.Click += new System.EventHandler(this.tsbAddRight_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 32);
            // 
            // tsbAddMultiDimension
            // 
            this.tsbAddMultiDimension.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAddMultiDimension.Image = global::Redstone_Simulator.Properties.Resources.GrowFront16;
            this.tsbAddMultiDimension.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddMultiDimension.Name = "tsbAddMultiDimension";
            this.tsbAddMultiDimension.Size = new System.Drawing.Size(23, 29);
            this.tsbAddMultiDimension.Text = "Add Multiple Dimensions";
            this.tsbAddMultiDimension.Click += new System.EventHandler(this.tsbAddMultiDimension_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 32);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1156, 629);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseClick);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }*/
    }
}

namespace Redstone_Simulator
{
    partial class BlockView 
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Display = new System.Windows.Forms.PictureBox();
            this.outerPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.Display)).BeginInit();
            this.outerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Display
            // 
            this.Display.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Display.Location = new System.Drawing.Point(0, 0);
            this.Display.Name = "Display";
            this.Display.Size = new System.Drawing.Size(921, 539);
            this.Display.TabIndex = 0;
            this.Display.TabStop = false;
            this.Display.Paint += new System.Windows.Forms.PaintEventHandler(this.Display_Paint);
            this.Display.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Display_MouseDown);
            this.Display.MouseEnter += new System.EventHandler(this.Display_MouseEnter);
            this.Display.MouseLeave += new System.EventHandler(this.Display_MouseLeave);
            this.Display.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Display_MouseMove);
            this.Display.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Display_MouseUp);
            // 
            // outerPanel
            // 
            this.outerPanel.AutoScroll = true;
            this.outerPanel.AutoSize = true;
            this.outerPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.outerPanel.Controls.Add(this.Display);
            this.outerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outerPanel.Location = new System.Drawing.Point(0, 0);
            this.outerPanel.Name = "outerPanel";
            this.outerPanel.Size = new System.Drawing.Size(921, 539);
            this.outerPanel.TabIndex = 2;
            // 
            // BlockView
            // 
            this.Controls.Add(this.outerPanel);
            this.Name = "BlockView";
            this.Size = new System.Drawing.Size(921, 539);
            this.Load += new System.EventHandler(this.BlockView_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BlockView_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.BlockView_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.BlockView_KeyUp);
            this.Resize += new System.EventHandler(this.BlockView_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.Display)).EndInit();
            this.outerPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox Display;
   
        private System.Windows.Forms.Panel outerPanel;
    }
}

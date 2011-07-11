using System;
using System.Drawing;
using System.Windows.Forms;
using NUnit.Framework;
using LibNbt.Tags;
using LibNbt;
using System.IO;

namespace MonoRedstoneSim
{
	class MForm : Form 
	{
   		public MForm() {
      		Text = "Simple menu";
       		MenuStrip ms = new MenuStrip();
       		ms.Parent = this;
        	this.Load = EventHandler(Form1_Load);
       		ToolStripMenuItem file = new ToolStripMenuItem("&File");          
       		ToolStripMenuItem exit = new ToolStripMenuItem("&Exit", null,
           	new EventHandler(OnExit));          
       		exit.ShortcutKeys = Keys.Control | Keys.X;
       		file.DropDownItems.Add(exit);

       		ms.Items.Add(file);
       		MainMenuStrip = ms;
       		Size = new Size(250, 200);

       		CenterToScreen();
   		}
		
		 private void Form1_Load(object sender, EventArgs e)
        {
            NbtFile test = new NbtFile("C:\\Users\\Paul Bruner\\Desktop\\Ol Drive\\alu-new-sixteen-bits.schematic",true);
            test.LoadFile();

            redBmp = new redstoneBmp();

        }
   		void OnExit(object sender, EventArgs e) {
     			Close();
   		}
	}

	class MApplication 
	{
    	public static void Main() 
		{
        	Application.Run(new MForm());
    	}
	}
}


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NUnit.Framework;
using LibNbt.Tags;
using LibNbt;
using System.IO;

namespace Mincraft_Simulator
{
    public partial class Form1 : Form
    {
        bool paintIt = false;
        redstoneBmp redBmp;
        redstoneObj[] rGrid;
        int rXmax;
        int rYmax;
        int rZmax;
        int currentZ = 0;
        string rMaterials;
        byte[] rBlocks;
        byte[] rData;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            NbtFile test = new NbtFile("C:\\Users\\Paul Bruner\\Desktop\\Ol Drive\\alu-new-sixteen-bits.schematic",true);
            test.LoadFile();

            redBmp = new redstoneBmp();

        }

        private void AssertNbtBigFile(NbtFile file)
        {
           // NbtList enities;
            //NbtList tileenities;
            
            // See TestFiles/bigtest.nbt.txt to see the expected format
  

            NbtCompound root = file.RootTag;
            if (root.Name == "Schematic")
            {
                redBmp = new redstoneBmp();
                // I don't know why but I keep getting cofused with the height, width, length stuff
                // maybe its because I deal with to many 2d objects.
                rYmax = root.Query<NbtShort>("/Schematic/Width").Value;
                rZmax = root.Query<NbtShort>("/Schematic/Height").Value;
                rXmax = root.Query<NbtShort>("/Schematic/Length").Value;
                rMaterials = root.Query<NbtString>("/Schematic/Materials").Value;
                rBlocks = root.Query<NbtByteArray>("/Schematic/Blocks").Value;
                rData = root.Query<NbtByteArray>("/Schematic/Data").Value;
                rGrid = new redstoneObj[rBlocks.Length]; 

                // Ok, lets get this link list a starting
                for (int i = 0; i < rBlocks.Length; i++)
                {
                    // Put the normal code here to fill the data for the blocks, and data
                    // skipping for now as its more important to get the link list working
                }

                // Just so I can get a good visual on it going to precaculate the numbers
                int xStride = rXmax;
                int zStride = xStride * rYmax;
                
                // Precaculate the last line in a floor and the last floor
                int zLast = rData.Length - zStride;
                int yLast = zStride - xStride;

                // Lets try something diffrent and do all bounds checking

                for (int i = 0; i < rBlocks.Length; i++)
                {
                    
                    int z =  i/zStride;
                    int y = (i - z * zStride)/xStride;
                    int x = i - (y * xStride) - (z * zStride);
                    rGrid[i] = new redstoneObj(redBmp.getSet(rBlocks[i]));
                    rGrid[i].X = x; rGrid[i].Y = y; rGrid[i].Z = z;
                }
            }

        }


        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string filename1 = "";
            //DDS test;           
            openFileDialog.Filter = "Schematic Files .schematic | *.schematic";
            openFileDialog.Title = "Select a Schematic";
            openFileDialog.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                NbtFile test = new NbtFile(openFileDialog.FileName, true);
                test.LoadFile();
                AssertNbtBigFile(test);
                paintIt = true;
                this.Width = rXmax * 20;
                this.Height = rYmax * 20;
                this.Refresh();

                Console.WriteLine("CLICK!");
            }
            
        }

        private void upToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentZ += 1;
            if (currentZ > rZmax) currentZ = rZmax;
            this.Refresh();
        }

        private void downToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentZ -= 1;
            if (currentZ < 0) currentZ = 0;
            this.Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            int yoffset = 50;
            if (paintIt)
            {
                // Just so I can get a good visual on it going to precaculate the numbers
                int xStride = rXmax;
                int zStride = xStride * rYmax;

                // Precaculate the last line in a floor and the last floor
                int zLast = rData.Length - zStride;
                int yLast = zStride - xStride;

                int bmpWidth = rXmax * (20 + 2); // With of the square + 2 for the lin left and rigg
                int bmpLength = rYmax * (20 + 2);

                e.Graphics.Clear(Color.White);
                for (int i = 0; i < rBlocks.Length; i++)
                {
                    redstoneObj temp = rGrid[i];
                    if (temp.Z == currentZ)
                    {
                        e.Graphics.DrawImage(temp.getBitmap(rData[i]), temp.X * 20, temp.Y * 20 + yoffset);
                        e.Graphics.DrawRectangle(Pens.Black, temp.X * 20, temp.Y * 20 + yoffset, 20, 20);
                    }

                }

            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'w')
            {
                currentZ += 1;
                if (currentZ > rZmax) currentZ = rZmax;
                this.Refresh();
            }
            if (e.KeyChar == 's')
            {
                currentZ -= 1;
                if (currentZ < 0) currentZ = 0;
                this.Refresh();
            }
        }
      
    }
}

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

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Mincraft_Simulator
{
    public partial class Form1 : Form
    {
        bool mouseDown = false;
        bool paintIt = false;
        bool formResize = true;
        bool glLoaded = true;
        Matrix4 lookat;
        redstoneBmp redBmp;
        gBlockTypeStruct[] rGrid;
        int rXmax;
        int rYmax;
        int rZmax;
        int currentZ = 0;
        string rMaterials;
        byte[] rBlocks;
        byte[] rData;
        int zoom=1;
        int currentX=0, currentY=0;
        int texTGrid;
        int texSGrid;
        int prevMouseX, prevMouseY;

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
                rGrid = new gBlockTypeStruct[rBlocks.Length]; 

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
                    rGrid[i] = redBmp.getSet(rBlocks[i]);
                    rGrid[i].X = x; rGrid[i].Y = y; rGrid[i].Z = z;
                }
            }

        }


        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
      
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


        private void glControl_Load(object sender, EventArgs e)
        {
            glLoaded = true;



            lookat = glHelper.setupLookAtOverhead();
            GL.ClearColor(Color.LightSkyBlue);
           // GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);

            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
        }

        private void Form1_ResizeBegin(object sender, EventArgs e)
        {
            formResize = true;
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            formResize = false;
            
        }

        private void glControl_Resize(object sender, EventArgs e)
        {
            if (!glLoaded) return;

            GL.Viewport(0, 0, glControl.Width, glControl.Height);
        //    Matrix4 pov = Matrix4.CreateOrthographicOffCenter(-10 + currentX, 10 + currentX, -10 + currentY, -10 + currentY, 1, -1);
          //  GL.MatrixMode(MatrixMode.Projection);
         //   GL.LoadMatrix(ref pov);
           // lookat = pov;
			
           // base.OnResize(e);
           // float fov = MathHelper.PiOver4;
           /// float aspect_ratio = Width / (float)Height;


          //  Matrix4 pov = Matrix4.CreatePerspectiveFieldOfView(fov, aspect_ratio, 1f, 1000f);
          //  GL.MatrixMode(MatrixMode.Projection);
           // GL.LoadMatrix(ref pov);
          //  GL.MatrixMode(MatrixMode.Projection);
         //   GL.LoadIdentity();
         //   GL.Ortho(0, 100, 0, 100, -1.0, 1.0);

            glControl.Invalidate();
        }
        private void MakeGrid()
        {

        }
        private void glControl_Paint(object sender, PaintEventArgs e)
        {
            if (!glLoaded) return;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
          // // GL.MatrixMode(MatrixMode.Projection);
            
            //Matrix4 pov = Matrix4.CreateOrthographicOffCenter(0 , 10 , 0 , 10 , -1, 1);
          //  Matrix4 pov = Matrix4.CreateOrthographic(this.Width, this.Height, -1, 1);
         //   GL.MatrixMode(MatrixMode.Projection);
         //   GL.LoadMatrix(ref pov);

        //   // GL.Translate(currentX, currentY, 0);
        //    GL.MatrixMode(MatrixMode.Modelview);
         //   GL.LoadIdentity();
            if (mouseDown)
            {
                drawRect(prevMouseX, prevMouseY, currentX, currentY);
            }
            GL.Begin(BeginMode.Quads);
                GL.Color3(Color.Green); //black
                GL.Vertex2(0, 0);
                GL.Vertex2(1, 0);
                GL.Vertex2(1, 1);
                GL.Vertex2(0, 1);
            GL.End();
            
            

            glControl.SwapBuffers();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (formResize)
            {
                glControl.Height = this.Height;
                glControl.Width = this.Width;
            }
        }

        private void glControl_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            prevMouseX = e.X;
            prevMouseY = e.Y;
        }

        private void glControl_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
        void drawRect(int x1, int y1, int x2, int y2)
        {
            GL.Enable(EnableCap.ColorLogicOp);
            GL.LogicOp(LogicOp.Xor);
            GL.Color4(1.0f, 1.0f, 1.0f, 1.0f);
            GL.PolygonMode(MaterialFace.Front, PolygonMode.Line);
            GL.Rect(x1, this.Height - y1, x2, this.Width - y2);
            GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill);
            GL.Disable(EnableCap.ColorLogicOp); 
        }
        private void glControl_MouseMove(object sender, MouseEventArgs e)
        {
            int x=0, y=0;
            if (!mouseDown) return;
            if (e.Button == MouseButtons.Left)
            {
                x = prevMouseX - e.X > 0 ? 1: -1;
                y = prevMouseY - e.Y > 0 ? 1 : -1;
                currentX = e.X;
                currentY = e.Y;
                //lookat = lookat * Matrix4.CreateTranslation(x, y, 0);
                glControl.Invalidate();
            }
            else if (e.Button == MouseButtons.Right)
            {

            }
            else if (e.Button == MouseButtons.Middle)
            {

            }
            
        }
      
    }
}

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
using System.Drawing.Imaging;
using System.Diagnostics;


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
        bool loaded = false;
        float[] positionz = new float[10];
        float[] positionx = new float[10];
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //NbtFile test = new NbtFile("C:\\Users\\Paul Bruner\\Desktop\\Ol Drive\\alu-new-sixteen-bits.schematic",true);
           // test.LoadFile();

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

        private Bitmap CreateGrid(int zLevel)
        {
            // Just so I can get a good visual on it going to precaculate the numbers
            int xStride = rXmax;
            int zStride = xStride * rYmax;

            int bmpWidth = rXmax * 20; // With of the square + 2 for the lin left and rigg
            int bmpLength = rYmax * 20;
            Bitmap buffer = new Bitmap(bmpWidth, bmpLength, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(buffer);

            g.Clear(Color.White);

            for (int i = 0; i < rBlocks.Length; i++)
            {
                redstoneObj temp = rGrid[i];
                if (temp.Z == zLevel)
                {
                    g.DrawImage(temp.getBitmap(rData[i]), temp.X * 20, temp.Y * 20);

                    int nZ = zLevel + 1 > rZmax ? rZmax : zLevel + 1;
              //      if (rGrid[i + zStride].bType == blockType.RedstoneWire)
                //        g.DrawImage(rGrid[i + zStride].getBitmap(rData[i + zStride]), temp.X * 20, temp.Y * 20);
                    g.DrawRectangle(Pens.Black, temp.X * 20, temp.Y * 20, 20, 20);
                }

            }
            return buffer;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (paintIt)
            {
                
               
                e.Graphics.DrawImage(CreateGrid(currentZ), 0, 0);

            }
        }
        Stopwatch sw = new Stopwatch();
        void Application_Idle(object sender, EventArgs e)
        {
            if (sw.ElapsedMilliseconds > 600 && loaded)
            {
                sw.Restart();
                glControl.Invalidate();
            }
            if (!sw.IsRunning)
                sw.Start();

        }
        void drawcube(int color, float lado)
        {

            GL.FrontFace(FrontFaceDirection.Ccw);
            GL.BindTexture(TextureTarget.Texture2D, cubeText);

            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(0.0f, 1.0f);
            GL.Vertex3(-lado, lado, lado);
            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex3(-lado, -lado, lado);
            GL.TexCoord2(1.0f, 0.0f);
            GL.Vertex3(lado, -lado, lado);
            GL.TexCoord2(1.0f, 1.0f);
            GL.Vertex3(lado, lado, lado);

            GL.TexCoord2(0.0f, 1.0f);
            GL.Vertex3(lado, lado, -lado);
            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex3(lado, -lado, -lado);
            GL.TexCoord2(1.0f, 0.0f);
            GL.Vertex3(-lado, -lado, -lado);
            GL.TexCoord2(1.0f, 1.0f);
            GL.Vertex3(-lado, lado, -lado);

            GL.TexCoord2(0.0f, 1.0f);
            GL.Vertex3(lado, lado, lado);
            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex3(lado, -lado, lado);
            GL.TexCoord2(1.0f, 0.0f);
            GL.Vertex3(lado, -lado, -lado);
            GL.TexCoord2(1.0f, 1.0f);
            GL.Vertex3(lado, lado, -lado);

            GL.TexCoord2(0.0f, 1.0f);
            GL.Vertex3(-lado, lado, -lado);
            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex3(-lado, -lado, -lado);
            GL.TexCoord2(1.0f, 0.0f);
            GL.Vertex3(-lado, -lado, lado);
            GL.TexCoord2(1.0f, 1.0f);
            GL.Vertex3(-lado, lado, lado);

            GL.TexCoord2(0.0f, 1.0f);
            GL.Vertex3(-lado, lado, lado);
            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex3(lado, lado, lado);
            GL.TexCoord2(1.0f, 0.0f);
            GL.Vertex3(lado, lado, -lado);
            GL.TexCoord2(1.0f, 1.0f);
            GL.Vertex3(-lado, lado, -lado);

            GL.TexCoord2(0.0f, 1.0f);
            GL.Vertex3(-lado, -lado, -lado);
            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex3(lado, -lado, -lado);
            GL.TexCoord2(1.0f, 0.0f);
            GL.Vertex3(lado, -lado, lado);
            GL.TexCoord2(1.0f, 1.0f);
            GL.Vertex3(-lado, -lado, lado);
            GL.End();
        }

        void Render()
        {
            	
            DrawTutorial();
        }
        #region ScrewingAround
        float tiltY, zoom, up, side;

        void display()
        {
            int x,y,z;
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.Enable(EnableCap.CullFace);
            GL.InitNames();
            GL.PushName(0);

            GL.LoadIdentity();
            GL.Translate(side, 0, zoom - 25.0f);
            GL.Rotate(up, 1.0f, 0.0f, 0.0f);
            GL.Rotate(tiltY, 0.0f, 1.0f, 0.0f);

            int side1 = 10;
            int side2 = 10;
            int side3 = 10;

            for (x = 1; x < side1 - 1; x++)
                for (y = 1; y < side2 - 1; y++)
                    for (z = 1; z < side3 - 1; z++)
                    {
                        GL.PushMatrix();
                        GL.Translate(x - (side1 - 1) / 2.0f, y - (side2 - 1) / 2.0f,  z - (side3 - 1) / 2.0f);
                        GL.LoadName(x * side2 * side3 + y * side3 + z);
                        GL.CallList(2); // hard code it
                        //glCallList(g->campoV[x][y][z] != NUEVO ? g->campoV[x][y][z] + 1 : 1);
                        GL.PopMatrix();
                    }
            glControl.SwapBuffers();
        }
       
        #endregion
        private void glControl_Load(object sender, EventArgs e)
        {
            sw.Start();
            Application.Idle += Application_Idle;
            glControl.Width = 640;
            glControl.Height = 480;
            int w = glControl.Width;
            int h = glControl.Height;
            GL.Viewport(0, 0, w, h); // Use all of the glControl painting area

            GL.Enable(EnableCap.Texture2D);
            GL.ClearColor(Color.White);
            GL.ClearDepth(1.0d);
            GL.Enable(EnableCap.DepthTest);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.LoadIdentity();
           
            cubeText= LoadTexture("Cube.bmp");
            // 29 empty lists
            GL.GenLists(29);
            for (int i = 0; i < 29; i++)
            {
                GL.NewList(i + 1, ListMode.Compile);
                drawcube(1, 0.25f);
                GL.EndList();
            }

            
            gluPerspective(60f, (float)(w / h), 1.0f, 100f);
            GL.MatrixMode(MatrixMode.Modelview);


            loaded = true;
            glControl.Refresh();
        }
        private void GLSettings()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.ClearColor(Color.White);
            GL.ClearDepth(1.0d);
            GL.Enable(EnableCap.DepthTest);
            GL.ShadeModel(ShadingModel.Smooth);
            
            
            GL.DepthFunc(DepthFunction.Lequal);
            GL.Enable(EnableCap.Light0);								// Quick And Dirty Lighting (Assumes Light0 Is Set Up)
            GL.Enable(EnableCap.Lighting);								// Enable Lighting
            GL.Enable(EnableCap.ColorMaterial);							// Enable Material Coloring
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
        }
        private void SetupViewport()
        {
            int w = glControl.Width;
            int h = glControl.Height;
            GL.Viewport(0, 0, w, h); // Use all of the glControl painting area
            GL.ClearColor(Color.White);
            GL.MatrixMode(MatrixMode.Projection);
            
        }
        void gluPerspective(float fov, float aspect, float near, float far)
        {
            double rad = (Math.PI / 180d) * fov;
            double range = near * Math.Tan(fov / 2d);
            GL.Frustum(-range * aspect, range * aspect, -range, range, near, far);
        }

        
    
        void camera()
        {
            GL.Rotate(xrot, 1f, 0f, 0f);
            GL.Rotate(yrot, 0f, 1f, 0f);
            GL.Translate(-xpos, -ypos, -zpos);
        }
        float xpos=0, ypos=0, zpos=0;

        private void DrawTutorial()
        {
            int w = glControl.Width;
            int h = glControl.Height;
            int xloop;
            int yloop;
            
            //GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.BindTexture(TextureTarget.Texture2D, cubeText);
            for (yloop = 1; yloop < 6; yloop++)							// Loop Through The Y Plane
            {
                for (xloop = 0; xloop < yloop; xloop++)					// Loop Through The X Plane
                {
                    //GL.LoadIdentity();                                  // Reset The View
                    
                    // Position The Cubes On The Screen
                    GL.PushMatrix(); //?
			        GL.Translate(1.4f+(xloop*2.8f)-(yloop)*1.4f,((6.0f-yloop)*2.4f)-7.0f,-20.0f);
                    //GL.Rotate(45.0f-(2.0f*yloop)+xrot,1.0f,0.0f,0.0f);		// Tilt The Cubes Up And Down
			        //GL.Rotate(45.0f+yrot,0.0f,1.0f,0.0f);				// Spin Cubes Left And Right
                    GL.Rotate(45.0f,1.0f,0.0f,0.0f);		// Tilt The Cubes Up And Down
                    GL.Rotate(45.0f,0.0f,1.0f,0.0f);				// Spin Cubes Left And Right
                    GL.Color3(boxcol[yloop - 1]);
                    GL.CallList(box);
                    GL.Color3(topcol[yloop - 1]);					// Select The Top Color
                    GL.CallList(top);
                    GL.PopMatrix(); //?
                }
            }
        }
        
        float[][]  boxcol = new float[5][] {
            new float[3] {1.0f,0.0f,0.0f },
            new float[3] {1.0f,0.5f,0.0f },
            new float[3] {1.0f,1.0f,0.0f },
            new float[3] {0.0f,1.0f,0.0f },
            new float[3] {0.0f,1.0f,1.0f },    
        };
      
           float[][]  topcol = new float[5][] {
            new float[3] {.5f,0.0f,0.0f },
            new float[3] {0.5f,0.25f,0.0f },
            new float[3] {0.5f,0.5f,0.0f },
            new float[3] {0.0f,0.5f,0.0f},
            new float[3] {0.0f,0.5f,0.5f },    
           };

        int box;
        int top;
        
        float xrot=0;
        float yrot=0;
        int cubeText;
        private void gluBox(float size)
        {
            GL.Begin(BeginMode.Quads);							// Start Drawing Quads
            // Bottom Face
            GL.TexCoord2(size, size); GL.Vertex3(-size, -size, -size);	// Top Right Of The Texture and Quad
            GL.TexCoord2(0.0f, size); GL.Vertex3(size, -size, -size);	// Top Left Of The Texture and Quad
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(size, -size, size);	// Bottom Left Of The Texture and Quad
            GL.TexCoord2(size, 0.0f); GL.Vertex3(-size, -size, size);	// Bottom Right Of The Texture and Quad
            // Front Face
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-size, -size, size);	// Bottom Left Of The Texture and Quad
            GL.TexCoord2(size, 0.0f); GL.Vertex3(size, -size, size);	// Bottom Right Of The Texture and Quad
            GL.TexCoord2(size, size); GL.Vertex3(size, size, size);	// Top Right Of The Texture and Quad
            GL.TexCoord2(0.0f, size); GL.Vertex3(-size, size, size);	// Top Left Of The Texture and Quad
            // Back Face
            GL.TexCoord2(size, 0.0f); GL.Vertex3(-size, -size, -size);	// Bottom Right Of The Texture and Quad
            GL.TexCoord2(size, size); GL.Vertex3(-size, size, -size);	// Top Right Of The Texture and Quad
            GL.TexCoord2(0.0f, size); GL.Vertex3(size, size, -size);	// Top Left Of The Texture and Quad
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(size, -size, -size);	// Bottom Left Of The Texture and Quad
            // Right face
            GL.TexCoord2(size, 0.0f); GL.Vertex3(size, -size, -size);	// Bottom Right Of The Texture and Quad
            GL.TexCoord2(size, size); GL.Vertex3(size, size, -size);	// Top Right Of The Texture and Quad
            GL.TexCoord2(0.0f, size); GL.Vertex3(size, size, size);	// Top Left Of The Texture and Quad
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(size, -size, size);	// Bottom Left Of The Texture and Quad
            // Left Face
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-size, -size, -size);	// Bottom Left Of The Texture and Quad
            GL.TexCoord2(size, 0.0f); GL.Vertex3(-size, -size, size);	// Bottom Right Of The Texture and Quad
            GL.TexCoord2(size, size); GL.Vertex3(-size, size, size);	// Top Right Of The Texture and Quad
            GL.TexCoord2(0.0f, size); GL.Vertex3(-size, size, -size);	// Top Left Of The Texture and Quad
            // Top Face
            GL.TexCoord2(0.0f, size); GL.Vertex3(-size, size, -size);	// Top Left Of The Texture and Quad
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-size, size, size);	// Bottom Left Of The Texture and Quad
            GL.TexCoord2(size, 0.0f); GL.Vertex3(size, size, size);	// Bottom Right Of The Texture and Quad
            GL.TexCoord2(size, size); GL.Vertex3(size, size, -size);	// Top Right Of The Texture and Quad
            GL.End();

        }
        private void buildList()
        {
 
            box = GL.GenLists(2);
            GL.NewList(box, ListMode.Compile);
            GL.Begin(BeginMode.Quads);							// Start Drawing Quads
            // Bottom Face
                GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(-1.0f, -1.0f, -1.0f);	// Top Right Of The Texture and Quad
                GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(1.0f, -1.0f, -1.0f);	// Top Left Of The Texture and Quad
                GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(1.0f, -1.0f, 1.0f);	// Bottom Left Of The Texture and Quad
                GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(-1.0f, -1.0f, 1.0f);	// Bottom Right Of The Texture and Quad
            // Front Face
                GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-1.0f, -1.0f, 1.0f);	// Bottom Left Of The Texture and Quad
                GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(1.0f, -1.0f, 1.0f);	// Bottom Right Of The Texture and Quad
                GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(1.0f, 1.0f, 1.0f);	// Top Right Of The Texture and Quad
                GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(-1.0f, 1.0f, 1.0f);	// Top Left Of The Texture and Quad
            // Back Face
                GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(-1.0f, -1.0f, -1.0f);	// Bottom Right Of The Texture and Quad
                GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(-1.0f, 1.0f, -1.0f);	// Top Right Of The Texture and Quad
                GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(1.0f, 1.0f, -1.0f);	// Top Left Of The Texture and Quad
                GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(1.0f, -1.0f, -1.0f);	// Bottom Left Of The Texture and Quad
            // Right face
                GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(1.0f, -1.0f, -1.0f);	// Bottom Right Of The Texture and Quad
                GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(1.0f, 1.0f, -1.0f);	// Top Right Of The Texture and Quad
                GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(1.0f, 1.0f, 1.0f);	// Top Left Of The Texture and Quad
                GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(1.0f, -1.0f, 1.0f);	// Bottom Left Of The Texture and Quad
            // Left Face
                GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-1.0f, -1.0f, -1.0f);	// Bottom Left Of The Texture and Quad
                GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(-1.0f, -1.0f, 1.0f);	// Bottom Right Of The Texture and Quad
                GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(-1.0f, 1.0f, 1.0f);	// Top Right Of The Texture and Quad
                GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(-1.0f, 1.0f, -1.0f);	// Top Left Of The Texture and Quad
            GL.End();
            GL.EndList();

            top=box+1;
            GL.NewList(top, ListMode.Compile);
            GL.Begin(BeginMode.Quads);							// Start Drawing Quad
			// Top Face
			    GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(-1.0f,  1.0f, -1.0f);	// Top Left Of The Texture and Quad
			    GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-1.0f,  1.0f,  1.0f);	// Bottom Left Of The Texture and Quad
			    GL.TexCoord2(1.0f, 0.0f); GL.Vertex3( 1.0f,  1.0f,  1.0f);	// Bottom Right Of The Texture and Quad
			    GL.TexCoord2(1.0f, 1.0f); GL.Vertex3( 1.0f,  1.0f, -1.0f);	// Top Right Of The Texture and Quad
		    GL.End();
            GL.EndList();
        }
        private void glControl_Paint(object sender, PaintEventArgs e)
        {
            if (!loaded) // Play nice
                return;
            display();
           // GL.ClearColor(Color.Black);
        //    GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        //    GL.LoadIdentity();
       //     camera();
            //enable();
       //     DrawTutorial();
            //cube(); //call the cube drawing function
       //     glControl.SwapBuffers();
       //     angle++;
        }
        void enable()
        {
            GL.Enable(EnableCap.DepthTest); //enable the depth testing
            GL.Enable(EnableCap.Lighting); //enable the lighting
            GL.Enable(EnableCap.Light0); //enable LIGHT0, our Diffuse Light
            GL.ShadeModel(ShadingModel.Smooth); //set the shader to smooth shade
        }
        void mouseMovement(int x, int y) 
        {
            int diffx=x-lastx; //check the difference between the current x and the last x position
            int diffy=y-lasty; //check the difference between the current y and the last y position
            lastx=x; //set lastx to the current x position
            lasty=y; //set lasty to the current y position
            xrot += (float) diffy; //set the xrot to xrot with the addition of the difference in the y position
            yrot += (float) diffx;    //set the xrot to yrot with the addition of the difference in the x position
        }
        int lastx, lasty;
        float angle;
       
        private int LoadTexture(string filename)
        {
            if (String.IsNullOrEmpty(filename))
                throw new ArgumentException(filename);
 
            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);
 
            //Bitmap bmp = new Bitmap(filename);
            Bitmap bmp = CreateBoxTexture();
            BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
 
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
            OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);
 
            bmp.UnlockBits(bmp_data);
 
            // We haven't uploaded mipmaps, so disable mipmapping (otherwise the texture will not appear).
            // On newer video cards, we can use GL.GenerateMipmaps() or GL.Ext.GenerateMipmaps() to create
            // mipmaps automatically. In that case, use TextureMinFilter.LinearMipmapLinear to enable them.
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
 
            return id;
        }


        private Bitmap CreateBoxTexture()
        {
            Bitmap tmp = new Bitmap(100, 100, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(tmp);
            g.Clear(Color.Yellow);
            g.DrawRectangle(new Pen(Brushes.Black, 10f), 0, 0, 100, 100);
            g.Dispose();
            return tmp;
        }
   

    
        
        private void glControl_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void glControl_KeyUp(object sender, KeyEventArgs e)
        {
           
        }

        private float pitch = 0.0f;
        private float facing = 0.0f;
        private int _mouseStartX = 0;
        private int _mouseStartY = 0;
        private float angleX = 0;
        private float angleY = 0;
        private float angleXS = 0;
        private float angleYS = 0;
        private float distance = 5;
        private float distanceS = 5;
        private void glControl_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
                mouseDown = true;
            MouseEventArgs ev = (e as MouseEventArgs);
            _mouseStartX = ev.X;
            _mouseStartY = ev.Y;

        }

        private void glControl_MouseMove(object sender, MouseEventArgs e)
        {
            MouseEventArgs ev = (e as MouseEventArgs);
            if (mouseDown && ev.Button == MouseButtons.Left)
                mouseMovement(e.X, e.Y);
            if (ev.Button == MouseButtons.Left)
            {
          //      angleX = angleXS + (ev.X - _mouseStartX)*0.5f;// *rotSpeed;
          //      angleY = angleYS + (ev.Y - _mouseStartY)*0.5f;// *rotSpeed;
            }
            if (ev.Button == MouseButtons.Right)
            {
           //     distance = Math.Max(2.9f, distanceS + (ev.Y - _mouseStartY) / 10.0f);
            }



            glControl.Invalidate();
        }
        void cube () {
            GL.BindTexture(TextureTarget.Texture2D, cubeText);
            for (int i=0;i<10;i++)
            {
                GL.PushMatrix();
                //GL.LoadIdentity();
                GL.Translate(-positionx[i + 1] * 10f, 0, -positionz[i + 1] * 10f); //translate the cube
                GL.Color3(boxcol[1]);
                gluBox(1);
                //GL.CallList(box);
                //GL.CallList(top);
                // glutSolidCube(2); //draw the cube
                GL.PopMatrix();
            }
        }
        bool mouseDown = false;
        private void glControl_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
            MouseEventArgs ev = (e as MouseEventArgs);

            angleXS = angleX;
            angleYS = angleY;
            angleX = angleY = 0;

            distanceS = distance;

        }
  
        private void glControl_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar=='q')
            {
                xrot += 1;
                if (xrot >360) xrot -= 360;
            }
            if (e.KeyChar=='z')
            {
                xrot -= 1;
                if (xrot < -360) xrot += 360;
            }
            if (e.KeyChar=='w')
            {
                float xrotrad, yrotrad;
                yrotrad = (yrot / 180 * 3.141592654f);
                xrotrad = (xrot / 180 * 3.141592654f); 
                xpos += (float)(Math.Sin(yrotrad)) ;
                zpos -= (float)(Math.Cos(yrotrad)) ;
                ypos -= (float)(Math.Sin(xrotrad)) ;
                if (up <= 45.0)
                        up += 0.25f;
            }
            if (e.KeyChar=='s')
            {
                float xrotrad, yrotrad;
                yrotrad = (yrot / 180 * 3.141592654f);
                xrotrad = (xrot / 180 * 3.141592654f); 
                xpos -= (float)(Math.Sin(yrotrad));
                zpos += (float)(Math.Cos(yrotrad)) ;
                ypos += (float)(Math.Sin(xrotrad));
            }
            if (e.KeyChar=='d')
            {
                float yrotrad;
                yrotrad = (yrot / 180 * 3.141592654f);
                xpos += (float)(Math.Cos(yrotrad) * 0.2);
                zpos += (float)(Math.Sin(yrotrad) * 0.2);
            }
            if (e.KeyChar=='a')
            {
                float yrotrad;
                yrotrad = (yrot / 180 * 3.141592654f);
                xpos -= (float)(Math.Cos(yrotrad) * 0.2);
                zpos -= (float)(Math.Sin(yrotrad) * 0.2);
            }
  
        }
    }
}

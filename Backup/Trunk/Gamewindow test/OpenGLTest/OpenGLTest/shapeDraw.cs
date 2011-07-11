using System;
using System.Drawing;
using System.Drawing.Imaging;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using OpenTK.Input;

namespace OpenGLTest
{
	public class shapeDraw
	{

		static public void yetAnotherFloor(float centerX, float centerY, float texSize)
		{
			//GL.Translatef(0.0f, -5.4f, 0.0f);
    		GL.Begin(BeginMode.Quads);
    			GL.Normal3(0.0f, 1.0f, 0.0f);
    			GL.TexCoord2(2000 / texSize, centerY/ texSize);
    			GL.Vertex3(-1000.0f, 0.0f, -1000.0f);
    			GL.TexCoord2(2000 / texSize, (2000 + centerY) / texSize);
    			GL.Vertex3(-1000.0f, 0.0f, 1000.0f);
    			GL.TexCoord2(0.0f, (2000 + centerY) / texSize);
    			GL.Vertex3(1000.0f, 0.0f, 1000.0f);
    			GL.TexCoord2(0.0f, centerY / texSize);
    			GL.Vertex3(1000.0f, 0.0f, -1000.0f);
    		GL.End();	
		}
        static public void gluPerspective(float fov, float aspect, float near, float far)
        {
            double rad = (Math.PI / 180d) * fov;
            double range = near * Math.Tan(fov / 2d);
            GL.Frustum(-range * aspect, range * aspect, -range, range, near, far);
        }
		// Makes a large floor so you can put a texture on it.
		static public void makeTexFloor(int GridSizeX, int GridSizeY)
		{
			GL.Begin(BeginMode.Quads);
				GL.TexCoord2(0.0f,0.0f);
				GL.Vertex2(0.0f,0.0f);
				GL.TexCoord2(GridSizeX*2.0f,0.0f);
				GL.Vertex2(1.0f,0.0f);
				GL.TexCoord2(GridSizeX*2.0f,GridSizeY*2.0f);
				GL.Vertex2(1.0f,1.0f);
				GL.TexCoord2(0.0f,GridSizeY*2.0f);
				GL.Vertex2(0.0f,1.0f);
			GL.End();	
		}
		static public void makeFloor(int x, int y)
		{
			GL.Begin(BeginMode.Quads);
			//GL.PushMatrix();
			//GL.Color3(Color.Green);
			//GL.Scale(10f,10,1f);
		//	GL.Translate(x,y,0);
		//	GL.TexCoord2(1.0f, 1.0f); 	// Top Right Of The Texture and Quad
          //  GL.TexCoord2(0.0f, 1.0f); 	// Top Left Of The Texture and Quad
         //   GL.TexCoord2(0.0f, 0.0f); 	// Bottom Left Of The Texture and Quad
           // GL.TexCoord2(1.0f, 0.0f); 	// Bottom Right Of The Texture and Qua
			GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, -1.0f);
		//	GL.PopMatrix();
			GL.End();
		}
        static public void DrawCube()
        {
            GL.Begin(BeginMode.Quads);

            GL.Color3(Color.Silver);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, -1.0f);

            GL.Color3(Color.Honeydew);
            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);

            GL.Color3(Color.Moccasin);

            GL.Vertex3(-1.0f, -1.0f, -1.0f);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);

            GL.Color3(Color.IndianRed);
            GL.Vertex3(-1.0f, -1.0f, 1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);

            GL.Color3(Color.PaleVioletRed);
            GL.Vertex3(-1.0f, 1.0f, -1.0f);
            GL.Vertex3(-1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);

            GL.Color3(Color.ForestGreen);
            GL.Vertex3(1.0f, -1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, -1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, -1.0f, 1.0f);

            GL.End();
        }
        static public void torch()
        {
            float width = 0.2f;
            float height = 0.5f;
            GL.Begin(BeginMode.Quads);

            GL.TexCoord2(width,width);
            GL.TexCoord2(0.0f, width);
            GL.TexCoord2(0.0f, 0.0f);
            GL.TexCoord2(width, 0.0f);



            GL.End();

        }
        static public void testBox()
        {

            GL.Begin(BeginMode.Polygon);/* f1: front */
            GL.Normal3(-1.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, 1.0f);
            GL.Vertex3(1.0f, 0.0f, 1.0f);
            GL.Vertex3(1.0f, 0.0f, 0.0f);
            GL.End();
            GL.Begin(BeginMode.Polygon);/* f2: bottom */
            GL.Normal3(0.0f, 0.0f, -1.0f);
            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(1.0f, 0.0f, 0.0f);
            GL.Vertex3(1.0f, 1.0f, 0.0f);
            GL.Vertex3(0.0f, 1.0f, 0.0f);
            GL.End();
            GL.Begin(BeginMode.Polygon);/* f3:back */
            GL.Normal3(1.0f, 0.0f, 0.0f);
            GL.Vertex3(1.0f, 1.0f, 0.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(0.0f, 1.0f, 1.0f);
            GL.Vertex3(0.0f, 1.0f, 0.0f);
            GL.End();
            GL.Begin(BeginMode.Polygon);/* f4: top */
            GL.Normal3(0.0f, 0.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, 0.0f, 1.0f);
            GL.Vertex3(0.0f, 0.0f, 1.0f);
            GL.Vertex3(0.0f, 1.0f, 1.0f);
            GL.End();
            GL.Begin(BeginMode.Polygon);/* f5: left */
            GL.Normal3(0.0f, 1.0f, 0.0f);
            GL.Vertex3(0.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, 1.0f, 0.0f);
            GL.Vertex3(0.0f, 1.0f, 1.0f);
            GL.Vertex3(0.0f, 0.0f, 1.0f);
            GL.End();
            GL.Begin(BeginMode.Polygon);/* f6: right */
            GL.Normal3(0.0f, -1.0f, 0.0f);
            GL.Vertex3(1.0f, 0.0f, 0.0f);
            GL.Vertex3(1.0f, 0.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, 1.0f);
            GL.Vertex3(1.0f, 1.0f, 0.0f);
            GL.End();



        }
        static public void rawbox()
        {
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
            // Top Face
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(-1.0f, 1.0f, -1.0f);	// Top Left Of The Texture and Quad
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-1.0f, 1.0f, 1.0f);	// Bottom Left Of The Texture and Quad
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(1.0f, 1.0f, 1.0f);	// Bottom Right Of The Texture and Quad
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(1.0f, 1.0f, -1.0f);	// Top Right Of The Texture and Quad
            GL.End();

        }
        static public int buildList()
        {

            //int box = GL.GenLists(2);
            int box = GL.GenLists(1);
        //    GL.PushMatrix();
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

           // top = box + 1;
            //GL.NewList(top, ListMode.Compile);
            //GL.Begin(BeginMode.Quads);							// Start Drawing Quad
            // Top Face
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex3(-1.0f, 1.0f, -1.0f);	// Top Left Of The Texture and Quad
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex3(-1.0f, 1.0f, 1.0f);	// Bottom Left Of The Texture and Quad
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex3(1.0f, 1.0f, 1.0f);	// Bottom Right Of The Texture and Quad
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex3(1.0f, 1.0f, -1.0f);	// Top Right Of The Texture and Quad
            GL.End();
            GL.EndList();
           // GL.PopMatrix();
            return box;
        }
        static public void drawcube(int color, float lado)
        {

            GL.FrontFace(FrontFaceDirection.Ccw);
            // GL.BindTexture(TextureTarget.Texture2D, cubeText);

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

        static public int LoadTexture(Bitmap bmp)
        {

            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);
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
		static public Bitmap CreateCheckFloorTexture()
		{
			Bitmap tmp = new Bitmap(1024, 1024, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(tmp);
            g.Clear(Color.White);
			g.FillRectangle(Brushes.Red,0,0,512,512);
            g.FillRectangle(Brushes.Red,512,512,512,512);
            g.Dispose();
            return tmp;
		}
		 static public  Bitmap CreateFloorTexture()
        {
            Bitmap tmp = new Bitmap(100, 100, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(tmp);
            g.Clear(Color.White);
            //g.DrawRectangle(new Pen(Brushes.Black, 10f), 0, 0, 100, 100);
            g.Dispose();
            return tmp;
        }
		
        static public  Bitmap CreateBoxTexture()
        {
            Bitmap tmp = new Bitmap(100, 100, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(tmp);
            g.Clear(Color.Yellow);
            g.DrawRectangle(new Pen(Brushes.Black, 10f), 0, 0, 100, 100);
            g.Dispose();
            return tmp;
        }

    }
}


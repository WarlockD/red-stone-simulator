using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using OpenTK.Input;
using System.Drawing.Imaging;


namespace OpenTKtest
{
    class Game : GameWindow
    {
        int box;
        int boxTexture;
        float angle;
        /// <summary>Creates a 800x600 window with the specified title.</summary>
        public Game()
            : base(800, 600, GraphicsMode.Default, "OpenTK Quick Start Sample")
        {
            VSync = VSyncMode.On;
        }

        /// <summary>Load resources here.</summary>
        /// <param name="e">Not used.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            box = shapeDraw.buildList();
            boxTexture = shapeDraw.LoadTexture(shapeDraw.CreateBoxTexture());
            GL.ClearColor(0.1f, 0.2f, 0.5f, 0.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);

            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
        }

        /// <summary>
        /// Called when your window is resized. Set your viewport here. It is also
        /// a good place to set up your projection matrix (which probably changes
        /// along when the aspect ratio of your window).
        /// </summary>
        /// <param name="e">Not used.</param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
          
            float aspect_ratio = Width / (float)Height;
            Matrix4 perpective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspect_ratio, 1, 64);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perpective);
            SwapBuffers();
        }

        double heading, yrot, walkbiasangle;
        float xpos, zpos, ypos, walkbias;
        
        /// <summary>
        /// Called when it is time to setup the next frame. Add you game logic here.
        /// </summary>
        /// <param name="e">Contains timing information for framerate independent logic.</param>
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            if (Keyboard[Key.W])
            {
                this.xpos -= (float)Math.Sin(this.heading * Math.PI / 180.0) * 0.05f;
                this.zpos -= (float)Math.Cos(this.heading * Math.PI / 180.0) * 0.05f;
                if (this.walkbiasangle >= 359.0f)
                    this.walkbiasangle = 0.0f;
                else
                    this.walkbiasangle += 10.0f;
                this.walkbias = (float)Math.Sin(this.walkbiasangle * Math.PI / 180.0) / 20.0f;

            }
            if (Keyboard[Key.S])
            {
                this.xpos += (float)Math.Sin(this.heading * Math.PI / 180.0) * 0.05f;
                this.zpos += (float)Math.Cos(this.heading * Math.PI / 180.0) * 0.05f;
                if (this.walkbiasangle >= 359.0f)
                    this.walkbiasangle = 0.0f;
                else
                    this.walkbiasangle -= 10.0f;
                this.walkbias = (float)Math.Sin(this.walkbiasangle * Math.PI / 180.0) / 20.0f;
            }
            if (Keyboard[Key.A])
            {
                this.heading -= 1.0f;
                this.yrot = this.heading;
            }
            if (Keyboard[Key.D])
            {
                this.heading += 1.0f;
                this.yrot = this.heading;
            }
            if (Keyboard[Key.Escape])
                Exit();
        }

        /// <summary>
        /// Called when it is time to render the next frame. Add your rendering code here.
        /// </summary>
        /// <param name="e">Contains timing information.</param>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            Matrix4 lookat = Matrix4.LookAt(0, 5, 5, 0, 0, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);

            //GL.Rotate(angle, 0.0f, 1.0f, 0.0f);
            //angle += 0.1f;
            //GL.Rotate(this.lookupdown, 1.0f, 0.0f, 0.0f);
            GL.Rotate(360.0f - this.yrot, 0.0f, 1.0f, 0.0f);

            GL.Translate(-this.xpos, -this.walkbias - 0.25f, -this.zpos);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.BindTexture(TextureTarget.Texture2D, boxTexture);
            shapeDraw.rawbox();
            

            SwapBuffers();
 
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // The 'using' idiom guarantees proper resource cleanup.
            // We request 30 UpdateFrame events per second, and unlimited
            // RenderFrame events (as fast as the computer can handle).
            using (Game game = new Game())
            {
                game.Run(30.0);
            }
        }
        void drawcube(int color, float lado)
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
       
    }
}

using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.IO;
//using System.Math;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using OpenTK.Input;

namespace OpenGLTest
{
	class Game : GameWindow
    {
        int box;
        int boxTexture;
		int floorTexture;
		int checkTexture;
        float angle;
 		float xpos,ypos,zpos,heading,xrot,yrot,zrot;
		bool mouseDown = false;
        int lastx, lasty;
		
		Matrix4 mForward = Matrix4.CreateTranslation(0,0,1);
		Matrix4 mBackward = Matrix4.CreateTranslation(0,0,-1);
		Matrix4 mSlideLeft = Matrix4.CreateTranslation(-1,0,0);
		Matrix4 mSlideRight = Matrix4.CreateTranslation(1,0,0);
		Matrix4 mSlideUp = Matrix4.CreateTranslation(0,1,0);
		Matrix4 mSlideDown = Matrix4.CreateTranslation(0,-1,0);
		Matrix4 rLeft = Matrix4.CreateRotationX(-1f);
		Matrix4 rRight = Matrix4.CreateRotationX(1f);
		
		Vector3 eye ;
		Vector3 target ;
		Vector3 up ;
		Matrix4 lookat ;
		
		void setupLookAt()
		{
			eye = new Vector3(0,1,0);
			target = new Vector3(1,1,1);
			up = new Vector3(0,1,0);
			lookat = Matrix4.LookAt(eye,target,up);	
		}
		
		
		
        /// <summary>Creates a 800x600 window with the specified title.</summary>
        public Game()
            : base(800, 600, GraphicsMode.Default, "OpenTK Quick Start Sample")
        {
            VSync = VSyncMode.On;
			
		
        }

        
        void MouseMove(object s,MouseEventArgs e)
        {
            if (this.mouseDown)
            {
				Vector2 test = new Vector2(e.X,e.Y);
				
                mouseMovement(e.X, e.Y);
            }
        }

        void MouseButtonDown(object s, MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Right)
            {
                this.mouseDown = true;
                lastx = e.X;
                lasty = e.Y;
            }
        }

        void MouseButtonUp(object s,OpenTK.Input.MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Right)
            {
                this.mouseDown = false;
                lastx = 0;
                lasty = 0;
            }
        }

        void mouseMovement(int x, int y)
        {
            int diffx = x - lastx; //check the difference between the current x and the last x position
            int diffy = y - lasty; //check the difference between the current y and the last y position
            lastx = x; //set lastx to the current x position
            lasty = y; //set lasty to the current y position
            xrot += (float)diffy; //set the xrot to xrot with the addition of the difference in the y position
            yrot += (float)diffx;// set the xrot to yrot with the addition of the difference in the x position
        }
        /// <summary>Load resources here.</summary>
        /// <param name="e">Not used.</param>
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            Mouse.Move += new System.EventHandler<MouseMoveEventArgs>(MouseMove);
            Mouse.ButtonDown += new System.EventHandler<MouseButtonEventArgs>(MouseButtonDown);
            Mouse.ButtonUp +=  new System.EventHandler<MouseButtonEventArgs>(MouseButtonUp);
            setupLookAt();


            box = shapeDraw.buildList();
            boxTexture = shapeDraw.LoadTexture(shapeDraw.CreateBoxTexture());
			floorTexture =shapeDraw.LoadTexture(shapeDraw.CreateFloorTexture());
			checkTexture =shapeDraw.LoadTexture(shapeDraw.CreateCheckFloorTexture());
            GL.ClearColor(Color.LightSkyBlue);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);

            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
			GL.MatrixMode(MatrixMode.Modelview);
			GL.LoadMatrix(ref lookat);
        }

        /// <summary>
        /// Called when your window is resized. Set your viewport here. It is also
        /// a good place to set up your projection matrix (which probably changes
        /// along when the aspect ratio of your window).
        /// </summary>
        /// <param name="e">Not used.</param>
        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);
          	float fov  = MathHelper.PiOver4;
            float aspect_ratio = Width / (float)Height;

			
			Matrix4 pov = Matrix4.CreatePerspectiveFieldOfView(fov,aspect_ratio,1f,1000f);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadMatrix(ref pov);
			
            
            SwapBuffers();
        }
		double previousTime;
        
        /// <summary>
        /// Called when it is time to setup the next frame. Add you game logic here.
        /// </summary>
        /// <param name="e">Contains timing information for framerate independent logic.</param>
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
			double delta = e.Time-previousTime;
			previousTime = e.Time;
			float x=0,y=0,z=0,xR=0;
			
			if (Keyboard[Key.Q]) xR-=0.2f; //lookat = Matrix4.Mult(lookat,rLeft);
			if (Keyboard[Key.E]) xR+=0.2f; //lookat = Matrix4.Mult(lookat,rRight);
            if (Keyboard[Key.W]) z-=1;  //lookat = Matrix4.Mult(lookat,mForward);
            if (Keyboard[Key.S]) z+=1;  //lookat = Matrix4.Mult(lookat,mBackward);
            if (Keyboard[Key.A]) x-=1;  //lookat = Matrix4.Mult(lookat,mSlideLeft);
            if (Keyboard[Key.D]) x+=1;  //lookat = Matrix4.Mult(lookat,mSlideRight);
            
			Matrix4 moveMatrix = Matrix4.CreateTranslation(x,y,z); // forward, backward and slide left and right
			Matrix4 rotationMatrix = Matrix4.CreateRotationX(xR); // In fps's, you only turn left and right using arrows, mouse for eveything else
      		lookat = moveMatrix * rotationMatrix * lookat; // Lets merge eveything
			
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
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);
			
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			//GL.ClearColor(Color.White);
			testPgrid();
			GL.BindTexture(TextureTarget.Texture2D,checkTexture);
			shapeDraw.yetAnotherFloor(target.X , target.Y,512);
			
								
            SwapBuffers();
 
        }
		void testPgrid()
		{

			int GridSizeX = 16;
			int GridSizeY = 16;
			int SizeX = 8;
		 	int SizeY = 8;

			GL.Begin(BeginMode.Quads);
			for (int x =0;x<GridSizeX;++x)
				for (int y =0;y<GridSizeY;++y)
				{
					int mod = (x+y)&0x00000001;
					if (mod==0) //modulo 2
						GL.Color3(1.0f,1.0f,1.0f); //white
					else
						GL.Color3(0.0f,0.0f,0.0f); //black

					GL.Vertex2(    x*SizeX,    y*SizeY);
					GL.Vertex2((x+1)*SizeX,    y*SizeY);
					GL.Vertex2((x+1)*SizeX,(y+1)*SizeY);
					GL.Vertex2(    x*SizeX,(y+1)*SizeY);

				}
			GL.End();
		}
		void testOrthoGrid()
		{
			int GridSizeX = 16;
			int GridSizeY = 16;
			int SizeX = 8;
			int SizeY = 8;

			GL.MatrixMode(MatrixMode.Modelview);
			//GL.LoadIdentity();

			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(0,GridSizeX*SizeX,0,GridSizeY*SizeY,-1.0,1.0);

			GL.Begin(BeginMode.Quads);
			for (int x =0;x<GridSizeX;++x)
				for (int y =0;y<GridSizeY;++y)
				{
					int mod = (x+y)&0x00000001;
					if (mod==0) //modulo 2
						GL.Color3(1.0f,1.0f,1.0f); //white
					else
						GL.Color3(0.0f,0.0f,0.0f); //black

					GL.Vertex2(    x*SizeX,    y*SizeY);
					GL.Vertex2((x+1)*SizeX,    y*SizeY);
					GL.Vertex2((x+1)*SizeX,(y+1)*SizeY);
					GL.Vertex2(    x*SizeX,(y+1)*SizeY);

				}
			GL.End();
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
        
       
    }
}

using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace MoneRedstoneConversion
{
	public static class BlockColors
	{
		public static Color cAir = Color.White;
        public static Color cWireOn = Color.Red;
        public static Color cWireOff = Color.FromArgb((int)0x800000);
        public static Color cBlock = Color.Yellow;
        public static Color cCover = Color.FromArgb(0x80,0x80,0x80,0x80);
        public static Color cFog = Color.FromArgb((int)0x40ffffff);
        public static Color cAircover = Color.FromArgb((int)0x60ffffff);
        public static Color cValve = Color.Gray;
        public static Color cButton = Color.FromArgb((int)0x4d4e50);
        public static Color cDoor = Color.FromArgb((int)0x614226);
        public static Color cGrid = Color.Gray;
        public static Color cDirt = Color.FromArgb((int)0x856043);
        public static Color cSand = Color.FromArgb((int)0xdbd371);
        public static Color cWater = Color.FromArgb((int)0x2a5eff);
		public static Brush bAir = new SolidBrush(cAir);
        public static Brush bWireOn = new SolidBrush(cWireOn);
        public static Brush bWireOff = new SolidBrush(cBlock);
        public static Brush bBlock = new SolidBrush(cBlock);
        public static Brush bCover = new SolidBrush(cCover);
        public static Brush bFog = new SolidBrush(cFog);
        public static Brush bAircover = new SolidBrush(cAircover);
        public static Brush bValve = new SolidBrush(cValve);
        public static Brush bButton = new SolidBrush(cButton);
        public static Brush bDoor = new SolidBrush(cDoor);
        public static Brush bGrid = new SolidBrush(cGrid);
        public static Brush bDirt = new SolidBrush(cDirt);
        public static Brush bSand = new SolidBrush(cSand);
        public static Brush bWater = new SolidBrush(cWater);
	}
	
	public class BlockImage
	{
		Rectangle r; // bounds
		Bitmap bmp;
		Graphics g;
		Bitmap Air; 
		Bitmap Dirt;
		Bitmap[][] Wire; 
		Bitmap[] Torch;
		Bitmap[] Lever;
		Bitmap[] Button;
		Bitmap[] DoorA;
		Bitmap[] DoorB;
		Bitmap[] Press;
		Bitmap[] Sand;
		Bitmap[] Water;
	    Bitmap[] Shadow;
		
		public void setupG()
		{
		//	r = new Rectangle(0,0,8,8);
		//	bmp = new Bitmap(r);
		//	g = Graphics.FromImage(bmp);
		}
		/*
		public Bitmap Wire(int c, bool on)
		{
			Wire[0] = new Bitmap[5];
			g.Clear(Color.Transparent);
            g.fillRect(Color.Red,r.X + 2, r.Y + 2, 4, 4);
        if((c & 1) != 0)
            g.fillRect(r.x + 3, r.y + 3, 2, 5);
        if((c & 2) != 0)
            g.fillRect(r.x + 3, r.y, 2, 5);
        if((c & 4) != 0)
            g.fillRect(r.x + 3, r.y + 3, 5, 2);
        if((c & 8) != 0)
            g.fillRect(r.x, r.y + 3, 5, 2);
			
		}
		
		public BlockImage()
		{
			Rectangle r = new Rectangle(0,0,8,8);
			Bitmap bmp = new Bitmap(r);
			Graphics g = Graphics.FromImage(bmp);
			
			g.Clear(cAir);		// Air
			Air = new Bitmap(bmp);
			
			g.Clear(cBlock);	// Dirt
			Dirt = new Bitmap(bmp);
			
			// Wire
			Wire = new Bitmap[2];
			
			// Wire on
			
			Wire[0] = new Bitmap[5];
			g.Clear(Color.Transparent);
            g.fillRect(Color.Red,r.X + 2, r.Y + 2, 4, 4);
        if((c & 1) != 0)
            g.fillRect(r.x + 3, r.y + 3, 2, 5);
        if((c & 2) != 0)
            g.fillRect(r.x + 3, r.y, 2, 5);
        if((c & 4) != 0)
            g.fillRect(r.x + 3, r.y + 3, 5, 2);
        if((c & 8) != 0)
            g.fillRect(r.x, r.y + 3, 5, 2);
			
		}*/
		
	}
	
}


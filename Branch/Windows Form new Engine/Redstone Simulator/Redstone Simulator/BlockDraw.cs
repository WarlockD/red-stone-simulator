using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Redstone_Simulator
{
    [Flags]
    public enum WireMask: int
    {
        NotConnected =0,
        North =1  ,
        South =2  ,
        East =4  ,
        West  = 8,
        BlockPower = 16,
        AllDir = North | South | East | West
    }

    public struct BlockDrawSettings
    {
        public Block B { get;  internal set; }
        public bool OverShadow { get; set; }
        public bool Fog { get;  set; }
        public bool On { get; internal set; }
        public bool OnBlock { get;  set; }
        public WireMask Mask { get { return B.Mask; } }
        public BlockDrawSettings(Block b) : this() { B = b; this.On = b.Powered ? true : false;  }
        public BlockDrawSettings(Block b, bool On) : this() { B = b; this.On = On; }
    }
    public class BlockColors
    {
        public static Color cRepeater = Color.FromArgb(0x66, 0xFF, 0xFF);
        public static  Color  cAir= Color.White;
        public static  Color  cWireOn = Color.Red;
        public static  Color  cWireOff = Color.FromArgb(0x80,0x00,0x00);
        public static  Color  cBlock = Color.Yellow;
        public static  Color  cCover  = Color.FromArgb(0x80,0x80,0x80,0x80);
        public static  Color  cFog = Color.FromArgb(0x40,0xff,0xff,0xff);
        public static  Color  cAirCover = Color.FromArgb(0x60,0xff,0xff,0xff);
        public static  Color  cValve = Color.Gray;
        public static  Color  cButton = Color.FromArgb(0x4d,0x4e,0x50);
        public static  Color  cDoor = Color.FromArgb(0x61,0x42,0x26);
        public static  Color  cTorch = Color.FromArgb(0x61,0x42,0x26);
        public static  Color  cLever = Color.FromArgb(0x61,0x42,0x26);
        public static  Color  cGrid  = Color.Gray;
        public static  Color  cHilite = Color.FromArgb(0xa0,0xb1,0x9c);
        public static  Color  cCopyFrom = Color.FromArgb(0x3e,0x88,0xf9);
        public static  Color  cCopyTo = Color.FromArgb(0xfb,0x66,0x12);
        public static  Color  cDirt = Color.FromArgb(0x85,0x60,0x43);
        public static  Color  cSand = Color.FromArgb(0xdb,0xd3,0x71);
        public static  Color  cWater = Color.FromArgb(0x2a,0x5e,0xff);
        public static  Color  cTooltip = Color.FromArgb(0xc0,0xdd,0xdd,0xdd);

        public static  Brush bRepeater = new SolidBrush(cRepeater);
        public static  Brush  bAir= new SolidBrush(cAir);
        public static  Brush  bWireOn = new SolidBrush(cWireOn);
        public static  Brush  bWireOff =new SolidBrush(cWireOff);
        public static  Brush  bBlock = new SolidBrush(cBlock);
        public static  Brush  bCover  =new SolidBrush(cCover);
        public static  Brush  bFog =new SolidBrush(cFog);
        public static  Brush  bAirCover =new SolidBrush(cAirCover);
        public static  Brush  bValve = new SolidBrush(cValve);
        public static  Brush  bButton = new SolidBrush(cButton);
        public static  Brush  bDoor = new SolidBrush(cDoor);
        public static  Brush  bTorch =new SolidBrush(cTorch);
        public static  Brush  bLever =new SolidBrush(cLever);
        public static  Brush  bGrid = new SolidBrush(cGrid);
        public static  Brush  bHilite = new SolidBrush(cHilite);
        public static  Brush  bCopyFrom = new SolidBrush(cCopyFrom);
        public static  Brush  bCopyTo = new SolidBrush(cCopyTo);
        public static  Brush  bDirt =new SolidBrush(cDirt);
        public static  Brush  bSand = new SolidBrush(cSand);
        public static  Brush  bWater =new SolidBrush(cWater);
        public static  Brush  bTooltip = new SolidBrush(cTooltip);
        
    }
  
    class BlockImages
    {
        public static  Point[] northArrow = 
        { new Point(1,4), new Point(4,1), new Point(7,4), new Point(5,4), new Point(5,7), new Point(3,7), new Point(3,4), new Point(1,4) };
        public static Point[] southArrow = 
        { new Point(1, 4), new Point(4, 7), new Point(7, 4), new Point(5, 4), new Point(5, 1), new Point(3, 1), new Point(3, 4), new Point(1, 4) };
        public static Point[] eastArrow = 
        { new Point(1, 4), new Point(4, 1), new Point(4, 3), new Point(7, 3), new Point(7, 5), new Point(4, 5), new Point(4, 7), new Point(1, 4) };
        public static Point[] westArrow = 
        { new Point(7, 4), new Point(4, 1), new Point(4, 3), new Point(1, 3), new Point(1, 5), new Point(4, 5), new Point(4, 7), new Point(7, 4) };

        public static void gDrawWire(Graphics g, Rectangle r, BlockDrawSettings s)
        {
            Brush b = s.On ? BlockColors.bWireOn : BlockColors.bWireOff;

            if (s.Mask.HasFlag(WireMask.North)) g.FillRectangle(b, r.X + 3, r.Y, 2, 5);
            if (s.Mask.HasFlag(WireMask.South)) g.FillRectangle(b, r.X + 3, r.Y + 3, 2, 5);
            if (s.Mask.HasFlag(WireMask.East)) g.FillRectangle(b, r.X + 3, r.Y + 3, 5, 2);
            if (s.Mask.HasFlag(WireMask.West)) g.FillRectangle(b, r.X, r.Y + 3, 5, 2);
           // if (s.Mask ==0) g.FillRectangle(b, r.X + 2, r.Y + 2, 4, 4);
        }
        public static void gDrawBlock(Graphics g, Rectangle r, BlockDrawSettings b)
        {
            if (b.OverShadow) g.FillRectangle(BlockColors.bGrid, r);
            else
                if (b.B.ID == BlockType.BLOCK || b.OnBlock) g.FillRectangle(BlockColors.bBlock, r);
                else
                    g.FillRectangle(BlockColors.bAir, r);

            /*
             * 0x1: Facing south
               0x2: Facing north
               0x3: Facing west
               0x4: Facing east
             */

            switch (b.B.ID)
            {
                case BlockType.WIRE:
                    gDrawWire(g, r, b);
                    break;
                case BlockType.REPEATER:
                    Point[] Arrow = new Point[8];
                    Rectangle t =r;
                    int tick = b.B.Ticks;
                    bool rpow = b.On;
                    //b.Mount = 
                    switch (b.B.Place)
                    {
                        // There a better, fasterway for a vector arrow?
                        case Direction.NORTH:
                            for (int i = 0; i < 8; i++) { Arrow[i] = northArrow[i]; Arrow[i].Offset(r.Location); }
                            
                            t = new Rectangle(r.X + 3, r.Y + 3, 2, 1);
                            t.Offset(0, tick);
                            break;
                        case Direction.SOUTH:
                            for (int i = 0; i < 8; i++) { Arrow[i] = southArrow[i]; Arrow[i].Offset(r.Location); }
                            t = new Rectangle(r.X + 3, r.Y + 4, 2, 1);
                            t.Offset(0, -tick);
                            break;
                        case Direction.WEST:
                            for (int i = 0; i < 8; i++) { Arrow[i] = westArrow[i]; Arrow[i].Offset(r.Location); }
                            t = new Rectangle(r.X + 4, r.Y + 3, 1, 2);
                            t.Offset(-tick, 0);
                            break;
                        case Direction.EAST:
                            for (int i = 0; i < 8; i++) { Arrow[i] = eastArrow[i]; Arrow[i].Offset(r.Location); }
                            t = new Rectangle(r.X + 3, r.Y + 3, 1, 2);
                            t.Offset(tick, 0);
                            break;
                    }
                    g.FillPolygon(BlockColors.bRepeater, Arrow);
                    // If its powered we light it up, if its not powered we don't need to display a single tick
                    if (rpow & tick == 0)
                        g.FillRectangle(BlockColors.bWireOn, t);
                    else
                        if (tick != 0)
                            if (rpow)
                                g.FillRectangle(BlockColors.bWireOn, t);
                            else
                                g.FillRectangle(BlockColors.bWireOff, t);

                        g.DrawString(((int)(b.B.Delay/2)).ToString(), new Font("Courier", 4), BlockColors.bDoor, (r.Right+r.Left)/2, (r.Top+r.Bottom)/2);
                    break;
                case BlockType.LEVER:
                    switch (b.B.Place)
                    {
                        case Direction.SOUTH: g.FillRectangle(BlockColors.bLever, r.X + 3, r.Y, 2, 5); break;
                        case Direction.NORTH: g.FillRectangle(BlockColors.bLever, r.X + 3, r.Y + 3, 2, 5); break;
                        case Direction.EAST: g.FillRectangle(BlockColors.bLever, r.X + 3, r.Y + 3, 5, 2); break;
                        case Direction.WEST: g.FillRectangle(BlockColors.bLever, r.X, r.Y + 3, 5, 2); break;
                    }
                    g.FillEllipse(BlockColors.bValve, r.X + 2, r.Y + 2, 4, 4);
                    if (b.On)
                        g.FillEllipse(BlockColors.bWireOn, r.X + 3, r.X + 3, 2, 2);
                    break;
                case BlockType.TORCH:
                    switch (b.B.Place)
                    {
                        case Direction.SOUTH: g.FillRectangle(BlockColors.bLever, r.X + 3, r.Y, 2, 5); break;
                        case Direction.NORTH: g.FillRectangle(BlockColors.bLever, r.X + 3, r.Y + 3, 2, 5); break;
                    //    case Direction.EAST: g.FillRectangle(BlockColors.bLever, r.X + 3, r.Y + 3, 5, 2); break;
                   //     case Direction.WEST: g.FillRectangle(BlockColors.bLever, r.X, r.Y + 3, 5, 2); break;
                    }

                    if (b.On)
                        g.FillEllipse(BlockColors.bWireOn, r.X + 2, r.Y + 2, 4, 4);
                    else
                        g.FillEllipse(BlockColors.bWireOff, r.X + 2, r.Y + 2, 4, 4);

                    break;

                case BlockType.BUTTON:

                    if (b.On)
                        switch (b.B.Place)
                        {
                            case Direction.NORTH: g.FillRectangle(BlockColors.bButton, r.X + 2, r.Y + 7, 4, 1); break;
                            case Direction.SOUTH: g.FillRectangle(BlockColors.bButton, r.X + 2, r.Y, 4, 1); break;
                            case Direction.EAST: g.FillRectangle(BlockColors.bButton, r.X + 7, r.Y + 2, 1, 4); break;
                            case Direction.WEST: g.FillRectangle(BlockColors.bButton, r.X, r.Y + 2, 1, 4); break;
                        }
                    else
                        switch (b.B.Place)
                        {
                            case Direction.SOUTH: g.FillRectangle(BlockColors.bButton, r.X + 2, r.Y + 5, 4, 3); break;
                            case Direction.NORTH: g.FillRectangle(BlockColors.bButton, r.X + 2, r.Y, 4, 3); break;
                            case Direction.EAST: g.FillRectangle(BlockColors.bButton, r.X + 5, r.Y + 2, 3, 4); break;
                            case Direction.WEST: g.FillRectangle(BlockColors.bButton, r.X, r.Y + 2, 3, 4); break;

                        }
                    break;

                case BlockType.PREASUREPAD: // '\t'
                    if (b.On)
                        g.FillRectangle(BlockColors.bWireOn, r.X + 1, r.Y + 1, 6, 6);
                    else
                        g.FillRectangle(BlockColors.bValve, r.X + 1, r.Y + 1, 6, 6);
                    break;
             /*   case BlockType.DOORB:
                    break; //Check for this 
                case BlockType.DOORA: // '\007'
                    if (b.Block.Powered)
                        switch (b.Block.Place) // THIS IS UNPOWERED, NEEDS TO BE MODIFIED.
                        // JUST LASY AND TESTING RIGHT NOW
                        {
                            // FYI, this is facing, so its hinge to edge not powered
                            case Direction.NORTH: // Hinge, bottom left, Closed
                                g.FillRectangle(BlockColors.bDoor, r.X, r.Y, 2, 8);
                                g.FillRectangle(BlockColors.bWireOff, r.X, r.Y + 6, 2, 2);
                                break;
                            case Direction.SOUTH: // Hinge upper right, Closed
                                g.FillRectangle(BlockColors.bDoor, r.X, r.Y + 6, 2, 8);
                                g.FillRectangle(BlockColors.bWireOff, r.X + 6, r.Y, 2, 2);
                                break;
                            case Direction.EAST: // Hinge, upper left
                                g.FillRectangle(BlockColors.bDoor, r.X, r.Y, 8, 2);
                                g.FillRectangle(BlockColors.bWireOff, r.X, r.Y, 2, 2);
                                break;
                            case Direction.WEST:  // bottom right
                                g.FillRectangle(BlockColors.bDoor, r.X, r.Y + 6, 8, 2);
                                g.FillRectangle(BlockColors.bWireOff, r.X + 6, r.Y + 6, 2, 2);
                                break;
                        }
                    else
                        switch (b.Block.Place)
                        {
                            // FYI, this is facing, so its hinge to edge not powered
                            case Direction.NORTH: // Hinge, bottom left, Closed
                                g.FillRectangle(BlockColors.bDoor, r.X, r.Y, 2, 8);
                                g.FillRectangle(BlockColors.bWireOff, r.X, r.Y + 6, 2, 2);
                                break;
                            case Direction.SOUTH: // Hinge upper right, Closed
                                g.FillRectangle(BlockColors.bDoor, r.X, r.Y + 6, 2, 8);
                                g.FillRectangle(BlockColors.bWireOff, r.X + 6, r.Y, 2, 2);
                                break;
                            case Direction.EAST: // Hinge, upper left
                                g.FillRectangle(BlockColors.bDoor, r.X, r.Y, 8, 2);
                                g.FillRectangle(BlockColors.bWireOff, r.X, r.Y, 2, 2);
                                break;
                            case Direction.WEST:  // bottom right
                                g.FillRectangle(BlockColors.bDoor, r.X, r.Y + 6, 8, 2);
                                g.FillRectangle(BlockColors.bWireOff, r.X + 6, r.Y + 6, 2, 2);
                                break;
                        }


                    break;*/


            }
            if (b.Fog) g.FillRectangle(BlockColors.bFog, r);
            if(!b.B.isAir && !b.B.isRepeater)
                g.DrawString(b.B.Charge.ToString(), new Font("Courier",4),BlockColors.bDoor, r.X,r.Y);
        }

      
        
    }
}

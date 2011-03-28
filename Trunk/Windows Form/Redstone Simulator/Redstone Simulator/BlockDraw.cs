using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Redstone_Simulator
{
    [Flags]
    public enum WireMask
    {
        None = 0, // Constant
        North = 1,
        South = 2,
        East = 4,
        West = 8,
        NoConn = 10
    }
    [Flags]
    public enum eBlockImage
    {
        Blank = 0,
        Air = 1,
        Block = 2,
        Shadow = 4,
        Fog = 8
    }
    public struct BlockIData
    {
        public WireMask Mask;
        public eBlockImage Flags;
        public Blocks Block;
        public BlockIData(Blocks b, eBlockImage f, WireMask m)
        { Mask = m; Flags = f; Block = b; }
        public BlockIData(Blocks b) : this(b, eBlockImage.Air, WireMask.None) { }
 
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
        
    /*
                case 1: // south
                        xP = new int[] { r.x +1,r.x+4,r.x+7,r.x+5,r.x+5,r.x+3,r.x+3,r.x+1 };
                        yP = new int[] { r.y +4,r.y+7,r.y+4,r.y+4,r.y+1,r.y+1,r.y+4,r.y+4 };

                case 0: // East
                        xP = new int[] { r.x +1,r.x+4,r.x+4,r.x+7,r.x+7,r.x+4,r.x+4,r.x+1 };
                        yP = new int[] { r.y +4,r.y+1,r.y+3,r.y+3,r.y+5,r.y+5,r.y+7,r.y+4 };
     
                case 2: // west
                        xP = new int[] { r.x +7,r.x+4,r.x+4,r.x+1,r.x+1,r.x+4,r.x+4,r.x+7 };
                        yP = new int[] { r.y +4,r.y+1,r.y+3,r.y+3,r.y+5,r.y+5,r.y+7,r.y+4 };


    /*
    }
        */

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

        public static void gDrawWire(Graphics g, Rectangle r, bool on, WireMask c)
        {
            gDrawWire(g, r, on, c, true);
        }
        public static void gDrawWire(Graphics g, Rectangle r, bool on, WireMask c, bool onNonePrintAll)
        {
            Brush b = on ? BlockColors.bWireOn : BlockColors.bWireOff;

            if (c == WireMask.None)
                if (onNonePrintAll)
                    c = WireMask.North | WireMask.South | WireMask.East | WireMask.West;
                else
                {
                    g.FillRectangle(b, r.X + 2, r.Y + 2, 4, 4);
                    return;
                }

            if (c.HasFlag(WireMask.North)) g.FillRectangle(b, r.X + 3, r.Y, 2, 5);
            if (c.HasFlag(WireMask.South)) g.FillRectangle(b, r.X + 3, r.Y + 3, 2, 5);
            if (c.HasFlag(WireMask.East)) g.FillRectangle(b, r.X + 3, r.Y + 3, 5, 2);
            if (c.HasFlag(WireMask.West)) g.FillRectangle(b, r.X, r.Y + 3, 5, 2);


        }
        public static void gDrawBlock(Graphics g, Rectangle r, BlockIData b)
        {
            if (b.Flags.HasFlag(eBlockImage.Shadow)) g.FillRectangle(BlockColors.bGrid, r);
            else
                if (b.Flags.HasFlag(eBlockImage.Block)) g.FillRectangle(BlockColors.bBlock, r);
                else
                    g.FillRectangle(BlockColors.bAir, r);
            gDrawBlock(g, r, b.Block);
            if (b.Flags.HasFlag(eBlockImage.Fog)) g.FillRectangle(BlockColors.bFog, r);
        }
        public static void gDrawBlock(Graphics g, Rectangle r, Blocks b, bool overShadow, bool onBlock, bool withFog)
        {
            if(overShadow) g.FillRectangle(BlockColors.bGrid, r);
            else
            if(onBlock) g.FillRectangle(BlockColors.bBlock, r);
            else
                g.FillRectangle(BlockColors.bAir, r);
            gDrawBlock(g, r, b);
            if(withFog) g.FillRectangle(BlockColors.bFog, r);
        }
        public static void gDrawBlock(Graphics g, Rectangle r, Blocks b)
        {
            /*
             * 0x1: Facing south
               0x2: Facing north
               0x3: Facing west
               0x4: Facing east
             */
            switch (b.Type)
            {
                case eBlock.REPEATER:
                    Point[] Arrow = new Point[8];
                    int tick = b.Ticks - 1;
                    bool rpow = b.Powered;
                    b.Mount = eMount.EAST;
                    switch (b.Mount)
                    {
                            // There a better, fasterway for a vector arrow?
                        case eMount.NORTH: 
                            for (int i = 0; i < 8; i++) { Arrow[i] = northArrow[i]; Arrow[i].Offset(r.Location); }
                            g.FillPolygon(BlockColors.bRepeater, Arrow);
                            r = new Rectangle(r.X + 3, r.Y + 3, 2, 1);
                            r.Offset(0, tick);
                            break;
                        case eMount.SOUTH:
                            for (int i = 0; i < 8; i++) { Arrow[i] = southArrow[i]; Arrow[i].Offset(r.Location); }
                            g.FillPolygon(BlockColors.bRepeater, Arrow);
                            r = new Rectangle(r.X + 3, r.Y + 4, 2, 1); 
                            r.Offset(0, -tick);
                            break;
                        case eMount.WEST: 
                            for (int i = 0; i < 8; i++) { Arrow[i] = westArrow[i]; Arrow[i].Offset(r.Location); }
                            g.FillPolygon(BlockColors.bRepeater, Arrow);
                            r = new Rectangle(r.X + 4, r.Y + 3, 1, 2);
                            r.Offset(-tick, 0);
                            break;
                        case eMount.EAST: 
                            for (int i = 0; i < 8; i++) { Arrow[i] = eastArrow[i]; Arrow[i].Offset(r.Location); }
                            g.FillPolygon(BlockColors.bRepeater, Arrow);
                            r = new Rectangle(r.X + 3, r.Y + 3, 1, 2);
                            r.Offset(tick, 0);
                            break;
                    }
                    
                    // If its powered we light it up, if its not powered we don't need to display a single tick
                    if (rpow & tick == 0)
                        g.FillRectangle(BlockColors.bWireOn, r);
                     else
                        if (tick != 0)
                            if (rpow)
                                g.FillRectangle(BlockColors.bWireOn, r);
                            else
                                g.FillRectangle(BlockColors.bWireOff, r);


                    break;
                case eBlock.WIRE: 
                    gDrawWire(g, r, b.Powered, b.wMask, true);  //FIX THIS
                    break;
                case eBlock.LEVER:
                    switch (b.Mount)
                    {
                        case eMount.SOUTH: g.FillRectangle(BlockColors.bLever, r.X + 3, r.Y, 2, 5); break;
                        case eMount.NORTH: g.FillRectangle(BlockColors.bLever, r.X + 3, r.Y + 3, 2, 5); break;
                        case eMount.EAST: g.FillRectangle(BlockColors.bLever, r.X + 3, r.Y + 3, 5, 2); break;
                        case eMount.WEST: g.FillRectangle(BlockColors.bLever, r.X, r.Y + 3, 5, 2); break;
                    }
                    g.FillEllipse(BlockColors.bValve, r.X + 2, r.Y + 2, 4, 4);
                    if (b.Powered)
                        g.FillEllipse(BlockColors.bWireOn, r.X + 3, r.X + 3, 2, 2);
                    break;
                case eBlock.TORCH: 
                    switch (b.Mount)
                    {
                        case eMount.SOUTH: g.FillRectangle(BlockColors.bLever, r.X + 3, r.Y, 2, 5); break;
                        case eMount.NORTH: g.FillRectangle(BlockColors.bLever, r.X + 3, r.Y + 3, 2, 5); break;
                        case eMount.EAST: g.FillRectangle(BlockColors.bLever, r.X + 3, r.Y + 3, 5, 2); break;
                        case eMount.WEST: g.FillRectangle(BlockColors.bLever, r.X, r.Y + 3, 5, 2); break;
                    }

                    if (b.Powered)
                        g.FillEllipse(BlockColors.bWireOn, r.X + 2, r.Y + 2, 4, 4);
                    else
                        g.FillEllipse(BlockColors.bWireOff, r.X + 2, r.Y + 2, 4, 4);

                    break;

                case eBlock.BUTTON: 

                    if (b.Powered)
                        switch (b.Mount)
                        {
                            case eMount.NORTH: g.FillRectangle(BlockColors.bButton, r.X + 2, r.Y + 7, 4, 1); break;
                            case eMount.SOUTH: g.FillRectangle(BlockColors.bButton, r.X + 2, r.Y, 4, 1); break;
                            case eMount.EAST: g.FillRectangle(BlockColors.bButton, r.X + 7, r.Y + 2, 1, 4); break;
                            case eMount.WEST: g.FillRectangle(BlockColors.bButton, r.X, r.Y + 2, 1, 4); break;
                        }
                    else
                        switch (b.Mount)
                        {
                            case eMount.SOUTH: g.FillRectangle(BlockColors.bButton, r.X + 2, r.Y + 5, 4, 3); break;
                            case eMount.NORTH: g.FillRectangle(BlockColors.bButton, r.X + 2, r.Y, 4, 3); break;
                            case eMount.EAST: g.FillRectangle(BlockColors.bButton, r.X + 5, r.Y + 2, 3, 4); break;
                            case eMount.WEST: g.FillRectangle(BlockColors.bButton, r.X, r.Y + 2, 3, 4); break;

                        }
                    break;

                case eBlock.PRESS: // '\t'
                    if (b.Powered)
                        g.FillRectangle(BlockColors.bWireOn, r.X + 1, r.Y + 1, 6, 6);
                    else
                        g.FillRectangle(BlockColors.bValve, r.X + 1, r.Y + 1, 6, 6);
                    break;
                case eBlock.DOORB:
                    break; //Check for this 
                case eBlock.DOORA: // '\007'
                    if (b.Powered)
                        switch (b.Mount) // THIS IS UNPOWERED, NEEDS TO BE MODIFIED.
                        // JUST LASY AND TESTING RIGHT NOW
                        {
                            // FYI, this is facing, so its hinge to edge not powered
                            case eMount.NORTH: // Hinge, bottom left, Closed
                                g.FillRectangle(BlockColors.bDoor, r.X, r.Y, 2, 8);
                                g.FillRectangle(BlockColors.bWireOff, r.X, r.Y + 6, 2, 2);
                                break;
                            case eMount.SOUTH: // Hinge upper right, Closed
                                g.FillRectangle(BlockColors.bDoor, r.X, r.Y + 6, 2, 8);
                                g.FillRectangle(BlockColors.bWireOff, r.X + 6, r.Y, 2, 2);
                                break;
                            case eMount.EAST: // Hinge, upper left
                                g.FillRectangle(BlockColors.bDoor, r.X, r.Y, 8, 2);
                                g.FillRectangle(BlockColors.bWireOff, r.X, r.Y, 2, 2);
                                break;
                            case eMount.WEST:  // bottom right
                                g.FillRectangle(BlockColors.bDoor, r.X, r.Y + 6, 8, 2);
                                g.FillRectangle(BlockColors.bWireOff, r.X + 6, r.Y + 6, 2, 2);
                                break;
                        }
                    else
                        switch (b.Mount)
                        {
                            // FYI, this is facing, so its hinge to edge not powered
                            case eMount.NORTH: // Hinge, bottom left, Closed
                                g.FillRectangle(BlockColors.bDoor, r.X, r.Y, 2, 8);
                                g.FillRectangle(BlockColors.bWireOff, r.X, r.Y + 6, 2, 2);
                                break;
                            case eMount.SOUTH: // Hinge upper right, Closed
                                g.FillRectangle(BlockColors.bDoor, r.X, r.Y + 6, 2, 8);
                                g.FillRectangle(BlockColors.bWireOff, r.X + 6, r.Y, 2, 2);
                                break;
                            case eMount.EAST: // Hinge, upper left
                                g.FillRectangle(BlockColors.bDoor, r.X, r.Y, 8, 2);
                                g.FillRectangle(BlockColors.bWireOff, r.X, r.Y, 2, 2);
                                break;
                            case eMount.WEST:  // bottom right
                                g.FillRectangle(BlockColors.bDoor, r.X, r.Y + 6, 8, 2);
                                g.FillRectangle(BlockColors.bWireOff, r.X + 6, r.Y + 6, 2, 2);
                                break;
                        }


                    break;

                case eBlock.WATER:
                    //int col = fake ? Colors.water.getRGB() : 
                    // How deap the water is?
                    // ((~gp(x, y, z + p) & 7) * 24 + 87 << 24) + (Colors.water.getRGB() & 0xffffff);
                    //g.setColor(new Color(col, true));
                    g.FillRectangle(BlockColors.bWater, r);

                    // Below puts a square if the water is dropping or a circle if the water is standing 
                    //  g.setColor(Color.BLACK);
                    //  if(!fake)
                    //     if((gp(x, y, z + p) & 8) != 0)
                    //          g.fillOval(r.x + 3, r.y + 3, 2, 2);
                    //      else
                    //     if((gp(x, y, z + p) & 0xf) == 0)
                    //          g.fillRect(r.x + 3, r.y + 3, 2, 2);
                    break;
            }

        }

        public static void gDrawBlockStack(Graphics g, Rectangle r, Blocks[] b)
        {
            if (b == null)  throw new ArgumentNullException("b", "Blocks is Null");  
            if (b.Length != 3)  throw new ArgumentException("Blocks is not 3 Length", "b"); 
            bool whiteout = false;
            int p = 0;


            // Figure out what our background color needs to be
            // By the block at the bottom.
            // We are always going to see 3 layers, I may change it latter.
            switch (b[0].Type)
            {
                case eBlock.BLOCK: p++; g.FillRectangle(BlockColors.bBlock, r); break;
                case eBlock.AIR:
                    if (b[1].isBlock) 
                        g.FillRectangle(BlockColors.bGrid, r);
                    else
                    {
                        whiteout = true;
                        p++;
                        g.FillRectangle(BlockColors.bAir, r);
                    }
                    break;
                default: g.FillRectangle(BlockColors.bAir, r); break;
            }

            if (b[0].Type == eBlock.WIRE)
            {
                // Filler, right now I will make the wire all sides till
                // I get the draw wire routine right.
                gDrawWire(g, r, true, WireMask.None, true);
                p++;
                if (!b[p].isAir && !b[p].isBlock) // if the 
                    g.FillRectangle(BlockColors.bAirCover, r);
            }
            if (b[p].Type == eBlock.DOORB) p--; // DOORB fix
            gDrawBlock(g, r, b[p]);



            if (b[1].isBlock)
            {
                g.FillRectangle(BlockColors.bCover, r);
                if (b[0].isBlock)
                    gDrawWire(g,r,true,WireMask.None);
            }
            else
                if (whiteout)
                {
                    g.FillRectangle(BlockColors.bFog, r);

                }
            //  if(b[1].isBlock)
            //  {
            //      g.FillRectangle(getBrush(BlockColors.Cover),r);
            //     if(b[2].Type == rBlocks.WIRE && b[0].Type ==  Blocks.WIRE || b[0].isBlock)
            //     if(fake)
            //          drawWire(g, r, true, 12, false);
            //     else
            //         drawWire(g, r, x, y, z + 2, false);
            //
            //  }

        }
    }
}

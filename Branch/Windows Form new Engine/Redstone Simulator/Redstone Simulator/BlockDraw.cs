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
        AllDir = North | South | East | West
    }

    public struct BlockDrawSettings
    {
        Blocks block;
        public Blocks Block { get { return block; } }
        public bool OverShadow { get; set; }
        public bool Fog { get; set; }
        public bool On { get; set; }
        public bool OnBlock { get; set; }
        public WireMask Mask { get; set; }
        public BlockDrawSettings(Blocks Block) : this() { block = Block; }
        public BlockDrawSettings(Blocks Block, WireMask Mask) : this(Block) { this.Mask = Mask; }
        public BlockDrawSettings(Blocks Block, WireMask Mask, bool On) : this(Block,Mask) { block.Power = On ? 16 : 0; }
        public BlockDrawSettings(Blocks Block,  bool On) : this(Block) { block.Power = On ? 16 : 0; }
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
            Brush b = s.Block.Powered ? BlockColors.bWireOn : BlockColors.bWireOff;

 
            if (s.Mask.HasFlag(WireMask.North)) g.FillRectangle(b, r.X + 3, r.Y, 2, 5);
            if (s.Mask.HasFlag(WireMask.South)) g.FillRectangle(b, r.X + 3, r.Y + 3, 2, 5);
            if (s.Mask.HasFlag(WireMask.East)) g.FillRectangle(b, r.X + 3, r.Y + 3, 5, 2);
            if (s.Mask.HasFlag(WireMask.West)) g.FillRectangle(b, r.X, r.Y + 3, 5, 2);
           // if (s.Mask.HasFlag(WireMask.None)) g.FillRectangle(b, r.X + 2, r.Y + 2, 4, 4);
        }
        public static void gDrawBlock(Graphics g, Rectangle r, BlockDrawSettings b)
        {
            if (b.OverShadow) g.FillRectangle(BlockColors.bGrid, r);
            else
                if (b.Block.Type == eBlock.BLOCK || b.OnBlock) g.FillRectangle(BlockColors.bBlock, r);
                else
                    g.FillRectangle(BlockColors.bAir, r);

            /*
             * 0x1: Facing south
               0x2: Facing north
               0x3: Facing west
               0x4: Facing east
             */

            switch (b.Block.Type)
            {
                case eBlock.WIRE:
                    gDrawWire(g, r, b);
                    break;
                case eBlock.REPEATER:
                    Point[] Arrow = new Point[8];
                    int tick = b.Block.Ticks;
                    bool rpow = b.Block.Powered;
                    //b.Mount = 
                    switch (b.Block.Mount)
                    {
                        // There a better, fasterway for a vector arrow?
                        case eMount.NORTH:
                            for (int i = 0; i < 8; i++) { Arrow[i] = northArrow[i]; Arrow[i].Offset(r.Location); }
                            r = new Rectangle(r.X + 3, r.Y + 3, 2, 1);
                            r.Offset(0, tick);
                            break;
                        case eMount.SOUTH:
                            for (int i = 0; i < 8; i++) { Arrow[i] = southArrow[i]; Arrow[i].Offset(r.Location); }
                            r = new Rectangle(r.X + 3, r.Y + 4, 2, 1);
                            r.Offset(0, -tick);
                            break;
                        case eMount.WEST:
                            for (int i = 0; i < 8; i++) { Arrow[i] = westArrow[i]; Arrow[i].Offset(r.Location); }
                            r = new Rectangle(r.X + 4, r.Y + 3, 1, 2);
                            r.Offset(-tick, 0);
                            break;
                        case eMount.EAST:
                            for (int i = 0; i < 8; i++) { Arrow[i] = eastArrow[i]; Arrow[i].Offset(r.Location); }
                            r = new Rectangle(r.X + 3, r.Y + 3, 1, 2);
                            r.Offset(tick, 0);
                            break;
                    }
                    g.FillPolygon(BlockColors.bRepeater, Arrow);
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
                case eBlock.LEVER:
                    switch (b.Block.Mount)
                    {
                        case eMount.SOUTH: g.FillRectangle(BlockColors.bLever, r.X + 3, r.Y, 2, 5); break;
                        case eMount.NORTH: g.FillRectangle(BlockColors.bLever, r.X + 3, r.Y + 3, 2, 5); break;
                        case eMount.EAST: g.FillRectangle(BlockColors.bLever, r.X + 3, r.Y + 3, 5, 2); break;
                        case eMount.WEST: g.FillRectangle(BlockColors.bLever, r.X, r.Y + 3, 5, 2); break;
                    }
                    g.FillEllipse(BlockColors.bValve, r.X + 2, r.Y + 2, 4, 4);
                    if (b.Block.Powered)
                        g.FillEllipse(BlockColors.bWireOn, r.X + 3, r.X + 3, 2, 2);
                    break;
                case eBlock.TORCH:
                    switch (b.Block.Mount)
                    {
                        case eMount.SOUTH: g.FillRectangle(BlockColors.bLever, r.X + 3, r.Y, 2, 5); break;
                        case eMount.NORTH: g.FillRectangle(BlockColors.bLever, r.X + 3, r.Y + 3, 2, 5); break;
                        case eMount.EAST: g.FillRectangle(BlockColors.bLever, r.X + 3, r.Y + 3, 5, 2); break;
                        case eMount.WEST: g.FillRectangle(BlockColors.bLever, r.X, r.Y + 3, 5, 2); break;
                    }

                    if (b.Block.Powered)
                        g.FillEllipse(BlockColors.bWireOn, r.X + 2, r.Y + 2, 4, 4);
                    else
                        g.FillEllipse(BlockColors.bWireOff, r.X + 2, r.Y + 2, 4, 4);

                    break;

                case eBlock.BUTTON:

                    if (b.Block.Powered)
                        switch (b.Block.Mount)
                        {
                            case eMount.NORTH: g.FillRectangle(BlockColors.bButton, r.X + 2, r.Y + 7, 4, 1); break;
                            case eMount.SOUTH: g.FillRectangle(BlockColors.bButton, r.X + 2, r.Y, 4, 1); break;
                            case eMount.EAST: g.FillRectangle(BlockColors.bButton, r.X + 7, r.Y + 2, 1, 4); break;
                            case eMount.WEST: g.FillRectangle(BlockColors.bButton, r.X, r.Y + 2, 1, 4); break;
                        }
                    else
                        switch (b.Block.Mount)
                        {
                            case eMount.SOUTH: g.FillRectangle(BlockColors.bButton, r.X + 2, r.Y + 5, 4, 3); break;
                            case eMount.NORTH: g.FillRectangle(BlockColors.bButton, r.X + 2, r.Y, 4, 3); break;
                            case eMount.EAST: g.FillRectangle(BlockColors.bButton, r.X + 5, r.Y + 2, 3, 4); break;
                            case eMount.WEST: g.FillRectangle(BlockColors.bButton, r.X, r.Y + 2, 3, 4); break;

                        }
                    break;

                case eBlock.PRESS: // '\t'
                    if (b.Block.Powered)
                        g.FillRectangle(BlockColors.bWireOn, r.X + 1, r.Y + 1, 6, 6);
                    else
                        g.FillRectangle(BlockColors.bValve, r.X + 1, r.Y + 1, 6, 6);
                    break;
                case eBlock.DOORB:
                    break; //Check for this 
                case eBlock.DOORA: // '\007'
                    if (b.Block.Powered)
                        switch (b.Block.Mount) // THIS IS UNPOWERED, NEEDS TO BE MODIFIED.
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
                        switch (b.Block.Mount)
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


            }
            if (b.Fog) g.FillRectangle(BlockColors.bFog, r);
        }

      
        
    }
}

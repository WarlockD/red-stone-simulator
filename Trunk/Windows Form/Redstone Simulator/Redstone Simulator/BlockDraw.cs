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
        West = 8
    }
    public enum BlockColors
    {
        Air,// = Color.White.ToArgb(),
        WireOn,// = Color.Red.ToArgb(),
        WireOff = 0x800000,
        Block,// = Color.Yellow.ToArgb(),
        Cover  = unchecked((int)0x80808080),
        Fog = 0x40ffffff,
        AirCover = 0x60ffffff,
        Valve,// = Color.Gray.ToArgb()},
        Button = 0x4d4e50,
        Door = 0x614226,
        Torch = 0x614226,
        Lever = 0x614226,
        Grid, // = Color.Gray.ToArgb(),
        Hilite = 0xa0b19c,
        CopyFrom = 0x3e88f9,
        CopyTo = 0xfb6612,
        Dirt = 0x856043,
        Sand = 0xdbd371,
        Water = 0x2a5eff,
        Tooltip = unchecked((int)0xc0dddddd)
    }


    class BlockImages
    {
        public static Brush getBrush(BlockColors c)
        {
            return new SolidBrush(getColor(c));
        }
        public static Color getColor(BlockColors c)
        {
            return Color.FromArgb((int)c);
        }
        public static void gDrawWire(Graphics g, Rectangle r, bool on, WireMask c, bool onNonePrintAll)
        {
            Brush b = on ? getBrush(BlockColors.WireOn) : getBrush(BlockColors.WireOff);

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
                case eBlock.WIRE: gDrawWire(g, r, true, WireMask.None, true); break;


                case eBlock.LEVER:
                    switch (b.Mount)
                    {
                        case eMount.SOUTH: g.FillRectangle(getBrush(BlockColors.Lever), r.X + 3, r.Y, 2, 5); break;
                        case eMount.NORTH: g.FillRectangle(getBrush(BlockColors.Lever), r.X + 3, r.Y + 3, 2, 5); break;
                        case eMount.EAST: g.FillRectangle(getBrush(BlockColors.Lever), r.X + 3, r.Y + 3, 5, 2); break;
                        case eMount.WEST: g.FillRectangle(getBrush(BlockColors.Lever), r.X, r.Y + 3, 5, 2); break;
                    }
                    g.FillEllipse(getBrush(BlockColors.Valve), r.X + 2, r.Y + 2, 4, 4);
                    if (b.Powered)
                        g.FillEllipse(getBrush(BlockColors.WireOn), r.X + 3, r.X + 3, 2, 2);
                    break;
                case eBlock.TORCH: // '\004'
                    switch (b.Mount)
                    {
                        case eMount.SOUTH: g.FillRectangle(getBrush(BlockColors.Lever), r.X + 3, r.Y, 2, 5); break;
                        case eMount.NORTH: g.FillRectangle(getBrush(BlockColors.Lever), r.X + 3, r.Y + 3, 2, 5); break;
                        case eMount.EAST: g.FillRectangle(getBrush(BlockColors.Lever), r.X + 3, r.Y + 3, 5, 2); break;
                        case eMount.WEST: g.FillRectangle(getBrush(BlockColors.Lever), r.X, r.Y + 3, 5, 2); break;
                    }

                    if (b.Powered)
                        g.FillEllipse(getBrush(BlockColors.WireOn), r.X + 2, r.Y + 2, 4, 4);
                    else
                        g.FillEllipse(getBrush(BlockColors.WireOff), r.X + 2, r.Y + 2, 4, 4);

                    break;

                case eBlock.BUTTON: // '\006'

                    if (b.Powered)
                        switch (b.Mount)
                        {
                            case eMount.SOUTH: g.FillRectangle(getBrush(BlockColors.Button), r.X + 2, r.Y + 7, 4, 1); break;
                            case eMount.NORTH: g.FillRectangle(getBrush(BlockColors.Button), r.X + 2, r.Y, 4, 1); break;
                            case eMount.EAST: g.FillRectangle(getBrush(BlockColors.Button), r.X + 7, r.Y + 2, 1, 4); break;
                            case eMount.WEST: g.FillRectangle(getBrush(BlockColors.Button), r.X, r.Y + 2, 1, 4); break;
                        }
                    else
                        switch (b.Mount)
                        {
                            case eMount.SOUTH: g.FillRectangle(getBrush(BlockColors.Button), r.X + 2, r.Y + 5, 4, 3); break;
                            case eMount.NORTH: g.FillRectangle(getBrush(BlockColors.Button), r.X + 2, r.Y, 4, 3); break;
                            case eMount.EAST: g.FillRectangle(getBrush(BlockColors.Button), r.X + 5, r.Y + 2, 3, 4); break;
                            case eMount.WEST: g.FillRectangle(getBrush(BlockColors.Button), r.X, r.Y + 2, 3, 4); break;

                        }
                    break;

                case eBlock.PRESS: // '\t'
                    if (b.Powered)
                        g.FillRectangle(getBrush(BlockColors.WireOn), r.X + 1, r.Y + 1, 6, 6);
                    else
                        g.FillRectangle(getBrush(BlockColors.Valve), r.X + 1, r.Y + 1, 6, 6);
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
                                g.FillRectangle(getBrush(BlockColors.Door), r.X, r.Y, 2, 8);
                                g.FillRectangle(getBrush(BlockColors.WireOff), r.X, r.Y + 6, 2, 2);
                                break;
                            case eMount.SOUTH: // Hinge upper right, Closed
                                g.FillRectangle(getBrush(BlockColors.Door), r.X, r.Y + 6, 2, 8);
                                g.FillRectangle(getBrush(BlockColors.WireOff), r.X + 6, r.Y, 2, 2);
                                break;
                            case eMount.EAST: // Hinge, upper left
                                g.FillRectangle(getBrush(BlockColors.Door), r.X, r.Y, 8, 2);
                                g.FillRectangle(getBrush(BlockColors.WireOff), r.X, r.Y, 2, 2);
                                break;
                            case eMount.WEST:  // bottom right
                                g.FillRectangle(getBrush(BlockColors.Door), r.X, r.Y + 6, 8, 2);
                                g.FillRectangle(getBrush(BlockColors.WireOff), r.X + 6, r.Y + 6, 2, 2);
                                break;
                        }
                    else
                        switch (b.Mount)
                        {
                            // FYI, this is facing, so its hinge to edge not powered
                            case eMount.NORTH: // Hinge, bottom left, Closed
                                g.FillRectangle(getBrush(BlockColors.Door), r.X, r.Y, 2, 8);
                                g.FillRectangle(getBrush(BlockColors.WireOff), r.X, r.Y + 6, 2, 2);
                                break;
                            case eMount.SOUTH: // Hinge upper right, Closed
                                g.FillRectangle(getBrush(BlockColors.Door), r.X, r.Y + 6, 2, 8);
                                g.FillRectangle(getBrush(BlockColors.WireOff), r.X + 6, r.Y, 2, 2);
                                break;
                            case eMount.EAST: // Hinge, upper left
                                g.FillRectangle(getBrush(BlockColors.Door), r.X, r.Y, 8, 2);
                                g.FillRectangle(getBrush(BlockColors.WireOff), r.X, r.Y, 2, 2);
                                break;
                            case eMount.WEST:  // bottom right
                                g.FillRectangle(getBrush(BlockColors.Door), r.X, r.Y + 6, 8, 2);
                                g.FillRectangle(getBrush(BlockColors.WireOff), r.X + 6, r.Y + 6, 2, 2);
                                break;
                        }


                    break;

                case eBlock.WATER:
                    //int col = fake ? Colors.water.getRGB() : 
                    // How deap the water is?
                    // ((~gp(x, y, z + p) & 7) * 24 + 87 << 24) + (Colors.water.getRGB() & 0xffffff);
                    //g.setColor(new Color(col, true));
                    g.FillRectangle(getBrush(BlockColors.Water), r);

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
            if (b == null) throw new ArgumentNullException("b", "Blocks is Null");
            if (b.Length != 3) throw new ArgumentException("Blocks is not 3 Length", "b");

            int p = 0;


            // Figure out what our background color needs to be
            // By the block at the bottom.
            // We are always going to see 3 layers, I may change it latter.
            switch (b[0].Type)
            {
                case eBlock.BLOCK: p++; g.FillRectangle(getBrush(BlockColors.Block), r); break;
                case eBlock.AIR: p++; g.FillRectangle(getBrush(BlockColors.Air), r); break;
                default: g.FillRectangle(getBrush(BlockColors.Air), r); break;
            }

            if (b[0].Type == eBlock.WIRE)
            {
                // Filler, right now I will make the wire all sides till
                // I get the draw wire routine right.
                gDrawWire(g, r, true, WireMask.None, true);
                p++;
                if (!b[p].isAir && !b[p].isBlock) // if the 
                    g.FillRectangle(getBrush(BlockColors.AirCover), r);
            }
            if (b[p].Type == eBlock.DOORB) p--; // DOORB fix
            gDrawBlock(g, r, b[p]);

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

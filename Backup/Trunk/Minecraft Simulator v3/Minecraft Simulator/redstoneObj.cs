using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Mincraft_Simulator
{

    // This class creates the tiny images used for pictures. I guess I could make this a super class and then inherit them all
    // ..  Sigh.  Hard to go from C to obj
    // Nono, this must be its own class.  I want one class to hold the images needed, this way we don't have thousands 
    // of single yellow squares .
    // Change the functions here on how the images look.  They are rotated in redstoneObj so
    // be sure each of these are facing north
    class redstoneBmp
    {
        const int blockMax = 256; // can only be 256 types of blocks
        const int tileHeight = 64;
        const int tileWidth = 64;
        Bitmap[][] img=null;

        int[][] textures = new int[blockMax][];

        
        // I'll make this vector drawing some day, right now its hardwired to 20,20.
        // public void redstoneBmp()
        // {
        //     makeImages();
        // }
        public redstoneBmp()
        {
            makeTextures();
        }
        static public Bitmap CreateBoxTexture()
        {
            Bitmap tmp = new Bitmap(100, 100, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(tmp);
            g.Clear(Color.Yellow);
            g.DrawRectangle(new Pen(Brushes.Black, 10f), 0, 0, 100, 100);
            g.Dispose();
            return tmp;
        }
        void RecreateImg()
        {
            // Overkill?  
            if (img != null)
            {
                for (int y = 0; y < img.Length; y++)
                {
                    if (img[y] != null)
                        for (int x = 0; x < img[y].Length; x++)
                            img[y][x].Dispose();
                    Array.Clear(img[y], 0, img[y].Length);
                    img[y] = null;
                }
                Array.Clear(img, 0, img.Length);
                img = null;
                
            }
            img = new Bitmap[blockMax][];
        }

        void debugSaveImages(string prefix, Bitmap[] iBmp)
        {
            for (int i = 0; i < iBmp.Length; i++)
            {
                iBmp[i].Save(prefix + "-" + i.ToString() + ".png", ImageFormat.Png);
            }

        }
        void makeTextures()
        {
            Bitmap tmpBmp = new Bitmap(tileWidth, tileHeight, PixelFormat.Format32bppArgb);
            RectangleF centerCircle = new RectangleF(
                (tileWidth / 2) - (tileWidth / 4),
                (tileHeight / 2) - (tileHeight / 4),
                (tileWidth / 4) * 2,
                (tileHeight / 4) * 2);
            RectangleF centerLine = new RectangleF(
                (tileWidth / 2) - (tileWidth / 8),
                (tileHeight / 2) - (tileHeight / 8),
                (tileWidth / 8) * 2,
                tileHeight);
            RectangleF entireTile = new RectangleF(0,0,tileWidth,tileHeight);


            int t;

            int[] nb = null;
            Graphics g;

            g = Graphics.FromImage(tmpBmp);
            //g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

            for (int idx = 0; idx < blockMax; idx++)
                if (Enum.IsDefined(typeof(blockType), (byte)idx))
                {
                    blockType cBlockType = (blockType)Enum.Parse(typeof(blockType), idx.ToString());
                    switch (cBlockType)
                    {
                        case blockType.StoneButton:
                            nb = new int[4];
                            //North,West,South,East
                            for (int i = 0; i < 4; i++)
                            {
                                g.Clear(Color.White);
                                g.FillRectangle(Brushes.Gray, 4, 14, 14, 19);
                                g.RotateTransform(i * 90);
                                nb[i] = glHelper.LoadTexture(tmpBmp);
                            }
                            //Why the hell did notch do it like this?
                            //South,North,West,East
                            t = nb[0]; nb[0] = nb[2]; nb[2] = nb[1]; nb[1] = t;
                            textures[idx] = nb;

                            break;
                        case blockType.RedstoneTorchOn:
                            //North,East,South,West
                            nb = new int[5];
                            g.Clear(Color.White);
                            g.FillRectangle(Brushes.Gray, centerLine);
                            g.FillEllipse(Brushes.Red, centerCircle);
                            for (int i = 0; i < 4; i++)
                            {
                                nb[i] = glHelper.LoadTexture(tmpBmp);
                                tmpBmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            }
                            // Floor Torch
                            g.Clear(Color.White);
                            g.FillEllipse(Brushes.Red, centerCircle);
                            nb[4] = glHelper.LoadTexture(tmpBmp);

                            //Why the hell did notch do it like this?
                            //South,North,West,East,Floor
                            t = nb[0];
                            nb[0] = nb[2];
                            nb[2] = nb[3];
                            nb[3] = nb[1];
                            nb[1] = t;
                            textures[idx] = nb;
                            break;
                        case blockType.RedstoneTorchOff:
                            //South,North,West,East,Floor
                            nb = new int[5];
                            g.Clear(Color.White);
                            g.FillRectangle(Brushes.Gray, centerLine);
                            g.FillEllipse(Brushes.Black, centerCircle);
                            for (int i = 0; i < 4; i++)
                            {
                                nb[i] = glHelper.LoadTexture(tmpBmp);
                                tmpBmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            }
                            // Floor Torch
                            g.Clear(Color.White);
                            g.FillEllipse(Brushes.Black, centerCircle);
                            nb[4] = glHelper.LoadTexture(tmpBmp);
                            t = nb[0];
                            nb[0] = nb[2];
                            nb[2] = nb[3];
                            nb[3] = nb[1];
                            nb[1] = t;
                            textures[idx] = nb;
                            break;
                        case blockType.RedstoneWire:
                            nb = new int[1];
                            Pen thickLine = new Pen(Color.Red, 5);
                            g.Clear(Color.White);
                            g.DrawLine(thickLine, 10, 0, 10, 20);
                            g.DrawLine(thickLine, 0, 10, 20, 10);
                            nb[0] = glHelper.LoadTexture(tmpBmp);
                            textures[idx] = nb;
                            break;
                        case blockType.Air:
                            nb = new int[1];
                            g.Clear(Color.Transparent);
                            nb[0] = glHelper.LoadTexture(tmpBmp);
                            textures[idx] = nb;
                            break;
                        default:
                            nb = new int[1];
                            g.Clear(Color.Yellow);
                            nb[0] = glHelper.LoadTexture(tmpBmp);
                            textures[idx] = nb;
                            break;
                    }
                }
                else
                {
                    // block not defined, going to make it white with a black X in it
                    Font boxFont = new Font("courier", 10, FontStyle.Regular);
                    StringFormat strCenterBox = new StringFormat();
                    strCenterBox.Alignment = StringAlignment.Center;
                    strCenterBox.LineAlignment = StringAlignment.Center;

                    nb = new int[1];
                    g.Clear(Color.Yellow);
                    
                    
                    g.Clear(Color.White);
                    g.DrawString(idx.ToString(), SystemFonts.DefaultFont, Brushes.Black, entireTile,strCenterBox);

                    nb[0] = glHelper.LoadTexture(tmpBmp);
                    textures[idx] = nb;
                }

        }

        public int[][] getAllTextures
        {
            get { return textures; }
        }
        public gBlockTypeStruct getSet(blockType iType)
        {
            return new gBlockTypeStruct(iType, textures[(int)iType]);
        }

        // This way we can fine the exact block and will image it, if its out of range it will display the 255 block
        // if it is in range but no defiend it wil display the block with the block number
        public gBlockTypeStruct getSet(int iType)
        {
            if (Enum.IsDefined(typeof(blockType), (byte)iType))
            {
                blockType cBlockType = (blockType)Enum.Parse(typeof(blockType), iType.ToString());
                return getSet(cBlockType);
            }
            else
                if (iType >= 0 && iType < blockMax)
                    return new gBlockTypeStruct(blockType.Undefined, textures[iType]);
                else
                    return new gBlockTypeStruct(blockType.Undefined, textures[blockMax - 1]);
        }
       
    }

    struct gBlockTypeStruct
    {
        public gBlockTypeStruct(blockType iType, int[] tiles) { bType = iType; bmp = tiles; X = 0; Y = 0; Z = 0; }
        public gBlockTypeStruct(blockType iType, int[] tiles, int x, int y, int z) { bType = iType; bmp = tiles; X = x; Y = y; Z = z; }
        public blockType bType;
        public int[]   bmp;
        public int X, Y, Z;
        public int getBitmap(int data)
        {
            // Set some offsets till I figure out the structure
            if (bType == blockType.RedstoneTorchOn)  data += 1;
            if (bType == blockType.RedstoneTorchOff) data += 1;
            if (data < bmp.Length)
                return bmp[data];
            else
                return bmp[0];
        }

    }
    // remember [FlagsAttribute]  I learned for masks
    // Only definding blocks that WILL BE USED IN THE PROGRAM.
    // Again, might change, but right now I want to draw something on the damn screen.
    // So only the stones that can have torches and wires attached are going to be defined, except for sand, lava and gravel
    // as they are affected by gravity and thats a whole another nightmare
    enum blockType : byte
    {
        // Blank Air, duh
        Air                 =   0,
        // Blocks you can stick switches and redstone on
        Stone               =   1,
        Grass               =   2,
        Dirt                =   3,
        Cobblestone         =   4,
        WoodenPlank         =   5,
        GoldOre             =   14,
        IronOre             =   15,
        CoalOre             =   16,
        Wood                =   17,
        GoldBrick           =   41,
        IronBrick           =   42,
        BrickBlock          =   45,
        Obsidian            =   49,
        DiamondOre          =   56,
        DiamondBlock        =   57,
        RedstoneOre         =   73,
        ClayBlock           =   82,
        // Things that need to know what direction they are facing
        Torch               =   50,     // Not redstone, but might impment so people can see at night or in a cave.
        RedstoneTorchOff     =  75,
        RedstoneTorchOn    =    76,
        StoneButton         =   77,
        Lever               =   69,
        RedstoneRepeaterOff =   93,
        RedstoneRepeaterOn  =   94,
        // Redstone Wire, have to make bitmaps of evey formation as its determed in game than in grid
        RedstoneWire        =   55,
        // PresurePlate
        StonePressurePlate  =   70,
        WoodenPressurePlate =   72,
        Undefined           =   0xFF    
        
    };
    enum dataWood : byte { Normal,RedWood,Birch };
    enum dataFire : byte { Placed,Eternal = 15 };
    enum dataLeaves : byte { Normal,RedWood,Birch } ;
    enum dataCoal : byte { Coal, Charcoal } ;
    enum dataSaplings : byte { Fresh,NewTree = 0xF };
    // I will put in more, but this is mainly a redstone simulator so I will stop here and
    // just put in the stuff I NEED

    // Same with redstone and normal Torches
    enum dataTorches : byte { South,North,West,East,Floor } ;
    // Mine cart, last 4 is when they go into a circle
    enum dataMineCart : byte { EastWest,NorthSouth,UpSouth,UpNorth,UpEast,UpWest,NorthEast,SouthEast,SouthWest,NorthWest }
    // Side of a block its attached
    enum dataLadders : byte { East=2,West,North,South } ; 
    // Leavers, the power bit is set if the switch is on. Directons is the faceing of the leaver, not the mount
    enum dataLeaver : byte { South=1,North,West,East,PointWestWhenOff,PointSouthWhenOff,Power = 8 }
    // Doors, location of the hinge on the block
    enum dataDoors : byte { NorthEast,SouthEast,SouthWest,NorthWest,CounterClockWise = 4, TopHalf = 8 };
    // Buttons, shouldn't need to worry about the pressed atrubute, just make sure its not set
    enum dataButton : byte { South,North,West,East,Pressed = 8 } ;
    // Humm, be useful to have sign posts, next version
    // Now for the copderestance (bad french:P), the whole reason I got back into minecraft
    // One, two, three and four are the tick wait for it, its the upper two bits
    // the lower two bits show the facing of the thing
    enum dataRedstoneRepeater : byte { East,South,West,North,One=0,Two=4,Three=8,Four=12 };

    
    // This is the basic redstone object that is used to hold the values of a block
    // Eventualy it will also have the simulator invovled, but for right now its 
    // a hollow class till I figure that nightmare out.
    class redstoneObj
    {
        int[] bmp;
        blockType bType;
        public redstoneObj North = null;
        public redstoneObj South = null;
        public redstoneObj West = null;
        public redstoneObj East = null;
        public redstoneObj Up = null;
        public redstoneObj Down = null;
        public int aData;
        public int X=0;
        public int Y=0;
        public int Z=0;

        // Constructors, MUST have the bmp to get refrences to images.
        public redstoneObj(gBlockTypeStruct bData)
        {
            bmp = bData.bmp;
            bType = bData.bType;
        }

        public int data
        {
            get
            {
                return aData;
            }
            set
            {
                aData = value;
            }

        }
        public int getBitmap()
        {
            return bmp[0];
        }
        public int getBitmap(int data)
        {
            if (bType == blockType.RedstoneTorchOn)
                data += 1;
            if (bType == blockType.RedstoneTorchOff)
                data += 1;
            if (data < bmp.Length)
                return bmp[data];
            else
                return bmp[0];
        }

    }
}

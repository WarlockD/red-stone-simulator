using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Redstone_Simulator
{
    public struct BlockVector
    {
        public static readonly BlockVector Null = new BlockVector();
        bool offset; public bool HasOffset { get { return offset; } }
        int x,y,z,xO,yO,zO;
        bool defined;
        public bool isNull { get { return !defined; }}


        public int X { get { if(offset) return xO; else return x; } }
        public int Y { get { if (offset) return yO; else return y; } }
        public int Z { get { if (offset) return zO; else return z; } }
        public BlockVector Old { get { return offset ? new BlockVector(x, y, z) : BlockVector.Null; } }
        public BlockVector(int X, int Y, int Z) : this() { x = X; y = Y; z = Z; defined = true; }
        
        public BlockVector(BlockVector v) : this(v.X, v.Y, v.Z) { }
        public void SetNext(int X, int Y, int Z) { xO = X ; yO = Y ; zO = Z ; offset = true; }
        public void SetNext(int X, int Y) { xO = X; yO = Y; zO = z; offset = true; }
        public void Offset(int X, int Y, int Z) { xO =X+ x; yO=Y + y; zO =Z + z; offset =true;}
        public void Offset(int X, int Y) { xO = X + x; yO = Y + y; zO = z; offset = true; }
        public void ClearOff() {  offset = false; }
        public BlockVector Up { get { return new BlockVector(X, Y, Z + 1); } }
        public BlockVector North {get { return new BlockVector(X, Y-1, Z); }}
        public BlockVector South { get { return new BlockVector(X, Y+1, Z); }}
        public BlockVector East { get { return new BlockVector(X+1, Y, Z); }}
        public BlockVector West { get { return new BlockVector(X-1, Y, Z); }}
        public BlockVector Down { get { return new BlockVector(X, Y, Z-1); }}
        
        public override bool Equals(object obj)
        {
            if (!(obj is BlockVector)) return false;
            BlockVector v = (BlockVector)obj;
            return v.X == X && v.Y == Y && v.Z == Z ;
        }
        public override int GetHashCode()
        {
            return new int[] { x, y, z }.GetHashCode();
        }
        public override string ToString()
        {
            return  String.Format("{0}:{0}:{0}",x,y,z); 
        }

    }
    public class BlockSim
    {
        private static int[,] dir = {
        {
            0, 0, -1
        }, {
            0, 1, 0
        }, {
            0, -1, 0
        }, {
            1, 0, 0
        }, {
            -1, 0, 0
        }
    };
        bool isJustWires = false;
        Blocks[, ,] data;
        int lenX, lenY, lenZ;
        int safeX, safeY, safeZ;
        bool isSafe = false;

        public int X { get { return lenX; } }
        public int Y { get { return lenY; } }
        public int Z { get { return lenZ; } }

        public BlockSim(int X, int Y, int Z)
        {
            data = new Blocks[X, Y, Z];
            lenX = X;
            lenY = Y;
            lenZ = Z;
        }

        /// <summary>
        /// Tests if two blocks can connect
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="tX"></param>
        /// <param name="tY"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public bool canConnectOld(int x, int y, int tX, int tY, int z)
        {
            BlockVector v = new BlockVector(x, y, z);
            Blocks test = GetBlock(v);
            if (test.isAir) // if the block is Air, return the connection below
                return GetBlock(v.Down).WireConn;
            if (test.isBlock) // if there isn't a block above us ANd we have a connection on the block in front..
                return !GetBlock(v.Old.Up).isBlock && GetBlock(v.Up).WireConn;
            if (test.Type == eBlock.REPEATER)
            {
                // Repeater matters what side its connected too.
                // Luckly ANYTHING can power a repeater
                switch (test.Mount)
                {
                    case eMount.NORTH:
                    case eMount.SOUTH:
                        if ((tY < y || tY > y) && tX == x) return true; else return false;
                    case eMount.EAST:
                    case eMount.WEST:
                        if ((tX < x || tX > x) && tY == y) return true; else return false;
                    default:
                        return false;
                }

            }
            else
                return true;
        }
        bool CheckMount(eBlock t)
        {
            switch (t)
            {
                case eBlock.TORCH: return true;
                case eBlock.LEVER: return true;
                case eBlock.BUTTON: return true;
                case eBlock.REPEATER: return true; // block can charge a repeater
                default:
                    return false;
            }
        }

        public bool canConnect(int x, int y, int tX, int tY, int z)
        {
            BlockVector v = new BlockVector(x, y, z); v.SetNext(tX, tY);
            Blocks test = GetBlock(v);

            if (test.WireConn)
                return true;
            if (test.isAir)
                return GetBlockType(v.Down)== eBlock.WIRE;
            if (test.isBlock)
            {
                BlockVector t = BlockVector.Null;
                if (GetBlockType(v.Up) == eBlock.WIRE && !(GetBlockType(v.Old.Up) == eBlock.BLOCK))
                    return true;

                if (GetBlock(v.North).Mount == eMount.NORTH)  t = v.North;
                if (GetBlock(v.South).Mount == eMount.SOUTH) t = v.South;
                if (GetBlock(v.East).Mount == eMount.EAST) t = v.East;
                if (GetBlock(v.West).Mount == eMount.WEST) t = v.West; 
                if (GetBlock(v.Up).Mount == eMount.TOP) t = v.Up;
                
                if(!t.isNull)
                        if (GetBlockType(t) == eBlock.TORCH)
                            return blockConnect(v.X, v.Y, v.Old.X - v.X, v.Old.Y - v.Y, z, false);
                        else
                            return GetBlock(v.Up).Mount != eMount.TOP || GetBlockType(t) != eBlock.LEVER;


                return GetBlockType(v.Down) == eBlock.TORCH;
            }
            else
            {
                return false;
            }

        }
        public void touchControl(int x, int y, int z)
        {

        }


        public Boolean blockConnect(int x, int y, int dy, int dx, int z, bool pow)
        {
            // Ok, top line is to check if its a wire or if its powered.  Ahh, this is for the wire drawing or sim
            if (this[x + dx, y + dy, z].Type != eBlock.WIRE || this[x + dx, y + dy, z].Powered && pow)
                return false;
            // 
            if (this[x + dx + dy, (y + dy) - dx, z].isBlock)
            {
                if (!this[x + dx, y + dy, z + 1].isBlock && this[x + dx + dy, (y + dy) - dx, z + 1].WireConn)
                    return false;
            }
            else
                if (this[x + dx + dy, (y + dy) - dx, z].isAir)
                {
                    if (this[x + dx + dy, (y + dy) - dx, z - 1].WireConn)
                        return false;
                }
                else
                {
                    return false;
                }
            if (this[(x + dx) - dy, y + dy + dx, z].isBlock) // Check the direction right to see if its a torch.  Not sure if I can use this
                return this[x + dx, y + dy, z + 1].isBlock || !this[(x + dx) - dy, y + dy + dx, z + 1].WireConn;
            if (this[(x + dx) - dy, y + dy + dx, z].isAir)
                return !this[(x + dx) - dy, y + dy + dx, z - 1].WireConn;
            else
                return false;
        }



        /// <summary>
        /// Tests if the location is valid
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public bool TestLoc(int x, int y, int z)
        {
            if (z < 0 || z >= lenZ || y < 0 || y >= lenY || x < 0 || x >= lenX)
            { isSafe = false; return false; }
            isSafe = false;
            safeX = x; safeY = y; safeZ = z;
            isSafe = true;

            return true;
        }
        public void SetBlock(BlockVector v, eBlock b)
        {
            if (v.Z < 0 || v.Z >= lenZ || v.Y < 0 || v.Y >= lenY || v.X < 0 || v.X >= lenX)
                return;

            data[v.X,v.Y,v.Z] = new Blocks(b);

        }
        public Blocks GetBlock(BlockVector v)
        {
            if (v.Z < 0)
                return Blocks.BLOCK;
            if (v.Z >= lenZ || v.Y < 0 || v.Y >= lenY || v.X < 0 || v.X >= lenX)
                return Blocks.AIR;

            return data[v.X, v.Y, v.Z];
        }
        public eBlock GetBlockType(BlockVector v)
        {
            if (v.Z < 0)
                return eBlock.BLOCK;
            if (v.Z >= lenZ || v.Y < 0 || v.Y >= lenY || v.X < 0 || v.X >= lenX)
                return eBlock.AIR;

            return data[v.X, v.Y, v.Z].Type;
        }
        public void SetBlock(int x, int y, int z, eBlock b)
        {
            if (z < 0 || z >= lenZ || y < 0 || y >= lenY || x < 0 || x >= lenX)
                return;

            data[x, y, z] = new Blocks(b);

        }
        public Blocks GetBlock(int x, int y, int z)
        {
            if (z < 0)
                return Blocks.BLOCK;
            if (z >= lenZ || y < 0 || y >= lenY || x < 0 || x >= lenX)
                return Blocks.AIR;

            return data[x, y, z];
        }
        public void SetPower(BlockVector v,int p)
        {
            if (v.Z < 0 || v.Z >= lenZ || v.Y < 0 || v.Y >= lenY || v.X < 0 || v.X >= lenX)
                return;

            data[v.X, v.Y, v.Z].Power = p;
        }
        public void SetPower(int x, int y, int z,int p)
        {
            if (z < 0 || z >= lenZ || y < 0 || y >= lenY || x < 0 || x >= lenX)
                return;

            data[x, y, z].Power=p;
        }
        public int GetPower(BlockVector v)
        {
            if (v.Z < 0 || v.Z >= lenZ || v.Y < 0 || v.Y >= lenY || v.X < 0 || v.X >= lenX)
                return 0;

            return data[v.X, v.Y, v.Z].Power;
        }
        public int GetPower(int x, int y, int z)
        {
            if (z < 0 || z >= lenZ || y < 0 || y >= lenY || x < 0 || x >= lenX)
                return 0;

            return data[x, y, z].Power;
        }

        public void RotateMount(int x, int y, int z)
        {
            if (z < 0 || z >= lenZ || y < 0 || y >= lenY || x < 0 || x >= lenX)
                return;

            data[x, y, z].Rotate();
        }

        public Blocks this[int x, int y, int z]
        {
            get
            {
                if (z < 0)
                    return Blocks.BLOCK;
                if (z >= lenZ)
                    return Blocks.AIR;
                if (y < 0 || y >= lenY || x < 0 || x >= lenX)
                     return Blocks.AIR;
                safeX = x; safeY = y; safeZ = z;
                return data[x, y, z];
            }
        }

        private void followWire(BlockVector v, int p)
        {
            Blocks b = GetBlock(v);
            if (p <= b.Power)
                return;
            SetPower(v, p);
            if (p == 0)
            {
                return;
            }
            else
            {
                followWireQ(v.North, p - 1);
                followWireQ(v.South, p - 1);
                followWireQ(v.West, p - 1);
                followWireQ(v.East, p - 1);
                return;
            }
        }

        private void followWireQ(BlockVector v, int p)
        {
            if (GetBlock(v).Type == eBlock.WIRE)
                followWire(v, p);
            else
                if (GetBlock(v).isBlock)
                {
                    if (GetBlock(v.Up).Type == eBlock.WIRE && !GetBlock(v.Old).isBlock)
                        followWire(v.Up, p);
                }
                else
                    if (GetBlock(v.Down).Type == eBlock.WIRE)
                        followWire(v.Down, p);
        }

        public void tick()
        {


            for (int z = 0; z < lenZ; z++)
                for (int y = 0; y < lenY; y++)
                    for (int x = 0; x < lenX; x++)
                    {
                        BlockVector v = new BlockVector(x, y, z);
                        Blocks b = data[x, y, z];
                        if (b.Type == eBlock.TORCH)
                        {
                            switch (b.Mount)
                            {
                                case eMount.TOP: SetPower(v, GetPower(v.Down) < 16 ? 16 : 0); break;
                                case eMount.SOUTH: SetPower(v, GetPower(v.South) < 16 ? 16 : 0); break;
                                case eMount.NORTH: SetPower(v, GetPower(v.North) < 16 ? 16 : 0); break;
                                case eMount.EAST: SetPower(v, GetPower(v.East) < 16 ? 16 : 0); break;
                                case eMount.WEST: SetPower(v, GetPower(v.West) < 16 ? 16 : 0); break;
                            }
                        }
                        else
                            if (b.CtrlOn && (b.Type == eBlock.BUTTON || b.Type == eBlock.PRESS))
                                data[x, y, z].Power--;

                        update();
                    }
        }
      

        public void update()
        {
            // Clear power, tick to torches.  Put in repeater in here
            for (int z = 0; z < lenZ; z++)
                for (int y = 0; y < lenY; y++)
                    for (int x = 0; x < lenX; x++)
                    {
                        BlockVector v = new BlockVector(x,y,z);
                        Blocks b = GetBlock(v);
                        if (b.Type == eBlock.WIRE || b.Type == eBlock.DOORB)
                            SetPower(v,0);
                        else
                            if (b.isBlock)
                            {
                                SetPower(v,0);
                                if (GetBlock(v.Down).Powered && GetBlock(v.Down).Type == eBlock.TORCH)
                                {
                                    // A torch below a block powers the block
                                    SetPower(v,17);
                                }
                                else
                                {
                                    // Check if there is a switch on the block and turn it on
                                    if(GetBlock(v.North).isControl && GetBlock(v.North).Mount == eMount.NORTH && GetBlock(v).Powered) SetPower(v,17);
                                    if(GetBlock(v.South).isControl && GetBlock(v.South).Mount == eMount.SOUTH && GetBlock(v).Powered) SetPower(v,17);
                                    if(GetBlock(v.East).isControl && GetBlock(v.East).Mount == eMount.EAST && GetBlock(v).Powered) SetPower(v,17);
                                    if(GetBlock(v.West).isControl && GetBlock(v.West).Mount == eMount.WEST && GetBlock(v).Powered) SetPower(v,17);
                                    if(GetBlock(v.Up).isControl && GetBlock(v.Up).Mount == eMount.TOP && GetBlock(v).Powered) SetPower(v,17);
                                }
                            }

                    }

            for (int z = 0; z < lenZ; z++)
                for (int y = 0; y < lenY; y++)
                    for (int x = 0; x < lenX; x++)
                    {
                        BlockVector v = new BlockVector(x,y,z);
                        Blocks b = GetBlock(v);
                        if (b.Power >= (b.Type != eBlock.BUTTON && b.Type != eBlock.PRESS ? 16 : 1)
                            && (b.Type == eBlock.TORCH || b.isControl || b.isBlock && b.Power == 17))
                        {
                            if (GetBlock(v.Up).Type == eBlock.WIRE) followWire(v.Up, 15);
                            if (GetBlock(v.Down).Type == eBlock.WIRE) followWire(v.Down, 15);
                            if (GetBlock(v.North).Type == eBlock.WIRE) followWire(v.North, 15);
                            if (GetBlock(v.South).Type == eBlock.WIRE) followWire(v.South, 15);
                            if (GetBlock(v.East).Type == eBlock.WIRE) followWire(v.East, 15);
                            if (GetBlock(v.West).Type == eBlock.WIRE) followWire(v.West, 15);
                        }
                    }



            for (int z = 0; z < lenZ; z++)
                for (int y = 0; y < lenY; y++)
                    for (int x = 0; x < lenX; x++)
                    {
                        BlockVector v = new BlockVector(x,y,z);
                        Blocks b = GetBlock(v);
                        if ((b.isBlock && !b.Powered || b.Type == eBlock.DOORA)
                            && (GetBlock(v.Up).Type == eBlock.WIRE && GetBlock(v.Up).Powered ||
                            blockConnect(x, y, 0, 1, z, true) ||
                            blockConnect(x, y, 0, -1, z, true) ||
                            blockConnect(x, y, 1, 0, z, true) ||
                            blockConnect(x, y, -1, 0, z, true)))
                            data[x, y, z].Power = 16;

                    }

        }




    }


}



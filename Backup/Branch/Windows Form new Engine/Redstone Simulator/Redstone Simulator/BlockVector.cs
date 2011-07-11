using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Redstone_Simulator
{
    public struct BlockVector : IEquatable<BlockVector>
    {
        //  static int lenX, lenY, lenZ;
        //  public static void SetLimit(int x, int y, int z) { lenX =x; lenY = y; lenZ = z; }
        int x, y, z;
        public int X { get { return x; } }
        public int Y { get { return y; } }
        public int Z { get { return z; } }
        public BlockVector(int X, int Y, int Z) : this() { x = X; y = Y; z = Z; }
        public BlockVector(BlockVector v) : this(v.X, v.Y, v.Z) { }
        public BlockVector(Point p, int z) : this(p.X, p.Y, z) { }
        public BlockVector Up { get { return new BlockVector(X, Y, Z + 1); } }
        public BlockVector North { get { return new BlockVector(X, Y - 1, Z); } }
        public BlockVector South { get { return new BlockVector(X, Y + 1, Z); } }
        public BlockVector East { get { return new BlockVector(X + 1, Y, Z); } }
        public BlockVector West { get { return new BlockVector(X - 1, Y, Z); } }
        public BlockVector Down { get { return new BlockVector(X, Y, Z - 1); } }
        public BlockVector RotateLeft(Direction d)
        {
            switch (d)
            {
                case Direction.NORTH: return this.West;
                case Direction.SOUTH: return this.East;
                case Direction.EAST: return this.North;
                case Direction.WEST: return this.South;
                default:
                    return this;
            }
        }
        public BlockVector RotateRight(Direction d)
        {
            switch (d)
            {
                case Direction.NORTH: return this.East;
                case Direction.SOUTH: return this.West;
                case Direction.EAST: return this.South;
                case Direction.WEST: return this.West;
                default:
                    return this;
            }
        }
        public BlockVector Flip(Direction d)
        {
            switch (d)
            {
                case Direction.UP: return this.Down;
                case Direction.DOWN: return this.Up;
                case Direction.NORTH: return this.South;
                case Direction.SOUTH: return this.North;
                case Direction.EAST: return this.West;
                case Direction.WEST: return this.East;
                default:
                    return this;
            }
        }
        public BlockVector Dir(Direction d)
        {
            switch (d)
            {
                case Direction.UP: return this.Up;
                case Direction.DOWN: return this.Down;
                case Direction.NORTH: return this.North;
                case Direction.SOUTH: return this.South;
                case Direction.EAST: return this.East;
                case Direction.WEST: return this.West;
                default:
                    return this;
            }
        }
        public BlockVector Offset(int x, int y) { return new BlockVector(this.x +x, this.y +y, this.z); }
        public BlockVector Offset(Point p) { return Offset(p.X, p.Y); } 
        public BlockVector Offset(int x, int y, int z) { return new BlockVector(this.x + x, this.y + y, this.z+z); }
        public BlockVector ChangeXY(int x, int y) { return new BlockVector( x,  y, this.z); }
        public BlockVector ChangeXY(Point p) { return ChangeXY(p.X, p.Y); }
        public BlockVector ChangeZ(int z) { return new BlockVector(this.x, this.y, z); }

       
        public override bool Equals(object obj)
        {
            if (!(obj is BlockVector)) return false;
            BlockVector v = (BlockVector)obj;
            return v.X == X && v.Y == Y && v.Z == Z;
        }
        public  bool Equals(BlockVector v)
        {
            return v.X == X && v.Y == Y && v.Z == Z;
        }
        public override string ToString()
        {
            return String.Format("(BlockVector= {0}:{1}:{2})", x, y, z);
        }
        public static bool operator ==(BlockVector v1, BlockVector v2)
        {
            return v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z;
        }
        public static bool operator !=(BlockVector v1, BlockVector v2)
        {
            return !(v1 == v2);
        }
        public override int GetHashCode()
        {
            return Hash(x, y, z);
        }
        /// <summary>
        /// Static Hash of the BlockVector.  Limits Z to having 256 levels and X,Y having 4096 blocks big
        /// </summary>
        /// <param name="x">X axis</param>
        /// <param name="y">Y axis</param>
        /// <param name="z">Z axis</param>
        /// <returns></returns>
        public static int Hash(int x, int y, int z) { return ((z & 0xFF) << 24) + ((y & 0xFFF) << 12) + (x & 0xFFF); }
    }
}

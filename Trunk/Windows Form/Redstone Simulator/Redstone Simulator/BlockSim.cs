using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Redstone_Simulator
{

    public class BlockSim
    {
        bool cyclic = false;
     
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
        /// Tests if the location is valid
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public bool TestLoc(int x, int y, int z)
        {
            if (z < 0 | z >= lenZ)
            { isSafe = false; return false;}
            if (!cyclic)
                if (y < 0 || y >= lenY || x < 0 || x >= lenX)
                { isSafe = false; return false; }
            isSafe = false;
            safeX = x; safeY = y; safeZ = z;
            isSafe = true;

            return true;
        }

        public  Blocks this[int x, int y, int z]
        {
            get
            {
                if (z < 0)
                    return Blocks.BLOCK;
                if (z >= lenZ)
                    return Blocks.AIR;
                if (cyclic)
                {
                    y = (y % lenY + lenY) % lenY;
                    x = (x % lenX + lenX) % lenX;
                }
                else
                    if (y < 0 || y >= lenY || x < 0 || x >= lenX)
                        return Blocks.AIR;
                safeX = x; safeY = y; safeZ = z;
                return  data[x, y, z];
            }
            set
            {
                if (isSafe || (safeX == x && safeY == y && safeZ == z))
                {
                    if(isSafe) isSafe = false;
                    data[safeX, safeY, safeZ] = value;
                    return;
                }
                if (z < 0 | z >= lenZ)
                    return;
                if (cyclic)
                {
                    y = (y % lenY + lenY) % lenY;
                    x = (x % lenX + lenX) % lenX;
                }
                else
                    if (y < 0 || y >= lenY || x < 0 || x >= lenX)
                        return;

                data[x, y, z] = value;
            }
        }


     

    }
}



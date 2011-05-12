﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;

namespace Redstone_Simulator
{
    
    
    public class BlockSim
    {
        Blocks data;
       // Blocks lastTick;
        readonly Direction[] Directions = {
                                 Direction.DOWN,
                                 Direction.NORTH,
                                 Direction.EAST,
                                 Direction.SOUTH,
                                 Direction.WEST,
                                 Direction.UP
                                  };
        readonly Direction[]  CompassDir = {
                                 Direction.NORTH,
                                 Direction.EAST,
                                 Direction.SOUTH,
                                 Direction.WEST,
                                  };

        List<BlockVector> update, source;
        int lenX, lenY, lenZ;
        public int X { get { return lenX; } }
        public int Y { get { return lenY; } }
        public int Z { get { return lenZ; } }

        public BlockSim(int X, int Y, int Z)
        {
          //  BlockVector.SetLimit(X,Y,Z);
            source = new List<BlockVector>();
            update = new List<BlockVector>();
            data = new Blocks(X, Y, Z);
            lenX = X; lenY = Y; lenZ = Z;
       
        }
        public void setConnections(BlockVector v)
        {
            data[v].Mask = getConnections(v);
            foreach (Direction d in CompassDir)
                data[v.Dir(d)].Mask = getConnections(v.Dir(d));
        }
        public Block this[int x, int y, int z]
        {
            get { return data[x, y, z]; }
            set { data[x, y, z] = value; }
        }
        public Block this[BlockVector v]
        {
            get { return data[v]; }
            set { data[v] = value;  }
        }
        public Block this[int i]
        {
            get { return data[i]; }
            set { data[i] = value; }
        }
       
        public static bool TestMask(WireMask obj,WireMask toTest )
        {
            return ((obj & toTest) == toTest);
        }
        void followWireQ(BlockVector v, Direction d, int pow)
        {
            BlockVector n = v.Dir(d);
            if (data[n].isWire) followWire(n, pow);
            if (data[n].isBlock)
            {
                if (data[n.Up].isWire && !data[v.Up].isBlock)
                    followWire(n.Up, pow);
            }
            else
                if (data[n.Down].isWire)
                    followWire(n.Down, pow);
        }
        void followWire(BlockVector v, int pow)
        {
            if (pow <= data[v].Charge)
                return;
            data[v].Charge = pow;
            if (pow > 0)
                foreach(Direction d in Directions)
                    followWireQ(v,d,pow -1);
        }
        public void newTick()
        {
            // Turn on all torches that are powered by a block, this is where we tick the repeater when I get to it
            for (int x = 0; x < lenX; x++) for (int y = 0; y < lenY; y++) for (int z = 0; z < lenZ; z++)
                    {
                        BlockVector v = new BlockVector(x, y, z);
                        Block b = data[x, y, z];
                        switch (b.ID)
                        { // Check torches and count down button, pad timers.
                            case BlockType.TORCH: // Power a torch from a powered block
                                b.Source = data[v.Dir(b.Place)].Source;
                                break;
                            case BlockType.PREASUREPAD:
                            case BlockType.BUTTON:
                                if (b.Powered) b.Charge--;
                                break;
                        }
                    }


            updateT();

        }
        public void updateT()
        {
            // Change this to be using a stack
            // Clear all old power caculations and power the blocks for the next tick
            for (int x = 0; x < lenX; x++) for (int y = 0; y < lenY; y++) for (int z = 0; z < lenZ; z++) 
                {
                        BlockVector v = new BlockVector(x, y, z);
                        Block b = data[v];
                        switch (b.ID)
                        {
                            case BlockType.WIRE: b.Charge = 0; break;
                            case BlockType.BLOCK:
                                b.Source = false;
                                if(data[v.Down].Source && data[v.Down].isTorch)
                                    b.Source = true; // Block is powered by a torch
                                 else 
                                    foreach(Direction d in CompassDir)
                                    {
                                        Block t = data[v.Dir(d)];
                                        if (t.isControl && t.Place == d && t.Source)
                                            b.Source = true;
                                    }
                                
                                break;
                        }
                }

            // Power on all the wires
            for (int x = 0; x < lenX; x++) for (int y = 0; y < lenY; y++) for (int z = 0; z < lenZ; z++)
                    {
                        BlockVector v = new BlockVector(x, y, z);
                        Block b = data[v];
                        switch (b.ID)
                        {
                            case BlockType.BUTTON:
                            case BlockType.PREASUREPAD:
                            case BlockType.TORCH:
                                if (b.Powered)
                                    SearchWire(v);
                                break;
                            case BlockType.BLOCK:
                                if (b.Charge == 17)
                                    SearchWire(v);
                                break;
                        }
                    }

            for (int x = 0; x < lenX; x++) for (int y = 0; y < lenY; y++) for (int z = 0; z < lenZ; z++)
                    {
                        BlockVector v = new BlockVector(x, y, z);
                        Block b = data[v];
                        if (b.isBlock && !b.Powered ||
                            blockConnect(v, Direction.NORTH, true) ||
                            blockConnect(v, Direction.SOUTH, true) ||
                            blockConnect(v, Direction.EAST, true) ||
                            blockConnect(v, Direction.WEST, true))
                            b.Charge = 16;

                    }

        }
        void SearchWire(BlockVector v)
        {
            foreach (Direction d in Directions)
                if (data[v.Dir(d)].isWire)
                    followWire(v.Dir(d), 15);
        }
        public void tick()
        {
            //lastTick = data.Copy();
            for (int x = 0; x < lenX; x++) for (int y = 0; y < lenY; y++) for (int z = 0; z < lenZ; z++)
                        if (!data[x,y,z].isAir) // lets make this quick and not worry about stuff not air.
                        {
                            BlockVector v = new BlockVector(x, y, z);
                            Block b = data[x, y, z];
                            bool isPowered = false;
                            switch (b.ID)
                            {
                                case BlockType.TORCH:
                                    switch (b.Place)
                                    {
                                        case Direction.DOWN:
                                            if (z > 0) if (data[v.Down].Powered) isPowered = true; break;
                                        case Direction.NORTH:
                                            if (y > 0) if (data[v.North].Powered) isPowered = true; break;
                                        case Direction.EAST:
                                            if (x < lenX - 1) if (data[v.East].Powered) isPowered = true; break;
                                        case Direction.SOUTH:
                                            if (y < lenY - 1) if (data[v.South].Powered) isPowered = true; break;
                                        case Direction.WEST:
                                            if (x > 0) if (data[v.West].Powered) isPowered = true; break;
                                    }
                                    if (data[v].waitTick(isPowered)) update.Add(v);
                                    break;

                                case BlockType.REPEATER:
                                    switch (b.Place)
                                    {
                                        case Direction.NORTH: // pointing north
                                            if (y < lenY - 1)
                                                if ((data[v.South].canBePoweredByRepeater(b.Place)) ||
                                                    (data[v.South].canBePoweredByRepeaterTorch(getConnections(v.North), WireMask.North)))
                                                    data[v].waitTick(true);
                                            break;
                                        case Direction.EAST:
                                            if (x < lenX - 1)
                                                if ((data[v.East].canBePoweredByRepeater(b.Place)) ||
                                                        (data[v.East].canBePoweredByRepeaterTorch(getConnections(v.East), WireMask.East)))
                                                    data[v].waitTick(true);
                                            break;
                                        case Direction.SOUTH:
                                            if (y < lenY - 1)
                                                if ((data[v.South].canBePoweredByRepeater(b.Place)) ||
                                                        (data[v.South].canBePoweredByRepeaterTorch(getConnections(v.South), WireMask.East)))
                                                    data[v].waitTick(true);
                                            break;
                                        case Direction.WEST:
                                            if (x > 0)
                                                if ((data[v.West].canBePoweredByRepeater(b.Place)) ||
                                                       (data[v.West].canBePoweredByRepeaterTorch(getConnections(v.West), WireMask.East)))
                                                    data[v].waitTick(true);
                                            break;
                                    }
                                    update.Add(v);
                                    break;
                                case BlockType.BUTTON:
                                case BlockType.PREASUREPAD:
                                    if (data[x, y, z].waitTick(false))
                                        update.Add(v);
                                    break;

                            }
                        }

        }

        public void noTick()
        {
            data.ClearChanged();
            for (int x = 0; x < lenX; x++) for (int y = 0; y < lenY; y++) for (int z = 0; z < lenZ; z++)
                        if (!data[x, y, z].isBlock && data[x, y, z].Source) // look for sources!
                        {
                            source.Add(new BlockVector(x, y, z));
                            update.Add(new BlockVector(x, y, z));
                        }
                        else if (data[x, y, z].Powered) // set charge to temp to zero?
                        {
                            data[x, y, z].Charge = 0;
                            update.Add(new BlockVector(x, y, z));
                        }


            //spreading the power
            for (int h = 0; h < 20; h++)
            { //runs long enough
                List<BlockVector> now = new List<BlockVector>(source); // currently under investgation
                source.Clear();
                foreach (BlockVector v in now)
                {
                    Block block = data[v];
                    switch (block.ID)
                    {
                        case BlockType.BLOCK:
                            if (block.Charge == 16) BlockPower(v); break; //directly powered
                        case BlockType.WIRE: WirePower(v); break;
                        case BlockType.TORCH: TorchPower(v); break;
                        case BlockType.REPEATER: RepeaterPower(v); break;
                        case BlockType.BUTTON:
                        case BlockType.PREASUREPAD:
                        case BlockType.LEVER: InputPower(v); break;
                    }
                }


            }

            //updates all not similar to before
            for (int i = 0; i < update.Count; i++)
                if (data[update[i]].isWire)  //only wire will can be bypassed
                    if (data[update[i]].changedCharge)
                    {
                        data[update[i]].ClearChanged();
                        update.RemoveAt(i); i--; 
                    }
        

            // We can clone update and have the GUI only update those blocks.

            update.Clear();
            source.Clear();



            //GUI.updateBlocks(xArray, yArray, zArray);

        }

        public bool WireConn(BlockVector v, Direction dir)
        {
            // Fix for repeater
            BlockVector n = v.Dir(dir); // Cache the current direction we are facing
            if (data[n].isAir) return data[n.Down].canConnect;// if 
            if (data[n].isBlock) return !data[v.Up].isBlock && data[n.Up].canConnect;
            else
                return true;
           
        }
    
        public bool getSingleConnection(BlockVector v, Direction dir)
        {
                BlockVector n = v.Dir(dir);
                if (data[n].isAir && data[n.Down].canConnect) return true;
                if(data[n].canConnect) return true;
                if(data[v.Up].isAir && data[n].isBlock && data[n.Up].canConnect) return true;
                return false;
        }
        bool blockConnect(BlockVector v, Direction d, bool pow)
        {
            BlockVector n = v.Dir(d);
            BlockVector t1 = n.RotateLeft(d);
            BlockVector t2 = n.RotateRight(d);
            if (!data[n].isWire || !data[n].Powered && pow)
                return false ;
            if (data[t1].isBlock)
            {
                if (!data[n].isBlock && data[t1.Up].canConnect)
                    return false;
            }
            else if (data[t1].isAir)
            {
                if (data[t1.Down].canConnect)
                    return false;
            }
            if(data[t2].isBlock)
                return data[n.Up].isBlock || !data[t2.Up].canConnect;
            if(data[t2].isAir)
                return !data[t2.Down].canConnect;
            else
                return false;
        }



        public WireMask getConnections(BlockVector v)
        { //note: 5. boolean value is showing, if it should power blocks
            // This is only on blocks DUH
            if (!data[v].isWire)
                return WireMask.NotConnected;

            WireMask o = WireMask.NotConnected;
            int no = 0;
            if(getSingleConnection(v,Direction.NORTH)) { o |= WireMask.North; no++; }
            if(getSingleConnection(v,Direction.SOUTH)) { o |= WireMask.South; no++; }
            if(getSingleConnection(v,Direction.EAST)) { o |= WireMask.East; no++; }
            if(getSingleConnection(v,Direction.WEST)) { o |= WireMask.West; no++; }
            //in special cases
  
            switch (no)
            {
                case 0: // Not connected
                    return WireMask.North | WireMask.South | WireMask.East | WireMask.West | WireMask.BlockPower;
                case 1: // One Connected
                    switch (o)
                    {
                        case WireMask.North: o |= WireMask.South; break;
                        case WireMask.South: o |= WireMask.North; break;
                        case WireMask.West: o |= WireMask.East; break;
                        case WireMask.East: o |= WireMask.West; break;
                    }
                    o |= WireMask.BlockPower;
                    break;
                case 2:
                    if ((o & WireMask.North & WireMask.South) == (WireMask.North & WireMask.South) ||
                    (o & WireMask.East & WireMask.West) == (WireMask.East & WireMask.West))
                        o |= WireMask.BlockPower;
                    break;
            }
            return o;

        }
        

        private void TorchPower(BlockVector v)
        {
            foreach (Direction d in Directions)
                if(d == Direction.UP)
                    Power(v, d, BlockType.BLOCK, 16);
                else
                    Power(v, d, BlockType.WIRE, 15);
        }

        private void RepeaterPower(BlockVector v)
        {
            Power(v, data[v].Place, BlockType.WIRE, 15);
            Power(v, data[v].Place, BlockType.BLOCK, 16);
        }

        private void InputPower(BlockVector v)
        {
            Power(v, data[v].Place, BlockType.BLOCK, 16);
            foreach (Direction d in Directions)
                if(d!=Direction.UP)
                    Power(v, d, BlockType.WIRE, 15);
        }
        private void BlockPower(BlockVector v)
        {
            foreach (Direction d in Directions)
                Power(v, d, BlockType.WIRE, 15);
        }
        private void WirePower(BlockVector v)
        {
            WireMask c = getConnections(v);
            Block current = data[v];
            //up
            if (v.Z < lenZ)
                if (!data[v.Up].isBlock)
                { //can be lead up
                    if (((c & WireMask.North) == WireMask.North) && (v.Y > 0))  // Connected north and not a wall
                        if (current.Charge - 1 > data[v.North.Up].Charge)
                            Power(v.Up, Direction.NORTH, BlockType.WIRE, current.Charge - 1);

                    if (((c & WireMask.West) == WireMask.West) && (v.X > 0))  // Connected north and not a wall
                        if (current.Charge - 1 > data[v.West.Up].Charge)
                            Power(v.Up, Direction.WEST, BlockType.WIRE, current.Charge - 1);

                    if (((c & WireMask.South) == WireMask.South) && (v.Y < lenY - 1))  // Connected north and not a wall
                        if (current.Charge - 1 > data[v.South.Up].Charge)
                            Power(v.Up, Direction.SOUTH, BlockType.WIRE, current.Charge - 1);

                    if (((c & WireMask.East) == WireMask.East) && (v.X < lenX - 1))  // Connected north and not a wall
                        if (current.Charge - 1 > data[v.East.Up].Charge)
                            Power(v.Up, Direction.EAST, BlockType.WIRE, current.Charge - 1);
                }

            //down
            if (v.Z > 0)
            {
                if (current.Charge > data[v.Down].Charge) // If block directly under
                    Power(v, Direction.DOWN, BlockType.BLOCK, data[v].Charge);


                if (((c & WireMask.North) == WireMask.North) && (v.Y > 0))  // Connected north and not a wall
                    if (!data[v.North].isBlock && current.Charge - 1 > data[v.North.Down].Charge )
                        Power(v.Down, Direction.NORTH, BlockType.WIRE, current.Charge - 1);

                if ( ((c & WireMask.West) == WireMask.West) && (v.X > 0))  // Connected north and not a wall
                    if (!data[v.West].isBlock && current.Charge - 1 > data[v.West.Down].Charge)
                        Power(v.Up, Direction.WEST, BlockType.WIRE, current.Charge - 1);

                if ( ((c & WireMask.South) == WireMask.South) && (v.Y < lenY - 1))  // Connected north and not a wall
                    if (!data[v.South].isBlock && current.Charge - 1 > data[v.South.Down].Charge)
                        Power(v.Up, Direction.SOUTH, BlockType.WIRE, current.Charge - 1);

                if ( ((c & WireMask.East) == WireMask.East) && (v.X < lenX - 1))  // Connected north and not a wall
                    if (!data[v.East].isBlock && current.Charge - 1 > data[v.East.Down].Charge)
                        Power(v.Up, Direction.EAST, BlockType.WIRE, current.Charge - 1);
            }



            //same level
            if (((c & WireMask.North) == WireMask.North) && (v.Y > 0))
            {// Connected north and not a wall
                if (current.Charge - 1 > data[v.North].Charge)
                    Power(v, Direction.NORTH, BlockType.WIRE, current.Charge - 1);
                if ((c & WireMask.BlockPower) == WireMask.BlockPower && (current.Charge > data[v.North].Charge))
                    Power(v, Direction.NORTH, BlockType.BLOCK, current.Charge);
            }
            if (((c & WireMask.West) == WireMask.West) && (v.X > 0))
            { // Connected north and not a wall
                if (current.Charge - 1 > data[v.West].Charge)
                    Power(v, Direction.WEST, BlockType.WIRE, current.Charge - 1);
                if ((c & WireMask.BlockPower) == WireMask.BlockPower && (current.Charge > data[v.West].Charge))
                    Power(v, Direction.NORTH, BlockType.BLOCK, current.Charge);
            }
            if (((c & WireMask.South) == WireMask.South) && (v.Y < lenY - 1))
            { // Connected north and not a wall
                if (current.Charge - 1 > data[v.South].Charge)
                    Power(v, Direction.SOUTH, BlockType.WIRE, current.Charge - 1);
                if ((c & WireMask.BlockPower) == WireMask.BlockPower && (current.Charge > data[v.South].Charge))
                    Power(v, Direction.NORTH, BlockType.BLOCK, current.Charge);
            }
            if (((c & WireMask.East) == WireMask.East) && (v.Y < lenX - 1))
            {  // Connected north and not a wall
                if (current.Charge - 1 > data[v.East].Charge)
                    Power(v, Direction.EAST, BlockType.WIRE, current.Charge - 1);
                if ((c & WireMask.BlockPower) == WireMask.BlockPower && (current.Charge > data[v.East].Charge))
                    Power(v, Direction.NORTH, BlockType.BLOCK, current.Charge);
            }

        }

        private bool isValid(BlockVector v, Direction dir)
        {
            switch(dir)
            {
                case Direction.DOWN: return v.Z == 0;
                case Direction.NORTH: return v.Y == 0;
                case Direction.EAST: return v.X == lenX - 1;
                case Direction.SOUTH: return v.Y == lenY - 1;
                case Direction.WEST: return v.X == 0;
                case Direction.UP: return v.Z == lenZ - 1;
                default:
                    return false;
            }
        }
        private BitArray isValid(BlockVector v)
        {
            return isValid(v.X, v.Y, v.Z);
        }
        private BitArray isValid(int x, int y, int z)
        {
            BitArray end = new BitArray(6);
            end[0]= z == 0;
            end[1]= y == 0;
            end[2]= x == lenX - 1;
            end[3]=y == lenY - 1;
            end[4]= x == 0;
            end[5] = z == lenZ - 1;
            return end;
        }
     /*   private DirectionMask isValid(BlockVector v)
        {
            DirectionMask d;
            d |= z == 0?DirectionMask.DOWN;
            d |= y == 0?Direction.EAST;
            d |= x == lenX - 1?DirectionMask.SOUTH;
            d |= y == lenY - 1?DirectionMask.WEST;
            d |= x == 0?DirectionMask.;
            d |= z == lenZ - 1?;

        }*/

        private void Power(BlockVector v, Direction dir, BlockType target, int charge)
        {
            if (isValid(v,dir))
            {
                BlockVector t = v.Dir(dir);
                if (data[t].ID == target)
                {
                    data[t].Charge = charge;
                    source.Add(v);
                    update.Add(v);
                }
            }
        }


       
        public int Loc(int x, int y, int z)
        {
            if (z < 0 || z >= lenZ || y < 0 || y >= lenY || x < 0 || x >= lenX)
                return -1;

            return z * lenX * lenY + lenY * y + x;
        }
        public int Loc(BlockVector v)
        {
            if (v.Z < 0 || v.Z >= lenZ || v.Y < 0 || v.Y >= lenY || v.X < 0 || v.X >= lenX)
                return -1;

            return v.Z * lenX * lenY + lenX * v.Y + v.X;
        }

        public void addBlock(int x, int y, int z)
        {
            source.Add(new BlockVector(x, y, z));
            update.Add(new BlockVector(x, y, z));
        }

    }
}



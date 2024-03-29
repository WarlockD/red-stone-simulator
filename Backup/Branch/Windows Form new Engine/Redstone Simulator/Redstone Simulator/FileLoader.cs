﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibNbt;
using LibNbt.Queries;
using LibNbt.Tags;

namespace Redstone_Simulator
{
    public enum BlockID : byte
    {
        Air = 0,
        Stone = 1,
        Grass = 2,
        Dirt = 3,
        Cobblestone = 4,
        WoodenPlank = 5,
        Sapling = 6,
        Bedrock = 7,
        Water = 8,
        StationaryWater = 9,
        Lava = 10,
        StaionaryLava = 11,
        Sand = 12,
        Gravel = 13,
        GoldOre = 14,
        IronOre = 15,
        CoalOre = 16,
        Wood = 17,
        Leaves = 18,
        Sponge = 19,
        Glass = 20,
        LapisLazuliOre = 21,
        LapisLazuliBlock = 22,
        Dispenser = 23,
        Sandstone = 24,
        NoteBlock = 25,
        Bed = 26,
        PoweredRail = 27,
        DetectorRail = 28,
        Web = 30,
        Wool = 35,
        Dandelion = 37,
        Rose = 38,
        BrownMushroom = 39,
        RedMushroom = 40,
        GoldBlock = 41,
        IronBlock = 42,
        DoubleSlabs = 43,
        Slabs = 44,
        BrickBlock = 45,
        TNT = 46,
        Bookshelf = 47,
        MossStone = 48,
        Obsidian = 49,
        Torch = 50,
        Fire = 51,
        MonsterSpawner = 52,
        WoodenStairs = 53,
        Chest = 54,
        RedstoneWire = 55,
        DiamondOre = 56,
        DimondBlock = 57,
        CraftingTable = 58,
        Seeds = 59,
        Farmland = 60,
        Furnace = 61,
        BurningFurnace = 62,
        SignPost = 63,
        WoodenDoor = 64,
        Ladders = 65,
        Rails = 66,
        CobblestoneStairs = 67,
        WallSign = 68,
        Lever = 69,
        StonePressurePlate = 70,
        IronDoor = 71,
        WoodenPressurePlate = 72,
        RedstoneOre = 73,
        GlowingRedstoneOre = 74,
        RedstoneTorchOff = 75,
        RedstoneTorchOn = 76,
        StoneButton = 77,
        Snow = 78,
        Ice = 79,
        SnowBlock = 80,
        Catus = 81,
        ClayBlock = 82,
        SugarCane = 83,
        Jukebox = 84,
        Fence = 85,
        Pumpkin = 86,
        Netherrack = 87,
        SoulSand = 88,
        GlowstoneBlock = 89,
        Portal = 90,
        JackOLantern = 91,
        CakeBlock = 92,
        RedstoneRepeaterOff = 93,
        RedstoneRepeaterOn = 94,
        LockedChest = 95
    }
    class FileLoader
    {
        static readonly byte[] BlockToFile = new byte[] { 5, 2, 4, 1, 2 };
        static readonly Direction[] FileToBlock = new Direction[] { Direction.UP, Direction.EAST,
           Direction.WEST,Direction.SOUTH,Direction.NORTH,Direction.DOWN };
       // static readonly Direction[] FileToBlock = new Direction[] { Direction.UP, Direction.WEST,
       //    Direction.EAST,Direction.NORTH,Direction.SOUTH,Direction.DOWN };
        public static Blocks LoadTest()
        {
            return Load("MC14500bv6.schematic");
        }
        static Direction PlaceTorch(byte data)
        {
            switch (data & 0x7)
            {
                case 1: return  Direction.EAST;
                case 2: return Direction.WEST;
                case 3: return Direction.SOUTH;
                case 4: return Direction.NORTH;
                case 5: return Direction.DOWN;
            }
            throw new InvalidDataException("Something is wrong");
        }
        static Direction PlaceButton(byte data)
        {
            switch (data & 0x7)
            {
                case 1: return Direction.WEST;
                case 2: return Direction.EAST;
                case 3: return Direction.SOUTH;
                case 4: return Direction.NORTH;
            }
            throw new InvalidDataException("Something is wrong");
        }
        public static Blocks Load(string filename)
        {
            NbtFile f = new NbtFile();
            f.LoadFile(filename);
            NbtCompound root = f.RootTag;
            NbtTag nBlocks = root["Blocks"];
            NbtTag nData = root["Data"];
            NbtTag nWidth = root["Width"];
            NbtTag nLength = root["Length"];
            NbtTag nHeight = root["Height"];
            byte[] blocks = ((NbtByteArray)nBlocks).Value;
            byte[] extra = ((NbtByteArray)nData).Value;
            int X = (int)((NbtShort)nWidth).Value;
            int Y = (int)((NbtShort)nLength).Value;
            int Z = (int)((NbtShort)nHeight).Value;
            Blocks b = new Blocks(X, Y, Z);
            //bool sch = filename.EndsWith(".schematic");

            for (int i = 0; i < blocks.Length; i++)
                switch (blocks[i])
                {
                    case 0:
                    case 6:
                    case 37:
                    case 38:
                    case 39:
                    case 40:
                    case 51:
                    case 59:
                    case 63:
                    case 65:
                    case 66:
                    case 68:
                    case 78:
                    case 83:
                        b[i] = Block.AIR;
                        break;
                    case 55:
                        b[i] = Block.WIRE;
                        break;
                    case 75: // Off 
                        b[i] = new Block(BlockType.TORCH, PlaceTorch(extra[i]), 0, 0, 0);
                 
                        break;
                    case 76: // Off
                        b[i] = new Block(BlockType.TORCH, PlaceTorch(extra[i]), 16, 0, 0);
       
                       // System.Console.WriteLine("Loc: {2}  Torch Dir: {0}    Byte: {1}", b[i].Place.ToString(), extra[i] & 0x7,i);
                        break;
                    case 69:
                        b[i] = new Block(BlockType.LEVER,  PlaceTorch(extra[i]),(extra[i] & 0x8) == 1 ? 16 : 0, 0, 0);
              
                     //   System.Console.WriteLine("Lever Dir: {0}    Byte: {1}", b[i].Place.ToString(), extra[i] & 0x7);
                        break;
                    case 70:
                    case 72:
                        b[i] = Block.PREASUREPAD;
                        break;
                    case 77:
                        b[i] = Block.BUTTON;
                        b[i].Place = PlaceButton(extra[i]);
                        break;
                    case 64: // doors not working yet
                    case 71:
                        break;
                    case 93: // skip repeaters for now

                        break;
                    default:
                        b[i] = Block.BLOCK;
                        break;


                }

                
            return b;

        }
    }
}

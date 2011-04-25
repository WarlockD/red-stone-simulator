using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redstone_Simulator
{
    
    public enum  eBlock : byte
    {
        AIR=0,
        BLOCK, 
        WIRE,
        TORCH,
        LEVER,
        BUTTON, 
        DOORA, 
        DOORB, 
        PRESS, 
        REPEATER,
    }
    public enum eMount : int
    {
        TOP=0,
        SOUTH=1,
        NORTH=2,
        WEST=3,
        EAST=4 
    }

    [Flags]
    public enum eDirection
    {
        UP=1,
        SOUTH=2,
        NORTH=4,
        EAST=8,
        WEST=10,
        DOWN=12
    }
    public enum eExtraParm
    {
        TorchFace,
        RepeatTick
    }
    public struct Blocks
    {
        public WireMask wMask;
        public eBlock Type;
        public eMount Mount;
        public int Power;
        public int Ticks;
        int cTicks;
        public bool CtrlOn { get; set; }
        public bool Powered { get { return Power != 0; } }
        public bool Tick
        {
            get
            {
                if (Type == eBlock.REPEATER)
                    if (Ticks == cTicks)
                    {
                        cTicks = 0;
                        return true;
                    }
                    else
                        cTicks++;
                return false;
            }
        }
        public Blocks(eBlock Type) : this() { this.Type = Type; if (canStand) Mount = eMount.TOP; else Mount = eMount.NORTH; }
        public Blocks(eBlock Type, int Power) : this(Type) { this.Power = Power; }
        public static Blocks AIR { get { return new Blocks(eBlock.AIR); } }
        public static Blocks BLOCK { get { return new Blocks(eBlock.BLOCK); } }
        public static Blocks WIRE { get { return new Blocks(eBlock.WIRE); } }
        public static Blocks TORCH { get { return new Blocks(eBlock.TORCH); } }
        public static Blocks LEVER { get { return new Blocks(eBlock.LEVER); } }
        public static Blocks BUTTON { get { return new Blocks(eBlock.BUTTON); } }
        public static Blocks DOORA { get { return new Blocks(eBlock.DOORA); } }
        public static Blocks DOORB { get { return new Blocks(eBlock.DOORB); } }
        public static Blocks PRESS { get { return new Blocks(eBlock.PRESS); } }
        public static Blocks REPEATER { get { return new Blocks(eBlock.REPEATER); } }

        public static Blocks[][] PickBlocks
        {
            get
            {
                Blocks[][] tmp =  new Blocks[][] 
                 {
                    new Blocks[] { Blocks.AIR,Blocks.AIR,Blocks.AIR },
                                new Blocks[]  { Blocks.BLOCK,Blocks.AIR,Blocks.AIR },
                                new Blocks[]  { Blocks.BLOCK,Blocks.BLOCK,Blocks.AIR },
                                new Blocks[]  { Blocks.WIRE,Blocks.AIR,Blocks.AIR },
                                new Blocks[]  { Blocks.TORCH,Blocks.AIR,Blocks.AIR },
                                new Blocks[]  { Blocks.BLOCK,Blocks.WIRE,Blocks.AIR },
                               new Blocks[]   { Blocks.BLOCK,Blocks.TORCH,Blocks.AIR },
                               new Blocks[]   { Blocks.WIRE,Blocks.BLOCK,Blocks.AIR },
                                new Blocks[]  { Blocks.TORCH,Blocks.BLOCK,Blocks.AIR },
                                new Blocks[]  { Blocks.WIRE,Blocks.TORCH,Blocks.AIR },
                                new Blocks[]  { Blocks.WIRE,Blocks.BLOCK,Blocks.WIRE },
                               new Blocks[]   { Blocks.LEVER,Blocks.AIR,Blocks.AIR },
                                new Blocks[]  { Blocks.BUTTON,Blocks.AIR,Blocks.AIR },
                                new Blocks[]  { Blocks.PRESS,Blocks.AIR,Blocks.AIR },
                                new Blocks[]  { Blocks.DOORA,Blocks.AIR,Blocks.AIR }  ,
                                 new Blocks[]  { Blocks.REPEATER,Blocks.AIR,Blocks.AIR }
                 };
                for(int i=0; i < tmp.Length; i++)
                    for(int j=0; j < tmp[i].Length; j++)
                    {
                        if (tmp[i][j].isControl) tmp[i][j].Power = 0;
                        else
                            tmp[i][j].Power = 16;
                        tmp[i][j].Mount = eMount.NORTH;


                    }
               
                return tmp;
            }
        }




        public void Rotate()
        {
            if (!this.canRotate)
                return;
            switch (Mount)
            {
                case eMount.TOP: Mount = eMount.NORTH; break;
                case eMount.NORTH: Mount = eMount.SOUTH; break;
                case eMount.SOUTH: Mount = eMount.EAST; break;
                case eMount.EAST: Mount = eMount.WEST; break;
                case eMount.WEST:
                    if (Type == eBlock.BUTTON || Type == eBlock.DOORB || Type == eBlock.REPEATER)
                        Mount = eMount.NORTH;
                    else
                        Mount = eMount.TOP;
                    break;
            }
        }

        public bool Conn
        {
            get
            {
                switch (Type)
                {
                    case eBlock.BUTTON:
                    case eBlock.LEVER:
                    case eBlock.PRESS:
                    case eBlock.TORCH:
                    case eBlock.WIRE:
                    case eBlock.DOORB:
                    case eBlock.REPEATER:
                        return true;
                    default:
                        return false;
                }
            }
        }






        /// <summary>
        /// Checks if this is a control element that needs the user to change its state
        /// </summary>
        public bool isControl
        {
            get
            {
                return this.Type == eBlock.LEVER || this.Type == eBlock.BUTTON || this.Type == eBlock.PRESS || this.Type == eBlock.REPEATER;
            }
        }
        /// <summary>
        /// Checks to see if this is a block.
        /// </summary>
        public bool isBlock
        {
            get
            {
                return this.Type == eBlock.BLOCK;
            }
        }
        /// <summary>
        /// Checks if this is Air or any other object that does not need to be caculated.
        /// </summary>
        public bool isAir
        {
            get
            {
                return this.Type == eBlock.AIR;
            }
        }
        /// <summary>
        /// This control object or torch and stand ontop of a block, Always have an UP
        /// </summary>
        public bool canStand { get { return this.Type == eBlock.LEVER || this.Type == eBlock.PRESS || this.Type == eBlock.REPEATER || this.Type == eBlock.TORCH; } }
        public bool canRotate { get { return this.Type == eBlock.LEVER || this.Type == eBlock.REPEATER || this.Type == eBlock.TORCH; } }
        /// <summary>
        /// You can put this on the side of a block
        /// </summary>
        public bool canMount { get { return this.Type == eBlock.LEVER || this.Type == eBlock.TORCH; } }
    }
}

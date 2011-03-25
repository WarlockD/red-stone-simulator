using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Redstone_Simulator
{
    public enum  eBlock : byte
    {
        AIR,
        BLOCK, 
        WIRE,
        TORCH,
        LEVER,
        BUTTON, 
        DOORA, 
        DOORB, 
        PRESS, 
        WATER,
        REPEATER
    }
    public enum eMount : byte
    {
        TOP,
        SOUTH=1,
        NORTH,
        EAST,
        WEST
    }
    public enum eDirection
    {
        UP,
        SOUTH=1,
        NORTH,
        EAST,
        WEST,
        DOWN
    }
    public enum eExtraParm
    {
        TorchFace,
        RepeatTick
    }
    public struct Blocks
    {
        eBlock _type; 
        eMount _mount; 
        short _extra;
        short _distance;
        short up, down, north, south, east, west;
        short x,y,z; // Hard position
        bool _powered;

        /// <summary>
        /// This is the mounted direction.  It is facing this direction or mounted
        /// in this direction.  Levers require to use the Extra byte
        /// </summary>
        public eMount Mount
        {
            get
            {
                return _mount;
            }
            set
            {
                _mount = value;
                if (!canMount && value == eMount.TOP)
                    throw new ArgumentException("Object must mount to Block", "Direction");
            }
        }



        public bool Powered { 
            get {
                if(_type == eBlock.WIRE)
                    return _distance > 0;
                else
                    return _powered; 
            } 
            set 
            {
                switch(_type)
                {
                    case eBlock.LEVER:
                    case eBlock.TORCH: 
                    case eBlock.REPEATER: 
                    case eBlock.BUTTON:
                    case eBlock.DOORA:
                    case eBlock.DOORB:
                        _powered = value;
                        break;
                        // The wire is powered from a direct source
                    case eBlock.WIRE:
                        _powered = value; 
                        if(value)
                            _distance  = 15;
                        break;
                    default:
                        _powered = false;
                        break;
                }
            }
        }
        

       
        public Blocks(eBlock Type)
        {
            _type = Type;
            x = y = z = up = down = north = south = east = west = -1;
            _distance = _extra = 0;
            _powered = false;
            _mount = Type == eBlock.LEVER || Type == eBlock.TORCH ? eMount.TOP : eMount.NORTH;
        }

        /// <summary>
        /// Get tye type of the Block
        /// </summary>
        public eBlock Type{ get { return _type; } }

        /// <summary>
        /// Distance from power, mainly for wire caculation
        /// </summary>
        public short Distance { get { return _distance; } set { _distance = value; } }

        public void setLoc(short X, short Y, short Z) { x = X; y = Y; z = Z; }
        public void setLoc(short[] loc) { setLoc(loc[0], loc[1], loc[2]); }
        public short[] getLoc() { return new short[3] { x, y, z }; }

        public void setPtr(short i, eDirection d)
        {
            switch(d)
            {
                case eDirection.DOWN: down = i; break;
                case eDirection.UP: up = i; break;
                case eDirection.WEST: west = i; break;
                case eDirection.EAST: east = i; break;
                case eDirection.NORTH: north = i; break;
                case eDirection.SOUTH: south = i; break;
            }
        }
        public short getPtr(eDirection d)
        {
            switch (d)
            {
                case eDirection.DOWN: return down;
                case eDirection.UP: return up;
                case eDirection.WEST: return west;
                case eDirection.EAST: return east;
                case eDirection.NORTH: return north;
                case eDirection.SOUTH: return south; 
            }
            return 0;
        }



        /// <summary>
        /// Checks if this is a control element that needs the user to change its state
        /// </summary>
        public bool isControl
        {
            get
            {
                return this._type == eBlock.LEVER || this._type == eBlock.BUTTON || this._type == eBlock.PRESS || this._type == eBlock.REPEATER;
            }
        }
        /// <summary>
        /// Checks to see if this is a block.
        /// </summary>
        public bool isBlock
        {
            get
            {
                return this._type == eBlock.BLOCK;
            }
        }
        /// <summary>
        /// Checks if this is Air or any other object that does not need to be caculated.
        /// </summary>
        public bool isAir
        {
            get
            {
                return this._type == eBlock.AIR;
            }
        }
        /// <summary>
        /// Checks if this object can be destroyed by lava
        /// </summary>
        /// <returns></returns>
        public bool canBurn
        {
            get
            {
                return !isBlock && this._type != eBlock.PRESS && this._type != eBlock.DOORA && this._type != eBlock.DOORB && this._type != eBlock.WATER;
            }
        }
        /// <summary>
        /// This control object or torch and stand ontop of a block, Always have an UP
        /// </summary>
        public bool canStand { get { return this._type == eBlock.LEVER  || this._type == eBlock.PRESS || this._type == eBlock.REPEATER || this._type == eBlock.TORCH;  }}

        /// <summary>
        /// You can put this on the side of a block
        /// </summary>
        public bool canMount { get { return this._type == eBlock.LEVER   || this._type == eBlock.TORCH;  }}
    }
}

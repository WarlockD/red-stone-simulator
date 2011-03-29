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
        WATER,
        REPEATER
    }
    [Flags]
    public enum eMount : byte
    {
        TOP=1,
        SOUTH=2,
        NORTH=4,
        EAST=8,
        WEST=10
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
        WireMask _wireMask;
        public WireMask wMask { get { return _wireMask; } set { _wireMask = value; } }

        eBlock _type; 
        eMount _mount; 
        short up, down, north, south, east, west;
        short x,y,z; // Hard position
        short _power;
        short _ticks;
        public short Ticks
        {
            get
            {
                if (_type == eBlock.REPEATER)
                    return _ticks;
                else
                    return 0;
            }
            set
            {
                _ticks = value;
                if (_ticks < 1) _ticks = 1;
                if (_ticks > 4) _ticks = 4;
            }
        }
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


        public short Power
        {
            set
            {
                
                if (value > 16) _power = 16;
                else
                    if (value < 0) _power = 0;
                    else
                        _power = value;
            }
            get { return _power; }
        }
        public bool Powered { 
            get {
                    return _power > 0;
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
                    case eBlock.WIRE:
                        _power = 16;
                        break;

                    default:
                        _power = 16;
                        break;
                }
            }
        }

        public static Blocks AIR { get { return new Blocks(eBlock.AIR); } }
        public static Blocks BLOCK { get { return new Blocks(eBlock.BLOCK); } }
        public static Blocks WIRE { get { return new Blocks(eBlock.WIRE); } }
        public static Blocks TORCH { get { return new Blocks(eBlock.TORCH); } }
        public static Blocks LEVER { get { return new Blocks(eBlock.LEVER); } }
        public static Blocks BUTTON { get { return new Blocks(eBlock.BUTTON); } }
        public static Blocks DOORA { get { return new Blocks(eBlock.DOORA); } }
        public static Blocks DOORB { get { return new Blocks(eBlock.DOORB); } }
        public static Blocks PRESS { get { return new Blocks(eBlock.PRESS); } }
        public static Blocks WATER { get { return new Blocks(eBlock.WATER); } }
        public static Blocks REPEATER { get { return new Blocks(eBlock.REPEATER); } }

        public static Blocks sAIR { get { return new Blocks(eBlock.AIR,true); } }
        public static Blocks sBLOCK { get { return new Blocks(eBlock.BLOCK,true); } }
        public static Blocks sWIRE { get { return new Blocks(eBlock.WIRE, true); } }
        public static Blocks sTORCH { get { return new Blocks(eBlock.TORCH, true); } }
        public static Blocks sLEVER { get { return new Blocks(eBlock.LEVER, true); } }
        public static Blocks sBUTTON { get { return new Blocks(eBlock.BUTTON, true); } }
        public static Blocks sDOORA { get { return new Blocks(eBlock.DOORA, true); } }
        public static Blocks sDOORB { get { return new Blocks(eBlock.DOORB, true); } }
        public static Blocks sPRESS { get { return new Blocks(eBlock.PRESS, true); } }
        public static Blocks sWATER { get { return new Blocks(eBlock.WATER, true); } }
        public static Blocks sREPEATER { get { return new Blocks(eBlock.REPEATER, true); } }


        
      /*  {
            _ticks = 1;
            _wireMask = WireMask.None;
            _type = eBlock.AIR;
            x = y = z = up = down = north = south = east = west = -1;
            _extra = 0;
            _power = 0;
            _mount = eMount.TOP;
        }*/
        public void Rotate()
        {
            if (!this.canRotate)
                return;
            switch(_mount)
            {
                case eMount.TOP: _mount = eMount.NORTH; break;
                case eMount.NORTH: _mount = eMount.SOUTH; break;
                case eMount.SOUTH: _mount = eMount.EAST; break;
                case eMount.EAST: _mount = eMount.WEST; break;
                case eMount.WEST:
                    if(_type == eBlock.BUTTON || _type == eBlock.DOORB || _type == eBlock.REPEATER)
                        _mount = eMount.NORTH;
                else
                    _mount = eMount.TOP;
                    break;

            }
        }
        /// <summary>
        /// Rotate object but not in these directions
        /// </summary>
        /// <param name="dirs">Direction flag not to be moved in</param>
        public void Rotate(eMount dirs)
        {
            if (!this.canRotate)
                return;
            if (!dirs.HasFlag(eMount.NORTH) && _mount == eMount.NORTH)
            { _mount = eMount.WEST; return; }
            if (!dirs.HasFlag(eMount.WEST) && _mount == eMount.WEST)
            { _mount = eMount.SOUTH; return; }
            if (!dirs.HasFlag(eMount.SOUTH) && _mount == eMount.SOUTH)
            { _mount = eMount.EAST; return; }
            if (!dirs.HasFlag(eMount.EAST) && _mount == eMount.EAST)
                if (this.canStand)
                { _mount = eMount.TOP; return; }
                else
                { _mount = eMount.NORTH; return; }
            if (!dirs.HasFlag(eMount.TOP) && _mount == eMount.TOP)
            { _mount = eMount.NORTH; return; }


        }
        public Blocks(eBlock Type)
        {
            _ticks = 1;
            _wireMask = WireMask.None;
            _type = Type;
            x = y = z = up = down = north = south = east = west = -1;
            _power = 0;
            _mount = Type == eBlock.LEVER || Type == eBlock.TORCH ? eMount.TOP : eMount.NORTH;
        }

        public Blocks(eBlock Type, bool select)
        {
            _ticks = 1;
            _wireMask = WireMask.None;
            _type = Type;
            x = y = z = up = down = north = south = east = west = -1;
            _power = 0;
            _mount = Type == eBlock.LEVER || Type == eBlock.TORCH ? eMount.TOP : eMount.NORTH;

            // if true, then lets make them model just for the selection screen
            if (select)
            {
                switch (Type)
                {
                    case eBlock.TORCH: _power = 16; _mount = eMount.NORTH; break;
                    case eBlock.LEVER: _mount = eMount.NORTH; break;
                    case eBlock.BUTTON: _mount = eMount.NORTH; break;
                    case eBlock.WIRE: _power = 16; break;
                }

            }
        }


        /// <summary>
        /// Get tye type of the Block
        /// </summary>
        public eBlock Type{ get { return _type; } }



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
        public bool canRotate { get { return this._type == eBlock.LEVER ||  this._type == eBlock.REPEATER || this._type == eBlock.TORCH; } }
        /// <summary>
        /// You can put this on the side of a block
        /// </summary>
        public bool canMount { get { return this._type == eBlock.LEVER   || this._type == eBlock.TORCH;  }}
    }
}

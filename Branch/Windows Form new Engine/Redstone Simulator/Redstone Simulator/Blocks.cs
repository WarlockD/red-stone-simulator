using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace Redstone_Simulator
{

    public enum BlockType : byte
    {
        AIR=0,
        BLOCK,
        WIRE,
        TORCH,
        REPEATER,
        BUTTON,
        LEVER,
        PREASUREPAD
    }
    public enum Direction : int
    {
        DOWN=0,
        NORTH=1,
        EAST=2,
        SOUTH=3,
        WEST=4,
        UP=5
    }
    


    public class Blocks : ICollection, ICloneable//, ICollection<Blocks>
    {
        Block[] data;
        int totalCount;
        int lenX, lenY, lenZ;
        public int Length { get { return lenX * lenY * lenZ; } }
        public int X { get { return lenX; } }
        public int Y { get { return lenY; } }
        public int Z { get { return lenZ; } }
        public Blocks(int x, int y, int z) { lenX = x; lenY = y; lenZ = z; totalCount = x * y * z; data = new Block[totalCount];
        for (int i = 0; i < totalCount; i++) data[i] = Block.AIR; 
        }
        public Blocks(int x, int y, int z, Block[] d) { data = d; lenX = x; lenY = y; lenZ = z; }
        
        // ICollection Members
     
        public void CopyTo(Array array, int index)
        {
            int j = index;
            for (int i = 0; i < totalCount; i++)
            {
                array.SetValue((object)data[i], j);
                j++;
            }
        }
        public int Count { get { return totalCount; } }
        public bool IsSynchronized { get { return false; } }
        public object SyncRoot { get { return this; } }
        public IEnumerator GetEnumerator() { throw new Exception("This method is not impmented"); }
        public  Block this[int i]
        {
            get { return data[i]; }
            set { data[i] = value; }
        }
        public  Block this[BlockVector v] {
            get { return this[v.X, v.Y, v.Z]; }
            set { this[v.X, v.Y, v.Z] = value; }
        }
        public Blocks Copy()
        {
            Blocks b = new Blocks(X, Y, Z);
            for (int i = 0; i < totalCount; i++)
                b[i] = new Block(this[i]);
            return b;
        }
        public object Clone()
        {
            return (object)Copy();
        }
        public Block this[int x, int y, int z] {
            get
            {
                if (z < 0)
                    return Block.BLOCK;
                if (z >= lenZ || y < 0 || y >= lenY || x < 0 || x >= lenX)
                    return Block.AIR;

                return data[z * lenY * lenX + y * lenX + x];
            }
            set
            {
                if (z >= lenZ || y < 0 || y >= lenY || x < 0 || x >= lenX || z < 0)
                    return;

                data[z * lenY * lenX + y * lenX + x] = value;
            }

        }
       
   


    }
    public class  Block : ICloneable
    {
      
         WireMask wmask; public WireMask Mask { get { return wmask; } set { wmask = value; } }
         BlockType id; public BlockType ID { get { return id; } }
         Direction place; public Direction Place { get { return place; } set { place = value;  } }
         int charge; public int Charge { get { return charge; } set { charge = value;  } }
         int delay; public int Delay { get { return delay; } set { delay = value;  } }
         int tickspassed; public int Ticks { get { return tickspassed; } set { tickspassed = value;  } }

        public bool isAir { get { return this.ID == BlockType.AIR; }}
        public bool isBlock { get { return this.ID == BlockType.BLOCK; }}
        public bool isWire { get { return this.ID == BlockType.WIRE; }}
        public bool isTorch {get {  return this.ID == BlockType.TORCH; }}
        public bool isRepeater { get { return this.ID == BlockType.REPEATER; }}
        public bool isButtons {get {  return this.ID == BlockType.BUTTON; }}
        public bool isLeaver { get { return this.ID == BlockType.LEVER; }}
        public bool isPreasurePad { get { return this.ID == BlockType.PREASUREPAD; } }

        public bool Powered { get { return Charge > 0; } set { Charge = value ? 16 : 0; } }
        public bool Source { get { return Charge == 17; } set { Charge = value ? 17 : 0; } }
        public bool canConnect // 2 3 5 6 7
        {
            get
            {
                return this.ID == BlockType.WIRE || this.ID == BlockType.TORCH || this.ID == BlockType.BUTTON || this.ID == BlockType.LEVER || this.ID == BlockType.PREASUREPAD;
            }
        }
        public bool canMount
        { get { return this.ID == BlockType.TORCH || this.ID == BlockType.BUTTON || this.ID == BlockType.LEVER; } }
        public bool canBePoweredByRepeater(Direction dir)
        {
            return id == BlockType.BLOCK || id == BlockType.TORCH || (id == BlockType.REPEATER && place == dir) && charge > 0;
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public bool canBePoweredByRepeaterTorch(WireMask dir,WireMask test)
        {
            return isTorch && ((dir & test) == test) && charge > 0;
        }
        public bool isControl
        {
            get
            {
                return this.ID == BlockType.BUTTON || this.ID == BlockType.LEVER || this.ID == BlockType.PREASUREPAD;
            }
        }
        Block() { this.id = BlockType.AIR; place = 0; charge = 0; delay = 0; tickspassed = 0; }
        public Block(BlockType ID, Direction Place, int Charge, int Delay, int Passed) {
            this.id = ID;
            this.place = Place;
            this.charge = Charge;
            this.delay = Delay;
            this.tickspassed = Passed;
        }
        public Block(Block b) : this(b.ID, b.Place, b.Charge, b.Delay, b.Ticks) { }
        public static Block New(BlockType t) {
            switch (t)
            {
                case BlockType.AIR: return Block.AIR;
                case BlockType.BLOCK: return Block.BLOCK;
                case BlockType.BUTTON: return Block.BUTTON;
                case BlockType.LEVER: return Block.LEVER;
                case BlockType.PREASUREPAD: return Block.PREASUREPAD;
                case BlockType.REPEATER: return Block.REPEATER;
                case BlockType.TORCH: return Block.TORCH;
                case BlockType.WIRE: return Block.WIRE;
            }
            return Block.AIR;
        }
        public static Block AIR { get { return new Block(BlockType.AIR,Direction.DOWN,0,0,0); } }
        public static Block BLOCK { get { return new Block(BlockType.BLOCK, Direction.DOWN, 0, 0, 0); } }
        public static Block WIRE { get { Block b = new Block(BlockType.WIRE, Direction.DOWN, 0, 0, 0); b.Mask = WireMask.AllDir; return b; } }
        public static Block TORCH { get { return new Block(BlockType.TORCH,Direction.DOWN,16,2,0); } }
        public static Block LEVER { get { return new Block(BlockType.LEVER, Direction.DOWN, 0, 0, 0); } }
        public static Block BUTTON { get { return new Block(BlockType.BUTTON,Direction.NORTH,0,20,0); } }
        public static Block PREASUREPAD { get { return new Block(BlockType.PREASUREPAD,Direction.DOWN,0,20,0); } }
        public static Block REPEATER { get { return new Block(BlockType.REPEATER,Direction.NORTH,0,2,0); } }
        public void increaseTick()
        { //for reapeater only
            if (isRepeater)
            {
                Delay++;
                Delay++;
                if (Delay > 8) Delay = 2;
            }
        }
        public bool waitTickisPowered()
        {
            return waitTick(Powered);
        }
        public bool waitTick(bool input)
        { //returns true if state changes
            Ticks++;
            if (isTorch)
            { //inverted for torch
                if ((input && Charge == 0) || (!input && Charge == 16))
                {
                    Ticks = 0; //stops if input and charge is not equal
                    return false;
                }
                if (Ticks >= Delay)
                {
                    Ticks = 0;
                    if (input)
                        Charge = 0;
                    else
                        Charge = 16;
                    return true;
                }
            }
            else
            {
                if ((input && Charge == 16) || (!input && Charge == 0))
                {
                    Ticks = 0; //stops if input and charge is equal
                    return false;
                }
                if (tickspassed >= Delay)
                {
                    Ticks = 0;
                    if (input)
                    {
                        Charge = 16;
                    }
                    else
                    {
                        Charge = 0;
                    }
                    return true;
                }
            }
            return false;
        }



        public void Rotate()
        {
            Direction after = Place;

            switch (ID)
            {
                //0:air; 1:block; 2:wire; 3:torch; 4:repeater; 5:buttons; 6:lever; 7:Pressure pad
                case BlockType.TORCH:
                    if (Place == Direction.WEST) { after = Direction.DOWN; } else { after = Place + 1; }
                    Charge = 16;
                    break;
                case BlockType.REPEATER:
                    if (Place == Direction.WEST) { after = Direction.NORTH; } else { after = Place + 1; }
                    Charge = 0;
                    break;
                case BlockType.BUTTON:
                    if (Place == Direction.WEST) { after = Direction.NORTH; } else { after = Place + 1; }
                    break;
                case BlockType.LEVER:
                    if (Place == Direction.WEST) { after = Direction.DOWN; } else { after = Place + 1; }
                    break;
            }
            Place = after;
        }


        

    }
}

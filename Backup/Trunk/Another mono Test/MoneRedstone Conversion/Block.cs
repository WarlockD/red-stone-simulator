using System;
using System.Drawing;
using System.Collections;

namespace MoneRedstoneConversion
{
	public struct Vector3
	{
		public int X;	
		public int Y;
		public int Z;
		public Vector3(int x, int y, int z) { X = x; Y = y; Z=z; }
	}
	public enum BlockTypes
	{
		Air, 
		Block, 
		Wire, 
		Torch, 
		Lever, 
		Button, 
		DoorA, 
		DoorB, 
		Press, 
		Sand, 
        Water, 
		Shadow
	}
	/// <summary>
	/// South=1, North=2,West=3,East=4
	/// </summary>
	public enum Direction
	{
		Up=0,
		South=1,
		North=2,
		West=3,
		East=4,
	}
	public struct Block
	{
		//public Color color;
		public BlockTypes type;
		public byte extra;
		public byte wall;
		bool conn; public bool canConnect { get { return conn; } }
		public bool isCtrl { get { return this.type == BlockTypes.Lever || this.type == BlockTypes.Button || this.type == BlockTypes.Press;} }
		public bool isBlock { get {return this.type == BlockTypes.Block || this.type == BlockTypes.Sand; }}
		public bool isAir { get { return this.type == BlockTypes.Air || this.type == BlockTypes.Shadow ;} }
		/// <summary>
		/// I GET IT!  W shifts the upper 3 bits that contain direction info
		/// The lower 5 bits contain distace info
		/// </summary>
		public int w { get { return extra >> 5 & 7; } } 
		public Direction Side { get 
			{ 
			switch(w)
			{
				case 0: return Direction.Up;
				case 1: return Direction.South;
				case 2: return Direction.North;
				case 3: return Direction.West;
				case 4: return Direction.East;
				default: return Direction.Up;
			}
			}}
		public int Power { 
			get 
			{ return isAir ? 0 : extra & 0x1f; }
			set
			{
				extra = (byte)((extra & 0xe0)+value);	
			}
		}
		public int Distance { get { return Power; } }
		/// <summary>
		/// Can be powered, os its below 16
		/// </summary>
		public bool isPowered { get { return Power !=0; }}
		public bool isWallandFloor { get { return this.type == BlockTypes.Lever || this.type == BlockTypes.Lever; } }
		
		public Block(BlockTypes t,bool c, byte w) { this.type = t; conn = c; wall = w; extra = 0;}
		public static Block Air { get {return new Block(BlockTypes.Air,false,0); } }
		public static Block Dirt { get {return new Block(BlockTypes.Block,false,0); } }
		public static Block Wire { get {return new Block(BlockTypes.Wire,true,0); } }
		public static Block Torch { get {return new Block(BlockTypes.Torch,true,1); } }
		public static Block Lever { get {return new Block(BlockTypes.Lever,true,1); } }
		public static Block Button { get {return new Block(BlockTypes.Button,true,3); } }
		public static Block DoorA { get {return new Block(BlockTypes.DoorA,true,2); } }// don't get it
		public static Block DoorB { get {return new Block(BlockTypes.DoorB,true,2); } }// don't get it
		public static Block Press { get {return new Block(BlockTypes.Press,true,0); } }
		public static Block Sand { get {return new Block(BlockTypes.Sand,false,0); } }
		public static Block Water { get {return new Block(BlockTypes.Water,false,0); } }
		public static Block Shadow { get {return new Block(BlockTypes.Shadow,false,0); } }
	}
	
	public class Blocks
	{
		Block[,,] data;
		int[,] dir = {
        {0, 0, -1},		// Up 
		{0, 1, 0 },		// South
		{0, -1, 0},		// North
        {1, 0, 0}, 		// West
        {-1, 0, 0}};	// East
            
   
		//Blocks[][][] data; //Ugh, fuck it, its not THAT signifigantly slower
		int wires;
		int torches;
		int lenX,lenY,lenZ;
		bool cyclic = false;
		public bool MCwires = false;
		bool dummyGdValve = false;
		public int Z { get { return lenZ; }}
		public int Y { get { return lenY; }}
		public int X { get { return lenX; }}
		void CacluateLen()
		{
			lenZ = data.GetLength(0);
			lenY = data.GetLength(1);
			lenX = data.GetLength(2);
		}
		
		public bool c(int x, int y, int x2, int y2, int z) 
		{
			Block tmp = this[x2,y2,z]; // cashe block
			if(MCwires)
        	{
            	if(tmp.isAir) // Ok, so the block we are testing agenst is air
                	//return this[x2, y2, z - 1].canConnect; // return the block BELOW that as the wire goes down the block to the ground
					return z!=0 ? data[x2,y2,z-1].canConnect : true; 
						// Instead of calling the general check, we just need to check if we hit the floor.  return true as  we assume its on blocks
						//	with z = 0
            	if(tmp.isBlock) // Ok its air
                	//return this[x, y, z + 1].isBlock && this[x2, y2, z + 1].canConnect;
						// Since there is a block above me and we can connect to the line above we are testing its true?
						// AHHH, this tests that a switch can power a block BELOW, pressure plates and switches etc.
					return z<lenZ ? data[x,y,z+1].isBlock && data[x,y,z+1].canConnect : false;
						// Instead of calling general check, just need to test for ceeling, return false as above us is air
           		else
                	return true;
        	}
			// Meat and potatos
        if(tmp.canConnect)
            return true; // no complex here, we can connect to the tested object
			
		
        if(tmp.isAir)
            return z!=0 ? data[x2,y2,z-1].type == BlockTypes.Wire : false; //g(x2, y2, z - 1) == Blocks.wire;
			// Another optiomize, if the block below our test is wire it connects, otherwise its a normal block so false
			
        if(tmp.isBlock)
        {
			// By far this is the harder of the ones to optimize
			// This thing checks the blocks in all the directions to see if it connects, lots of directions
			// humm.
			// Ok, it checks to see if there is a switch, torch or button is on itself, if so it can provide power
				
			//if the block above the test is a wire AND the block above is a block then we are connected
			
			if(this[x2, y2, z + 1].type == BlockTypes.Wire && !this[x, y, z + 1].isBlock)
                return true;  // ok if there is a wire above the test AND we arn't blocked above us, then we can connect to that
			
			// Ok, since the test is a block, we have to see if any of the sides of the block have something that can power.
			// otherwise there is no connection.
            for(int i = 0; i < 5; i++)
			{
				// First cashe the direction we are testing.  0= up.
				Block tmp2 = this[x2 - dir[i,0], y2 - dir[i,1], z - dir[i,2]];
                if(tmp2.wall % 2 == 1 && tmp2.w == i) // Torch, Lever, or Button and the 5 bit isn't set if its 0, then its on our head.
                {
					// I didn't get wall at first, we are checking if control item CAN be put on the block or the sides, and if it can, if
					// its attached ot our side of the block.
					// So if it falls though here, that means it IS attached to us
                    if(tmp2.type == BlockTypes.Torch)
                        return blockConnect(x2, y2, x - x2, y - y2, z, false);
						// This one was wierd, It may be a recursive function.  I think it tests if there is a wire connected to the torch.
						
						// Ok, we got a lever on us, we don't care if its on the top of the block as that won't power the block?
                    return i != 0 || this[x2, y2, z + 1].type != BlockTypes.Lever || !dummyGdValve;
                }
			}
			// if there is a torch right below the test, then we connect
            return this[x2, y2, z - 1].type == BlockTypes.Torch;
        } 
		else
            return false;
			
		}
		Block gDir(int x, int y, int z, int d)
		{
			return gDir(x,y,z,(Direction)d);	
		}
		Block gDir(int x,int y, int z, Direction d)
		{
			switch(d)
			{
			case Direction.Up: return this[x,y,z+1];
			case Direction.North: return this[x,y+1,z];
			case Direction.South: return this[x,y-1,z];
			case Direction.East: return this[x+1,y,z];
			case Direction.West: return this[x-1,y,z];	
			}
			return Block.Air;
		}
		
		public bool blockConnect(int x, int y, int dx, int dy, int z, bool pow)
    	{
			
        	if(this[x + dx, y + dy, z].type != BlockTypes.Wire || !this[x + dx, y + dy, z].isPowered && pow)
            	return false; // Test if block is powered or has a wire powered?
        	if(this[x + dx + dy, (y + dy) - dx, z].isBlock)
        	{
            	if(!this[x + dx, y + dy, z + 1].isBlock && this[x + dx + dy, (y + dy) - dx, z + 1].canConnect)
                	return false;
        	} else
        	if(this[x + dx + dy, (y + dy) - dx, z].isAir)
        	{
            	if(this[x + dx + dy, (y + dy) - dx, z - 1].canConnect)
                	return false;
        	} else
        	{
            	return false;
        	}
        	if(this[(x + dx) - dy, y + dy + dx, z].isBlock)
            	return this[x + dx, y + dy, z + 1].isBlock || !this[(x + dx) - dy, y + dy + dx, z + 1].canConnect;
        	if(this[(x + dx) - dy, y + dy + dx, z].isAir)
            	return !this[(x + dx) - dy, y + dy + dx, z - 1].canConnect;
        	else
            	return false;
    	}
		public Blocks()
		{
			lenX =lenY =lenZ=0;
			data = null;
		}
		public Blocks (int x, int y, int z)
		{
			data = new Block[z,y,x];
			wires = torches = 0;
			CacluateLen();
		}
		public Blocks( Block[,,] d)
		{
			this.data = d;
			wires = torches = 0;
			CacluateLen();
		}
		
		public bool isValidCord(int x, int y, int z)
		{
			if(z<0 || z>=lenZ)
				return false;
			if(cyclic)
        		{ y = (y % lenY + lenY) % lenY; x = (x % lenX+ lenX) % lenX; }  
			else
        		if(y < 0 || y >= lenY|| x < 0 || x >= lenX)
					return false;
			
			return true;
		}
		
		public BlockTypes g(int x, int y, int z)
		{
			return this[x,y,z].type;
		}
		
		// meh its a hack
		public void SetExtra(int x, int y, int z, byte e)
		{
			if(!isValidCord(x,y,z))
				return;
			data[z,y,x].extra = e;
		}
		public void SetPower(int x, int y, int z, int p)
    	{
        	if(isValidCord(x,y,z))
				return;
			
        	if(data[z,y,x].type == BlockTypes.DoorA)
				SetPower(x, y, z + 1, p); // Set the power settings for the bottom of the door and top.  This really how it works?
        	
			data[z,y,x].Power = p;
    	}
		
		public bool s(int x, int y, int z, Block b, int e)
    	{
        	if(!isValidCord(x,y,z))
				return false;
			
			Block tmp = data[z,y,x];
        	if(tmp.type == b.type && tmp.extra == e)
        	{ // no change?
            	return false;
        	} else
        	{	
				b.extra = (byte)e;
				this[x,y,z] = b;
            	return true;
        	}
    	}
		
		
		public Block this[int x, int y, int z] 
		{
			get 
			{
        		if(z < 0)
            		return Block.Dirt;
        		if(z >= lenZ)
            		return Block.Air;
        		if(cyclic)
        			{ y = (y % lenY + lenY) % lenY; x = (x % lenX+ lenX) % lenX; } 
				else
        			if(y < 0 || y >= lenY|| x < 0 || x >= lenX)
            			return Block.Air;
				return data[z,y,x];
			}
			set
			{
				if(!isValidCord(x,y,z))
					return;
			
				switch(data[z,y,x].type)
				{
					case BlockTypes.Torch:torches--; break;
					case BlockTypes.Wire: wires--; break;
					case BlockTypes.Block:
						Block tmp = this[x,y,z+1];
						if(tmp.type == BlockTypes.Wire || tmp.type==BlockTypes.DoorA)
								this[x,y,z+1] = Block.Air;
						for(int i = 0; i < 5; i++)
						{
							tmp = this[x - dir[i,0], y - dir[i,1], z - dir[i,2]];
							if(tmp.wall % 2 == 1 && tmp.w == i)
								this[x - dir[i,0], y - dir[i,1], z - dir[i,2]] = Block.Air;
						} break;
					case BlockTypes.DoorA:
						this[x,y,z +1] = Block.Air;
				  		break;
					case BlockTypes.DoorB:
						this[x,y,z -1] = Block.Air;
				 		break;				  
				}
				if(value.type == BlockTypes.Torch) torches++;
				else 
				if(value.type == BlockTypes.Wire) wires++;
			
				data[z,y,x] = value;
				//Going to have to throw some kind of redraw
			}
		}
				
			
	}
}


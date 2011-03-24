using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace MoneRedstoneConversion
{
	
	public class BlockField
	{
		
		
		Blocks data;
		int wires;
		int torches;
		public  bool cyclic = false;
		public  bool dummyGdValue = false;
		public  bool MCwires= true;
		public  bool bridge = true;
		public  int layers = 3;
		public static int[,] dir =  {
			{ 0,0,-1},
			{ 0,1,0 },
			{0,-1,0},
			{1,0,0},
			{-1,0,0}};
		Bitmap parent;
		
		public BlockField (ref Bitmap v, int x, int y, int z)
		{
			data = new Blocks(x,y,z);
			wires = torches = 0;
			parent = v;
		}
		public BlockField(ref Bitmap v, Block[,,] d)
		{
			parent = v;
			this.data = new Blocks(d);
			wires = torches = 0;
		}
		
		/// <summary>
		/// Check to see if there is a block around an object?
		/// </summary>
		/// <param name="x">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <param name="y">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <param name="z">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <param name="w">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		bool valid(int x, int y, int z, int w)
    	{
			Block tmp = data[x,y,z];
        	if(w == 0)
            	return tmp.wall < 2;
        	if(tmp.wall % 2 == 0)
            	return tmp.wall == 2;
       		else
            	return data[x + dir[w,0], y + dir[w,1], z + dir[w,2]].isBlock;
    	}
		/// <summary>
		/// Not to sure what this does, seems to count torches, wires and replace blocks above and below doors and wires.
		/// </summary>
		/// <param name="x">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <param name="y">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <param name="z">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <param name="v">
		/// A <see cref="Blocks"/>
		/// </param>
		public void s(int x, int y, int z, Block v)
		{
			if(!data.isValidCord(x,y,z))
				return;
			
			switch(data[x,y,z].type)
			{
			case BlockTypes.Torch:	torches--; break;
			case BlockTypes.Wire: wires--; break;
			case BlockTypes.Block:
				Block tmp = data[x,y,z+1];
				if(tmp.type == BlockTypes.Wire || tmp.type==BlockTypes.DoorA)
					s(x,y,z+1,Block.Air);
				for(int i = 0; i < 5; i++)
				{
					tmp = data[x - dir[i,0], y - dir[i,1], z - dir[i,2]];
					if(tmp.wall % 2 == 1 && tmp.w == i)
						s(x - dir[i,0], y - dir[i,1], z - dir[i,2],Block.Air);
				} break;
			case BlockTypes.DoorA:
				data[x,y,z] = v;
				s(x,y,z +1, Block.Air);
				  break;
			case BlockTypes.DoorB:
				data[x,y,z] = v;
				s(x,y,z -1, Block.Air);
				  break;				  
			}
			
			if(v.type == BlockTypes.Torch) torches++;
			else if(v.type == BlockTypes.Wire) wires++;
			data[x,y,z] = v;
			//parent.updateRed();
		}
		
		/// <summary>
		/// Play Music?
		/// </summary>
		/// <param name="x">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <param name="y">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <param name="z">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <param name="p">
		/// A <see cref="System.Int32"/>
		/// </param>
		
	
		
		
		
		
		public void drawWire(Graphics g, Rectangle r, int x, int y, int z, bool thick)
    	{
        	drawWire(g, r, data[x,y,z].isPowered, 
					 (data.c(x, y, x - 1, y, z) ? 8 : 0) + 
			         (data.c(x, y, x + 1, y, z) ? 4 : 0) + 
			         (data.c(x, y, x, y - 1, z) ? 2 : 0) + 
			         (data.c(x, y, x, y + 1, z) ? 1 : 0), 
			         false);
    	}
		
		public void drawWire(Graphics g, Rectangle r, bool on, int c, bool thick)
    	{
			Brush fillBrush;
        	if(on)
            	fillBrush = BlockColors.bWireOn;
	        else
	            fillBrush = BlockColors.bWireOff;
        	if(data.MCwires)
        	{
            	if((c & 3) == 0)
                	c = (c & 0xc) != 0 ? 12 : 15;
            	else
            	if((c & 0xc) == 0)
                	c = 3;
        	} 
			else
        		if(c == 0)
					g.FillRectangle(fillBrush,r.X +2,r.Y +2,4,4);

        		if((c & 1) != 0)
            		g.FillRectangle(fillBrush,r.X + 3, r.Y + 3, 2, 5);
        		if((c & 2) != 0)
            		g.FillRectangle(fillBrush,r.X + 3, r.Y, 2, 5);
        		if((c & 4) != 0)
            		g.FillRectangle(fillBrush,r.X + 3, r.Y + 3, 5, 2);
        		if((c & 8) != 0)
            		g.FillRectangle(fillBrush,r.X, r.Y + 3, 5, 2);
    		}
		public void fakeDraw(int x, int y, int z, Graphics g, Rectangle r, Block[] b)
		{
		}
		public void draw(int x, int y, int z, Graphics g, Rectangle r, Block[] b)
		{
			Brush fillColor = BlockColors.bAir;
			//bool drawGrid=false;
			int p = 0;
			bool whiteout = false;
			Block tmp;
			bool fake =b.Length !=0;
			if(!fake)
				{ b = new Block[3]; b[0]= data[x,y,z]; b[1]=data[x,y,z+1]; b[2]=data[x,y,z+2];}
				// Might make this a function in c, might be faster
			
			// Set the color of the block
			switch(b[0].type)
			{
				case BlockTypes.Sand:
				case BlockTypes.Block: 
					p++;
					fillColor = BlockColors.bBlock;
					break;
				case BlockTypes.Shadow:
					if( layers == 1 || b[1].isAir)
					{ fillColor = BlockColors.bAircover; break; } // Grid color, block is grey because something is over it
					goto case BlockTypes.Air;
				case BlockTypes.Air:
					p++;
					whiteout = true;
					goto default;
					//blockColor = Brushes.White;	// nothing is above it // redundent
				default:
					fillColor = BlockColors.bAir; // eveything is white then
				break;
			}
			// Set the back color
			g.FillRectangle(fillColor,r);
			if(b[0].type == BlockTypes.Wire)
        	{
				if(fake)
					drawWire(g,r,true,15,false);
				else
					drawWire(g,r,x,y,z,false);
				
				p++;
           		if(layers > 1 && !b[p].isAir && !b[p].isBlock)
            	{
                	g.FillRectangle(BlockColors.bAircover,r);
            	}
        	}
			if(p > 0 && layers == 1) // Remeber this for debuging
            	return;
			
			bool tog = true;
			
			// He has an array of bytes that store the raw data.  Going to output that 
			// in another format
        	switch(b[p].type)
       	 	{
			case BlockTypes.Wire:
            		if(fake)
                		drawWire(g, r, true, 15, false);
            		else
                		drawWire(g, r, x, y, z + p, false);
            		break;

        	case BlockTypes.Lever:
				tog = false; 
				goto case BlockTypes.DoorA;
        	case BlockTypes.Torch: 
				tmp = data[x,y,z+p];
            	if(fake || tmp.Side == Direction.South)
                	g.FillRectangle(BlockColors.bDoor,r.X + 3, r.Y + 3, 2, 5);
            	else
            	if(tmp.Side == Direction.North)
                	g.FillRectangle(BlockColors.bDoor,r.X + 3, r.Y, 2, 5);
            	else
            		if(tmp.Side == Direction.West)
                	g.FillRectangle(BlockColors.bDoor,r.X+ 3, r.Y + 3, 5, 2);
            	else
            		if(tmp.Side == Direction.East)
                	g.FillRectangle(BlockColors.bDoor,r.X, r.Y + 3, 5, 2);
            	if(!tog)
					g.FillEllipse(BlockColors.bValve,r.X + 2, r.Y + 2, 4, 4);
            	else
            		if(fake || tmp.isPowered )
						g.FillEllipse(BlockColors.bWireOn,r.X + 2, r.Y + 2, 4, 4);
            		else
                		g.FillEllipse(BlockColors.bWireOff,r.X + 2, r.Y + 2, 4, 4);
            	if(!tog && !fake && tmp.isPowered)
                	g.FillEllipse(BlockColors.bWireOn,r.X + 3, r.Y + 3, 2, 2);
            break;

			case BlockTypes.Button: //6: // '\006'
           // g.setColor(BlockColors.bButton);
				tmp = data[x,y,z+p];
            if(!fake && tmp.isPowered)
            {
                if(tmp.Side == Direction.South)
                    g.FillRectangle(BlockColors.bButton,r.X + 2, r.Y + 7, 4, 1);
                else
                if(tmp.Side == Direction.North)
                    g.FillRectangle(BlockColors.bButton,r.X + 2, r.Y, 4, 1);
                else
                if(tmp.Side == Direction.West)
                    g.FillRectangle(BlockColors.bButton,r.X + 7, r.Y + 2, 1, 4);
                else
                if(tmp.Side == Direction.East)
                    g.FillRectangle(BlockColors.bButton,r.X, r.Y + 2, 1, 4);
            } else
            if(fake || tmp.Side == Direction.South)
                g.FillRectangle(BlockColors.bButton,r.X + 2, r.Y + 5, 4, 3);
            else
            if(tmp.Side == Direction.North)
                g.FillRectangle(BlockColors.bButton,r.X + 2, r.Y, 4, 3);
            else
            if(tmp.Side == Direction.West)
                g.FillRectangle(BlockColors.bButton,r.X + 5, r.Y + 2, 3, 4);
            else
            if(tmp.Side == Direction.East)
                g.FillRectangle(BlockColors.bButton,r.X, r.Y + 2, 3, 4);
            break;

			case BlockTypes.Press:
            	if(!fake && data[x, y, z + p].isPowered)
					g.FillRectangle(BlockColors.bWireOn,r.X + 1, r.Y + 1, 6, 6);
            	else
					g.FillRectangle(BlockColors.bValve,r.X + 1, r.Y + 1, 6, 6);
            break;

        case BlockTypes.DoorB: // '\b'
            p--;
			goto case BlockTypes.DoorA;
            // fall through

        case BlockTypes.DoorA: // '\007'

            int w = 1;
            int c = 2;
            if(!fake)
            {
                w = (new int[] {
                    2, 0, 3, 1
                })[data[x, y, z + p].w - 1];
                c = w;
                if(data[x, y, z + p + 1].w != 2)
                    c = (c + 1) % 4;
                if(data[x, y, z + p].isPowered)
                    w = (w + (data[x, y, z + p + 1].w != 2 ? 1 : 3)) % 4;
            }
            if(w == 0)
                g.FillRectangle(BlockColors.bDoor,r.X, r.Y, 8, 2);
            else
            if(w == 1)
                 g.FillRectangle(BlockColors.bDoor,r.X + 6, r.Y, 2, 8);
            else
            if(w == 2)
                 g.FillRectangle(BlockColors.bDoor,r.X, r.Y + 6, 8, 2);
            else
            if(w == 3)
                 g.FillRectangle(BlockColors.bDoor,r.X, r.Y, 2, 8);
            if(!fake && data[x, y, z + p].isPowered)
                g.setColor(Colors.wireOn);
            else
                g.setColor(Colors.wireOff);
            if(c == 0)
                g.fillRect(r.x, r.y, 2, 2);
            else
            if(c == 1)
                g.fillRect(r.x + 6, r.y, 2, 2);
            else
            if(c == 2)
                g.fillRect(r.x + 6, r.y + 6, 2, 2);
            else
            if(c == 3)
                g.fillRect(r.x, r.y + 6, 2, 2);
            break;

        case 11: // '\013'
            int col = fake ? Colors.water.getRGB() : ((~gp(x, y, z + p) & 7) * 24 + 87 << 24) + (Colors.water.getRGB() & 0xffffff);
            g.setColor(new Color(col, true));
            g.fillRect(r.x, r.y, r.width, r.height);
            g.setColor(Color.BLACK);
            if(!fake)
                if((gp(x, y, z + p) & 8) != 0)
                    g.fillOval(r.x + 3, r.y + 3, 2, 2);
                else
                if((gp(x, y, z + p) & 0xf) == 0)
                    g.fillRect(r.x + 3, r.y + 3, 2, 2);
            break;
        }
        if(b[1].block() && layers > 1)
        {
            g.setColor(Colors.cover);
            g.fillRect(r.x, r.y, r.width, r.height);
            if(layers > 2 && b[2] == Blocks.wire && (b[0] == (bridge ? Blocks.wire : Blocks.air) || b[0].block()))
                if(fake)
                    drawWire(g, r, true, 12, false);
                else
                    drawWire(g, r, x, y, z + 2, false);
        } else
        if(whiteout)
        {
            g.setColor(Colors.fog);
            g.fillRect(r.x, r.y, r.width, r.height);
        }

        

			
				
		}
			
			
			
	}
}

			
			
		
		




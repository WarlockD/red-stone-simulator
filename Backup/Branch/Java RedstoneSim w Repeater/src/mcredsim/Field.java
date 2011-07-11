
package mcredsim;

import java.awt.*;
import java.io.*;


public class Field
{

    private Viewport parent;
    boolean twatch[][][];
    byte data[][][];
    byte extra[][][];
    byte ticks[][][]; // fuckit, lets add another byte
    byte tickMax[][][];
    int wires;
    int torches;
    
    public static boolean cyclic = false;
    public static boolean dummyGdValve = false;
    public static boolean MCwires = true;
    public static boolean bridge = true;
    public static int layers = 3;
    private static final int dir[][] = {
        {
            0, 0, -1
        }, {
            0, 1, 0
        }, {
            0, -1, 0
        }, {
            1, 0, 0
        }, {
            -1, 0, 0
        }
    };
    public Field(Viewport v, int x, int y, int z)
    {
        parent = v;
        data = new byte[z][][];
        extra = new byte[z][][];
        ticks = new byte[z][][];
        tickMax = new byte[z][][];
        twatch = new boolean[z][][];
        wires = torches = 0;
        for(int i = 0; i < z; i++)
        {
            data[i] = new byte[y][];
            extra[i] = new byte[y][];
            ticks[i] = new byte[y][];
            tickMax[i] = new byte[y][];
            twatch[i] = new boolean[y][];
            for(int j = 0; j < y; j++)
            {
                data[i][j] = new byte[x];
                extra[i][j] = new byte[x];
                ticks[i][j] = new byte[x];
                tickMax[i][j] = new byte[x];
                twatch[i][j] = new boolean[x];
                for(int k = 0; k < x; k++)
                {
                    data[i][j][k] = 0;
                    extra[i][j][k] = 0;
                    ticks[i][j][k] = 0;
                    tickMax[i][j][k] = 0;
                    twatch[i][j][k] = false;
                }

            }

        }

    }

    public Field(Viewport v, byte d[][][], byte e[][][])
    {
        parent = v;
        data = d;
        extra = e;
        wires = torches = 0;
    }

    public Blocks g(int x, int y, int z)
    {
        if(z < 0)
            return Blocks.BLOCK;
        if(z >= data.length)
            return Blocks.AIR;
        if(cyclic)
        {
            y = (y % data[0].length + data[0].length) % data[0].length;
            x = (x % data[0][0].length + data[0][0].length) % data[0][0].length;
        } else
        if(y < 0 || y >= data[0].length || x < 0 || x >= data[0][0].length)
            return Blocks.AIR;
        return Blocks.values()[data[z][y][x]];
    }

    public boolean p(int x, int y, int z)
    {
        return gp(x, y, z) != 0;
    }

    public int gp(int x, int y, int z)
    {
        if(z < 0 || g(x, y, z).air())
            return 0;
        if(cyclic)
        {
            y = (y % data[0].length + data[0].length) % data[0].length;
            x = (x % data[0][0].length + data[0][0].length) % data[0][0].length;
        } else
        if(y < 0 || y >= data[0].length || x < 0 || x >= data[0][0].length)
            return 0;
        // Ok, part of the hack is that we return the block as "powered" when the ticks of the block hit 0
        // shifted from the bit 5 and 6.  
        return extra[z][y][x] & 0x1f;
    }

    public int w(int x, int y, int z)
    {
        if(z < 0 || z >= data.length)
            return 0;
        if(cyclic)
        {
            y = (y % data[0].length + data[0].length) % data[0].length;
            x = (x % data[0][0].length + data[0][0].length) % data[0][0].length;
        } else
        if(y < 0 || y >= data[0].length || x < 0 || x >= data[0][0].length)
            return 0;
        return extra[z][y][x] >> 5 & 7;
    }
    
    public int r(int x, int y, int z)
    {
        if(z < 0 || z >= data.length)
            return 0;
        if(cyclic)
        {
            y = (y % data[0].length + data[0].length) % data[0].length;
            x = (x % data[0][0].length + data[0][0].length) % data[0][0].length;
        } else
        if(y < 0 || y >= data[0].length || x < 0 || x >= data[0][0].length)
            return 0;
        return (extra[z][y][x] >> 4) & 0xF;
    }
    
    public int gTicks(int x, int y, int z)
    {
        if(z < 0 || z >= data.length)
            return 0;
        if(cyclic)
        {
            y = (y % data[0].length + data[0].length) % data[0].length;
            x = (x % data[0][0].length + data[0][0].length) % data[0][0].length;
        } else
        if(y < 0 || y >= data[0].length || x < 0 || x >= data[0][0].length)
            return 0;
        
        return tickMax[z][y][x];
    }
    public boolean t(int x,int y, int z)
  {
        if(z < 0 || z >= data.length)
            return false;
        if(cyclic)
        {
            y = (y % data[0].length + data[0].length) % data[0].length;
            x = (x % data[0][0].length + data[0][0].length) % data[0][0].length;
        } else
        if(y < 0 || y >= data[0].length || x < 0 || x >= data[0][0].length)
            return false;
        
        return ticks[z][y][x] ==0 && twatch[z][y][x];
    }
    public void TogleTick(int x, int y, int z, boolean flag)
    {
        if(z < 0 || z >= data.length)
            return ;
        if(cyclic)
        {
            y = (y % data[0].length + data[0].length) % data[0].length;
            x = (x % data[0][0].length + data[0][0].length) % data[0][0].length;
        } else
        if(y < 0 || y >= data[0].length || x < 0 || x >= data[0][0].length)
            return ;

        if(flag && !twatch[z][y][x]) ticks[z][y][x] = tickMax[z][y][x];
        twatch[z][y][x] = flag;
    }
   
     public boolean it(int x,int y, int z)
  {
        if(z < 0 || z >= data.length)
            return false ;
        if(cyclic)
        {
            y = (y % data[0].length + data[0].length) % data[0].length;
            x = (x % data[0][0].length + data[0][0].length) % data[0][0].length;
        } else
        if(y < 0 || y >= data[0].length || x < 0 || x >= data[0][0].length)
            return false;
        if(!twatch[z][y][z])
            return false;
 
        if( ticks[z][y][x] >0 )
        {
            ticks[z][y][x] -=1;
                return false;
        }
        return true;
  }
    
    
    
 

    public void s(int x, int y, int z, Blocks v)
    {
        if(z < 0 || z >= data.length || g(x, y, z) == v)
            return;
        if(cyclic)
        {
            y = (y % data[0].length + data[0].length) % data[0].length;
            x = (x % data[0][0].length + data[0][0].length) % data[0][0].length;
        } else
        if(y < 0 || y >= data[0].length || x < 0 || x >= data[0][0].length)
            return;
        if(g(x, y, z) == Blocks.TORCH)
            torches--;
        else
        if(g(x, y, z) == Blocks.WIRE)
            wires--;
        else
        if(g(x, y, z).block())
        {
            if(g(x, y, z + 1) == Blocks.WIRE || g(x, y, z + 1) == Blocks.DOORA)
                s(x, y, z + 1, Blocks.AIR);
            for(int i = 0; i < 5; i++)
                if(g(x - dir[i][0], y - dir[i][1], z - dir[i][2]).wall % 2 == 1 && w(x - dir[i][0], y - dir[i][1], z - dir[i][2]) == i)
                    s(x - dir[i][0], y - dir[i][1], z - dir[i][2], Blocks.AIR);

        } else
        if(g(x, y, z) == Blocks.DOORA)
        {
            data[z][y][x] = (byte)v.ordinal();
            s(x, y, z + 1, Blocks.AIR);
        } else
        if(g(x, y, z) == Blocks.DOORB)
        {
            data[z][y][x] = (byte)v.ordinal();
            s(x, y, z - 1, Blocks.AIR);
        }
        if(v == Blocks.TORCH)
            torches++;
        else
        if(v == Blocks.WIRE)
            wires++;
        data[z][y][x] = (byte)v.ordinal();
        parent.updateRed();
    }

    public boolean s(int x, int y, int z, int w)
    {
        if(!valid(x, y, z, w))
        {
            return false;
        } else
        {
            
                extra[z][y][x] = (byte)((w << 5) + (extra[z][y][x] & 0x1f));
            return true;
        }
    }
  
    
    public void sp(int x, int y, int z, int p)
    {
        if(z < 0 || z >= data.length)
            return;
        if(cyclic)
        {
            y = (y % data[0].length + data[0].length) % data[0].length;
            x = (x % data[0][0].length + data[0][0].length) % data[0][0].length;
        } else
        if(y < 0 || y >= data[0].length || x < 0 || x >= data[0][0].length)
            return;
        if(g(x, y, z) == Blocks.DOORA)
        {
            if(p(x, y, z))
            {
                if(p == 0)
                    parent.play(false);
            } else
            if(p != 0)
                parent.play(true);
            sp(x, y, z + 1, p);
        }
        
        extra[z][y][x] = (byte)((extra[z][y][x] & 0xe0) + p);
    }

    public boolean s(int x, int y, int z, Blocks b, int e)
    {
        if(z < 0 || z >= data.length)
            return false;
        if(cyclic)
        {
            y = (y % data[0].length + data[0].length) % data[0].length;
            x = (x % data[0][0].length + data[0][0].length) % data[0][0].length;
        } else
        if(y < 0 || y >= data[0].length || x < 0 || x >= data[0][0].length)
            return false;
        if(g(x, y, z) == b && extra[z][y][x] == e)
        {
            return false;
        } else
        {
            s(x, y, z, b);
            extra[z][y][x] = (byte)e;
            return true;
        }
    }

    public boolean match(int x, int y, int z, Palette p)
    {
        return g(x, y, z) == p.a && (p.b == null || g(x, y, z + 1) == p.b) && (p.c == null || g(x, y, z + 1) == p.c);
    }

    private boolean valid(int x, int y, int z, int w)
    {
        if(g(x,y,z) == Blocks.REPEATER)
                return w >0;
        if(w == 0)
            return g(x, y, z).wall < 2; // valid if we are air.
        if(g(x, y, z).wall % 2 == 0)
            return g(x, y, z).wall == 2; // if door?
        else
            return g(x + dir[w][0], y + dir[w][1], z + dir[w][2]).block(); // is next to me?
    }

    public void draw(int x, int y, int z, Graphics g, Rectangle r, Blocks b[])
    {
        int p = 0;
        boolean whiteout = false;
        boolean fake;
        if(!(fake = b.length != 0))
            b = (new Blocks[] {
                g(x, y, z), g(x, y, z + 1), g(x, y, z + 2)
            });
        if(b[0].block())
        {
            p++;
            g.setColor(Colors.block);
       
        } else
        if(b[0].air())
        {
            if(b[0] == Blocks.SHADOW && (layers == 1 || b[1].air()))
            {
                g.setColor(Colors.grid);
            } else
            {
                p++;
                whiteout = true;
                g.setColor(Colors.air);
            }
        } else
        {
            g.setColor(Colors.air);
        }
        g.fillRect(r.x, r.y, r.width, r.height);
        if(b[0] == Blocks.WIRE)
        {
            if(fake)
                drawWire(g, r, true, 15, false);
            else
                drawWire(g, r, x, y, z, false);
            p++;
            if(layers > 1 && !b[p].air() && !b[p].block())
            {
                g.setColor(Colors.aircover);
                g.fillRect(r.x, r.y, r.width, r.height);
            }
        }
        
        if(p > 0 && layers == 1)
            return;
        boolean tog = true;
        switch(b[p])   
        {
        case REPEATER:
            /*
             * Low (1st & 2nd) bits:
                0x0: Facing east
                0x1: Facing south
                0x2: Facing west
                0x3: Facing north
                High (3rd & 4th) bits:
                0x0: 1 tick delay
                0x1: 2 tick delay
                0x2: 3 tick delay
                0x3: 4 tick delay
             */
            /*
             * Changing the internal poisitions so its more like the torch
             *  0x1: Pointing south
                0x2: Pointing north
                0x3: Pointing west
                0x4: Pointing east
             */
            int[] xP;
            int[] yP;
            g.setColor(Colors.repeater);
            //g.setColor(Colors.door);
            int rdir = fake ? 2 : w(x,y,z);
            System.out.print(rdir);
            
            int tick = 0; //this.gRepeater_ticks(x, y, z);
            boolean rpow = fake ? false : t(x,y,z);
            switch(rdir)
            {
                
                case 2: // North
                        xP = new int[] { r.x +1,r.x+4,r.x+7,r.x+5,r.x+5,r.x+3,r.x+3,r.x+1 };
                        yP = new int[] { r.y +4,r.y+1,r.y+4,r.y+4,r.y+7,r.y+7,r.y+4,r.y+4 };
                        if(rpow)
                        {
                            g.setColor(Colors.wireOn);
                            g.fillPolygon(xP,yP,8);
                            if(tick>0)
                            {
                                g.setColor(Colors.wireOff);   
                                g.fillRect(r.x+3, r.y+3+tick, 2, 1);
                            }
                        }
                        else
                             g.fillPolygon(xP,yP,8);
                      

                    break;
                case 1: // south
                        xP = new int[] { r.x +1,r.x+4,r.x+7,r.x+5,r.x+5,r.x+3,r.x+3,r.x+1 };
                        yP = new int[] { r.y +4,r.y+7,r.y+4,r.y+4,r.y+1,r.y+1,r.y+4,r.y+4 };
                        if(rpow)
                        {
                            g.setColor(Colors.wireOn);
                            g.fillPolygon(xP,yP,8);
                            if(tick>0)
                            {
                                g.setColor(Colors.wireOff);   
                                g.fillRect(r.x+3, r.y+3-tick, 2, 1);
                            }
                        }
                        else
                             g.fillPolygon(xP,yP,8);
                
                    break;
                case 4: // East
                        xP = new int[] { r.x +1,r.x+4,r.x+4,r.x+7,r.x+7,r.x+4,r.x+4,r.x+1 };
                        yP = new int[] { r.y +4,r.y+1,r.y+3,r.y+3,r.y+5,r.y+5,r.y+7,r.y+4 };
                        if(rpow)
                        {
                            g.setColor(Colors.wireOn);
                            g.fillPolygon(xP,yP,8);
                            if(tick>0)
                            {
                                g.setColor(Colors.wireOff);   
                                g.fillRect(r.x+3, r.y+3+tick, 1, 2);
                            }
                        }
                        else
                             g.fillPolygon(xP,yP,8);
                     
                    break;
                case 3: // west
                        xP = new int[] { r.x +7,r.x+4,r.x+4,r.x+1,r.x+1,r.x+4,r.x+4,r.x+7 };
                        yP = new int[] { r.y +4,r.y+1,r.y+3,r.y+3,r.y+5,r.y+5,r.y+7,r.y+4 };
                        if(rpow)
                        {
                            g.setColor(Colors.wireOn);
                            g.fillPolygon(xP,yP,8);
                            if(tick>0)
                            {
                                g.setColor(Colors.wireOff);   
                                g.fillRect(r.x+3, r.y+3+tick, 1, 2);
                            }
                        }
                        else
                             g.fillPolygon(xP,yP,8);
                     
                    break;
            }
            g.fillPolygon(xP,yP,8);
            if(rpow)
            g.setColor(Colors.wireOn);
            else
            g.setColor(Colors.wireOff);
            
            if(rpow & tick ==0)
                g.fillRect(rt.x,rt.y,rt.width, rt.height);
            else
                if(tick>0)
                    g.fillRect(rt.x,rt.y,rt.width,rt.height);
            
            
            
            break;
           
            
        case WIRE: // '\003'
            if(fake)
                drawWire(g, r, true, 15, false);
            else
                drawWire(g, r, x, y, z + p, false);
            break;

        case LEVER: // '\005'
            tog = false;
            // fall through

        case TORCH: 
            /*
             *  0x1: Pointing south
                0x2: Pointing north
                0x3: Pointing west
                0x4: Pointing east
             */
            g.setColor(Colors.door);
            if(fake || w(x, y, z + p) == 1)
                g.fillRect(r.x + 3, r.y + 3, 2, 5);
            else
            if(w(x, y, z + p) == 2)
                g.fillRect(r.x + 3, r.y, 2, 5);
            else
            if(w(x, y, z + p) == 3)
                g.fillRect(r.x + 3, r.y + 3, 5, 2);
            else
            if(w(x, y, z + p) == 4)
                g.fillRect(r.x, r.y + 3, 5, 2);
            if(!tog)
                g.setColor(Colors.valve);
            else
            if(fake || p(x, y, z + p))
                g.setColor(Colors.wireOn);
            else
                g.setColor(Colors.wireOff);
            g.fillOval(r.x + 2, r.y + 2, 4, 4);
            g.setColor(Colors.wireOn);
            if(!tog && !fake && p(x, y, z + p))
                g.fillOval(r.x + 3, r.y + 3, 2, 2);
            break;
        case BUTTON: // '\006'
            g.setColor(Colors.button);
            if(!fake && p(x, y, z + p))
            {
                if(w(x, y, z + p) == 1)
                    g.fillRect(r.x + 2, r.y + 7, 4, 1);
                else
                if(w(x, y, z + p) == 2)
                    g.fillRect(r.x + 2, r.y, 4, 1);
                else
                if(w(x, y, z + p) == 3)
                    g.fillRect(r.x + 7, r.y + 2, 1, 4);
                else
                if(w(x, y, z + p) == 4)
                    g.fillRect(r.x, r.y + 2, 1, 4);
            } else
            if(fake || w(x, y, z + p) == 1)
                g.fillRect(r.x + 2, r.y + 5, 4, 3);
            else
            if(w(x, y, z + p) == 2)
                g.fillRect(r.x + 2, r.y, 4, 3);
            else
            if(w(x, y, z + p) == 3)
                g.fillRect(r.x + 5, r.y + 2, 3, 4);
            else
            if(w(x, y, z + p) == 4)
                g.fillRect(r.x, r.y + 2, 3, 4);
            break;

        case PRESS: // '\t'
            if(!fake && p(x, y, z + p))
                g.setColor(Colors.wireOn);
            else
                g.setColor(Colors.valve);
            g.fillRect(r.x + 1, r.y + 1, 6, 6);
            break;

        case DOORB: // '\b'
            p--;
            // fall through

        case DOORA: // '\007'
            g.setColor(Colors.door);
            int w = 1;
            int c = 2;
            if(!fake)
            {
                w = (new int[] {2, 0, 3, 1})[w(x, y, z + p) - 1];
                c = w;
                if(w(x, y, z + p + 1) != 2)
                    c = (c + 1) % 4;
                if(p(x, y, z + p))
                    w = (w + (w(x, y, z + p + 1) != 2 ? 1 : 3)) % 4;
            }
            if(w == 0)
                g.fillRect(r.x, r.y, 8, 2);
            else
            if(w == 1)
                g.fillRect(r.x + 6, r.y, 2, 8);
            else
            if(w == 2)
                g.fillRect(r.x, r.y + 6, 8, 2);
            else
            if(w == 3)
                g.fillRect(r.x, r.y, 2, 8);
            if(!fake && p(x, y, z + p))
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

       
        }
        if(b[1].block() && layers > 1)
        {
            g.setColor(Colors.cover);
            g.fillRect(r.x, r.y, r.width, r.height);
            if(layers > 2 && b[2] == Blocks.WIRE && (b[0] == (bridge ? Blocks.WIRE : Blocks.AIR) || b[0].block()))
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

    private boolean c(int x, int y, int x2, int y2, int z)
    {
        if(MCwires)
        {
             if(g(x2,y2,z) == Blocks.REPEATER)
             {
            // Might be a better way, but trying to find the direction faceing
            int dx = x - x2;
            int dy = y - y2;
            if(dx ==0 && dy !=0)
            {
                if(dy>0 && w(x2,y2,z) == 1)  // if pointing south
                    return true;
                if(dy<0 && w(x2,y2,z) == 2) // if pointing north
                    return true;
            }
             if(dx !=0 && dy ==0)
            {
                if(dx>0 && w(x2,y2,z) == 4)  // if pointing south
                    return true;
                if(dx<0 && w(x2,y2,z) == 3) // if pointing north
                    return true;
            }
                return false;
            }
            if(g(x2, y2, z).air())
                return g(x2, y2, z - 1).conn;
            if(g(x2,y2,z).repeater())
                return g(x2,y2,z).wall 
            if(g(x2, y2, z).block())
                return !g(x, y, z + 1).block() && g(x2, y2, z + 1).conn;
            else
                return true;
        }
        if(g(x2, y2, z).conn)
            return true;
        if(g(x2, y2, z).air())
            return g(x2, y2, z - 1) == Blocks.WIRE;
        if(g(x2, y2, z).block())
        {
            if(g(x2, y2, z + 1) == Blocks.WIRE && !g(x, y, z + 1).block())
                return true;
            for(int i = 0; i < 5; i++)
                if(g(x2 - dir[i][0], y2 - dir[i][1], z - dir[i][2]).wall % 2 == 1 && w(x2 - dir[i][0], y2 - dir[i][1], z - dir[i][2]) == i)
                {
                    if(g(x2 - dir[i][0], y2 - dir[i][1], z - dir[i][2]) == Blocks.TORCH)
                        return blockConnect(x2, y2, x - x2, y - y2, z, false);
                    return i != 0 || g(x2, y2, z + 1) != Blocks.LEVER || !dummyGdValve;
                }

            return g(x2, y2, z - 1) == Blocks.TORCH;
        } else
        if(g(x2,y2,z) == Blocks.REPEATER)
        {
            // Might be a better way, but trying to find the direction faceing
            int dx = x - x2;
            int dy = y - y2;
            if(dx ==0 && dy !=0)
            {
                if(dy>0 && w(x2,y2,z) == 1)  // if pointing south
                    return true;
                if(dy<0 && w(x2,y2,z) == 2) // if pointing north
                    return true;
            }
             if(dx !=0 && dy ==0)
            {
                if(dx>0 && w(x2,y2,z) == 4)  // if pointing south
                    return true;
                if(dx<0 && w(x2,y2,z) == 3) // if pointing north
                    return true;
            }
   

            return false;
            
        } else 
        {
            return false;
        }
    }

    public void drawWire(Graphics g, Rectangle r, int x, int y, int z, boolean thick)
    {
        drawWire(g, r, p(x, y, z), 
                (c(x, y, x - 1, y, z) ? 8 : 0) + 
                (c(x, y, x + 1, y, z) ? 4 : 0) + 
                (c(x, y, x, y - 1, z) ? 2 : 0) + 
                (c(x, y, x, y + 1, z) ? 1 : 0), 
                false);
    }

    public static void drawWire(Graphics g, Rectangle r, boolean on, int c, boolean thick)
    {
        if(on)
            g.setColor(Colors.wireOn);
        else
            g.setColor(Colors.wireOff);
        if(MCwires)
        {
            if((c & 3) == 0)
                c = (c & 0xc) != 0 ? 12 : 15;
            else
            if((c & 0xc) == 0)
                c = 3;
        } else
        if(c == 0)
            g.fillRect(r.x + 2, r.y + 2, 4, 4);
        if((c & 1) != 0)
            g.fillRect(r.x + 3, r.y + 3, 2, 5);
        if((c & 2) != 0)
            g.fillRect(r.x + 3, r.y, 2, 5);
        if((c & 4) != 0)
            g.fillRect(r.x + 3, r.y + 3, 5, 2);
        if((c & 8) != 0)
            g.fillRect(r.x, r.y + 3, 5, 2);
    }

    public void update()
    {
        // clear wires and set blocks power by torches
        for(int z = 0; z < data.length; z++)
        {
            for(int y = 0; y < data[0].length; y++)
            {
                for(int x = 0; x < data[0][0].length; x++)
                {
                    if(g(x, y, z) == Blocks.WIRE || g(x, y, z) == Blocks.DOORB || g(x,y,z) == Blocks.REPEATER)
                        sp(x, y, z, 0);
                    else
                    if(g(x, y, z).block())
                    {
                        if(p(x, y, z - 1) && g(x, y, z - 1) == Blocks.TORCH)
                        {
                            sp(x, y, z, 17);
                        } else
                        {
                            for(int i = dummyGdValve ? 1 : 0; i < 5; i++)
                            {
                                if(g(x - dir[i][0], y - dir[i][1], z - dir[i][2]).ctrl() && 
                                        w(x - dir[i][0], y - dir[i][1], z - dir[i][2]) == i && 
                                        p(x - dir[i][0], y - dir[i][1], z - dir[i][2]))
                                    sp(x, y, z, 17);
                                
                                    
                            }
                        }
                        sp(x, y, z, 0);
                    }
                    if(g(x,y,z) == Blocks.REPEATER)
                    {
                        if(g(x + dir[i][0], y + dir[i][1], z ) == Blocks.REPEATER)
                                {
                                    int w = w(x + dir[i][0], y + dir[i][1], z);
                                    if(w == i) // Arrow pointing at this block
                                        if(it(x + dir[i][0], y+ dir[i][1], z + dir[i][2]))
                                            sp(x,y,z,17); 
                                }
                        
                    }
                }

            }

        }

        for(int z = 0; z < data.length; z++)
        {
            for(int y = 0; y < data[0].length; y++)
            {
                for(int x = 0; x < data[0][0].length; x++)
                    if(
                            gp(x, y, z) >= (g(x, y, z) != Blocks.BUTTON && g(x, y, z) != Blocks.PRESS ? 16 : 1) && (g(x, y, z) == Blocks.TORCH || 
                            g(x, y, z).ctrl() || 
                            g(x, y, z).block() && gp(x, y, z) == 17))
                    {
                        if(g(x, y, z - 1) == Blocks.WIRE)
                            followWire(x, y, z - 1, 15);
                        if(g(x, y, z + 1) == Blocks.WIRE)
                            followWire(x, y, z + 1, 15);
                        if(g(x, y + 1, z) == Blocks.WIRE)
                            followWire(x, y + 1, z, 15);
                        if(g(x, y - 1, z) == Blocks.WIRE)
                            followWire(x, y - 1, z, 15);
                        if(g(x + 1, y, z) == Blocks.WIRE)
                            followWire(x + 1, y, z, 15);
                        if(g(x - 1, y, z) == Blocks.WIRE)
                            followWire(x - 1, y, z, 15);
                    }

            }

        }

        for(int z = 0; z < data.length; z++)
        {
            for(int y = 0; y < data[0].length; y++)
            {
                for(int x = 0; x < data[0][0].length; x++)
                {
                    if(
                            (g(x, y, z).block() && !p(x, y, z) || g(x, y, z) == Blocks.DOORA) && 
                            (g(x, y, z + 1) == Blocks.WIRE && p(x, y, z + 1) || blockConnect(x, y, 0, 1, z, true) || blockConnect(x, y, 0, -1, z, true) || blockConnect(x, y, 1, 0, z, true) || blockConnect(x, y, -1, 0, z, true)))
                        sp(x, y, z, 16);
                    if(g(x,y,z) == Blocks.REPEATER)
                    {
                            int w = w(x,y,z);
                            if(repeaterConnect(x,y,dir[w][0],dir[w][1],z,true))
                            {
                                    this.TogleTick(x, y, z, true);
                            }
                    }
                    
                }

            }

        }

        for(int z = 0; z < data.length; z++)
        {
            for(int y = 0; y < data[0].length; y++)
            {
                for(int x = 0; x < data[0][0].length; x++)
                    if(g(x, y, z) == Blocks.DOORA && !p(x, y, z + 1))
                        if(powerDoor(x, y, z + 2) || powerDoor(x, y, z - 1) || powerDoor(x, y + 1, z) || powerDoor(x, y + 1, z + 1) || powerDoor(x, y - 1, z) || powerDoor(x, y - 1, z + 1) || powerDoor(x + 1, y, z) || powerDoor(x + 1, y, z + 1) || powerDoor(x - 1, y, z) || powerDoor(x - 1, y, z + 1))
                            sp(x, y, z, 16);
                        else
                            sp(x, y, z, 0);

            }

        }

    }
    private boolean repeaterConnect(int x, int y, int dx, int dy, int z, boolean pow)
    {
        Blocks g = g(x+dx,y+dy,z);
        
        int l = 0;
        if(dx ==0 && dy >0) l = 1; // is south
        if(dx ==0 && dy <0) l = 2; // is north
        if(dx >0 && dy ==0) l = 3; // is east
        if(dx <0 && dy ==0) l = 4; // is west
        if(l==0) return false; // invalid data
        if(l != w(x,y,z)) return false; // the back end is not at this location so we cannot power the repeater
        if(g == Blocks.WIRE )
        {
            if(p(x+dx,y+dy,z))
                return true;
            else
                return false;
            // Ok this is a filler, figuring out wire connections for the back end is going to be a bitch
        }
        if((g.block() || g.ctrl() || g == Blocks.TORCH ) && p(x+dx,y+dy,z))
            return true;
        if(g == Blocks.REPEATER && w(x,y,z) == w(x + dx, y + dy, z))
            return true;
        return false;
    }
    private boolean powerDoor(int x, int y, int z)
    {
        return g(x, y, z) != Blocks.DOORA && g(x, y, z) != Blocks.DOORB && p(x, y, z);
    }

    private boolean blockConnect(int x, int y, int dx, int dy, int z, boolean pow)
    {
       // if(g(x+dx,y+dy,z) == Blocks.REPEATER && t(x+dx,y+dy,z))
        if(g(x + dx, y + dy, z) != Blocks.WIRE || !p(x + dx, y + dy, z) && pow)
            return false;
        if(g(x + dx + dy, (y + dy) - dx, z).block())
        {
            if(!g(x + dx, y + dy, z + 1).block() && g(x + dx + dy, (y + dy) - dx, z + 1).conn)
                return false;
        } else
        if(g(x + dx + dy, (y + dy) - dx, z).air())
        {
            if(g(x + dx + dy, (y + dy) - dx, z - 1).conn)
                return false;
        } else
        {
            return false;
        }
        if(g((x + dx) - dy, y + dy + dx, z).block())
            return g(x + dx, y + dy, z + 1).block() || !g((x + dx) - dy, y + dy + dx, z + 1).conn;
        if(g((x + dx) - dy, y + dy + dx, z).air())
            return !g((x + dx) - dy, y + dy + dx, z - 1).conn;
        else
            return false;
    }

    private void followWire(int x, int y, int z, int p)
    {
        if(p <= gp(x, y, z))
            return;
        sp(x, y, z, p);
        if(p == 0)
        {
            return;
        } else
        {
            followWireQ(x, y, x, y + 1, z, p - 1);
            followWireQ(x, y, x, y - 1, z, p - 1);
            followWireQ(x, y, x + 1, y, z, p - 1);
            followWireQ(x, y, x - 1, y, z, p - 1);
            return;
        }
    }

    private void followWireQ(int x, int y, int x2, int y2, int z, int p)
    {
        if(g(x2, y2, z) == Blocks.WIRE)
            followWire(x2, y2, z, p);
        else
        if(g(x2, y2, z).block()) // if we reached a block, and that block has a wire on top
        {
            if(g(x2, y2, z + 1) == Blocks.WIRE && !g(x, y, z + 1).block())
                followWire(x2, y2, z + 1, p);
        } else
        if(g(x2, y2, z - 1) == Blocks.WIRE) //if the wire is below the next block
            followWire(x2, y2, z - 1, p);
    }

  

    public void tick()
    {
        parent.modify();
        

        for(int z = 0; z < data.length; z++)
        {
            for(int y = 0; y < data[0].length; y++)
            {
                for(int x = 0; x < data[0][0].length; x++)
                    if(g(x, y, z) == Blocks.TORCH)
                    {
                        int w[] = dir[w(x, y, z)];
                        sp(x, y, z, gp(x + w[0], y + w[1], z + w[2]) < 16 ? 16 : 0);
                    } else
                    if(p(x, y, z) && (g(x, y, z) == Blocks.BUTTON || g(x, y, z) == Blocks.PRESS && (parent.lastX != x || parent.lastY != y || z != parent.lyr && z != parent.lyr + 1)))
                        sp(x, y, z, gp(x, y, z) - 1);

            }

        }

        update();
    }

    public void save(File f)
        throws FileNotFoundException, IOException
    {
        DataOutputStream dos = new DataOutputStream(new FileOutputStream(f));
        dos.writeInt(0x52656453);
        dos.writeByte(1);
        dos.writeShort(parent.z);
        dos.writeShort(parent.y);
        dos.writeShort(parent.x);
        for(int i = 0; i < parent.z; i++)
        {
            for(int j = 0; j < parent.y; j++)
                dos.write(data[i][j]);

        }

        for(int i = 0; i < parent.z; i++)
        {
            for(int j = 0; j < parent.y; j++)
                dos.write(extra[i][j]);

        }

        dos.close();
    }

    public void load(File f)
        throws IllegalArgumentException, IOException
    {
        DataInputStream dis = new DataInputStream(new FileInputStream(f));
        if(dis.readInt() != 0x52656453)
            throw new IllegalArgumentException("Not a redstone file.");
        if(dis.read() > 1)
            throw new IllegalArgumentException("File has an incompatible version number.");
        int z = dis.readShort();
        int y = dis.readShort();
        int x = dis.readShort();
        data = new byte[z][][];
        for(int i = 0; i < z; i++)
        {
            data[i] = new byte[y][];
            for(int j = 0; j < y; j++)
            {
                data[i][j] = new byte[x];
                dis.read(data[i][j]);
            }

        }

        extra = new byte[z][][];
        for(int i = 0; i < z; i++)
        {
            extra[i] = new byte[y][];
            for(int j = 0; j < y; j++)
            {
                extra[i][j] = new byte[x];
                dis.read(extra[i][j]);
            }

        }

        dis.close();
        parent.setSize(x, y, z);
        parent.setLyr(0);
        parent.recountRed();
        parent.view.repaint();
    }

    public void clone(int c[])
    {
        byte cdat[][][] = new byte[c[5]][][];
        byte cext[][][] = new byte[c[5]][][];
        for(int i = 0; i < c[5]; i++)
        {
            cdat[i] = new byte[c[4]][];
            cext[i] = new byte[c[4]][];
            for(int j = 0; j < c[4]; j++)
            {
                cdat[i][j] = new byte[c[3]];
                cext[i][j] = new byte[c[3]];
                for(int k = 0; k < c[3]; k++)
                {
                    cdat[i][j][k] = data[c[2] + i][c[1] + j][c[0] + k];
                    cext[i][j][k] = extra[c[2] + i][c[1] + j][c[0] + k];
                }

            }

        }

        if(c[8] > data.length - c[5])
            c[8] = data.length - c[5];
        if(c[7] > data[0].length - c[4])
            c[7] = data[0].length - c[4];
        if(c[6] > data[0][0].length - c[3])
            c[6] = data[0][0].length - c[3];
        for(int i = 0; i < c[5]; i++)
        {
            for(int j = 0; j < c[4]; j++)
            {
                for(int k = 0; k < c[3]; k++)
                {
                    data[c[8] + i][c[7] + j][c[6] + k] = cdat[i][j][k];
                    extra[c[8] + i][c[7] + j][c[6] + k] = cext[i][j][k];
                }

            }

        }

        parent.modify();
    }


}

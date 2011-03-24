
package mcredsim;

import java.awt.*;
import java.io.*;


public class Field
{

    public Field(Viewport v, int x, int y, int z)
    {
        parent = v;
        data = new byte[z][][];
        extra = new byte[z][][];
        wires = torches = 0;
        for(int i = 0; i < z; i++)
        {
            data[i] = new byte[y][];
            extra[i] = new byte[y][];
            for(int j = 0; j < y; j++)
            {
                data[i][j] = new byte[x];
                extra[i][j] = new byte[x];
                for(int k = 0; k < x; k++)
                {
                    data[i][j][k] = 0;
                    extra[i][j][k] = 0;
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
    
    public int rh(int x, int y, int z)
    {
        return r(x,y,z) >>2;
    }
    public int rl(int x, int y, int z)
    {
        return r(x,y,z) & 3;
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
    // Set the repeater.  Sigh, why does the repeater have to be special
    public boolean sr(int x, int y, int z, int w)
    {
        if(!valid(x, y, z, w))
        {
            return false;
        } else
        {
            extra[z][y][x] = (byte)((w << 4) + (extra[z][y][x] & 0xf));
            return true;
        }
    }
    public boolean srh(int x, int y, int z, int w)
    {
        return sr(x,y,z,(w & 3) <<2);
    }
    public boolean srl(int x, int y, int z, int w)
    {
        return sr(x,y,z,(w & 3));
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
        if(w == 0)
            return g(x, y, z).wall < 2;
        if(g(x, y, z).wall % 2 == 0)
            return g(x, y, z).wall == 2;
        else
            return g(x + dir[w][0], y + dir[w][1], z + dir[w][2]).block();
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
            if(Viewport.waterMode)
            {
                if(b[0] == Blocks.SAND)
                    g.setColor(Colors.sand);
                else
                    g.setColor(Colors.dirt);
            } else
            {
                g.setColor(Colors.block);
            }
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
            g.setColor(Colors.door);
            int dir = r(x, y, z + p);
            switch(dir & 3)
            {
                case 0: // east
                    if(fake)
                       g.fillRect(r.x+3, r.y , 2, 2);
                    else
                        g.fillRect(r.x, r.y + 3, 2, 2);
                    break;
                case 1: // south
                    g.fillRect(r.x+ 3, r.y + 6, 2, 2);
                    break;
                case 2: // west
                    g.fillRect(r.x+6, r.y + 3, 2, 2);
                    break;
                case 3: // north
                    g.fillRect(r.x+3, r.y , 2, 2);
                    break;
            }
            g.setFont(new Font("Courier", Font.BOLD, 5));
            switch(dir>>2 & 3)
            {
                case 0: // 1 tick
                    
                   g.drawString("1", r.x+3, r.y+6);
                    break;
                case 1: // 2 tick
                  g.drawString("2", r.x+3, r.y+6);
                    break;
                case 2: // 3 tick
                   g.drawString("3",r.x+3, r.y+6);
                    break;
                case 3: // 4 tick
                   g.drawString("4", r.x+3, r.y+6);
                    break;
            }
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

        case WATER: // '\013'
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
            if(g(x2, y2, z).air())
                return g(x2, y2, z - 1).conn;
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
        {
            return false;
        }
    }

    public void drawWire(Graphics g, Rectangle r, int x, int y, int z, boolean thick)
    {
        drawWire(g, r, p(x, y, z), (c(x, y, x - 1, y, z) ? 8 : 0) + (c(x, y, x + 1, y, z) ? 4 : 0) + (c(x, y, x, y - 1, z) ? 2 : 0) + (c(x, y, x, y + 1, z) ? 1 : 0), false);
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
        for(int z = 0; z < data.length; z++)
        {
            for(int y = 0; y < data[0].length; y++)
            {
                for(int x = 0; x < data[0][0].length; x++)
                    if(g(x, y, z) == Blocks.WIRE || g(x, y, z) == Blocks.DOORB)
                        sp(x, y, z, 0);
                    else
                    if(g(x, y, z).block())
                    {
                        sp(x, y, z, 0);
                        if(p(x, y, z - 1) && g(x, y, z - 1) == Blocks.TORCH)
                        {
                            sp(x, y, z, 17);
                        } else
                        {
                            for(int i = dummyGdValve ? 1 : 0; i < 5; i++)
                                if(g(x - dir[i][0], y - dir[i][1], z - dir[i][2]).ctrl() && w(x - dir[i][0], y - dir[i][1], z - dir[i][2]) == i && p(x - dir[i][0], y - dir[i][1], z - dir[i][2]))
                                    sp(x, y, z, 17);

                        }
                    }

            }

        }

        for(int z = 0; z < data.length; z++)
        {
            for(int y = 0; y < data[0].length; y++)
            {
                for(int x = 0; x < data[0][0].length; x++)
                    if(gp(x, y, z) >= (g(x, y, z) != Blocks.BUTTON && g(x, y, z) != Blocks.PRESS ? 16 : 1) && (g(x, y, z) == Blocks.TORCH || g(x, y, z).ctrl() || g(x, y, z).block() && gp(x, y, z) == 17))
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
                    if((g(x, y, z).block() && !p(x, y, z) || g(x, y, z) == Blocks.DOORA) && (g(x, y, z + 1) == Blocks.WIRE && p(x, y, z + 1) || blockConnect(x, y, 0, 1, z, true) || blockConnect(x, y, 0, -1, z, true) || blockConnect(x, y, 1, 0, z, true) || blockConnect(x, y, -1, 0, z, true)))
                        sp(x, y, z, 16);

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

    private boolean powerDoor(int x, int y, int z)
    {
        return g(x, y, z) != Blocks.DOORA && g(x, y, z) != Blocks.DOORB && p(x, y, z);
    }

    private boolean blockConnect(int x, int y, int dx, int dy, int z, boolean pow)
    {
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
        if(g(x2, y2, z).block())
        {
            if(g(x2, y2, z + 1) == Blocks.WIRE && !g(x, y, z + 1).block())
                followWire(x2, y2, z + 1, p);
        } else
        if(g(x2, y2, z - 1) == Blocks.WIRE)
            followWire(x2, y2, z - 1, p);
    }

    private int findLowGround(int x, int y, int z, int depth)
    {
        if(!g(x, y, z).destruct())
            return 0x7fffffff;
        if(g(x, y, z - 1).destruct())
            return depth;
        if(depth == 5)
            return 0x7fffffff;
        else
            return Math.min(Math.min(findLowGround(x, y + 1, z, depth + 1), findLowGround(x, y - 1, z, depth + 1)), Math.min(findLowGround(x + 1, y, z, depth + 1), findLowGround(x - 1, y, z, depth + 1)));
    }

    public void tick()
    {
        parent.modify();
        for(int z = 0; z < data.length; z++)
        {
            for(int y = 0; y < data[0].length; y++)
            {
                for(int x = 0; x < data[0][0].length; x++)
                    if(g(x, y, z) == Blocks.WATER && (gp(x, y, z) & 0x10) != 0 && w(x, y, z) == 0)
                    {
                        boolean mod = false;
                        if(g(x, y, z - 1).destruct() || g(x, y, z - 1) == Blocks.WATER && (gp(x, y, z - 1) & 8) == 0 && (gp(x, y, z - 1) & 0xf) != 0)
                            mod |= s(x, y, z - 1, Blocks.WATER, gp(x, y, z) | 0x38);
                        else
                        if((gp(x, y, z) & 0xf) != 7 && (g(x, y, z - 1) != Blocks.WATER ? !g(x, y, z - 1).destruct() : (gp(x, y, z) & 0xf) == 0))
                        {
                            int sp[] = new int[4];
                            int spMin = 0x7fffffff;
                            for(int i = 1; i < 5; i++)
                                if((sp[i - 1] = findLowGround(x + dir[i][0], y + dir[i][1], z, 1)) < spMin)
                                    spMin = sp[i - 1];

                            int g = gp(x, y, z) & 0xf;
                            g = g < 8 ? g + 1 : 1;
                            for(int i = 1; i < 5; i++)
                                if(sp[i - 1] == spMin && (g(x + dir[i][0], y + dir[i][1], z).destruct() || g(x + dir[i][0], y + dir[i][1], z) == Blocks.WATER && (gp(x + dir[i][0], y + dir[i][1], z) ^ 7) > (g ^ 7)))
                                    mod |= s(x + dir[i][0], y + dir[i][1], z, Blocks.WATER, g + 48);

                        }
                        if(!mod)
                            s(x, y, z, Blocks.WATER, gp(x, y, z) & 0xf);
                    }

            }

        }

        for(int z = 0; z < data.length; z++)
        {
            for(int y = 0; y < data[0].length; y++)
            {
                for(int x = 0; x < data[0][0].length; x++)
                    if(g(x, y, z) == Blocks.WATER)
                        s(x, y, z, 0);
                    else
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

    private Viewport parent;
    byte data[][][];
    byte extra[][][];
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

}
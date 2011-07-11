
package mcredsim;


import java.io.FileInputStream;
import java.io.FileOutputStream;


public class LevelLoader
{

    public LevelLoader()
    {
    }

    public static void load(Viewport p, String world, int cX, int cY)
    {
        load(p, (new StringBuilder("C:\\Users\\Mario\\AppData\\Roaming\\.minecraft\\saves\\")).append(world).append("\\").append(Integer.toString(cX & 0x3f, 36)).append("\\").append(Integer.toString(cY & 0x3f, 36)).append("\\c.").append(Integer.toString(cX, 36)).append(".").append(Integer.toString(cY, 36)).append(".dat").toString());
    }

    public static void load(Viewport p, String file)
    {
        byte data[] = (byte[])null;
        byte extra[] = (byte[])null;
        int sX = 16;
        int sY = 16;
        int sZ = 128;
        boolean sch = file.endsWith(".schematic");
        try
        {
            Tag root = Tag.readFrom(new FileInputStream(file));
            data = (byte[])root.findTagByName("Blocks").getValue();
            extra = (byte[])root.findTagByName("Data").getValue();
            if(sch)
            {
                sX = ((Short)root.findTagByName("Width").getValue()).shortValue();
                sY = ((Short)root.findTagByName("Length").getValue()).shortValue();
                sZ = ((Short)root.findTagByName("Height").getValue()).shortValue();
            }
        }
        catch(Exception exception) { }
        byte chunk[][][] = new byte[sZ][][];
        byte cext[][][] = new byte[sZ][][];
        int ai[];
        int ai1[];
        for(int i = 0; i < sZ; i++)
        {
            chunk[i] = new byte[sY][];
            cext[i] = new byte[sY][];
            for(int j = 0; j < sY; j++)
            {
                chunk[i][j] = new byte[sX];
                cext[i][j] = new byte[sX];
                for(int k = 0; k < sX; k++)
                {
                    int n;
                    byte d;
                    if(sch)
                    {
                        d = (byte)(extra[n = i * sY * sX + j * sX + k] & 0xf);
                    } else
                    {
                        d = extra[(n = i + j * sZ + k * sY * sZ) >> 1];
                        if((i & 1) == 0)
                            d &= 0xf;
                        else
                            d = (byte)((d & 0xf0) >> 4);
                    }
                    switch(data[n])
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
                        chunk[i][j][k] = (byte)Blocks.AIR.ordinal();
                        break;

                    case 55: 
                        cext[i][j][k] = d;
                        chunk[i][j][k] = (byte)Blocks.WIRE.ordinal();
                        break;

                    case 76: 
                        cext[i][j][k] = 16;
                        // fall through

                    case 75: 
                        ai = new int[6];
                        ai[1] = 128;
                        ai[2] = 96;
                        ai[3] = 64;
                        ai[4] = 32;
                        cext[i][j][k] += ai[d];
                        chunk[i][j][k] = (byte)Blocks.TORCH.ordinal();
                        break;

                    case 69: 
                        chunk[i][j][k] = (byte)Blocks.LEVER.ordinal();
                        if(d >= 8)
                        {
                            d -= 8;
                            cext[i][j][k] = 16;
                        }
                        ai1 = new int[7];
                        ai1[1] = 128;
                        ai1[2] = 96;
                        ai1[3] = 64;
                        ai1[4] = 32;
                        cext[i][j][k] += (byte)ai1[d];
                        break;

                    case 70: 
                    case 72: 
                        chunk[i][j][k] = (byte)Blocks.PRESS.ordinal();
                        cext[i][j][k] = (byte)(new int[] {
                            0, 10
                        })[d];
                        break;

                    case 77: 
                        chunk[i][j][k] = (byte)Blocks.BUTTON.ordinal();
                        if(d >= 8)
                        {
                            d -= 8;
                            cext[i][j][k] = 16;
                        }
                        cext[i][j][k] += (new int[] {
                            32, 128, 96, 64, 32
                        })[d];
                        break;

                    case 64: 
                    case 71: 
                        if(d < 8)
                        {
                            chunk[i][j][k] = (byte)Blocks.DOORA.ordinal();
                            if(d >= 4)
                            {
                                d -= 4;
                                cext[i][j][k] = 16;
                            }
                            cext[i][j][k] += (new int[] {
                                96, 64, 128, 32
                            })[d];
                        } else
                        {
                            chunk[i][j][k] = (byte)Blocks.DOORB.ordinal();
                            cext[i][j][k] = 0;
                        }
                        break;

                    case 8: 
                    case 10: 
                        cext[i][j][k] = 16;
                        

                    case 9: 
                    case 11: 
                        chunk[i][j][k] = (byte)Blocks.WATER.ordinal();
                        cext[i][j][k] += d;
                       

                    case 12: 
                    case 13: 
                        chunk[i][j][k] = (byte)Blocks.SAND.ordinal();
                        break;

                    default:
                        chunk[i][j][k] = (byte)Blocks.BLOCK.ordinal();
                        break;
                    }
                }

            }

        }

        p.field.data = chunk;
        p.field.extra = cext;
        p.setSize(sX, sY, sZ);
        p.recountRed();
        p.view.repaint();
    }

    public static void save(Viewport p, String file)
    {
        short sZ = (short)p.field.data.length;
        short sY = (short)p.field.data[0].length;
        short sX = (short)p.field.data[0][0].length;
        byte blocks[] = new byte[sX * sY * sZ];
        byte data[] = new byte[sX * sY * sZ];
        int i = 0;
        int n = 0;
        for(; i < sZ; i++)
        {
            for(int j = 0; j < sY; j++)
            {
                for(int k = 0; k < sX;)
                {
                    switch(p.field.g(k, j, i))
                    {
                    case SAND: 
                    default:
                        break;

                    case AIR: 
                    case SHADOW:
                        blocks[n] = data[n] = 0;
                        break;

                    case BLOCK: 
                        if(p.block == 2)
                            blocks[n] = (byte)(p.field.g(k, j, i + 1).block() ? 3 : 2);
                        else
                            blocks[n] = (byte)p.block;
                        data[n] = 0;
                        break;

                    case WIRE: 
                        blocks[n] = 55;
                        data[n] = p.field.extra[i][j][k];
                        break;

                    case TORCH: 
                        blocks[n] = (byte)(p.field.p(k, j, i) ? 76 : 75);
                        data[n] = (byte)(5 - (p.field.extra[i][j][k] >> 5 & 7));
                        break;

                    case BUTTON: 
                        blocks[n] = 77;
                        data[n] = (byte)((new byte[] {
                            0, 4, 3, 2, 1
                        })[p.field.extra[i][j][k] >> 5 & 7] + (p.field.p(k, j, i) ? 8 : 0));
                        break;

                    case LEVER: 
                        blocks[n] = 69;
                        data[n] = (byte)((new byte[] {
                            (byte)(Field.dummyGdValve ? 6 : 5), 4, 3, 2, 1
                        })[p.field.extra[i][j][k] >> 5 & 7] + (p.field.p(k, j, i) ? 8 : 0));
                        break;

                    case PRESS: 
                        blocks[n] = 70;
                        data[n] = (byte)(p.field.extra[i][j][k] != 0 ? 1 : 0);
                        break;

                    case DOORA: 
                        blocks[n] = 71;
                        data[n] = (byte)((new byte[] {
                            0, 3, 1, 0, 2
                        })[p.field.extra[i][j][k] >> 5 & 7] + (p.field.p(k, j, i) ? 4 : 0));
                        break;

                    case DOORB: 
                        blocks[n] = 71;
                        data[n] = (byte)(data[n - sX * sY] + 8);
                        break;

                    case WATER: 
                        blocks[n] = (byte)((p.field.extra[i][j][k] & 0x10) != 0 ? 8 : 9);
                        data[n] = (byte)(p.field.extra[i][j][k] & 0xf);
                        break;
                    }
                    k++;
                    n++;
                }

            }

        }

        try
        {
            (new Tag(Tag.Type.TAG_Compound, "Schematic", new Tag[] {
                new Tag(Tag.Type.TAG_Short, "Height", Short.valueOf(sZ)), new Tag(Tag.Type.TAG_Short, "Length", Short.valueOf(sY)), new Tag(Tag.Type.TAG_Short, "Width", Short.valueOf(sX)), new Tag(Tag.Type.TAG_Byte_Array, "Blocks", blocks), new Tag(Tag.Type.TAG_Byte_Array, "Data", data), new Tag(Tag.Type.TAG_String, "Materials", "Alpha"), new Tag("Entities", Tag.Type.TAG_Compound), new Tag("TileEntities", Tag.Type.TAG_Compound), new Tag(Tag.Type.TAG_End, null, null)
            })).writeTo(new FileOutputStream(file));
        }
        catch(Exception exception) { }
    }

   // public static void main(String args[])
   // {
    //    Viewport v = new Viewport(0, 0, 0);
     //   load(v, "bcd.schematic");
     //   v.stats.revalidate();
     //   v.view.revalidate();
     //   v.frame.pack();
     //   save(v, "bcd2.schematic");
   // }

}
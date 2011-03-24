// Decompiled by DJ v3.10.10.93 Copyright 2007 Atanas Neshkov  Date: 23-02-2011 22:37:49
// Home Page: http://members.fortunecity.com/neshkov/dj.html  http://www.neshkov.com/dj.html - Check often for new version!
// Decompiler options: packimports(3) 
// Source File Name:   LevelLoader.java

package com.carneiro.mcredsim;

import java.io.FileInputStream;
import java.io.FileOutputStream;
import javax.swing.JFrame;
import javax.swing.JPanel;

// Referenced classes of package com.carneiro.mcredsim:
//            Tag, Blocks, Viewport, Field

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
                    case 0: // '\0'
                    case 6: // '\006'
                    case 37: // '%'
                    case 38: // '&'
                    case 39: // '\''
                    case 40: // '('
                    case 51: // '3'
                    case 59: // ';'
                    case 63: // '?'
                    case 65: // 'A'
                    case 66: // 'B'
                    case 68: // 'D'
                    case 78: // 'N'
                    case 83: // 'S'
                        chunk[i][j][k] = (byte)Blocks.air.ordinal();
                        break;

                    case 55: // '7'
                        cext[i][j][k] = d;
                        chunk[i][j][k] = (byte)Blocks.wire.ordinal();
                        break;

                    case 76: // 'L'
                        cext[i][j][k] = 16;
                        // fall through

                    case 75: // 'K'
                        ai = new int[6];
                        ai[1] = 128;
                        ai[2] = 96;
                        ai[3] = 64;
                        ai[4] = 32;
                        cext[i][j][k] += ai[d];
                        chunk[i][j][k] = (byte)Blocks.torch.ordinal();
                        break;

                    case 69: // 'E'
                        chunk[i][j][k] = (byte)Blocks.lever.ordinal();
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

                    case 70: // 'F'
                    case 72: // 'H'
                        chunk[i][j][k] = (byte)Blocks.press.ordinal();
                        cext[i][j][k] = (byte)(new int[] {
                            0, 10
                        })[d];
                        break;

                    case 77: // 'M'
                        chunk[i][j][k] = (byte)Blocks.button.ordinal();
                        if(d >= 8)
                        {
                            d -= 8;
                            cext[i][j][k] = 16;
                        }
                        cext[i][j][k] += (new int[] {
                            32, 128, 96, 64, 32
                        })[d];
                        break;

                    case 64: // '@'
                    case 71: // 'G'
                        if(d < 8)
                        {
                            chunk[i][j][k] = (byte)Blocks.doorA.ordinal();
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
                            chunk[i][j][k] = (byte)Blocks.doorB.ordinal();
                            cext[i][j][k] = 0;
                        }
                        break;

                    case 8: // '\b'
                    case 10: // '\n'
                        cext[i][j][k] = 16;
                        // fall through

                    case 9: // '\t'
                    case 11: // '\013'
                        chunk[i][j][k] = (byte)Blocks.water.ordinal();
                        cext[i][j][k] += d;
                        // fall through

                    case 12: // '\f'
                    case 13: // '\r'
                        chunk[i][j][k] = (byte)Blocks.sand.ordinal();
                        break;

                    default:
                        chunk[i][j][k] = (byte)Blocks.block.ordinal();
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
                    switch($SWITCH_TABLE$com$carneiro$mcredsim$Blocks()[p.field.g(k, j, i).ordinal()])
                    {
                    case 10: // '\n'
                    default:
                        break;

                    case 1: // '\001'
                    case 12: // '\f'
                        blocks[n] = data[n] = 0;
                        break;

                    case 2: // '\002'
                        if(p.block == 2)
                            blocks[n] = (byte)(p.field.g(k, j, i + 1).block() ? 3 : 2);
                        else
                            blocks[n] = (byte)p.block;
                        data[n] = 0;
                        break;

                    case 3: // '\003'
                        blocks[n] = 55;
                        data[n] = p.field.extra[i][j][k];
                        break;

                    case 4: // '\004'
                        blocks[n] = (byte)(p.field.p(k, j, i) ? 76 : 75);
                        data[n] = (byte)(5 - (p.field.extra[i][j][k] >> 5 & 7));
                        break;

                    case 6: // '\006'
                        blocks[n] = 77;
                        data[n] = (byte)((new byte[] {
                            0, 4, 3, 2, 1
                        })[p.field.extra[i][j][k] >> 5 & 7] + (p.field.p(k, j, i) ? 8 : 0));
                        break;

                    case 5: // '\005'
                        blocks[n] = 69;
                        data[n] = (byte)((new byte[] {
                            (byte)(Field.dummyGdValve ? 6 : 5), 4, 3, 2, 1
                        })[p.field.extra[i][j][k] >> 5 & 7] + (p.field.p(k, j, i) ? 8 : 0));
                        break;

                    case 9: // '\t'
                        blocks[n] = 70;
                        data[n] = (byte)(p.field.extra[i][j][k] != 0 ? 1 : 0);
                        break;

                    case 7: // '\007'
                        blocks[n] = 71;
                        data[n] = (byte)((new byte[] {
                            0, 3, 1, 0, 2
                        })[p.field.extra[i][j][k] >> 5 & 7] + (p.field.p(k, j, i) ? 4 : 0));
                        break;

                    case 8: // '\b'
                        blocks[n] = 71;
                        data[n] = (byte)(data[n - sX * sY] + 8);
                        break;

                    case 11: // '\013'
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

    public static void main(String args[])
    {
        Viewport v = new Viewport(0, 0, 0);
        load(v, "C:\\Users\\Mario\\Minecraft\\bcd.schematic");
        v.stats.revalidate();
        v.view.revalidate();
        v.frame.pack();
        save(v, "C:\\Users\\Mario\\Minecraft\\bcd2.schematic");
    }

    static int[] $SWITCH_TABLE$com$carneiro$mcredsim$Blocks()
    {
        $SWITCH_TABLE$com$carneiro$mcredsim$Blocks;
        if($SWITCH_TABLE$com$carneiro$mcredsim$Blocks == null) goto _L2; else goto _L1
_L1:
        return;
_L2:
        JVM INSTR pop ;
        int ai[] = new int[Blocks.values().length];
        try
        {
            ai[Blocks.air.ordinal()] = 1;
        }
        catch(NoSuchFieldError _ex) { }
        try
        {
            ai[Blocks.block.ordinal()] = 2;
        }
        catch(NoSuchFieldError _ex) { }
        try
        {
            ai[Blocks.button.ordinal()] = 6;
        }
        catch(NoSuchFieldError _ex) { }
        try
        {
            ai[Blocks.doorA.ordinal()] = 7;
        }
        catch(NoSuchFieldError _ex) { }
        try
        {
            ai[Blocks.doorB.ordinal()] = 8;
        }
        catch(NoSuchFieldError _ex) { }
        try
        {
            ai[Blocks.lever.ordinal()] = 5;
        }
        catch(NoSuchFieldError _ex) { }
        try
        {
            ai[Blocks.press.ordinal()] = 9;
        }
        catch(NoSuchFieldError _ex) { }
        try
        {
            ai[Blocks.sand.ordinal()] = 10;
        }
        catch(NoSuchFieldError _ex) { }
        try
        {
            ai[Blocks.shadow.ordinal()] = 12;
        }
        catch(NoSuchFieldError _ex) { }
        try
        {
            ai[Blocks.torch.ordinal()] = 4;
        }
        catch(NoSuchFieldError _ex) { }
        try
        {
            ai[Blocks.water.ordinal()] = 11;
        }
        catch(NoSuchFieldError _ex) { }
        try
        {
            ai[Blocks.wire.ordinal()] = 3;
        }
        catch(NoSuchFieldError _ex) { }
        return $SWITCH_TABLE$com$carneiro$mcredsim$Blocks = ai;
    }

    private static int $SWITCH_TABLE$com$carneiro$mcredsim$Blocks[];
}
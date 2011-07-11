// Decompiled by DJ v3.10.10.93 Copyright 2007 Atanas Neshkov  Date: 23-02-2011 22:37:49
// Home Page: http://members.fortunecity.com/neshkov/dj.html  http://www.neshkov.com/dj.html - Check often for new version!
// Decompiler options: packimports(3) 
// Source File Name:   Colors.java

package com.carneiro.mcredsim;

import java.awt.Color;
import java.awt.image.IndexColorModel;

public class Colors
{

    public Colors()
    {
    }

    public static final Color air;
    public static final Color wireOn;
    public static final Color wireOff;
    public static final Color block;
    public static final Color cover;
    public static final Color fog;
    public static final Color aircover;
    public static final Color valve;
    public static final Color button;
    public static final Color door;
    public static final Color grid;
    public static final Color hilite = new Color(0xa0b19c);
    public static final Color copyFrom = new Color(0x3e88f9);
    public static final Color copyTo = new Color(0xfb6612);
    public static final Color dirt;
    public static final Color sand;
    public static final Color water;
    public static final Color tooltip = new Color(0xc0dddddd, true);
    public static final IndexColorModel icm;

    static 
    {
        air = Color.WHITE;
        wireOn = Color.RED;
        wireOff = new Color(0x800000);
        block = Color.YELLOW;
        cover = new Color(0x80808080, true);
        fog = new Color(0x40ffffff, true);
        aircover = new Color(0x60ffffff, true);
        valve = Color.GRAY;
        button = new Color(0x4d4e50);
        door = new Color(0x614226);
        grid = Color.GRAY;
        dirt = new Color(0x856043);
        sand = new Color(0xdbd371);
        water = new Color(0x2a5eff);
        Color trans[] = {
            fog, aircover, cover
        };
        Color cols[][] = {
            {
                new Color(0), air, wireOn, wireOff, block, valve, button, door, dirt, sand, 
                water
            }, {
                wireOn, wireOff, valve, button, door
            }, {
                wireOn, wireOff
            }, {
                air, wireOn, wireOff, block, button, door
            }
        };
        int m = 0;
        int p = 0;
        Color acolor[][];
        int l = (acolor = cols).length;
        for(int k = 0; k < l; k++)
        {
            Color c[] = acolor[k];
            m += c.length;
        }

        byte r[] = new byte[m];
        byte g[] = new byte[m];
        byte b[] = new byte[m];
        for(int j = 0; j < cols[0].length;)
        {
            r[p] = (byte)cols[0][j].getRed();
            g[p] = (byte)cols[0][j].getGreen();
            b[p] = (byte)cols[0][j].getBlue();
            j++;
            p++;
        }

        for(int i = 1; i < cols.length; i++)
        {
            double a = (double)trans[i - 1].getAlpha() / 255D;
            for(int j = 0; j < cols[i].length;)
            {
                r[p] = (byte)(int)(((double)cols[i][j].getRed() * (1.0D - a) + (double)trans[i - 1].getRed() * a) - 0.01D);
                g[p] = (byte)(int)(((double)cols[i][j].getGreen() * (1.0D - a) + (double)trans[i - 1].getGreen() * a) - 0.01D);
                b[p] = (byte)(int)(((double)cols[i][j].getBlue() * (1.0D - a) + (double)trans[i - 1].getBlue() * a) - 0.01D);
                j++;
                p++;
            }

        }

        int d = 0;
        for(int m2 = m - 1; m2 > 0;)
        {
            m2 >>= 1;
            d++;
        }

        icm = new IndexColorModel(d, m, r, g, b, 0);
    }
}
// Decompiled by DJ v3.10.10.93 Copyright 2007 Atanas Neshkov  Date: 23-02-2011 22:37:49
// Home Page: http://members.fortunecity.com/neshkov/dj.html  http://www.neshkov.com/dj.html - Check often for new version!
// Decompiler options: packimports(3) 
// Source File Name:   Palette.java

package com.carneiro.mcredsim;

import java.util.ArrayList;

// Referenced classes of package com.carneiro.mcredsim:
//            Blocks

public final class Palette extends Enum
{

    private Palette(String s, int i, Blocks _a)
    {
        this(s, i, _a, null, null);
    }

    private Palette(String s, int i, Blocks _a, Blocks _b)
    {
        this(s, i, _a, _b, null);
    }

    private Palette(String s, int i, Blocks _a, Blocks _b, Blocks _c)
    {
        super(s, i);
        a = _a;
        b = _b;
        c = _c;
    }

    public static Palette[] values()
    {
        Palette apalette[];
        int i;
        Palette apalette1[];
        System.arraycopy(apalette = ENUM$VALUES, 0, apalette1 = new Palette[i = apalette.length], 0, i);
        return apalette1;
    }

    public static Palette valueOf(String s)
    {
        return (Palette)Enum.valueOf(com/carneiro/mcredsim/Palette, s);
    }

    public static final Palette air;
    public static final Palette shadow;
    public static final Palette block;
    public static final Palette wire;
    public static final Palette torch;
    public static final Palette lever;
    public static final Palette button;
    public static final Palette press;
    public static final Palette sand;
    public static final Palette water;
    public static final Palette air2;
    public static final Palette shadow2;
    public static final Palette blockblock;
    public static final Palette blockwire;
    public static final Palette blocktorch;
    public static final Palette blocklever;
    public static final Palette blockpress;
    public static final Palette wireblock;
    public static final Palette torchblock;
    public static final Palette leverblock;
    public static final Palette wiretorch;
    public static final Palette door;
    public static final Palette bridge;
    public final Blocks a;
    public final Blocks b;
    public final Blocks c;
    public static Palette pal1[];
    public static Palette pal2[];
    public static Palette pal3[];
    public static final Palette wireP[];
    public static final Palette waterP[];
    private static final Palette ENUM$VALUES[];

    static 
    {
        air = new Palette("air", 0, Blocks.air);
        shadow = new Palette("shadow", 1, Blocks.shadow);
        block = new Palette("block", 2, Blocks.block);
        wire = new Palette("wire", 3, Blocks.wire);
        torch = new Palette("torch", 4, Blocks.torch);
        lever = new Palette("lever", 5, Blocks.lever);
        button = new Palette("button", 6, Blocks.button);
        press = new Palette("press", 7, Blocks.press);
        sand = new Palette("sand", 8, Blocks.sand);
        water = new Palette("water", 9, Blocks.water);
        air2 = new Palette("air2", 10, Blocks.air, Blocks.air);
        shadow2 = new Palette("shadow2", 11, Blocks.shadow, Blocks.shadow);
        blockblock = new Palette("blockblock", 12, Blocks.block, Blocks.block);
        blockwire = new Palette("blockwire", 13, Blocks.block, Blocks.wire);
        blocktorch = new Palette("blocktorch", 14, Blocks.block, Blocks.torch);
        blocklever = new Palette("blocklever", 15, Blocks.block, Blocks.lever);
        blockpress = new Palette("blockpress", 16, Blocks.block, Blocks.press);
        wireblock = new Palette("wireblock", 17, Blocks.wire, Blocks.block);
        torchblock = new Palette("torchblock", 18, Blocks.torch, Blocks.block);
        leverblock = new Palette("leverblock", 19, Blocks.lever, Blocks.block);
        wiretorch = new Palette("wiretorch", 20, Blocks.wire, Blocks.torch);
        door = new Palette("door", 21, Blocks.doorA, Blocks.doorB);
        bridge = new Palette("bridge", 22, Blocks.wire, Blocks.block, Blocks.wire);
        ENUM$VALUES = (new Palette[] {
            air, shadow, block, wire, torch, lever, button, press, sand, water, 
            air2, shadow2, blockblock, blockwire, blocktorch, blocklever, blockpress, wireblock, torchblock, leverblock, 
            wiretorch, door, bridge
        });
        waterP = (new Palette[] {
            air, block, sand, water, torch, shadow
        });
        wireP = (new Palette[] {
            air, air2, block, blockblock, wire, torch, blockwire, blocktorch, wireblock, torchblock, 
            wiretorch, bridge, lever, button, press, door, shadow, shadow2
        });
        ArrayList a = new ArrayList();
        ArrayList a2 = new ArrayList();
        ArrayList a3 = new ArrayList();
        for(int i = 0; i < wireP.length; i++)
        {
            Palette p = wireP[i];
            if(p.b == null)
                a.add(p);
            if(p.c == null && p != air && p != shadow)
                a2.add(p);
            if(p != air && p != shadow)
                a3.add(p);
        }

        pal1 = (Palette[])a.toArray(new Palette[0]);
        pal2 = (Palette[])a2.toArray(new Palette[0]);
        pal3 = (Palette[])a3.toArray(new Palette[0]);
    }
}
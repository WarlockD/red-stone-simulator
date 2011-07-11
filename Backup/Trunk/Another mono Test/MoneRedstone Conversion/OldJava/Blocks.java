// Decompiled by DJ v3.10.10.93 Copyright 2007 Atanas Neshkov  Date: 23-02-2011 22:37:49
// Home Page: http://members.fortunecity.com/neshkov/dj.html  http://www.neshkov.com/dj.html - Check often for new version!
// Decompiler options: packimports(3) 
// Source File Name:   Blocks.java

package com.carneiro.mcredsim;


public final class Blocks extends Enum
{

    private Blocks(String s1, int i, int w, boolean c, String s)
    {
        super(s1, i);
        wall = (byte)w;
        conn = c;
        name = s;
    }

    public boolean ctrl()
    {
        return this == lever || this == button || this == press;
    }

    public boolean block()
    {
        return this == block || this == sand;
    }

    public boolean air()
    {
        return this == air || this == shadow;
    }

    public boolean destruct()
    {
        return !block() && this != press && this != doorA && this != doorB && this != water;
    }

    public static Blocks[] values()
    {
        Blocks ablocks[];
        int i;
        Blocks ablocks1[];
        System.arraycopy(ablocks = ENUM$VALUES, 0, ablocks1 = new Blocks[i = ablocks.length], 0, i);
        return ablocks1;
    }

    public static Blocks valueOf(String s)
    {
        return (Blocks)Enum.valueOf(com/carneiro/mcredsim/Blocks, s);
    }

    public static final Blocks air;
    public static final Blocks block;
    public static final Blocks wire;
    public static final Blocks torch;
    public static final Blocks lever;
    public static final Blocks button;
    public static final Blocks doorA;
    public static final Blocks doorB;
    public static final Blocks press;
    public static final Blocks sand;
    public static final Blocks water;
    public static final Blocks shadow;
    public final byte wall;
    public final boolean conn;
    public final String name;
    private static final Blocks ENUM$VALUES[];

    static 
    {
        air = new Blocks("air", 0, 0, false, "air");
        block = new Blocks("block", 1, 0, false, "block");
        wire = new Blocks("wire", 2, 0, true, "wire");
        torch = new Blocks("torch", 3, 1, true, "torch");
        lever = new Blocks("lever", 4, 1, true, "switch");
        button = new Blocks("button", 5, 3, true, "button");
        doorA = new Blocks("doorA", 6, 2, true, "door");
        doorB = new Blocks("doorB", 7, 2, true, "door");
        press = new Blocks("press", 8, 0, true, "pressure pad");
        sand = new Blocks("sand", 9, 0, false, "sand");
        water = new Blocks("water", 10, 0, false, "water");
        shadow = new Blocks("shadow", 11, 0, false, "shadow");
        ENUM$VALUES = (new Blocks[] {
            air, block, wire, torch, lever, button, doorA, doorB, press, sand, 
            water, shadow
        });
    }
}

package mcredsim;

import java.util.ArrayList;


public enum Palette 
{
    air ( Blocks.AIR),
    shadow ( Blocks.SHADOW),
    block ( Blocks.BLOCK),
    wire ( Blocks.WIRE),
    torch ( Blocks.TORCH),
    lever ( Blocks.LEVER),
    button ( Blocks.BUTTON),
    press ( Blocks.PRESS),
    sand ( Blocks.SAND),
    water ( Blocks.WATER),
    air2 (Blocks.AIR, Blocks.AIR),
    shadow2 ( Blocks.SHADOW, Blocks.SHADOW),
    blockblock ( Blocks.BLOCK, Blocks.BLOCK),
    blockwire ( Blocks.BLOCK, Blocks.WIRE),
    blocktorch ( Blocks.BLOCK, Blocks.TORCH),
    blocklever (  Blocks.BLOCK, Blocks.LEVER),
    blockpress ( Blocks.BLOCK, Blocks.PRESS),
    wireblock ( Blocks.WIRE, Blocks.BLOCK),
    torchblock ( Blocks.TORCH, Blocks.BLOCK),
    leverblock ( Blocks.LEVER, Blocks.BLOCK),
    wiretorch ( Blocks.WIRE, Blocks.TORCH),
    door ( Blocks.DOORA, Blocks.DOORB),
    bridge ( Blocks.WIRE, Blocks.BLOCK, Blocks.WIRE);
    
    private Palette( Blocks _a)
    {
        this( _a, null, null);
    }

    private Palette( Blocks _a, Blocks _b)
    {
        this( _a, _b, null);
    }

    private Palette( Blocks _a, Blocks _b, Blocks _c)
    {
        a = _a;
        b = _b;
        c = _c;
    }


    public  Blocks a;
    public  Blocks b;
    public  Blocks c;
    public static Palette pal1[];
    public static Palette pal2[];
    public static Palette pal3[];
    public static final Palette wireP[] = new Palette[] {
            air, air2, block, blockblock, wire, torch, blockwire, blocktorch, wireblock, torchblock, 
            wiretorch, bridge, lever, button, press, door, shadow, shadow2
        };
    public static final Palette waterP[] = new Palette[] {
            air, block, sand, water, torch, shadow
        };
    

    static 
    {
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
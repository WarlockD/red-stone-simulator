
package mcredsim;


public enum Blocks
{
    AIR ( 0, false, "air"),
    BLOCK (  0, false, "block"),
    WIRE ( 0, true, "wire"),
    TORCH ( 1, true, "torch"),
    LEVER ( 1, true, "switch"),
    BUTTON ( 3, true, "button"),
    DOORA ( 2, true, "door"),
    DOORB ( 2, true, "door"),
    PRESS ( 0, true, "pressure pad"),
    SHADOW ( 0, false, "shadow"),
    REPEATER (1, false, "repeater");
    
    private Blocks(int w, boolean c, String s)
    {
        wall = (byte)w;
        conn = c;
        name = s;
    }
    public boolean repeater()
    {
        return this == REPEATER;
    }
    public boolean ctrl()
    {
        return this == LEVER || this == BUTTON || this == PRESS || this == REPEATER;
    }

    public boolean block()
    {
        return this == BLOCK ;
    }

    public boolean air()
    {
        return this == AIR || this == SHADOW;
    }

    public boolean destruct()
    {
        return !block() && this != PRESS && this != DOORA && this != DOORB ;
    }
    public byte wall;
    public boolean conn;
    public String name;
   
}

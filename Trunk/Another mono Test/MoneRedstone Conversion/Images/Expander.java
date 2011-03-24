// Decompiled by DJ v3.10.10.93 Copyright 2007 Atanas Neshkov  Date: 23-02-2011 22:37:49
// Home Page: http://members.fortunecity.com/neshkov/dj.html  http://www.neshkov.com/dj.html - Check often for new version!
// Decompiler options: packimports(3) 
// Source File Name:   Expander.java

package com.carneiro.mcredsim;

import java.awt.event.ActionEvent;
import javax.swing.*;

// Referenced classes of package com.carneiro.mcredsim:
//            Viewport, Field, Blocks

public class Expander extends AbstractAction
{

    public Expander(Viewport parent, boolean expand, boolean top, int dimension)
    {
        p = parent;
        ex = expand;
        T = top;
        D = dimension;
    }

    public void actionPerformed(ActionEvent e)
    {
        int newSz[] = {
            p.z, p.y, p.x
        };
        newSz[D] += ex ? 1 : -1;
        if(newSz[0] * newSz[1] * newSz[2] == 0)
            return;
        if(!ex && (e.getModifiers() & 1) == 0)
        {
            int z = 0;
            if(T && D == 0)
                z = p.z - 1;
label0:
            do
            {
                int y = 0;
                if(T && D == 1)
                    y = p.y - 1;
                do
                {
                    int x = 0;
                    if(T && D == 2)
                        x = p.x - 1;
                    do
                    {
                        if(p.field.g(x, y, z) != (z >= p.gd ? Blocks.air : Blocks.block))
                        {
                            if(JOptionPane.showConfirmDialog(null, "The plane you are removing has components on it.\nAre you sure you want to remove it? (hold Shift to skip)", "Deleting data", 0) != 0)
                                return;
                            break label0;
                        }
                        x++;
                    } while(D != 2 && x < p.x);
                    y++;
                } while(D != 1 && y < p.y);
                z++;
            } while(D != 0 && z < p.z);
        }
        if(D == 0)
            if(!T)
            {
                p.gd += ex ? 1 : -1;
                p.setLyr(p.lyr + (ex ? 1 : -1));
            } else
            if(p.lyr >= newSz[0])
                p.setLyr(newSz[0] - 1);
        byte newDat[][][] = new byte[newSz[0]][][];
        byte newExt[][][] = new byte[newSz[0]][][];
        if(D == 0)
        {
            System.arraycopy(p.field.data, !ex && !T ? 1 : 0, newDat, !ex || T ? 0 : 1, p.z - (ex ? 0 : 1));
            System.arraycopy(p.field.extra, !ex && !T ? 1 : 0, newExt, !ex || T ? 0 : 1, p.z - (ex ? 0 : 1));
            if(ex)
            {
                newDat[T ? p.z : 0] = new byte[p.y][];
                newExt[T ? p.z : 0] = new byte[p.y][];
                for(int y = 0; y < p.y; y++)
                {
                    newDat[T ? p.z : 0][y] = new byte[p.x];
                    newExt[T ? p.z : 0][y] = new byte[p.x];
                    for(int x = 0; x < p.x; x++)
                        if(p.gd > (T ? p.z : 0))
                            newDat[T ? p.z : 0][y][x] = (byte)Blocks.block.ordinal();

                }

            }
        } else
        {
            for(int z = 0; z < p.z; z++)
            {
                newDat[z] = new byte[newSz[1]][];
                newExt[z] = new byte[newSz[1]][];
                if(D == 1)
                {
                    System.arraycopy(p.field.data[z], !ex && !T ? 1 : 0, newDat[z], !ex || T ? 0 : 1, p.y - (ex ? 0 : 1));
                    System.arraycopy(p.field.extra[z], !ex && !T ? 1 : 0, newExt[z], !ex || T ? 0 : 1, p.y - (ex ? 0 : 1));
                    if(ex)
                    {
                        newDat[z][T ? p.y : 0] = new byte[p.x];
                        newExt[z][T ? p.y : 0] = new byte[p.x];
                        for(int x = 0; x < p.x; x++)
                            if(p.gd > z)
                                newDat[z][T ? p.y : 0][x] = (byte)Blocks.block.ordinal();

                    }
                } else
                {
                    for(int y = 0; y < p.y; y++)
                    {
                        newDat[z][y] = new byte[newSz[2]];
                        newExt[z][y] = new byte[newSz[2]];
                        if(D == 2)
                        {
                            System.arraycopy(p.field.data[z][y], !ex && !T ? 1 : 0, newDat[z][y], !ex || T ? 0 : 1, p.x - (ex ? 0 : 1));
                            System.arraycopy(p.field.extra[z][y], !ex && !T ? 1 : 0, newExt[z][y], !ex || T ? 0 : 1, p.x - (ex ? 0 : 1));
                            if(ex && p.gd > z)
                                newDat[z][y][T ? p.x : 0] = (byte)Blocks.block.ordinal();
                        }
                    }

                }
            }

        }
        p.field.data = newDat;
        p.field.extra = newExt;
        p.setSize(newSz[2], newSz[1], newSz[0]);
        p.recountRed();
        p.modify();
        p.view.repaint();
    }

    public Viewport p;
    public boolean ex;
    public boolean T;
    public int D;
}
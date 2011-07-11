
package mcredsim;


import java.awt.*;
import java.awt.event.*;
import java.awt.image.*;
import java.io.File;
//import java.io.PrintStream;
import java.net.URL;
import java.util.ArrayList;
//import java.util.Iterator;
import javax.imageio.*;
import javax.imageio.metadata.*;
import javax.imageio.stream.ImageOutputStream;
import javax.sound.sampled.AudioSystem;
import javax.sound.sampled.Clip;
import javax.swing.*;
import javax.swing.filechooser.FileFilter;
import org.w3c.dom.Node;


public class Viewport
    implements MouseWheelListener
{
    public class ButtonAction extends AbstractAction
    {

        @Override
         public void actionPerformed(ActionEvent e)        
        {
            doAction();
        }

        private void doAction()
        {
            switch(vers)
            {
            default:
                break;

            case 0: 
                if(!areYouSure())
                    return;
                modified = false;
                save = null;
                field = new Field(Viewport.this, 30, 20, 7);
                setSize(30, 20, 7);
                setLyr(0);
                recountRed();
                for(int i = 0; i < palArr.length; i++)
                    if(palArr[i] == Palette.wire)
                        pal = i;

                view.repaint();
                pView.repaint();
                updateTooltip();
                updateTitle();
                break;

            case 3: 
                if(!areYouSure())
                    return;
                saveOpenDialog(true, false);
                break;

            case 2: 
                if(save != null)
                {
                    save();
                    break;
                }
              

            case 15: 
                saveOpenDialog(false, false);
                break;

            case 16: 
                saveOpenDialog(false, true);
                break;

            case 1: 
                cloneMode = 1;
                clone = new int[6];
                break;

            case 4: 
                if(playing)
                    break;
               

            case 5: 
                field.tick();
                view.repaint();
                break;

            case 6: 
                playToggle();
                break;

            case 7: 
                lyr += 2;
                

            case 8: 
                setLyr(lyr - 1);
                view.repaint();
                updateTooltip();
                break;

            case 9: 
                adjF.setVisible(true);
                break;

            case 10: 
                optF.setVisible(true);
                break;

            case 11: 
                cloneMode = 0;
                clone = null;
                view.repaint();
                break;

            case 12:
                addScale(1);
                break;

            case 13: 
                addScale(-1);
                break;

            case 14: 
                if(areYouSure())
                    System.exit(0);
                break;
            }
        }

        public static final int NEW = 0;
        public static final int CLONE = 1;
        public static final int SAVE = 2;
        public static final int OPEN = 3;
        public static final int TICK = 4;
        public static final int TICKALL = 5;
        public static final int PLAYPAUSE = 6;
        public static final int LYR_UP = 7;
        public static final int LYR_DN = 8;
        public static final int ADJUST = 9;
        public static final int OPT = 10;
        public static final int CLONE_ESC = 11;
        public static final int ZOOM_IN = 12;
        public static final int ZOOM_OUT = 13;
        public static final int EXIT = 14;
        public static final int SAVEAS = 15;
        public static final int GIF = 16;
        public int vers;

        public ButtonAction(int v)
        {
            vers = v;
        }
    }

    public class LAFActionListener
        implements ActionListener
    {

        @Override
        public void actionPerformed(ActionEvent e)
        {
            try
            {
                UIManager.setLookAndFeel(laf);
                SwingUtilities.updateComponentTreeUI(frame);
                SwingUtilities.updateComponentTreeUI(optF);
                SwingUtilities.updateComponentTreeUI(adjF);
                optF.pack();
                adjF.pack();
            }
            catch(Exception ex)
            {
                ((Component)e.getSource()).setEnabled(false);
            }
        }

        String laf;

        public LAFActionListener(String s)
        {
            laf = s;
        }
    }

    public class MissingIcon
        implements Icon
    {

        @Override
        public void paintIcon(Component c, Graphics g, int x, int y)
        {
            Graphics2D g2d = (Graphics2D)g.create();
            g2d.setColor(Color.WHITE);
            g2d.fillRect(x + 1, y + 1, width - 2, height - 2);
            g2d.setColor(Color.BLACK);
            g2d.drawRect(x + 1, y + 1, width - 2, height - 2);
            g2d.setColor(Color.RED);
            g2d.setStroke(stroke);
            double fac = 0.34999999999999998D;
            int wf = (int)((double)width * fac);
            int hf = (int)((double)height * fac);
            g2d.drawLine(x + wf, y + hf, (x + width) - wf, (y + height) - hf);
            g2d.drawLine(x + wf, (y + height) - hf, (x + width) - wf, y + hf);
            g2d.dispose();
        }

        @Override
        public int getIconWidth()
        {
            return width;
        }

        @Override
        public int getIconHeight()
        {
            return height;
        }

        private int width;
        private int height;
        private BasicStroke stroke;

        public MissingIcon(int w, int h)
        {
            stroke = new BasicStroke(4F);
            width = w;
            height = h;
        }
    }

    public class OptionAction
        implements ItemListener
    {

        @Override
        public void itemStateChanged(ItemEvent e)
        {
            switch(vers)
            {
            default:
                break;

            case 0: 
                Field.cyclic = e.getStateChange() == 1;
                field.update();
                break;

            case 1: 
                Field.MCwires = e.getStateChange() == 2;
                break;

            case 2: 
                Field.dummyGdValve = e.getStateChange() == 2;
                field.update();
                break;

            case 3: 
                if(e.getStateChange() == 1)
                {
                    c3Lyr.setSelected(false);
                    cBridge.setEnabled(false);
                    cWater.setSelected(false);
                    Field.layers = 1;
                    setPalette(Palette.pal1);
                } else
                {
                    Field.layers = 2;
                    setPalette(Palette.pal2);
                }
                break;

            case 4: 
                if(e.getStateChange() == 1)
                {
                    c1Lyr.setSelected(false);
                    cBridge.setEnabled(true);
                    cWater.setSelected(false);
                    Field.layers = 3;
                    setPalette(Palette.pal3);
                } else
                {
                    cBridge.setEnabled(false);
                    Field.layers = 2;
                    setPalette(Palette.pal2);
                }
                break;

            case 5: 
                Field.bridge = e.getStateChange() == 1;
                break;

            case 6: 
                if(Viewport.waterMode = e.getStateChange() == 1)
                {
                    c1Lyr.setSelected(false);
                    c3Lyr.setSelected(false);
                    cBridge.setEnabled(false);
                    Field.layers = 1;
                    setPalette(Palette.waterP);
                } else
                {
                    c1Lyr.setSelected(true);
                }
                break;
            }
            view.repaint();
        }

        public static final int CYCLIC = 0;
        public static final int NEW_WIRE = 1;
        public static final int DUMMY_SW = 2;
        public static final int LAYER1 = 3;
        public static final int LAYER3 = 4;
        public static final int BRIDGE = 5;
        public static final int WATER = 6;
        public int vers;

        public OptionAction(int v)
        {

            vers = v;
        }
    }

    public static class StatusBar extends JPanel
    {

        public JSeparator addSeparator()
        {
            
            JPanel p = new JPanel(new BorderLayout());
            JSeparator s = new JSeparator(1);
            p.setMaximumSize(new Dimension(2, 0x7fffffff));
            p.setBorder(BorderFactory.createEmptyBorder(1, 0, 1, 0));
            p.add(s);
            add(p);
            return s;
        }

        @Override
        public Component add(Component comp)
        {
            Component c = super.add(comp);
            super.add(Box.createHorizontalStrut(5));
            return c;
        }

        public void addGlue()
        {
            remove(getComponentCount() - 1);
            super.add(Box.createHorizontalGlue());
        }

        public StatusBar()
        {
            setLayout(new BoxLayout(this, 2));
            setPreferredSize(new Dimension(getPreferredSize().width, 23));
            Color c = getBackground();
            if(c == null)
                c = new Color(0xdddddd);
            setBorder(BorderFactory.createCompoundBorder(BorderFactory.createMatteBorder(1, 0, 0, 0, c.darker().darker()), BorderFactory.createLineBorder(c.brighter())));
            super.add(Box.createHorizontalStrut(5));
        }
    }


    private Icon getImage(String loc, int s, String desc)
    {
        URL u = getClass().getResource((new StringBuilder("/images/")).append(loc).append(s).append(".gif").toString());
        if(u == null)
            return new MissingIcon(s, s);
        else
            return new ImageIcon(u, desc);
    }

    private URL getSound(String loc)
    {
        return getClass().getResource((new StringBuilder("/sound/")).append(loc).toString());
    }

    private void addCtrl(JPanel p, JComponent c)
    {
        c.setAlignmentX(0.5F);
        p.add(c);
    }

    private JButton addBtn(JPanel p, String img, String t, ActionListener a)
    {
        JButton b = new JButton(t);
        b.setMargin(new Insets(0, 0, 0, 0));
        b.setPreferredSize(new Dimension(40, 20));
        b.setMaximumSize(new Dimension(40, 20));
        b.addActionListener(a);
        addCtrl(p, b);
        return b;
    }

    private void addAdjCol(JPanel p, String s, String b1, String b2, boolean e1, boolean e2, int e3)
    {
        JPanel p2 = new JPanel();
        p2.setLayout(new BoxLayout(p2, 3));
        JLabel l = new JLabel(s);
        l.setAlignmentX(0.5F);
        Dimension d = l.getPreferredSize();
        l.setMaximumSize(new Dimension(d.width + 1, d.height));
        p2.add(l);
        p2.add(Box.createRigidArea(new Dimension(5, 5)));
        addBtn(p2, (new StringBuilder(String.valueOf(e1 ? "Grow" : "Shrink"))).append(s).toString(), b1, new Expander(this, e1, e2, e3));
        p2.add(Box.createRigidArea(new Dimension(5, 5)));
        addBtn(p2, (new StringBuilder(String.valueOf(e1 ? "Shrink" : "Grow"))).append(s).toString(), b2, new Expander(this, !e1, e2, e3));
        p.add(p2);
        p.add(Box.createRigidArea(new Dimension(5, 5)));
    }

    public void setSize(int _x, int _y, int _z)
    {
        x = _x;
        y = _y;
        z = _z;
        lSz.setText((new StringBuilder(String.valueOf(x))).append("x").append(y).append("x").append(z).toString());
        Dimension d = new Dimension((x * 9 + 1) * scale, (y * 9 + 1) * scale);
        view.setPreferredSize(d);
        view.revalidate();
    }

    public void addScale(int ds)
    {
        scale += ds;
        if(scale < 1)
            scale = 1;
        Dimension d = new Dimension((x * 9 + 1) * scale, (y * 9 + 1) * scale);
        view.setPreferredSize(d);
        view.revalidate();
        view.repaint();
    }

    public void setLyr(int l)
    {
        if(l < 0)
            l = 0;
        if(l >= z)
            l = z - 1;
        lyr = l;
        lLyr.setText((new StringBuilder("Layer ")).append(lyr + 1).toString());
        if(lyr < gd)
            lLyr.setForeground(Colors.dirt);
        else
            lLyr.setForeground(Color.BLUE.darker());
        if(clone != null)
            if(clone.length == 6)
                setClone(clone[3], clone[4], lyr);
            else
            if(clone.length == 9)
                setClone(clone[6], clone[7], lyr);
    }

    public final void recountRed()
    {
        field.wires = field.torches = 0;
        for(int i = 0; i < z; i++)
        {
            for(int j = 0; j < y; j++)
            {
                for(int k = 0; k < x; k++)
                    if(field.g(k, j, i) == Blocks.WIRE)
                        field.wires++;
                    else
                    if(field.g(k, j, i) == Blocks.TORCH)
                        field.torches++;

            }

        }

        updateRed();
    }

    public final void updateRed()
    {
        lRed.setText((new StringBuilder(String.valueOf(field.wires))).toString());
        lTorch.setText((new StringBuilder(String.valueOf(field.torches))).toString());
        lTot.setText((new StringBuilder(String.valueOf(field.wires + field.torches))).toString());
        stats.revalidate();
    }

    private int[] findDoor(int x, int y, int z)
    {
        for(int z2 = z - 1; z2 < z + 2; z2++)
        {
            if(field.g(x - 1, y, z2) == Blocks.DOORA)
                return (new int[] {
                    x - 1, y, z2, 1
                });
            if(field.g(x + 1, y, z2) == Blocks.DOORA)
                return (new int[] {
                    x + 1, y, z2, 2
                });
            if(field.g(x, y - 1, z2) == Blocks.DOORA)
                return (new int[] {
                    x, y - 1, z2, 3
                });
            if(field.g(x, y + 1, z2) == Blocks.DOORA)
                return (new int[] {
                    x, y + 1, z2, 4
                });
        }

        return null;
    }

    private void place(int x, int y, int z, Palette pal)
    {
        if(field.match(x, y, z, pal) && pal != Palette.water)
            return;
        if(pal == Palette.door && z == this.z - 1)
            return;
        if(pal.a.wall == 3 && !field.g(x, y + 1, z).block() && !field.g(x, y - 1, z).block() && !field.g(x + 1, y, z).block() && !field.g(x - 1, y, z).block())
            return;
        modify();
        field.s(x, y, z, pal.a);
        if(Field.layers > 1)
        {
            if(pal.b != null)
                field.s(x, y, z + 1, pal.b);
            if(Field.layers > 2 && pal.c != null)
                field.s(x, y, z + 2, pal.c);
        }
        if(pal.a == Blocks.WIRE || pal.a == Blocks.PRESS)
            field.s(x, y, z - 1, Blocks.BLOCK);
        int d[];
        if(pal == Palette.water)
            field.sp(x, y, z, 16);
        else
        if(pal == Palette.door && (d = findDoor(x, y, z)) != null && field.w(d[0], d[1], d[2]) == d[3])
        {
            field.s(x, y, z, 4 - field.w(d[0], d[1], d[2]));
            field.s(x, y, z + 1, 1);
        } else
        {
            int p = z + 1;
            if(field.g(x, y, z).wall != 0)
                p--;
            if(p == z || field.g(x, y, z + 1).wall != 0)
            {
                int w = 0;
                if(field.g(x, y, p) == Blocks.TORCH)
                    field.sp(x, y, p, 16);
                do
                    w = ++w % 5;
                while(!field.s(x, y, p, w));
                if(field.w(x, y, p) == 0 || field.g(x, y, z).wall == 2)
                    field.s(x, y, p - 1, Blocks.BLOCK);
                if(pal == Palette.door)
                    field.s(x, y, p + 1, 1);
            }
        }
        field.update();
        view.repaint();
    }

    public Viewport(int _x, int _y, int _z)
    {
        block = 2;
        palArr = Palette.pal3;
        scale = 3;
        pScale = 5;
        lyr = 0;
        gd = 0;
        cloneMode = 0;
        clone = null;
        save = null;
        folder = new File("~");
        playing = false;
        isCtrlDown = false;
        modified = false;
        x = _x;
        y = _y;
        z = _z;
        field = new Field(this, x, y, z);
        init();
    }

    public Viewport(byte d[][][], byte e[][][])
    {
        block = 2;
        palArr = Palette.pal3;
        scale = 3;
        pScale = 5;
        lyr = 0;
        gd = 0;
        cloneMode = 0;
        clone = null;
        save = null;
        folder = new File("~");
        playing = false;
        isCtrlDown = false;
        modified = false;
        x = d[0][0].length;
        y = d[0].length;
        z = d.length;
        field = new Field(this, d, e);
        init();
        recountRed();
        field.update();
    }

    private void init()
    {
        init_setupFrame();
        init_buildAdjust();
        init_buildOptions();
        frame.setContentPane(init_buildMenuBar(init_buildStatusBar(init_buildToolbar(init_buildPView(init_buildView())))));
        init_doKeyBindings();
        setSize(x, y, z);
        frame.pack();
        frame.setLocationByPlatform(true);
        view.requestFocusInWindow();
        frame.setVisible(true);
    }

    private void init_setupFrame()
    {
        for(int i = 0; i < palArr.length; i++)
            if(palArr[i] == Palette.wire)
                pal = i;

        frame = new JFrame(title);
        URL u = getClass().getResource("/images/Logo16.png");
        if(u != null)
        {
            Image im = (new ImageIcon(u, "")).getImage();
            ArrayList al = new ArrayList();
            al.add(im);
            for(int i = 16; i < 256; i *= 2)
                al.add(im.getScaledInstance(i, i, 8));

            frame.setIconImages(al);
        }
        frame.setDefaultCloseOperation(0);
        frame.addWindowListener(new WindowAdapter() {

            @Override
            public void windowClosing(WindowEvent e)
            {
                (new ButtonAction(14)).actionPerformed(null);
            }

           
        }
);
    }

    private void init_buildAdjust()
    {
        adjF = new JDialog(frame, "Adjust Size");
        JPanel p = new JPanel();
        p.setLayout(new BoxLayout(p, 2));
        p.setBorder(BorderFactory.createEmptyBorder(5, 5, 5, 0));
        addAdjCol(p, "Top", "\u2191", "\u2193", true, false, 1);
        addAdjCol(p, "Bottom", "\u2191", "\u2193", false, true, 1);
        addAdjCol(p, "Left", "\u2192", "\u2190", false, false, 2);
        addAdjCol(p, "Right", "\u2192", "\u2190", true, true, 2);
        addAdjCol(p, "Front", "\u2299", "\u2295", true, true, 0);
        addAdjCol(p, "Back", "\u2299", "\u2295", false, false, 0);
        adjF.setContentPane(p);
        adjF.setDefaultCloseOperation(1);
        adjF.pack();
        adjF.setLocationByPlatform(true);
    }

    private void init_buildOptions()
    {
        optF = new JDialog(frame, "Options");
        JPanel p = new JPanel();
        p.setLayout(new BoxLayout(p, 3));
        p.setBorder(BorderFactory.createEmptyBorder(5, 5, 5, 5));
        JCheckBox c;
        p.add(c = new JCheckBox("Cyclic (in X and Z only) "));
        c.addItemListener(new OptionAction(0));
        p.add(c = new JCheckBox("\"Natural\" wire connections "));
        c.addItemListener(new OptionAction(1));
        p.add(c = new JCheckBox("Ground switches power blocks below "));
        c.setSelected(true);
        c.addItemListener(new OptionAction(2));
        p.add(c1Lyr = new JCheckBox("Show only one layer "));
        c1Lyr.addItemListener(new OptionAction(3));
        p.add(c3Lyr = new JCheckBox("Show three layers "));
        c3Lyr.setSelected(true);
        c3Lyr.addItemListener(new OptionAction(4));
        p.add(cBridge = new JCheckBox("Show bridges "));
        cBridge.setSelected(true);
        cBridge.addItemListener(new OptionAction(5));
        String bS[] = new String[blockNames.length];
        for(int i = 0; i < bS.length; i++)
            bS[i] = (String)blockNames[i][0];

        JComboBox cb;
        p.add(cb = new JComboBox(bS));
        cb.setAlignmentX(0.0F);
        cb.setSelectedIndex(1);
        cb.addActionListener(new ActionListener() {

            @Override
            public void actionPerformed(ActionEvent e)
            {
                block = ((Integer)Viewport.blockNames[((JComboBox)e.getSource()).getSelectedIndex()][1]).intValue();
            }

          
        }
);
        cWater = new JCheckBox("Water circuit mode ");
        cWater.addItemListener(new OptionAction(6));
        optF.setContentPane(p);
        optF.setDefaultCloseOperation(1);
        optF.setResizable(false);
        optF.pack();
        optF.setLocationByPlatform(true);
    }

    
    private JScrollPane init_buildView()
    {
        view = new JPanel(null) {

            @Override
            protected void paintComponent(Graphics g)
            {
                super.paintComponent(g);
                paintView(g);
            }

           

            
        }
;
        view.addMouseMotionListener(new MouseMotionListener() {

            @Override
            public void mouseMoved(MouseEvent e)
            {
                viewMouseMoved(e, false);
            }

            @Override
            public void mouseDragged(MouseEvent e)
            {
                viewMouseMoved(e, true);
            }

          

            
           
        }
);
        view.addMouseListener(new MouseListener() {

            @Override
            public void mouseReleased(MouseEvent mouseevent)
            {
            }

            @Override
            public void mousePressed(MouseEvent e)
            {
                viewMousePressed(e);
            }

            @Override
            public void mouseExited(MouseEvent e)
            {
                lastPX = lastPY = -1;
                updateTooltip();
            }

            @Override
            public void mouseEntered(MouseEvent mouseevent)
            {
            }

            @Override
            public void mouseClicked(MouseEvent mouseevent)
            {
            }

            
        }
);
        frame.addComponentListener(new ComponentListener() {

            @Override
            public void componentShown(ComponentEvent componentevent)
            {
            }

            @Override
            public void componentMoved(ComponentEvent componentevent)
            {
            }

            @Override
            public void componentHidden(ComponentEvent componentevent)
            {
            }

            @Override
            public void componentResized(ComponentEvent e)
            {
                frameResized();
            }
        }
);
        view.add(tooltip = new JLabel(""));
        tooltip.setVisible(false);
        tooltip.setOpaque(true);
        tooltip.setBackground(Colors.tooltip);
        JScrollPane sp = new JScrollPane(view);
        view.setFocusable(true);
        view.setFocusTraversalKeysEnabled(false);
        sp.setAlignmentX(0.5F);
        sp.setWheelScrollingEnabled(false);
        frame.addMouseWheelListener(this);
        sp.addMouseWheelListener(this);
        return sp;
    }

    private void init_doKeyBindings()
    {
        InputMap im = view.getInputMap(0);
        im.put(KeyStroke.getKeyStroke("UP"), "up");
        im.put(KeyStroke.getKeyStroke("DOWN"), "down");
        im.put(KeyStroke.getKeyStroke("PERIOD"), "tick");
        im.put(KeyStroke.getKeyStroke("ctrl PERIOD"), "tick");
        im.put(KeyStroke.getKeyStroke("ENTER"), "pause");
        im.put(KeyStroke.getKeyStroke("ctrl ENTER"), "pause");
        im.put(KeyStroke.getKeyStroke("ESCAPE"), "esc");
        im.put(KeyStroke.getKeyStroke("CONTROL"), "cDown");
        im.put(KeyStroke.getKeyStroke("released CONTROL"), "cUp");
        view.getActionMap().put("up", new ButtonAction(7));
        view.getActionMap().put("down", new ButtonAction(8));
        view.getActionMap().put("tick", new ButtonAction(4));
        view.getActionMap().put("pause", new ButtonAction(6));
        view.getActionMap().put("esc", new ButtonAction(11));
        view.getActionMap().put("cDown", new AbstractAction() {

            @Override
            public void actionPerformed(ActionEvent e)
            {
                isCtrlDown = true;
                updateTooltip();
                System.out.println("down");
            }

        }
);
        view.getActionMap().put("cUp", new AbstractAction() {

            public void actionPerformed(ActionEvent e)
            {
                isCtrlDown = false;
                updateTooltip();
            }
        }
);
    }

    private JPanel init_buildPView(JScrollPane sp)
    {
        pView = new JPanel() {

            @Override
            protected void paintComponent(Graphics g)
            {
                super.paintComponent(g);
                paintPView(g);
            }
        }
;
        Dimension d = new Dimension((palArr.length * 9 + 1) * pScale, 10 * pScale);
        pView.setPreferredSize(d);
        pView.addMouseListener(new MouseListener() {

            @Override
            public void mouseReleased(MouseEvent mouseevent)
            {
            }

            @Override
            public void mousePressed(MouseEvent e)
            {
                pViewMousePressed(e);
            }

            @Override
            public void mouseExited(MouseEvent mouseevent)
            {
            }

            @Override
            public void mouseEntered(MouseEvent mouseevent)
            {
            }

            @Override
            public void mouseClicked(MouseEvent mouseevent)
            {
            }
        }
);
        JPanel main = new JPanel();
        GroupLayout layout = new GroupLayout(main);
        main.setLayout(layout);
        pView.setAlignmentX(0.5F);
        JLabel lC = new JLabel();
        JLabel lD = new JLabel();
        layout.setHorizontalGroup(layout.createParallelGroup().addComponent(sp, 0, -2, 32767).addGroup(layout.createSequentialGroup().addComponent(lC, 0, 0, 32767).addComponent(pView, -2, -2, -2).addComponent(lD, 0, 0, 32767)));
        layout.setVerticalGroup(layout.createSequentialGroup().addComponent(sp, 0, -2, 32767).addGroup(layout.createParallelGroup().addComponent(lC, 0, 0, 0).addComponent(pView, -2, -2, -2).addComponent(lD, 0, 0, 0)));
        return main;
    }

    private JPanel init_buildToolbar(JPanel main)
    {
        JPanel p = new JPanel(new BorderLayout());
        JToolBar tb = new JToolBar("Tools");
        tb.setRollover(true);
        addButton(tb, "New", "New", 0);
        addButton(tb, "Open", "Open", 3);
        addButton(tb, "Save", "Save", 2);
        tb.addSeparator();
        addButton(tb, "Export as GIF", "Camera", 16);
        addButton(tb, "Clone", "Copy", 1);
        tb.addSeparator();
        tick = addButton(tb, "Tick", "StepForward", 4);
        play = addButton(tb, "Play", "Play", 6);
        playII = play.getIcon();
        pauseII = getImage("Pause", 24, "Pause");
        tb.addSeparator();
        addButton(tb, "Layer Up", "Up", 7);
        addButton(tb, "Layer Down", "Down", 8);
        tb.addSeparator();
        addButton(tb, "Zoom In", "ZoomIn", 12);
        addButton(tb, "Zoom Out", "ZoomOut", 13);
        tb.addSeparator();
        addButton(tb, "Adjust Size", "AlignCenter", 9);
        addButton(tb, "Options", "Preferences", 10);
        p.add(main, "Center");
        p.add(tb, "First");
        return p;
    }

    private JButton addButton(JToolBar tb, String name, String file, int action)
    {
        JButton b = new JButton();
        b.setToolTipText(name);
        b.addActionListener(new ButtonAction(action));
        b.setIcon(getImage(file, 24, name));
        b.setFocusable(false);
        b.setFocusTraversalKeysEnabled(false);
        tb.add(b);
        return b;
    }

    private JPanel init_buildStatusBar(JPanel p)
    {
        JPanel p2 = new JPanel(new BorderLayout());
        p2.add(p, "Center");
        stats = new StatusBar();
        stats.add(lLyr = new JLabel("Layer 1"));
        lLyr.setForeground(Color.BLUE.darker());
        stats.addSeparator();
        stats.add(lSz = new JLabel((new StringBuilder(String.valueOf(x))).append("x").append(y).append("x").append(z).toString()));
        stats.addSeparator();
        stats.add(lLoc = new JLabel());
        sLoc = stats.addSeparator();
        lLoc.setVisible(false);
        sLoc.setVisible(false);
        stats.addGlue();
        stats.addSeparator();
        stats.add(lRed = new JLabel("0"));
        stats.add(new JLabel(getImage("Redstone", 16, "Redstone")));
        stats.addSeparator();
        stats.add(lTorch = new JLabel("0"));
        stats.add(new JLabel(getImage("Torch", 16, "Torches")));
        stats.addSeparator();
        stats.add(lTot = new JLabel("0"));
        stats.add(new JLabel(getImage("Ore", 16, "Total")));
        p2.add(stats, "Last");
        lRed.setForeground(Colors.wireOff.darker());
        lTorch.setForeground(Colors.wireOff.darker());
        lTot.setForeground(Colors.wireOff.darker());
        return p2;
    }

    private JPanel init_buildMenuBar(JPanel p2)
    {
        JMenuBar menubar = new JMenuBar();
        int CTRL = Toolkit.getDefaultToolkit().getMenuShortcutKeyMask();
        int SHIFT = 64;
        int ALT = 512;
        JMenu menu = new JMenu(" File ");
        menu.setMnemonic(70);
        menubar.add(menu);
        addMenuItem(menu, "New", 78, "New", CTRL, 78, new ButtonAction(0));
        addMenuItem(menu, "Open", 79, "Open", CTRL, 79, new ButtonAction(3));
        menu.addSeparator();
        addMenuItem(menu, "Save", 83, "Save", CTRL, 83, new ButtonAction(2));
        addMenuItem(menu, "Save As...", 65, "SaveAs", CTRL | SHIFT, 83, new ButtonAction(15));
        menu.addSeparator();
        addMenuItem(menu, "Export as GIF", 71, "Camera", CTRL, 71, new ButtonAction(16));
        menu.addSeparator();
        if(IS_MAC)
            addMenuItem(menu, "Quit", 81, "Stop", CTRL, 81, new ButtonAction(14));
        else
            addMenuItem(menu, "Exit", 88, "Stop", ALT, 115, new ButtonAction(14));
        menu = new JMenu(" Edit ");
        menu.setMnemonic(69);
        menubar.add(menu);
        addMenuItem(menu, "Clone", 67, "Copy", CTRL, 67, new ButtonAction(1));
        menu.addSeparator();
        tickMI = addMenuItem(menu, "Tick", 84, "StepForward", 0, 46, new ButtonAction(4));
        addMenuItem(menu, "Play/Pause", 80, "Play", 0, 10, new ButtonAction(6));
        menu = new JMenu(" View ");
        menu.setMnemonic(86);
        menubar.add(menu);
        addMenuItem(menu, "Layer Up", 85, "Up", 0, 87, new ButtonAction(7));
        addMenuItem(menu, "Layer Down", 68, "Down", 0, 83, new ButtonAction(8));
        menu.addSeparator();
        addMenuItem(menu, "Zoom In", 73, "ZoomIn", 0, 61, new ButtonAction(12));
        addMenuItem(menu, "Zoom Out", 79, "ZoomOut", 0, 45, new ButtonAction(13));
        menu.addSeparator();
        addMenuItem(menu, "Options...", 80, "Preferences", CTRL, 44, new ButtonAction(10));
        menu.addSeparator();
        JMenu menu2 = new JMenu("Look & Feel");
        menu2.setMnemonic(76);
        menu.add(menu2);
        ButtonGroup group = new ButtonGroup();
        String mn = "";
        javax.swing.UIManager.LookAndFeelInfo alookandfeelinfo[];
        int i1 = (alookandfeelinfo = UIManager.getInstalledLookAndFeels()).length;
        for(int k = 0; k < i1; k++)
        {
            javax.swing.UIManager.LookAndFeelInfo l = alookandfeelinfo[k];
            JRadioButtonMenuItem i = new JRadioButtonMenuItem(l.getName(), l.getClassName().equals(UIManager.getCrossPlatformLookAndFeelClassName()));
            for(int j = 0; j < l.getName().length(); j++)
            {
                char c = l.getName().toLowerCase().charAt(j);
                if(mn.contains((new StringBuilder(String.valueOf(c))).toString()))
                    continue;
                mn = (new StringBuilder(String.valueOf(mn))).append(c).toString();
                i.setMnemonic(c);
                break;
            }

            i.addActionListener(new LAFActionListener(l.getClassName()));
            group.add(menu2.add(i));
        }

        menu = new JMenu(" Adjust Size ");
        menu.setMnemonic(65);
        menubar.add(menu);
        addMenuItem(menu, "Adjust Size...", 65, "AlignCenter", CTRL, 65, new ButtonAction(9));
        menu.addSeparator();
        menu2 = new JMenu("Shrink");
        menu2.setMnemonic(83);
        menu.add(menu2);
        addMenuItem2(menu2, "Top face down", 84, "ShrinkTop", CTRL, 104, new Expander(this, false, false, 1));
        addMenuItem2(menu2, "Bottom face up", 66, "ShrinkBottom", CTRL, 98, new Expander(this, false, true, 1));
        addMenuItem2(menu2, "Left face right", 76, "ShrinkLeft", CTRL, 100, new Expander(this, false, false, 2));
        addMenuItem2(menu2, "Right face left", 82, "ShrinkRight", CTRL, 102, new Expander(this, false, true, 2));
        addMenuItem2(menu2, "Front (sky) face down", 83, "ShrinkFront", CTRL, 105, new Expander(this, false, true, 0));
        addMenuItem2(menu2, "Back (ground) face up", 71, "ShrinkBack", CTRL, 99, new Expander(this, false, false, 0));
        menu2 = new JMenu("Grow");
        menu2.setMnemonic(71);
        menu.add(menu2);
        addMenuItem2(menu2, "Top face up", 84, "GrowTop", 0, 104, new Expander(this, true, false, 1));
        addMenuItem2(menu2, "Bottom face down", 66, "GrowBottom", 0, 98, new Expander(this, true, true, 1));
        addMenuItem2(menu2, "Left face left", 76, "GrowLeft", 0, 100, new Expander(this, true, false, 2));
        addMenuItem2(menu2, "Right face right", 82, "GrowRight", 0, 102, new Expander(this, true, true, 2));
        addMenuItem2(menu2, "Front (sky) face up", 83, "GrowFront", 0, 105, new Expander(this, true, true, 0));
        addMenuItem2(menu2, "Back (ground) face down", 71, "GrowBack", 0, 99, new Expander(this, true, false, 0));
        p2.add(menubar, "First");
        return p2;
    }

    private JMenuItem addMenuItem2(JMenu m, String name, int mnem, String file, int mod, int key, Action al)
    {
        JMenuItem i = addMenuItem(m, name, mnem, file, mod, key, al);
        view.getInputMap(0).put(KeyStroke.getKeyStroke(key, mod | 0x40), file);
        view.getActionMap().put(file, al);
        return i;
    }

    private JMenuItem addMenuItem(JMenu m, String name, int mnem, String file, int mod, int key, ActionListener al)
    {
        JMenuItem i = new JMenuItem(name, mnem);
        if(file != null)
            i.setIcon(getImage(file, 16, name));
        if(key != 0)
            i.setAccelerator(KeyStroke.getKeyStroke(key, mod));
        i.addActionListener(al);
        return m.add(i);
    }

    private void paintView(Graphics g)
    {
        g.setColor(Colors.air);
        Rectangle r = g.getClipBounds();
        g.fillRect(r.x, r.y, r.width, r.height);
        Graphics2D g2 = (Graphics2D)g.create();
        g2.scale(scale, scale);
        g2.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);
        g2.setColor(Colors.grid);
        g2.fillRect(0, 0, x * 9 + 1, y * 9 + 1);
        if(clone != null)
        {
            int x1 = clone[0];
            int y1 = clone[1];
            int z1 = clone[2];
            int x2 = clone[3];
            int y2 = clone[4];
            int z2 = clone[5];
            if(x2 < x1)
            {
                x1 = x2;
                x2 = clone[0];
            }
            if(y2 < y1)
            {
                y1 = y2;
                y2 = clone[1];
            }
            if(z2 < z1)
            {
                z1 = z2;
                z2 = clone[2];
            }
            if(lyr >= z1 && lyr <= z2)
            {
                g2.setColor(Colors.copyFrom);
                g2.fillRect(x1 * 9, y1 * 9, (x2 - x1) * 9 + 10, 1);
                g2.fillRect(x1 * 9, y1 * 9, 1, (y2 - y1) * 9 + 10);
                g2.fillRect(x1 * 9, y2 * 9 + 9, (x2 - x1) * 9 + 10, 1);
                g2.fillRect(x2 * 9 + 9, y1 * 9, 1, (y2 - y1) * 9 + 10);
            }
            if(clone.length > 6)
            {
                int x3 = clone[6];
                int y3 = clone[7];
                int z3 = clone[8];
                int x4 = (x3 + x2) - x1;
                int y4 = (y3 + y2) - y1;
                int z4 = (z3 + z2) - z1;
                if(lyr >= z3 && lyr <= z4)
                {
                    g2.setColor(Colors.copyTo);
                    g2.fillRect(x3 * 9, y3 * 9, (x4 - x3) * 9 + 10, 1);
                    g2.fillRect(x3 * 9, y3 * 9, 1, (y4 - y3) * 9 + 10);
                    g2.fillRect(x3 * 9, y4 * 9 + 9, (x4 - x3) * 9 + 10, 1);
                    g2.fillRect(x4 * 9 + 9, y3 * 9, 1, (y4 - y3) * 9 + 10);
                }
            }
        }
        for(int i = r.x / scale / 9; (i * 9 + 1) * scale < r.x + r.width && i < x; i++)
        {
            for(int j = r.y / scale / 9; (j * 9 + 1) * scale < r.y + r.height && j < y; j++)
                field.draw(i, j, lyr, g2, new Rectangle(i * 9 + 1, j * 9 + 1, 8, 8), new Blocks[0]);

        }

    }

    private void viewMouseMoved(MouseEvent e, boolean dragged)
    {
        Point pt = e.getPoint();
        lastPX = pt.x;
        lastPY = pt.y;
        isCtrlDown = e.isControlDown();
        updateTooltip();
        if(lastX < 0 || lastY < 0)
            return;
        if(dragged)
        {
            if(cloneMode != 0)
                return;
            int bi;
            if((e.getModifiersEx() & 0x400) != 0 && !field.g(lastX, lastY, lyr).ctrl() && !field.g(lastX, lastY, lyr + 1).ctrl())
                bi = 0;
            else
            if((e.getModifiersEx() & 0x1000) != 0)
                bi = pal;
            else
                return;
            place(lastX, lastY, lyr, palArr[bi]);
        } else
        {
            if(!isCtrlDown && playing)
            {
                int p = lyr + 1;
                if(field.g(lastX, lastY, p) != Blocks.PRESS)
                    p--;
                if(p != lyr || field.g(lastX, lastY, p) == Blocks.PRESS)
                {
                    field.sp(lastX, lastY, p, 10);
                    field.update();
                    view.repaint();
                }
            }
            setClone(lastX, lastY, lyr);
        }
    }

    private void viewMousePressed(MouseEvent e)
    {
        Point pt = e.getPoint();
        lastPX = pt.x;
        lastPY = pt.y;
        isCtrlDown = e.isControlDown();
        updateTooltip();
        if(lastX < 0 || lastY < 0)
            return;
        if(cloneMode != 0)
        {
            cloneMode++;
            if(cloneMode == 3)
            {
                int l = clone[2];
                if(clone[5] < clone[2])
                    l = clone[5];
                int x[] = {
                    0, 0, 0, 0, 0, 0, lastX, lastY, l
                };
                System.arraycopy(clone, 0, x, 0, 6);
                clone = x;
                setLyr(l);
            } else
            if(cloneMode == 4)
            {
                cloneMode = 0;
                for(int i = 0; i < 3; i++)
                    if(clone[i + 3] >= clone[i])
                    {
                        clone[i + 3] += 1 - clone[i];
                    } else
                    {
                        int t = clone[i + 3];
                        clone[i + 3] = (clone[i] - t) + 1;
                        clone[i] = t;
                    }

                field.clone(clone);
                recountRed();
                clone = null;
            }
            view.repaint();
            return;
        }
        int bi = pal;
        switch(e.getButton())
        {
        default:
            break;

        case 1: 
        {
            bi = 0;
            int p = lyr + 1;
            if(field.g(lastX, lastY, lyr).ctrl())
                p--;
            if(p == lyr || field.g(lastX, lastY, lyr + 1).ctrl())
            {
                if(field.g(lastX, lastY, p) == Blocks.LEVER)
                    field.sp(lastX, lastY, p, field.p(lastX, lastY, p) ? 0 : 16);
                else
                if(field.g(lastX, lastY, p) == Blocks.BUTTON && !field.p(lastX, lastY, p))
                    field.sp(lastX, lastY, p, 10);
                else
                if(field.g(lastX, lastY, p) == Blocks.PRESS && !playing)
                    field.sp(lastX, lastY, p, field.p(lastX, lastY, p) ? 0 : 10);
                field.update();
                view.repaint();
                break;
            }
         
        }

        case 3:
        {
            place(lastX, lastY, lyr, palArr[bi]);
            break;
        }

        case 2: // '\002'
        {
            int p = lyr + 1;
            if(field.g(lastX, lastY, lyr).wall != 0)
                p--;
            if(p != lyr && field.g(lastX, lastY, lyr + 1).wall == 0)
                break;
            if(field.g(lastX, lastY, p) == Blocks.DOORB)
                p--;
            int w = field.w(lastX, lastY, p);
            int ow = w;
            do
                w = ++w % 5;
            while(!field.s(lastX, lastY, p, w));
            if(ow != w)
            {
                field.update();
                view.repaint();
            }
            break;
        }
        }
    }

    private void paintPView(Graphics g2)
    {
        g2.setColor(Colors.grid);
        Rectangle r = g2.getClipBounds();
        g2.fillRect(r.x, r.y, r.width, r.height);
        Graphics2D g = (Graphics2D)g2.create();
        g.scale(pScale, pScale);
        g.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);
        g.setColor(Colors.hilite);
        g.fillRect(pal * 9, 0, 10, 10);
        for(int i = 0; i < palArr.length; i++)
        {
            r = new Rectangle(i * 9 + 1, 1, 8, 8);
            Palette p = palArr[i];
            field.draw(0, 0, 0, g, r, new Blocks[] {
                p.a, p.b != null ? p.b : Blocks.AIR, p.c != null ? p.c : Blocks.AIR
            });
        }

    }

    private void pViewMousePressed(MouseEvent e)
    {
        Point p = e.getPoint();
        int pX = p.x / pScale;
        if(pX % 9 == 0)
            return;
        pX /= 9;
        if(pX >= palArr.length)
        {
            return;
        } else
        {
            pal = pX;
            pView.repaint();
            return;
        }
    }

    @Override
    public void mouseWheelMoved(MouseWheelEvent e)
    {
        if(e.isControlDown())
        {
            addScale(-e.getWheelRotation());
        } else
        {
            int l = palArr.length;
            pal = ((pal + e.getWheelRotation()) % l + l) % l;
            pView.repaint();
        }
    }

    private void frameResized()
    {
        pScale = frame.getContentPane().getWidth() / (palArr.length * 9 + 1);
        if(pScale > 5)
            pScale = 5;
        if(pScale < 2)
            pScale = 2;
        Dimension d = new Dimension((palArr.length * 9 + 1) * pScale, 10 * pScale);
        pView.setPreferredSize(d);
        pView.setMaximumSize(d);
        pView.revalidate();
    }

    private void playToggle()
    {
        if(playing)
        {
            pTimer.stop();
            pTimer = null;
            play.setIcon(playII);
            tick.setEnabled(true);
            tickMI.setEnabled(true);
        } else
        {
            (pTimer = new Timer(62, new ButtonAction(5))).start();
            play.setIcon(pauseII);
            tick.setEnabled(false);
            tickMI.setEnabled(false);
        }
        playing = !playing;
    }

    public void setClone(int pX, int pY, int pZ)
    {
        if(cloneMode == 1)
        {
            clone[0] = clone[3] = pX;
            clone[1] = clone[4] = pY;
            clone[2] = clone[5] = pZ;
        } else
        if(cloneMode == 2)
        {
            clone[3] = pX;
            clone[4] = pY;
            clone[5] = pZ;
        } else
        if(cloneMode == 3)
        {
            clone[6] = pX;
            clone[7] = pY;
            clone[8] = pZ;
        }
        if(cloneMode != 0)
            view.repaint();
    }

    private void setPalette(Palette p[])
    {
        Palette sel = palArr[pal];
        int wireInd = 0;
        pal = -1;
        palArr = p;
        for(int i = 0; i < p.length; i++)
            if(p[i] == sel)
                pal = i;
            else
            if(p[i] == Palette.wire)
                wireInd = i;

        if(pal == -1)
            pal = wireInd;
        Dimension d = new Dimension((palArr.length * 9 + 1) * pScale, 10 * pScale);
        pView.setPreferredSize(d);
        pView.setMaximumSize(d);
        pView.revalidate();
    }

    public void updateTooltip()
    {
        int pX = lastPX / scale;
        int pY = lastPY / scale;
        if(lastPX < 0 || lastPY < 0 || pX < 0 || pY < 0 || pX > x * 9 || pY > y * 9)
        {
            lastX = lastY = -1;
            tooltip.setVisible(false);
            lLoc.setVisible(false);
            sLoc.setVisible(false);
            return;
        }
        if(pX % 9 == 0 || pY % 9 == 0)
            return;
        pX /= 9;
        pY /= 9;
        lastX = pX;
        lastY = pY;
        lLoc.setText((new StringBuilder("X=")).append(lastX).append(", Y=").append(lyr).append(", Z=").append(lastY).toString());
        lLoc.setVisible(true);
        sLoc.setVisible(true);
        if(!isCtrlDown)
        {
            tooltip.setVisible(false);
            return;
        }
        tooltip.setLocation((lastX * 9 + 10) * scale, (lastY * 9 + 1) * scale);
        tooltip.setVisible(true);
        String s = (new StringBuilder("<html>")).append(lastX).append(",").append(lyr).append(",").append(lastY).append("<p>").toString();
        if(Field.layers > 2)
            s = (new StringBuilder(String.valueOf(s))).append(field.g(lastX, lastY, lyr + 2).name).append(" on ").toString();
        if(Field.layers > 1)
            s = (new StringBuilder(String.valueOf(s))).append(field.g(lastX, lastY, lyr + 1).name).append(" on ").toString();
        s = (new StringBuilder(String.valueOf(s))).append(field.g(lastX, lastY, lyr).name).toString();
        String dir[] = {
            "ground", "west", "east", "south", "north"
        };
        for(int i = 0; i < Field.layers; i++)
        {
            int p = lyr + i;
            switch(field.g(lastX, lastY, p))
            {
            case SAND: 
            default:
                break;

            case BUTTON: 
                s = (new StringBuilder(String.valueOf(s))).append("<p>button: attached to ").append(dir[field.w(lastX, lastY, p)]).append(" face").append("<p>button: ").append(field.p(lastX, lastY, p) ? (new StringBuilder(String.valueOf(field.gp(lastX, lastY, p)))).append(" ticks of power left").toString() : "unpowered").toString();
                break;

            case DOORB: 
                if(i != 0)
                    break;
                p--;
               

            case DOORA: 
                s = (new StringBuilder(String.valueOf(s))).append("<p>door: hinge at ").append((new String[] {
                    "no", "NW", "SE", "NE", "SW"
                })[field.w(lastX, lastY, p)]).append(" corner").append("<p>door: ").append(field.p(lastX, lastY, p) ? "" : "un").append("powered").toString();
                break;

            case TORCH: 
                s = (new StringBuilder(String.valueOf(s))).append("<p>torch: attached to ").append(dir[field.w(lastX, lastY, p)]).append(" face").append("<p>torch: ").append(field.p(lastX, lastY, p) ? "" : "un").append("powered").toString();
                break;

            case LEVER: 
                s = (new StringBuilder(String.valueOf(s))).append("<p>switch: attached to ").append(dir[field.w(lastX, lastY, p)]).append(" face").append("<p>switch: ").append(field.p(lastX, lastY, p) ? "" : "un").append("powered").toString();
                break;

            case PRESS: 
                s = (new StringBuilder(String.valueOf(s))).append("<p>pressure plate: ").append(field.p(lastX, lastY, p) ? field.gp(lastX, lastY, p) != 10 ? (new StringBuilder(String.valueOf(field.gp(lastX, lastY, p)))).append(" ticks of power left").toString() : "powered" : "unpowered").toString();
                break;

            case WIRE: 
                s = (new StringBuilder(String.valueOf(s))).append("<p>wire: ").append(field.p(lastX, lastY, p) ? (new StringBuilder("will carry power for ")).append(field.gp(lastX, lastY, p)).append(" blocks").toString() : "unpowered").toString();
                break;

            case WATER: 
                s = (new StringBuilder(String.valueOf(s))).append("<p>water: ").append((field.gp(lastX, lastY, p) & 0xf) != 0 ? (new StringBuilder("level ")).append(8 - (field.gp(lastX, lastY, p) & 7)).toString() : "source").append((field.gp(lastX, lastY, p) & 8) != 0 ? ", falling" : "").append((field.gp(lastX, lastY, p) & 0x10) != 0 ? " (dynamic)" : " (static)").toString();
                break;
            }
        }

        tooltip.setText(s);
        tooltip.setSize(tooltip.getPreferredSize());
    }

    public void play(boolean open)
    {
        URL u;
        u = getSound(open ? "door_open.wav" : "door_close.wav");
        if(u == null)
            return;
        try
        {
            Clip clip = AudioSystem.getClip();
            clip.open(AudioSystem.getAudioInputStream(u));
            clip.start();
        }
        catch(Exception e)
        {
            e.printStackTrace();
        }
        return;
    }

    private boolean areYouSure()
    {
        if(modified)
        {
            int answer = JOptionPane.showConfirmDialog(null, "Do you want to save first?", "Unsaved changes", 1);
            if(answer == 2)
                return false;
            if(answer == 0)
                if(save == null)
                    saveOpenDialog(false, false);
                else
                    save();
        }
        return true;
    }

    private void save()
    {
        try
        {
            if(playing)
                playToggle();
            String s = save.getAbsolutePath();
            if(s.endsWith(".rdat"))
            {
                field.save(new File(s));
            } else
            {
                if(!s.endsWith(".schematic"))
                    s = (new StringBuilder(String.valueOf(s))).append(".schematic").toString();
                LevelLoader.save(this, s);
            }
            modified = false;
            updateTitle();
        }
        catch(Exception ex)
        {
            JOptionPane.showMessageDialog(frame, ex.getMessage(), "Error", 0);
        }
    }

    private void export(File f)
    {
        byte abyte0[];
        try
        {
            ImageWriter wr = (ImageWriter)ImageIO.getImageWritersByFormatName("gif").next();
            ImageOutputStream ios = ImageIO.createImageOutputStream(f);
            wr.setOutput(ios);
            wr.prepareWriteSequence(null);
            for(int i = 0; i < z; i++)
            {
                BufferedImage bi = new BufferedImage((x * 9 + 1) * scale, (y * 9 + 1) * scale, 13, Colors.icm);
                BufferedImage back = new BufferedImage(bi.getWidth(), bi.getHeight(), 5);
                Graphics2D g = back.createGraphics();
                g.scale(scale, scale);
                g.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);
                for(int j = 0; j < y; j++)
                {
                    for(int k = 0; k < x; k++)
                        if(field.g(k, j, i) != Blocks.SHADOW || Field.layers > 1 && !field.g(k, j, i + 1).air())
                        {
                            g.setColor(Colors.grid);
                            g.fillRect(k * 9, j * 9, 10, 10);
                            field.draw(k, j, i, g, new Rectangle(k * 9 + 1, j * 9 + 1, 8, 8), new Blocks[0]);
                        }

                }

                byte pix[] = ((DataBufferByte)back.getRaster().getDataBuffer()).getData();
                byte ind[] = new byte[pix.length / 3];
                byte baR[] = new byte[Colors.icm.getMapSize()];
                Colors.icm.getReds(baR);
                byte baG[] = new byte[Colors.icm.getMapSize()];
                Colors.icm.getGreens(baG);
                byte baB[] = new byte[Colors.icm.getMapSize()];
                Colors.icm.getBlues(baB);
                for(int j = 0; j < ind.length; j++)
                {
                    byte n = 0;
                    int d = 0x7fffffff;
                    for(int k = 0; k < baR.length; k++)
                    {
                        int dr = (baR[k] & 0xff) - (pix[3 * j + 2] & 0xff);
                        int dg = (baG[k] & 0xff) - (pix[3 * j + 1] & 0xff);
                        int db = (baB[k] & 0xff) - (pix[3 * j] & 0xff);
                        int dc = dr * dr + dg * dg + db * db;
                        if(dc < d)
                        {
                            n = (byte)k;
                            d = dc;
                        }
                    }

                    ind[j] = n;
                }

                bi.setData(Raster.createRaster(new PixelInterleavedSampleModel(0, bi.getWidth(), bi.getHeight(), 1, bi.getWidth(), new int[1]), new DataBufferByte(ind, ind.length), null));
                IIOMetadata m = wr.getDefaultImageMetadata(new ImageTypeSpecifier(bi), wr.getDefaultWriteParam());
                Node root = m.getAsTree(m.getNativeMetadataFormatName());
                for(Node n = root.getFirstChild(); n != null; n = n.getNextSibling())
                {
                    if(!n.getNodeName().equals("GraphicControlExtension"))
                        continue;
                    IIOMetadataNode gce = (IIOMetadataNode)n;
                    gce.setAttribute("userInputFlag", "FALSE");
                    gce.setAttribute("disposalMethod", "restoreToBackgroundColor");
                    gce.setAttribute("delayTime", "200");
                    break;
                }

                if(i == 0)
                {
                    IIOMetadataNode aes = new IIOMetadataNode("ApplicationExtensions");
                    IIOMetadataNode ae = new IIOMetadataNode("ApplicationExtension");
                    ae.setAttribute("applicationID", "NETSCAPE");
                    ae.setAttribute("authenticationCode", "2.0");
                    abyte0 = new byte[3];
                    abyte0[0] = 1;
                    ae.setUserObject(abyte0);
                    aes.appendChild(ae);
                    root.appendChild(aes);
                }
                try
                {
                    m.setFromTree(m.getNativeMetadataFormatName(), root);
                }
                catch(IIOInvalidTreeException e)
                {
                    throw e;
                }
                wr.writeToSequence(new IIOImage(bi, null, m), null);
            }

            wr.endWriteSequence();
            ios.close();
        }
        catch(Exception e)
        {
            e.printStackTrace();
        }
    }
  
    
    private void saveOpenDialog(boolean open, final boolean gif)
    {
        JFileChooser fc = new JFileChooser();
        fc.setCurrentDirectory(folder);
        fc.setFileFilter(new FileFilter() {

            @Override
            public String getDescription()
            {
                if(gif)
                    return "GIF files (.gif)";
                else
                    return "Supported File Types (.rdat, .schematic)";
            }

            @Override
            public boolean accept(File f)
            {
                if(f.isDirectory())
                    return true;
                String s = f.getName().substring(f.getName().lastIndexOf('.') + 1);
                if(gif)
                    return s.equals("gif");
                return s.equals("rdat") || s.equals("schematic");
            }
        });
        
        if(open)
        {
            if(fc.showOpenDialog(frame) == 0)
                try
                {
                    save = fc.getSelectedFile();
                    folder = save.getParentFile();
                    if(save.getName().endsWith("rdat"))
                        field.load(save);
                    else
                        LevelLoader.load(this, save.getAbsolutePath());
                    modified = false;
                    updateTitle();
                }
                catch(Exception ex)
                {
                    JOptionPane.showMessageDialog(frame, ex.getMessage(), "Error", 0);
                }
        } else
        if(fc.showSaveDialog(frame) == 0)
            if(gif)
            {
                String s = fc.getSelectedFile().getAbsolutePath();
                if(!s.toLowerCase().endsWith(".gif"))
                    s = (new StringBuilder(String.valueOf(s))).append(".gif").toString();
                export(new File(s));
            } else
            {
                save = fc.getSelectedFile();
                folder = save.getParentFile();
                save();
            }
    }

    public void modify()
    {
        if(!modified)
        {
            modified = true;
            updateTitle();
        }
    }

    private void updateTitle()
    {
        String m = "";
        if(modified && !IS_MAC)
            m = "*";
        frame.setTitle((new StringBuilder(String.valueOf(m))).append(save == null ? "" : (new StringBuilder(String.valueOf(save.getName()))).append(" - ").toString()).append(title).toString());
        if(IS_MAC)
            frame.getRootPane().putClientProperty("windowModified", new Boolean(modified));
    }

    public static void main(String args[])
        throws Exception
    {
        new Viewport(30, 20, 7);
    }

   
    public static final String title = "Redstone Simulator v0.1a Mod";
    static final Object blockNames[][] = {
        {
            "Rock", Integer.valueOf(1)
        }, {
            "Grass/Dirt", Integer.valueOf(2)
        }, {
            "Dirt", Integer.valueOf(3)
        }, {
            "Cobblestone", Integer.valueOf(4)
        }, {
            "Wood", Integer.valueOf(5)
        }, {
            "Adminium", Integer.valueOf(7)
        }, {
            "Sand", Integer.valueOf(12)
        }, {
            "Gravel", Integer.valueOf(13)
        }, {
            "Gold ore", Integer.valueOf(14)
        }, {
            "Iron ore", Integer.valueOf(15)
        }, {
            "Coal ore", Integer.valueOf(16)
        }, {
            "Tree trunk", Integer.valueOf(17)
        }, {
            "Sponge", Integer.valueOf(19)
        }, {
            "Cloth", Integer.valueOf(35)
        }, {
            "Gold", Integer.valueOf(41)
        }, {
            "Iron", Integer.valueOf(42)
        }, {
            "Double stair", Integer.valueOf(43)
        }, {
            "Brick", Integer.valueOf(45)
        }, {
            "Bookshelf", Integer.valueOf(47)
        }, {
            "Mossy cobblestone", Integer.valueOf(48)
        }, {
            "Obsidian", Integer.valueOf(49)
        }, {
            "Diamond ore", Integer.valueOf(56)
        }, {
            "Diamond", Integer.valueOf(57)
        }, {
            "Redstone ore", Integer.valueOf(73)
        }, {
            "Snow", Integer.valueOf(80)
        }, {
            "Clay", Integer.valueOf(82)
        }
    };
    static final int iX = 30;
    static final int iY = 20;
    static final int iZ = 7;
    static final int MS_PER_TICK = 62;
    static final int GIF_DELAY = 200;
    static final boolean IS_MAC = System.getProperty("os.name").toLowerCase().indexOf("mac") != -1;
    static final boolean doubleDoors = false;
    static boolean waterMode = false;
    int block;
    JFrame frame;
    JDialog adjF;
    JDialog optF;
    JPanel view;
    JPanel pView;
    StatusBar stats;
    Field field;
    Palette palArr[];
    int pal;
    int scale;
    int pScale;
    int lyr;
    int gd;
    int x;
    int y;
    int z;
    int cloneMode;
    int lastX;
    int lastY;
    int lastPX;
    int lastPY;
    int clone[];
    JLabel lLyr;
    JLabel lSz;
    JLabel lLoc;
    JLabel lRed;
    JLabel lTorch;
    JLabel lTot;
    JLabel tooltip;
    JSeparator sLoc;
    JButton tick;
    JButton play;
    JCheckBox c1Lyr;
    JCheckBox c3Lyr;
    JCheckBox cBridge;
    JCheckBox cWater;
    Icon playII;
    Icon pauseII;
    JMenuItem tickMI;
    File save;
    File folder;
    boolean playing;
    boolean isCtrlDown;
    boolean modified;
    Timer pTimer;
}
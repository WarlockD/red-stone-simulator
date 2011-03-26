using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Redstone_Simulator
{
    public partial class Form1 : Form
    {
        int pScale = 5;

        public Form1()
        {
            InitializeComponent();
            this.MouseWheel += new MouseEventHandler(Form1_MouseWheel);
        }

        void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0 | e.Delta < 0)
                blockSelect.moveSelect(e.Delta > 0 ? 1 : -1);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0 | e.Delta < 0)
                blockSelect.moveSelect(e.Delta);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Redstone_Simulator
{
    class PanelScrollFix : Panel
    {
        protected override System.Drawing.Point ScrollToControl(Control activeControl)
        {
            //return base.ScrollToControl(activeControl);
            return DisplayRectangle.Location;
        }
    }
}

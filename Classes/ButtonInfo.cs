using System;
using StupidTemplate.Menu;

namespace StupidTemplate.Classes
{
    public class ButtonInfo
    {
        internal readonly object Page;
        public string buttonText = "-";
        public string overlapText = null;
        public Action method = null;
        public Action enableMethod = null;
        public Action disableMethod = null;
        public bool enabled = false;
        public bool isTogglable = true;
        public string toolTip = "This button doesn't have a tooltip/tutorial.";

    }
}

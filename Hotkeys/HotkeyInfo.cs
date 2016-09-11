using System.Windows.Forms;

namespace Youpload.Hotkeys
{
    public class HotkeyInfo
    {
        public Keys keyCode;

        //KeysModifiers
        public bool shift;   // use shift key?
        public bool control; // use control key?
        public bool alt;     // use alt key?
        public bool windows; // Need focus on window?

        public HotkeyInfo(Keys keyCode, bool shift, bool control, bool alt, bool windows)
        {
            this.keyCode = keyCode;
            this.shift = shift;
            this.control = control;
            this.alt = alt;
            this.windows = windows;
        }       
    }
}

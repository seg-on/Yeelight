using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YeelightTray
{
    public partial class BrightnessControlForm : Form
    {
        private DeviceIO m_DeviceIO;
        public BrightnessControlForm(Device device)
        {
            InitializeComponent();
            m_DeviceIO = new DeviceIO();
            if (m_DeviceIO.Connect(device) == true)
            {
                //Apply current device values to controls
                pbBrightness.Value = device.Brightness;
            }

            lBrightness.Text = pbBrightness.Value + "%";
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.pbBrightness_MouseWheel);
            this.LostFocus += new EventHandler(this.Event_LostFocus);

        }

        private void pbBrightness_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0 & pbBrightness.Value < 100)
            {
                pbBrightness.Value += 10;
                lBrightness.Text = pbBrightness.Value + "%";
                m_DeviceIO.SetBrightness(pbBrightness.Value,500);
            }
            else if (e.Delta < 0 & pbBrightness.Value > 0)
            {
                pbBrightness.Value -= 10;
                lBrightness.Text = pbBrightness.Value + "%";
                m_DeviceIO.SetBrightness(pbBrightness.Value, 500);
            }
        }


        private void Event_LostFocus(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}

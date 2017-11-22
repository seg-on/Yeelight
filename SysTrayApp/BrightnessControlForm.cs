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
        List<ColourTemp> CTList = new List<ColourTemp>();
        Device m_device;
        public BrightnessControlForm(Device device)
        {
            InitializeComponent();
            m_DeviceIO = new DeviceIO();

            CTList.Add(new ColourTemp() { Name = "Cloudly sky (6500°K)", Value = 6500 });
            CTList.Add(new ColourTemp() { Name = "Midday sun (5500°K)", Value = 5500 });
            CTList.Add(new ColourTemp() { Name = "Moonlight (4000°K)", Value = 4000 });
            CTList.Add(new ColourTemp() { Name = "Morning sun (3500°K)", Value = 3500 });
            CTList.Add(new ColourTemp() { Name = "Lightbulb (3000°K)", Value = 3000 });
            CTList.Add(new ColourTemp() { Name = "Sunrise (2500°K)", Value = 2500 });
            CTList.Add(new ColourTemp() { Name = "Candele flame (1700°K)", Value = 1700 });

            if (m_DeviceIO.Connect(device) == true)
            {
                //Apply current device values to controls
                pbBrightness.Value = device.Brightness;

                ColourTemp ct = CTList.Aggregate((x, y) => Math.Abs(x.Value - device.ColourTemperature) < Math.Abs(y.Value - device.ColourTemperature) ? x : y);
                cbColourTemperature.DataSource = CTList;
                cbColourTemperature.DisplayMember = "Name";
                cbColourTemperature.ValueMember = "Value";
                cbColourTemperature.DropDownStyle = ComboBoxStyle.DropDownList;

                cbColourTemperature.SelectedItem = ct;
            }
            m_device = device;
            lBrightness.Text = pbBrightness.Value + "%";
            this.Activate();
            this.MouseWheel += new MouseEventHandler(this.pbBrightness_MouseWheel);
            this.Deactivate += new EventHandler(this.Event_Deactivate);
            this.MouseDoubleClick += new MouseEventHandler(this.Event_Deactivate);

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


        private void Event_Deactivate(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public class ColourTemp
        {
            public string Name { get; set; }
            public int Value { get; set; }
        }

        private void cbColourTemperature_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_DeviceIO.SetColorTemperature((cbColourTemperature.SelectedItem as ColourTemp).Value, 500);
        }
    }
}

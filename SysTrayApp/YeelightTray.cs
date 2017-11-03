using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Net;
using System.ComponentModel;
using System.Device.Location;
using System.Globalization;
using System.Web.Script.Serialization;


namespace YeelightTray
{
    public class YeelightTray : Form
    {

        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;
        bool OnOff = true;

        private DevicesDiscovery m_DevicesDiscovery;
        private DeviceIO m_DeviceIO;
        private Device device;
        static Timer timerLock = new Timer();
        static Timer timerStatus = new Timer();

        public YeelightTray()
        {

            m_DeviceIO = new DeviceIO();

            m_DevicesDiscovery = new DevicesDiscovery();
            m_DevicesDiscovery.StartListening();

            //Send Discovery Message
            m_DevicesDiscovery.SendDiscoveryMessage();

            device = m_DevicesDiscovery.GetDiscoveredDevices()[m_DevicesDiscovery.GetDiscoveredDevices().FindIndex(a => a.Id == "0x00000000036da392")];

            if (m_DeviceIO.Connect(device) == true)
            {
                //Apply current device values to controls
                OnOff = device.State;
            }

            // Create a simple tray menu with only one item.
            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Exit", OnExit);

            // Create a tray icon. In this example we use a
            // standard system icon for simplicity, but you
            // can of course use your own custom icon too.
            trayIcon = new NotifyIcon();
            trayIcon.Text = "Yeelight";
            //trayIcon.Icon = new Icon(SystemIcons.Application, 40, 40);
            trayIcon.Icon = new Icon(OnOff ? Properties.Resources.yeelight_win_on : Properties.Resources.yeelight_win_off, 40, 40);
            //trayIcon.Icon = new Icon((device.State) ? Properties.Resources.yeelight_win_on : Properties.Resources.yeelight_win_off, 40, 40);
            trayIcon.MouseClick += new MouseEventHandler(OnMouseClick);

            // Add menu to tray icon and show it.
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;

            SunTime();

            /* Adds the event and the event handler for the method that will 
            process the timer event to the timer. */
            timerLock.Tick += new EventHandler(TimerLockEvent);
            // Sets the timer interval to 2 minutes.
            timerLock.Interval = 120000;

            timerStatus.Tick += new EventHandler(TimerStatusEvent);
            timerStatus.Interval = 10000;
            timerStatus.Start();

            //Session Switch Event
            Microsoft.Win32.SystemEvents.SessionSwitch += new Microsoft.Win32.SessionSwitchEventHandler(SystemEvents_SessionSwitch);
        }

        private void TimerStatusEvent(object sender, EventArgs e)
        {
            if (device.State)
            {
                trayIcon.Icon = Properties.Resources.yeelight_win_on;
                OnOff = true;
            }
            else
            {
                trayIcon.Icon = Properties.Resources.yeelight_win_off;
                OnOff = false;
            }

            timerStatus.Start();
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //MessageBox.Show(e.Button.ToString());
                if (device.State)
                {
                    trayIcon.Icon = Properties.Resources.yeelight_win_off;
                    m_DeviceIO.Toggle();
                    OnOff = false;
                }
                else
                {
                    trayIcon.Icon = Properties.Resources.yeelight_win_on;
                    m_DeviceIO.Toggle();
                    OnOff = true;
                }
            }
            else if (e.Button == MouseButtons.Middle)
            {
                this.Activate();
                BrightnessControlForm BSForm = new BrightnessControlForm(device);
                BSForm.ShowDialog();
        //m_DeviceIO.SetBrightness();
    }
            else if (e.Button == MouseButtons.None)
            {
                var brithness = device.Brightness;
                //m_DeviceIO.SetBrightness();
            }

        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.

            base.OnLoad(e);
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        protected override void Dispose(bool isDisposing)
        {
            trayIcon.Visible = false;
            while (trayIcon.Visible != false)
            {

            }
            if (isDisposing)
            {
                // Release the icon resource.
                trayIcon.Dispose();
            }

            base.Dispose(isDisposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SysTrayApp
            // 
            this.ClientSize = new System.Drawing.Size(120, 0);
            this.Name = "Yeelight";
            this.ResumeLayout(false);

        }

        void SystemEvents_SessionSwitch(object sender, Microsoft.Win32.SessionSwitchEventArgs e)
        {
            if (e.Reason == SessionSwitchReason.SessionLock)
            {
                if (device.State) timerLock.Start();
            }
            else if (e.Reason == SessionSwitchReason.SessionUnlock)
            {
                if (OnOff)
                {
                    trayIcon.Icon = Properties.Resources.yeelight_win_on;
                    m_DeviceIO.Toggle();
                }
            }
        }
        // This is the method to run when the timer is raised.
        private void TimerLockEvent(Object myObject, EventArgs myEventArgs)
        {
            timerLock.Stop();

            trayIcon.Icon = Properties.Resources.yeelight_win_off;
            m_DeviceIO.Toggle();

        }

        private void SunTime()
        {
            GeoCoordinate locality = new GeoCoordinate();
            //Piestany
            locality.Latitude = 48.584167;
            locality.Longitude = 17.833611;

            DateTime civil_twilight_end = DateTime.Now.AddMinutes(1);
            string Today = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString();
            string lat = locality.Latitude.ToString(CultureInfo.InvariantCulture);
            string lon = locality.Longitude.ToString(CultureInfo.InvariantCulture);
            string json = new WebClient().DownloadString("https://api.sunrise-sunset.org/json?lat=" + lat + "&lng=" + lon + "&date=" + Today);
            SSAPI piestanyInfo = new JavaScriptSerializer().Deserialize<SSAPI>(json);

            if (piestanyInfo.results.civil_twilight_end.ToLocalTime() < DateTime.Now & !OnOff)
            {
                trayIcon.Icon = Properties.Resources.yeelight_win_on;
                m_DeviceIO.Toggle();
                OnOff = true;
            }

        }
    }

    public class SSAPI
    {
        public SunInfo results { get; set; }
        public string status { get; set; }
    }
    public class SunInfo
    {
        public DateTime sunrise { get; set; }
        public DateTime sunset { get; set; }
        public DateTime solar_noon { get; set; }
        public DateTime day_length { get; set; }
        public DateTime civil_twilight_begin { get; set; }
        public DateTime civil_twilight_end { get; set; }
        public DateTime nautical_twilight_begin { get; set; }
        public DateTime nautical_twilight_end { get; set; }
        public DateTime astronomical_twilight_begin { get; set; }
        public DateTime astronomical_twilight_end { get; set; }

        public string PrintPropreties()
        {
            string allData = System.Environment.NewLine;
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(this))
            {
                string name = descriptor.Name;
                DateTime dValue = Convert.ToDateTime(descriptor.GetValue(this));
                string value = dValue.ToLocalTime().ToString();
                allData += name + ":" + value.PadLeft(50 - name.Length) + System.Environment.NewLine;
            }
            return allData;
        }

    }
}
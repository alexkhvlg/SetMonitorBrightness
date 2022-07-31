using System.Management;

namespace SetMonitorBrightness
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static int Get()
        {
            try
            {
                using var mclass = new ManagementClass("WmiMonitorBrightness")
                {
                    Scope = new ManagementScope(@"\\.\root\wmi")
                };
                using var instances = mclass.GetInstances();
                foreach (ManagementObject instance in instances)
                {
                    return (byte)instance.GetPropertyValue("CurrentBrightness");
                }
            }
            catch
            {
                // supress
            }
            return 0;
        }

        public static void Set(int brightness)
        {
            try
            {
                using var mclass = new ManagementClass("WmiMonitorBrightnessMethods")
                {
                    Scope = new ManagementScope(@"\\.\root\wmi")
                };
                using var instances = mclass.GetInstances();
                var args = new object[] { 1, brightness };
                foreach (ManagementObject instance in instances)
                {
                    instance.InvokeMethod("WmiSetBrightness", args);
                }
            }
            catch
            {
                // supress
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Set(0);
            ShowInTaskbar = false;
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
            Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Get() != 0)
            {
                Set(0);
            }
        }
    }
}
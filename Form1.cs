using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JukyUpdater
{
    public partial class Form1 : Form
    {
 
        public Form1()
        {
           


            InitializeComponent();
            this.Hide();
            try
            {
                RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Virtual Machine\Guest\Parameters");
                registrycontent =  registryKey.GetValue("VirtualMachineName").ToString();
            }
            catch { registrycontent = "Juky ????"; }

        }
        private ContextMenu m_menu;
        protected void Exit_Click(Object sender, System.EventArgs e)
        {
            Close();
        }
        protected void Hide_Click(Object sender, System.EventArgs e)
        {
            Hide();
        }
        protected void Show_Click(Object sender, System.EventArgs e)
        {
            Show();
        }
        public string registrycontent;          
        public void main()
        {
            this.Hide();

            string updatetxt = @"Z:\Config\Updating.txt";
            string juky = @"C:\Users\" + Environment.UserName + @"\Desktop\Public Juky\Guild Build\Release\Juky.exe";
            string zJuky = @"Z:\Juky.exe";

            try
            {
                string ctime = File.GetLastWriteTime(juky).ToShortTimeString();
                string octime = File.GetLastWriteTime(zJuky).ToShortTimeString();

                if (ctime != octime)
                {
                    Stopproc();
                    File.AppendAllText(updatetxt,Environment.NewLine +  registrycontent + " : Updated at " + DateTime.Now.ToString()) ;
                    File.Delete(juky);
                    File.Copy(zJuky, juky);
                    var runningProcessByName = Process.GetProcessesByName("Juky");
                    if (runningProcessByName.Length == 0)
                    {
                        
                        Process.Start(@"C:\Users\"+Environment.UserName+@"\Desktop\juky.bat");
                    }
                    runningProcessByName = null;



                }
            }
            catch { }
        }
        public void Stopproc()
        {
            try
            {
                Process[] processlist = Process.GetProcesses();

                foreach (Process process in processlist)
                {
                    if (process.ProcessName == "Juky")
                    {
                        int id = process.Id;

                        Process p = Process.GetProcessById(id);
                        if (p != null)
                        {
                            p.Kill();
                        }
                    }
                }
            }
            catch { }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            main();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            this.Visible = false;
            this.ShowInTaskbar = true;
            Rectangle workingArea = Screen.GetWorkingArea(this);
            this.Location = new Point(workingArea.Right - Size.Width,
                                      workingArea.Bottom - Size.Height);
            this.TopMost = true;
            m_menu = new ContextMenu();
            m_menu.MenuItems.Add(0,
                new MenuItem("Show", new System.EventHandler(Show_Click)));
            m_menu.MenuItems.Add(1,
                new MenuItem("Hide", new System.EventHandler(Hide_Click)));
            m_menu.MenuItems.Add(2,
                new MenuItem("Exit", new System.EventHandler(Exit_Click)));
            notifyIcon1.ContextMenu = m_menu;
        }
    }
}

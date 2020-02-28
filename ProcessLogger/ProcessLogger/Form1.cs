using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ProcessLogger
{
    public partial class ProcessLogger : Form
    {
        ArrayList processList = new ArrayList();
        long counter = 0;

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern Int32 GetWindowThreadProcessId(IntPtr cur, out uint processId);

        public ProcessLogger()
        {
            InitializeComponent();
        }

        private void ProcessLogger_Load(object sender, EventArgs e)
        {
            TmrLoadProcesses.Start();
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Created by DefyTheRush", "About");
        }

        private void TmrLoadProcesses_Tick(object sender, EventArgs e)
        {
            switch (counter)
            {
                case 0:
                    String process = GetProcessName();
                    if (!(process.Equals("")))
                    {
                        LstProcessWindow.Items.Add(process);
                        counter++;
                    }
                    process = null;
                    //GC.Collect();
                    break;
                default:
                    process = GetProcessName();
                    if (!(process.Equals("")))
                    {
                        if (process.Equals(LstProcessWindow.Items[LstProcessWindow.Items.Count - 1]))
                        {

                        }
                        else
                        {
                            LstProcessWindow.Items.Add(process);
                            counter++;
                        }
                        process = null;
                        //GC.Collect();
                    }
                    break;
            }
        }

        private string GetProcessName()
        {
            IntPtr cur = GetForegroundWindow();

            if (cur == null)
            {
                return "";
            }
            else
            {
                uint pId;
                GetWindowThreadProcessId(cur, out pId);

                foreach (System.Diagnostics.Process pro in System.Diagnostics.Process.GetProcesses())
                {
                    if (pro.Id == pId)
                    {
                        if (pro.ProcessName.Equals("ShellExperienceHost"))
                        {
                            return "";
                        }
                        else if (pro.ProcessName.Equals("ApplicationFrameHost"))
                        {
                            return pro.MainWindowTitle;
                        }
                        else
                        {

                            return pro.ProcessName;
                        }
                    }
                    //GC.Collect();
                }
            }
            return "";
        }

        private void LstProcessWindow_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

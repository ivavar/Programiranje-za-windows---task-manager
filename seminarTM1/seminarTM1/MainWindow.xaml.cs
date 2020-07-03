using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows.Threading;
using System.Timers;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading;
using System.Drawing.Drawing2D;

namespace seminarTM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            listBox3.Items.Add(Environment.UserName + " - " + Environment.MachineName);
        }

        Process[] procs;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GetProcesses();

            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            GetProcesses();
            PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

            dynamic firstValue = cpuCounter.NextValue();
            System.Threading.Thread.Sleep(1000);
            dynamic secondValue = cpuCounter.NextValue();

            progressBar1.Value = secondValue;

            PerformanceCounter ramCounter = new PerformanceCounter("Memory", "% Committed Bytes In Use");
            float fram = ramCounter.NextValue();
            progressBar2.Value = (int)fram;

            label4.Content = Math.Round(secondValue) + "%";
            label9.Content = Math.Round(secondValue) + "%";
            label6.Content = Math.Round(ramCounter.NextValue()) + "%";
            label10.Content = Math.Round(ramCounter.NextValue()) + "%";

        }

        private void GetProcesses()
        {

            procs = Process.GetProcesses();
            if (Convert.ToInt32(label1.Content) != procs.Length)
            {
                listBox1.Items.Clear();
                for (int i = 0; i < procs.Length; i++)
                {
                    if (!String.IsNullOrEmpty(procs[i].MainWindowTitle))
                    {
                        listBox1.Items.Add(procs[i].Id + "-" + procs[i].MainWindowTitle);  
                    }

                    listBox2.Items.Add(procs[i].Id + "-" + procs[i].ProcessName);
                }
                label1.Content = procs.Length;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (Process p in Process.GetProcesses())
                {
                    string[] arr1 = listBox1.SelectedItem.ToString().Split('-');
                    int iId1 = Convert.ToInt32(arr1[0].Trim());
                    if (p.Id == iId1)
                        p.Kill();
                }

                GetProcesses();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Message");
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Window1 f2 = new Window1();
            f2.ShowDialog();
        }

        private void exitTaskManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void taskManagerHelpTopicsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("http://www.microsoft.com");
            }
            catch { }
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (Process p in Process.GetProcesses())
                {
                    string[] arr = listBox2.SelectedItem.ToString().Split('-');
                    int iId1 = Convert.ToInt32(arr[0].Trim());
                    if (p.Id == iId1)
                        p.Kill();
                }

                listBox2.Items.RemoveAt(listBox2.SelectedIndex);
                GetProcesses();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Message");
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            Window1 f2 = new Window1();
            f2.ShowDialog();
        }

        private void minimazeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void maximazeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = WindowState.Maximized;
        }

        private void refreshNowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Refresh();
            listBox2.Items.Refresh();
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            button1.IsEnabled = true;
        }

        private void listBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            button4.IsEnabled = true;
        }
    }
}

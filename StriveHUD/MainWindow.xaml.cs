using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace StriveHUD
{

    public partial class MainWindow : Window
    {

        private static readonly Tuple<int, int[]> wallOnP1 = Tuple.Create(0x4C6E4F8, new int[] { 0x110, 0x0, 0xCFA0 });
        private static readonly Tuple<int, int[]> wallOnP2 = Tuple.Create(0x4C6E4F8, new int[] { 0x110, 0x8, 0xCFA0 });

        public int wallP1 = 0;
        public int wallP2 = 0;

        public MainWindow()
        {
            InitializeComponent();

            var hb = new BackgroundWorker();
            hb.WorkerReportsProgress = true;

            hb.DoWork += new DoWorkEventHandler(HeartBeat);
            hb.ProgressChanged += new ProgressChangedEventHandler(DoEvents);
            hb.RunWorkerAsync();
            
            WindowState = WindowState.Maximized;

        }

        private void HeartBeat(object sender, DoWorkEventArgs e)
        {
            var strive = MemoryLib.MemoryHandler.OpenProcessByName("GGST-Win64-Shipping", true);
            while ((sender as BackgroundWorker).CancellationPending == false)
            {
                try
                {
                    IntPtr wallP1Ptr = strive.GetAddressWithOffsets(wallOnP1.Item1, wallOnP1.Item2);
                    IntPtr wallP2Ptr = strive.GetAddressWithOffsets(wallOnP2.Item1, wallOnP2.Item2);

                    System.Threading.Thread.Sleep(10);

                    wallP1 = strive.ReadMemory<int>(wallP1Ptr);
                    wallP2 = strive.ReadMemory<int>(wallP2Ptr);
                    
                    (sender as BackgroundWorker).ReportProgress(0);

                }
                catch (Exception)
                {
                    wallP1 = 0;
                    wallP2 = 0;
                }
            }
        }

        private void DoEvents(object sender, ProgressChangedEventArgs e)
        {
            
            barP1.Value = wallP1;
            lblP1.Content = wallP1;
            
            barP2.Value = wallP2;
            lblP2.Content = wallP2;

            var converter = new System.Windows.Media.BrushConverter();

            try
            {

                switch (barP1.Value)
                {
                    case var expression when barP1.Value <= 2200:
                        barP1.Foreground = (Brush)converter.ConvertFromString("#FF06B225");
                        break;
                    case var expression when barP1.Value <= 2860:
                        barP1.Foreground = (Brush)converter.ConvertFromString("#FFFFAE00");
                        break;
                    case var expression when barP1.Value > 2860:
                        barP1.Foreground = (Brush)converter.ConvertFromString("#FFC92500");
                        break;
                    default:
                        barP1.Foreground = (Brush)converter.ConvertFromString("#7F000000");
                        break;
                }

                switch (barP2.Value)
                {
                    case var expression when barP2.Value <= 2200:
                        barP2.Foreground = (Brush)converter.ConvertFromString("#FF06B225");
                        break;
                    case var expression when barP2.Value <= 2860:
                        barP2.Foreground = (Brush)converter.ConvertFromString("#FFFFAE00");
                        break;
                    case var expression when barP2.Value > 2860:
                        barP2.Foreground = (Brush)converter.ConvertFromString("#FFC92500");
                        break;
                    default:
                        barP2.Foreground = (Brush)converter.ConvertFromString("#7F000000");
                        break;
                }

            }
            catch (Exception)
            {
                wallP1 = 0;
                wallP2 = 0;
                lblP1.Content = "----";
                lblP2.Content = "----";
            }
        }
    }

}

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
using System.Windows.Threading;


namespace VideoPlayerAndEditor
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        string path = "";
        List<TimeSpan> timey = new List<TimeSpan>();
        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += new EventHandler(timer_Tick);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (timey.Count % 2 != 1)
            {
                for (int i = 0; i < timey.Count; i += 2)
                {
                    if (mediaElement1.Position > timey[i] && mediaElement1.Position < timey[i + 1])
                    {
                        mediaElement1.Position = timey[i + 1];
                    }
                }
            }
            Snake.Value = mediaElement1.Position.TotalSeconds;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (mediaElement1.Source == null) { }
            else
            {
                mediaElement1.Play();
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (mediaElement1.Source == null) { }
            else
            {
                mediaElement1.Pause();
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (mediaElement1.Source == null) { }
            else
            {
                mediaElement1.Stop();
            }
        }

        private void Volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaElement1.Volume = (double)Volume.Value;
            
        }

        private void Snake_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaElement1.Position = TimeSpan.FromSeconds(Snake.Value);
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            string filename = (string)((DataObject)e.Data).GetFileDropList()[0];
            mediaElement1.Source = new Uri(filename);
            path = System.IO.Path.GetFullPath(filename);

            mediaElement1.LoadedBehavior = MediaState.Manual;
            mediaElement1.UnloadedBehavior = MediaState.Manual;
            mediaElement1.Volume = (double)Volume.Value;
            mediaElement1.Play();
        }

        private void mediaElement1_MediaOpened(object sender, RoutedEventArgs e)
        {
            TimeSpan ts = mediaElement1.NaturalDuration.TimeSpan;
            Snake.Maximum = ts.TotalSeconds;
            timer.Start();
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box 
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            dialog.FileName = "Videos"; // Default file name 
            //dialog.DefaultExt = ".WMV"; // Default file extension 
            //dialog.Filter = "WMV file (.wm)|*.wmv"; // Filter files by extension  
            
            // Show open file dialog box 
            Nullable<bool> result = dialog.ShowDialog();

            // Process open file dialog box results  
            if (result == true)
            {
                // Open document  
                mediaElement1.Source = new Uri(dialog.FileName);
                path = System.IO.Path.GetFullPath(dialog.FileName);
                mediaElement1.LoadedBehavior = MediaState.Manual;
                mediaElement1.UnloadedBehavior = MediaState.Manual;
                mediaElement1.Volume = (double)Volume.Value;
                mediaElement1.Play();
            } 
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            listBox1.Items.Add(mediaElement1.Position.TotalSeconds);
            timey.Add(mediaElement1.Position);
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            if (listBox1.Items.IsEmpty)
            {
                // do nothing
            }
            else
            {
                listBox1.Items.RemoveAt(listBox1.Items.Count - 1);
                timey.RemoveAt(timey.Count-1);
            }
        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            string temp = path;
            string removeExt = "";
            foreach (char x in temp)
            {
                if (x == '.')
                {
                    break;
                }
                removeExt += x;
            }
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@removeExt + ".txt", false))
            {
                foreach (var line in listBox1.Items)
                {
                    file.WriteLine(line);
                }
            }
            MessageBox.Show("Details have been saved");
            
        }

        private void button8_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box 
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            dialog.FileName = "Videos"; // Default file name 
            //dialog.DefaultExt = ".WMV"; // Default file extension 
            //dialog.Filter = "WMV file (.wm)|*.wmv"; // Filter files by extension  

            // Show open file dialog box 
            Nullable<bool> result = dialog.ShowDialog();

            // Process open file dialog box results  
            if (result == true)
            {
                // Open document  
                mediaElement1.Source = new Uri(dialog.FileName);
                path = System.IO.Path.GetFullPath(dialog.FileName);//saves the path in a globalish variable

                string temp = path;
                string removeExt = "";
                foreach (char x in temp)
                {
                    if (x == '.')
                    {
                        break;
                    }
                    removeExt += x;
                }

                string[] lines = System.IO.File.ReadAllLines(@removeExt+".txt");

                // Display the file contents by using a foreach loop.
                
                foreach (string line in lines)
                {
                    // Use a tab to indent each line of the file.
                    timey.Add(new TimeSpan(0, 0, 0, Convert.ToInt32(line.Split('.')[0]),0));
                    listBox1.Items.Add(line);
                    
                }

                mediaElement1.LoadedBehavior = MediaState.Manual;
                mediaElement1.UnloadedBehavior = MediaState.Manual;
                mediaElement1.Volume = (double)Volume.Value;
                mediaElement1.Play();
            } 
        }

        private void button9_Click(object sender, RoutedEventArgs e)
        {
            foreach (var line in listBox1.Items)
            {

            }
        }
    }
}

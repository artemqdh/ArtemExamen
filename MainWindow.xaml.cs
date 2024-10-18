using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Threading;

namespace artem_examen
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer _timer;

        public struct POINT
        {
            public int X;
            public int Y;
        }

        [DllImport("user32.dll")] ///< Importing the user32.dll library
        public static extern bool GetCursorPos(out POINT lpPoint); ///< Function to get position of cursor

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 3); ///< 3 seconds
            _timer.Tick += new EventHandler(Timer_Tick); ///< Every 3 seconds the data gets saved to file
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            SaveDataToFile();
        }

        private void StartProgram()
        {
            StartTimer();
        }

        /*!
         * @brief Saves the current time and mouse coordinates to coords.txt.
         */
        private void SaveDataToFile()
        {
            try
            {
                string data = $"{GetTimeNow()}:{GetMouseCoordinates()}"; ///< Format: "dd-MM-yyyy:HH-mm-ss:X-Y"
                string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

                string filePath = Path.Combine(currentDirectory, "coords.txt");

                File.AppendAllText(filePath, data + Environment.NewLine); ///< Environment.NewLine = "\n"
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data to file: {ex.Message}");
            }
        }

        /*!
         * @brief Retrieves the current cursor position as a string.
         * return string - The X and Y coordinates of the mouse as a formatted string.
         */
        private string GetMouseCoordinates()
        {
            POINT mousePoint;
            if (GetCursorPos(out mousePoint)) ///< Function of "user32.dll" library
            {
                return $"{mousePoint.X}-{mousePoint.Y}";
            }
            return "error";
        }

        /*!
         * @brief Retrieves the current time as a formatted string.
         * return time - The current time in "dd-MM-yyyy:HH-mm-ss" format.
         */
        private string GetTimeNow()
        {
            string time = DateTime.Now.ToString("dd-MM-yyyy:HH-mm-ss");

            return time;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StartProgram();
            this.Hide();
        }
    }
}
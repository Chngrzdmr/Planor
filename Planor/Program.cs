using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Planor
{
    static class Program
    {
        /// <summary>
        /// Uygulamanın ana girdi noktası.
        /// </summary>
        private static Mutex mutex = new Mutex(true, "{8F6F0AC4-B9A1-45fd-A8CF-72F04E6BDE8F}");

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [STAThread]
        static void Main()
        {
            try
            {
                if (mutex.WaitOne(TimeSpan.Zero, true))
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new LoginForm());
                }
                else
                {
                    ShowWindow(GetConsoleWindow(), 0); // hide console window
                    MessageBox.Show("Program sadece bir defa açılabilir.");
                }

                mutex.ReleaseMutex();
            }
            catch (Exception ex)
            {
                // handle any exceptions that might occur
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}

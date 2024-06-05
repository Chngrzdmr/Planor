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
                    // Hide console window
                    var handle = GetConsoleWindow();
                    ShowWindow(handle, 0);

                    // Show error message
                    MessageBox.Show("Program sadece bir defa açılabilir.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // Release mutex
                    mutex.ReleaseMutex();

                    // Close application
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                // Log error
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                // Release mutex
                mutex.ReleaseMutex();
            }
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();
    }
}

using System.Runtime.InteropServices;
using System.Threading;
using Moonlight.Core.Import;

namespace Moonlight.Example
{
    public static class DllMain
    {
        [DllExport]
        public static void Main()
        {
            Thread thread = new Thread(() =>
            {
                Kernel32.AllocConsole();
                App app = new App();

                app.InitializeComponent();
                app.Run();
            });
            
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlightSimulatorApp.server
{
    public static class DelayedExecutionService
    {
        public static void DelayedExecute(Action action, int delay = 5000)
        {
            new Thread(() =>
            {
                Thread.Sleep(delay);
                action();
            }).Start();
        }
    }
}

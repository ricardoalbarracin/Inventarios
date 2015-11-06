using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;


namespace ClassLibraryLog
{
    public class ClassPerformanceCounter : IDisposable
    {
        private PerformanceCounter cpuCounter;
        private PerformanceCounter ramCounter;
        private Timer timerCheckProcessor = new Timer();
        private int cpuUsage = 0;
        private bool disposed = false;

        public ClassPerformanceCounter()
        {
            cpuCounter = new PerformanceCounter();

            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";

            ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            timerCheckProcessor.Interval = 1000;
            timerCheckProcessor.Elapsed += timerCheckProcessorElapsed;
            timerCheckProcessor.Start();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            timerCheckProcessor.Stop();
            timerCheckProcessor.Dispose();
            cpuCounter.Dispose();
            ramCounter.Dispose();
        }

        public string getCurrentCpuUsage()
        {
            return cpuUsage.ToString() + "%";
        }

        public string getAvailableRAM()
        {
            return ramCounter.NextValue()+"MB";
        } 

        private void timerCheckProcessorElapsed(object sender, EventArgs e)
        {
            cpuUsage = (int)cpuCounter.NextValue();
        }
    }
}

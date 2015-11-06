using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Concurrent;

namespace ClassLibraryLog
{
    public enum LogLevel : int
    {
        DEBUG = 0,
        INFO = 1,
        WARN = 2,
        ERROR = 3,
        FATAL = 4
    }

    public class ClassLogger
    {
        private class ClassLoggingInfo
        {
            public LogLevel level;
            public string message;

            public ClassLoggingInfo (LogLevel level, string message)
            {
                this.level = level;
                this.message = message;
            }
        }

        [DllImport("kernel32.dll")]
        static extern void OutputDebugString(string lpOutputString);
        public static bool ActivarDebug { get; set; }
        private static bool Ini = false;
        private static ILog log = LogManager.GetLogger(typeof(ClassLogger));
        private static BlockingCollection<ClassLoggingInfo> loggingQueue = new BlockingCollection<ClassLoggingInfo>();
        private static bool stopThreads = false;
        private static Thread logThread;

        private static bool enablePerformanceCounter = false;
        private static ClassPerformanceCounter performanceCounter;

        public static void HabilitarPerformanceCounter()
        {
            performanceCounter = new ClassPerformanceCounter();
            enablePerformanceCounter = true;
        }

        public static void StopThreads()
        {
            stopThreads = true;
        }

        public static void LogMsg(string textToShow, LogLevel level = LogLevel.DEBUG)
        {
            try
            {
                if (enablePerformanceCounter) textToShow += ". P: " + performanceCounter.getCurrentCpuUsage() + "; RAM: " + performanceCounter.getAvailableRAM();
                StackTrace stackTrace = new StackTrace();
                if (textToShow.Length > 0)
                {
                    ClassLogger.LogMsg(level, System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.Name, stackTrace.GetFrame(1).GetMethod().Name, textToShow);
                }
            }
            catch (Exception e)
            {
                OutputDebugString("Error log4net" + e.ToString());
            }
        }

        public static void LogMsg(int level, System.Reflection.MethodBase metodo, string msg, string AppName = "")
        {
            try
            {
                string NameSpace = metodo.DeclaringType.Namespace;
                string ClassName = metodo.DeclaringType.Name;
                string MetodName = metodo.Name;
                string appname = AppDomain.CurrentDomain.FriendlyName.Replace(".exe", "").Replace(".vshost", "");
                if (AppName.Trim().Length > 0)
                    log4net.GlobalContext.Properties["NameAplication"] = AppName;
                else
                    GlobalContext.Properties["NameAplication"] = appname;

                try
                {
                    if (!Ini)
                    {
                        log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.xml"));
                        logThread = new Thread(new ThreadStart(() =>
                        {
                            WriteLogThread();
                        })) { Name = "logThread" };
                        logThread.Start();
                        Ini = true;
                    }
                }
                catch (Exception e)
                {
                    OutputDebugString("Error log4net" + e.ToString());
                }

                string message = string.Format("{0}:{1}:{2}:{3}: {4}", appname, NameSpace, ClassName, MetodName, msg);
                if (enablePerformanceCounter) message += ". P: " + performanceCounter.getCurrentCpuUsage() + "; RAM: " + performanceCounter.getAvailableRAM();
                loggingQueue.Add(new ClassLoggingInfo((LogLevel)level, message));
            }
            catch
            {
                return;
            }

        }
        /// <summary>
        /// Sobrecarga de funcion encargada de publicar log en archivo y en DebugView
        /// </summary>
        /// <param name="level">Nivel de Log</param>
        /// <param name="servicio">Cadena que identifica al programa que esta enviando el log</param>
        /// <param name="funcion">Cadena que identifica las clases y funciones que estan siendo llamadas</param>
        /// <param name="messageLog">Mensaje a ser publicado en el log</param>
        /// <param name="activeLogger">Parametro opcional que permite activar o no la publicacion en el debugView</param>
        public static void LogMsg(LogLevel level, string servicio, string funcion, string messageLog, bool activeLogger = true) 
        {
            try
            {
                //messageLog += ". P: " + performanceCounter.getCurrentCpuUsage() + "; RAM: " + performanceCounter.getAvailableRAM();
                string NameSpace = servicio;
                string ClassName = funcion;
                string appname = AppDomain.CurrentDomain.FriendlyName.Replace(".exe","");
                ActivarDebug = activeLogger;
                log4net.GlobalContext.Properties["NameAplication"] = appname;
          
                try
                {
                    if (!Ini)
                    {
                        log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.xml"));
                        logThread = new Thread(new ThreadStart(() => { WriteLogThread(); }));
                        logThread.Name = "logThread";
                        logThread.Start();
                        Ini = true;
                    }
                }
                catch (Exception e)
                {
                    OutputDebugString("Ultrapark Error log4net" + e.ToString()); 
                }

                string message = string.Format("{0}:{1}:{2}: {3}", appname, NameSpace, ClassName, messageLog);
                if (enablePerformanceCounter) message += ". P: " + performanceCounter.getCurrentCpuUsage() + "; RAM: " + performanceCounter.getAvailableRAM();
                loggingQueue.Add(new ClassLoggingInfo(level, message));
            }
            catch 
            {
                return;
            }
        }

        private static void WriteLogThread()
        {
            ActivarDebug = true;

            while (stopThreads == false)
            {
                ClassLoggingInfo loggingInfo;
                if (loggingQueue.TryTake(out loggingInfo, 200) == true)
                {
                    switch (loggingInfo.level)
                    {
                        case LogLevel.DEBUG:
                            if (ActivarDebug)
                            {
                                OutputDebugString(string.Format("{0}:{1}", LogLevel.DEBUG, loggingInfo.message));
                            }
                            log.Debug(loggingInfo.message);
                            break;

                        case LogLevel.INFO:
                            if (ActivarDebug)
                            {
                                OutputDebugString(string.Format("{0}:{1}", LogLevel.INFO, loggingInfo.message));
                            }
                            log.Info(loggingInfo.message);
                            break;

                        case LogLevel.WARN:
                            if (ActivarDebug)
                            {
                                OutputDebugString(string.Format("{0}:{1}", LogLevel.WARN, loggingInfo.message));
                            }
                            log.Warn(loggingInfo.message);
                            break;

                        case LogLevel.ERROR:
                            if (ActivarDebug)
                            {
                                OutputDebugString(string.Format("{0}:{1}", LogLevel.ERROR, loggingInfo.message));
                            }
                            log.Error(loggingInfo.message);
                            break;

                        case LogLevel.FATAL:
                            if (ActivarDebug)
                            {
                                OutputDebugString(string.Format("{0}:{1}", LogLevel.FATAL, loggingInfo.message));
                            }
                            log.Fatal(loggingInfo.message);
                            break;
                    }    
                }
            }
        }
    }
}

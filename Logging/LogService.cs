using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager.Logging
{
    
    public class LogService
    {
        public static String LogPath;

        private object synchLogEntry;

        private object synchBackup;

        private static LogService instance = null;

        private LogService()
        {
            synchLogEntry = new object();
            synchBackup = new object();
        }

        public static LogService Instance()
        {
            if (instance == null)
                instance = new LogService();

            return instance;

        }

        #region Writing

        public void WriteEntry(System.Diagnostics.EventLogEntryType type, String message, String applicationName)
        {

            lock (synchLogEntry)
            {

                try
                {
                    String logFile = "";
                    DateTime curr = DateTime.Now;

                    if (type == System.Diagnostics.EventLogEntryType.Error)
                        logFile = createErrorLogFile(curr, applicationName);
                    else
                        logFile = createLogFile(curr, applicationName);

                    string dateTime = string.Format("{0:yyyy}-{0:MM}-{0:dd} {0:HH}:{0:mm}:{0:ss}.{0:fff} ",
                            curr);

                    using (System.IO.StreamWriter w = new System.IO.StreamWriter(logFile, true))
                    {
                        w.WriteLine(dateTime + message);
                        //w.Flush();
                        w.Close();
                    }

                }
                catch (Exception ex)
                {
                    String error = ex.Message;
                }
            }


        }

        /// <summary>
        /// Moves file to backup folder in log destination of application
        /// </summary>
        /// <param name="applicationName">Name of application</param>
        /// <param name="pathSourceFile">Full path of source file to move</param>
        public void ArchiveFile(String applicationName, String pathSourceFile)
        {
            lock (synchBackup)
            {

                try
                {
                    System.IO.FileInfo fi = new System.IO.FileInfo(pathSourceFile);

                    DateTime curr = DateTime.Now;

                    String pathToMove = System.IO.Path.Combine(createBackupFolder(curr, applicationName), fi.Name);

                    System.IO.File.Copy(pathSourceFile, pathToMove, true);
                    System.IO.File.Delete(pathSourceFile);


                }
                catch (Exception ex)
                {
                    String error = ex.Message;
                }
            }
        }

        #endregion

        #region Create Files

        /// <summary>
        /// Creates a backup folder according to date and application name
        /// </summary>
        /// <param name="t">DateTime for path name</param>
        /// <param name="applicationName">Application name for the path</param>
        /// <returns></returns>
        private String createBackupFolder(DateTime t, String applicationName)
        {
            String dtString = t.ToString("yyyyMMdd");

            String path = LogPath;

            String dir = path + "\\" + dtString + "\\" + applicationName + "\\Backup";

            if (!System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);

            return dir;
        }

        private String createLogFile(DateTime t, String applicationName)
        {
            String dtString = t.ToString("yyyyMMdd");

            String path = LogPath;

            String dir = path + "\\" + dtString + "\\" + applicationName;

            if (!System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);

            path = dir + "\\" + applicationName + "_" + dtString + ".log";

            return path;
        }

        private String createErrorLogFile(DateTime t, String applicationName)
        {
            String dtString = t.ToString("yyyyMMdd");

            String path = LogPath;

            String dir = path + "\\" + dtString + "\\" + applicationName;

            if (!System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);

            path = dir + "\\" + applicationName + "_Error" + "_" + dtString + ".log";

            /* if (!System.IO.File.Exists(path))
                 System.IO.File.Create(path);*/

            return path;
        }

        #endregion
    }
}

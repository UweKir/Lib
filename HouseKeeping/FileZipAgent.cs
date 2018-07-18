using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Ionic.Zip;


namespace DataManager.HouseKeeping
{
    public class FileZipAgent
    {
        /// <summary>
        /// Jobs for the zipWorker,
        /// generated from configuraton
        /// </summary>
        private List<ZipJob> lstJobs;
        private System.Timers.Timer zipWorker;

        /// <summary>
        /// Configurations 
        /// </summary>
        private String strPathes;  
        private String strDaysToZip;
        private String strDaysToDelete;

        private object synchObject;

        private bool isRunning;

        public FileZipAgent(String strPathList, String strLstDaysToZip, String strLstDaysToDelete)
        {
            this.strPathes = strPathList;
            this.strDaysToZip = strLstDaysToZip;
            this.strDaysToDelete = strLstDaysToDelete;

            synchObject = new object();
            isRunning = false;

            zipWorker = new System.Timers.Timer();
            zipWorker.Elapsed += zipWorker_Elapsed;
            zipWorker.Interval = 60000;

        }

        private void zipWorker_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (isRunning)
                return;

            lock (synchObject)
            {
                isRunning = true;

                try
                {
                    foreach (ZipJob job in lstJobs)
                    {
                        DirectoryInfo root = new DirectoryInfo(job.RootPath);

                        if (root.Exists)
                            zipDirectory(root, job);

                    }
                }
                catch (Exception)
                { }

                zipWorker.Interval = 36000000 * 4;
                isRunning = false;
            }


        }

        /// <summary>
        /// Reads the configuration and creates the zipJobs
        /// </summary>
        /// <returns>bool - valid zipJobs are found in config</returns>
        public bool init()
        {
            bool success = false;

            lstJobs = new List<ZipJob>();

            string[] lstPathes = strPathes.Split(';');
            string[] lstDaysToZip = strDaysToZip.Split(';');
            string[] lstDaysToDelete = strDaysToDelete.Split(';');

            // create the zip jobs
            if (lstPathes.Count() == lstDaysToZip.Count() && lstPathes.Count() == lstDaysToDelete.Count())
            {
                for (int i = 0; i < lstPathes.Count(); i++)
                {
                    ZipJob job = new ZipJob();
                    job.RootPath = lstPathes[i];
                    job.AgeDaysToZip = Convert.ToInt32(lstDaysToZip[i]);
                    job.AgeDaysToDelete = Convert.ToInt32(lstDaysToDelete[i]);

                    // job is only done, if age to zip > 0
                    if (job.AgeDaysToZip > 0)
                        lstJobs.Add(job);
                }
            }

            //valid jobs are found
            if (lstJobs.Count > 0)
                success = true;

            return success;
        }

        public void Start()
        {
            zipWorker.Start();
        }

        public void Terminate()
        {
            zipWorker.Stop();
        }

        private void zipDirectory(DirectoryInfo parent, ZipJob job)
        {

            DirectoryInfo[] lst = parent.GetDirectories();
            FileInfo[] lstFi = parent.GetFiles();

            // first try to delete old files in folder
            if (job.AgeDaysToDelete > 0)
            {
                foreach (FileInfo fi in lstFi)
                {
                    TimeSpan diff = DateTime.Now - fi.CreationTime;

                    if (diff.TotalDays > job.AgeDaysToDelete)
                        fi.Delete();
                }
            }

            try
            {
                // No subfolders in folder anymore, try to zip the 
                // the current parent, only inf parent is not the global root folder
                if (lst.Count() == 0 && parent.FullName != job.RootPath)
                {
                    DateTime dt = parent.CreationTime;

                    TimeSpan diff = DateTime.Now - dt;

                    // Zip if the folder is old enough to zip
                    if (diff.TotalDays > job.AgeDaysToZip)
                    {
                        string zipFilePath = parent.FullName + ".zip";

                        ZipFile zip = new ZipFile();

                        lstFi = parent.GetFiles();

                        foreach (FileInfo fi in lstFi)
                        {
                            zip.AddFile(fi.FullName, @"\");
                        }

                        zip.Save(zipFilePath);

                        // file is zipped, delete the files
                        foreach (FileInfo fi in lstFi)
                        {
                            fi.Delete();
                        }

                        // finally delete the folder
                        parent.Delete();

                    }


                }

                foreach (DirectoryInfo di in lst)
                {
                    zipDirectory(di, job);
                }
            }
            catch (Exception)
            { }

        }

    }
}

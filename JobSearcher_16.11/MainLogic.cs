using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSearcher_16._11
{
    public class MainLogic
    {
        public String[] BestSearchTerms = { "ללא נסיון", "מתחיל/ה", "מתחיל /ה", "ללא ניסיון", "ג'וניור", "Junior", "junior", "0-", "1-", "Up to", "up to", " עד ", "graduate", " שנת " };
        public String[] BadTerms = { "בכיר/ה", "בכיר /ה", "ראש צוות", "Team Leader", "מנוסים /ות", "מנוסים/ות", "מנוסה", "מומח /ית", "בכיר /ה", "senior", "Senior", "team lead", "Team Lead", "Lead", "שנים", "years", "שנות", "expert", "Expert", "בכיר", "ר\"צ", "ש\"נ", "QA", "qa", "אוטומציה", "בדיק", "utomation", "Java", "JAVA", "PHP" };

        public int currentPageID { get; set; }
        // public event EventHandler CreateTable;

        public JobMaster jobMaster { get; set; }
        public List<Job> RecommendedJobs { get; set; }
        public List<Job> IrrelevantJobs { get; set; }
        string jobSiteChangedPage { get; set; }    
        public JobSite currentJobSite { get; set; }
        public JobSearcher_16._11.Helpers.MySQLManager sqlm { get; set; }
        public DateTime today { get; set; }
        public string msgForUser { get; set; }
        public string StatusLabel { get; set; }
        public string NumPagesLabel { get; set; }
        public bool wasPageIdChanged { get; set; }
        public bool CreateTable { get; set; }



        public MainLogic()
        {
            wasPageIdChanged = false;
            currentPageID = 0;
            jobMaster = new JobMaster();

            IrrelevantJobs = new List<Job>();
            RecommendedJobs = new List<Job>();


            jobSiteChangedPage = "";
            sqlm = new Helpers.MySQLManager();
        }

        public void changePageID(int i_id)
        {
            currentPageID= i_id;
            wasPageIdChanged = true;
        }

        private void checkIfPageWasChanged()
        {
            if (wasPageIdChanged)
            {
                currentJobSite.pageId = currentPageID;
                wasPageIdChanged = false;

            }

        }
        public void JobMasterWasChosen()
        {
            currentJobSite = jobMaster;
            CheckIfTableWasCreated();
            checkIfPageWasChanged();
            string currentURL = jobMaster.createSiteURL();

            List<Job> jobsToSort = jobMaster.readJobs(currentURL);
            
            SortJobs(jobsToSort);
            
            int idStr = jobMaster.pageId - 1;
            NumPagesLabel = "JobMaster - Page No. " + idStr;
            
        }

        private void CheckIfTableWasCreated()
        {
            if (CreateTable == true)
            {
                CreateTable = false;
                RecommendedJobs = new List<Job>();
                IrrelevantJobs = new List<Job>();
            }
        }

        private void SortJobs(List<Job> i_jobsToSort)
        {
            List<Job> jobsToSort = i_jobsToSort;
            foreach (Job currentJob in jobsToSort)
            {
                sortJob(currentJob);
            }
            StatusLabel = RecommendedJobs.Count.ToString() + " Jobs So Far";
        }

        public bool checkIfToCreateTable()
        {
            if ((CreateTable == false) && (RecommendedJobs.Count >= 10))
            {
                CreateTable = true;
                return CreateTable;
            }
            else
            {
                return false;
            }
        }



         private bool checkIfRecommanded(Job i_currentJob)
        {
            Job currentJob = i_currentJob;
            string info = currentJob.Requirements;
            string title = currentJob.Title;
            bool isBest = false;
            for (int i = 0; i < BestSearchTerms.Length; i++)
            {
                int TermIndexInfo = -1;
                int TermIndexTitle = -1;
                TermIndexInfo = info.IndexOf(BestSearchTerms[i]);
                TermIndexTitle = title.IndexOf(BestSearchTerms[i]);

                if (TermIndexInfo != -1 || TermIndexTitle != -1)
                {
                    isBest = true;
                    currentJob.isRecommended = true;
                    break;
                }

            }
            if (isBest == false)
            {
                bool badTermExist = false;
                for (int i = 0; i < BadTerms.Length; i++)
                {
                    int TermIndexInfo = -1;
                    int TermIndexTitle = -1;
                    TermIndexInfo = info.IndexOf(BadTerms[i]);
                    TermIndexTitle = title.IndexOf(BadTerms[i]);

                    if (TermIndexInfo != -1 || TermIndexTitle != -1)
                    {
                        badTermExist = true;
                        break;
                    }

                }
                if (badTermExist == false)
                {
                    isBest = true;
                }

            }

            return isBest;
        }

         //update data on DB
        public void changeDate(string i_day, string i_month, string i_currentJobID)
         {
             //currentJobSite.SiteName,i_currentJobID

             sqlm.changeDate(currentJobSite.SiteName, i_currentJobID, i_day, i_month);


         }

         public void changetoIrrelevant(string i_currentJobID)
         {
             sqlm.changeJobtoIrrelevat(currentJobSite.SiteName, i_currentJobID);
             sqlm.insertJobToTable(currentJobSite.SiteName, i_currentJobID);
         }

         public string checkIfSent(string i_currentJobID)
         {

             List<string> res = sqlm.CheckJob(currentJobSite.SiteName, i_currentJobID);

             if (res != null)
             {
                 string dateSTR = res[0];
                 string Relevancy = res[1];
                 bool isRelevant = true;
                 if (Relevancy == "2")
                 {
                     isRelevant = false;
                     //return "Irrelevant";
                 }
                 if (dateSTR != null)
                 {
                     DateTime LastSent = Convert.ToDateTime(dateSTR);
                     TimeSpan tspan = today - LastSent;
                     if (isRelevant == true)
                     {

                         if (dateSTR == "0/0/0000 00:00:00")
                         {
                             return "Viewed Not Sent";

                         }
                         else if (dateSTR == "0")
                         {
                             return "Not Viewed";
                         }

                         if (tspan.TotalDays > 30)
                         {

                             return "Sent Over 30 Days";
                         }
                          else if (tspan.TotalDays > 14)
                            {

                                return "Sent over 2 weeks";
                            }
                         else if (tspan.TotalDays <= 5)
                         {

                             return "Sent";
                         }
                     }
                     else
                     {
                         if (tspan.TotalDays > 14)
                         {

                             return "Irrelevant before 2 weeks";
                         }
                         else
                         {
                             return "Irrelevant";
                         }
                     }

                 }
             }


             return "Not Viewed";


         }

         public void insertOrUpdate(string i_currentJobID)
         {
             try
             {
                 sqlm.insertJobToTable(currentJobSite.SiteName, i_currentJobID);
             }
             catch (Exception e)
             {
                 throw (e);
             }
         }

         public void insertOrUpdateViewed(string i_currentJobID)
         {
             try
             {
                 sqlm.insertJobViewedToTable(currentJobSite.SiteName, i_currentJobID);
             }
             catch (Exception e)
             {
                 throw (e);
             }
         }

         //update that we changed the current job site


      



       

         //sort if this job is relevant - if keywords are correct, did I already send CV?
         public void sortJob(Job i_Job)
         {
             //Job currentJob = i_currentJob;
             Job job = i_Job;
             bool isbest = checkIfRecommanded(job);

             //if non of the terms was in the array
             if (isbest == false)
             {
                 IrrelevantJobs.Add(job);
             }
             else
             {
                 try
                 {
                     job.Status = checkIfSent(i_Job.ID);
                     if ((job.Status != "Sent") && (job.Status != "Irrelevant"))
                       {
                           RecommendedJobs.Add(job);
                       }
                 }
                 catch (Exception e)
                 {
                     throw (e);
                 }
                 /*  if (currentJobSite.SiteName == "Drushim")
                   {
                       if ((job.Status != "Sent") && (job.Status != "Irrelevant") && (job.Status != "Sent over 1 week"))
                       {
                           re.Add(job);
                       }
                   }
                   else
                   {
                       if ((currentJob.Status != "Sent") && (currentJob.Status != "Irrelevant"))
                       {
                           RecommendedJobs.Add(currentJob);
                       }

                   }*/
             }

         }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using System.Net;
using System.Text.RegularExpressions;
namespace JobSearcher_16._11
{
    public class Drushim:JobSite
    {
        public String siteURL { get; set; }
        //areas: shfela - 16,sharon -17,darom=4,markaz=1,
        public Drushim()
        {
            siteURL = "http://www.drushim.co.il/jobs/cat6/?area=1&page=";
            SpecificJobURL = "http://www.drushim.co.il/job/";
            pageId = 0;
            SiteName = "Drushim";
        }

        override public string createSiteURL()
        {
            string CompleteSiteURL = siteURL + pageId.ToString();
            pageId++;
            return CompleteSiteURL;
        }

         public bool checkIfRecommanded(Job i_currentJob)
        {
            bool isBest = false;
            Job curJob = i_currentJob;
            string experience = curJob.RequestedExperience;
            if (curJob.RequestedExperience == "1-2 שנים")
            {
                isBest = true;
            }
            else if (curJob.RequestedExperience == "ללא נסיון")
            {
                isBest = true;
                curJob.isRecommended = true;
            }
            else if (curJob.RequestedExperience == "ללא ניסיון")
            {
                isBest = true;
                curJob.isRecommended = true;
            }

            return isBest;
        }

        override public string getJobID(string i_stringWithId)
        {
            int slashIndex = -1;
            string IdPrefix = "/";
            string stringWithId = i_stringWithId;
            string currentjobID = "";
            slashIndex = stringWithId.IndexOf(IdPrefix);
            if (slashIndex != -1)
            {
                string[] substrings = Regex.Split(stringWithId, IdPrefix);
                if (substrings.Length > 1)
                {
                    currentjobID = substrings[4];
                }
            }

            return currentjobID;
        }

        public override List<Job> readJobs(string i_url)
        {
            List<Job> jobs = new List<Job>();

            WebClient GodLikeClient = new WebClient();
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();



            JobSearcher_16._11.Helpers.HttpDownloader downloader = new Helpers.HttpDownloader(i_url, null, null);
            document.LoadHtml(downloader.GetPage());

            HtmlNodeCollection HTMLjobs = document.DocumentNode.SelectNodes("//div[@class='jobContainer']");
            if (HTMLjobs != null)
            {
                foreach (var Hjob in HTMLjobs)
                {

                    HtmlNode curnode = Hjob;
                    Job DrushimJob = new Job();
                    HtmlNode titleHolder = curnode.SelectSingleNode("div//h2");
                    DrushimJob.Title = titleHolder.InnerText;

                    HtmlNode Company = curnode.SelectSingleNode("div//span[@class='fieldTitle']");
                    DrushimJob.Company = Company.InnerText;

                    HtmlNodeCollection description = curnode.SelectNodes("div/div/div[2]/span");
                    DrushimJob.Description = description[1].InnerText;

                    HtmlNodeCollection experience = curnode.SelectNodes("div/div/div[3]/span");
                    DrushimJob.RequestedExperience = experience[1].InnerText;

                    HtmlNode Id = curnode.SelectSingleNode("div//a");
                    DrushimJob.ID = Id.Attributes["jobcode"].Value;
                    DrushimJob.URL = SpecificJobURL + DrushimJob.ID;
                 
                    jobs.Add(DrushimJob);

                }
            }

            return jobs;
        }

    

    }
}

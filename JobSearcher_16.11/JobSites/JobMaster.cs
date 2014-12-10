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
     public class JobMaster: JobSite
    {
        public string SiteURL1 { get; set; }
        public string SiteURL2 { get; set; }
        public string CompleteSiteURL { get; set; }
        public String SpecificJobURL { get; set; }

        public JobMaster()
        {
            SpecificJobURL = "http://www.jobmaster.co.il/code/check/checknum.asp?key=";
            pageId = 1;
            SiteName = "JobMaster";
        }

        public override List<Job> readJobs(string i_url)
        {
            List<Job> jobs = new List<Job>();
            
            WebClient GodLikeClient = new WebClient();
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();



            JobSearcher_16._11.Helpers.HttpDownloader downloader = new Helpers.HttpDownloader(i_url, null, null);
            document.LoadHtml(downloader.GetPage());

            HtmlNodeCollection HTMLjobs = document.DocumentNode.SelectNodes("//table[@bgcolor='#B8B8B8']");

            if (HTMLjobs != null)
            {
                foreach (var Hjob in HTMLjobs)
                {
                    HtmlNode curnode = Hjob;
                    Job jmJob = new Job();
                    HtmlNode titleHolder = curnode.SelectSingleNode("tr[@class='jobTitle']//b");
                    jmJob.Title = titleHolder.InnerText.ToString();
                    HtmlNodeCollection desceHolders = curnode.SelectNodes("tr[@bgcolor='white']//div");
                    StringBuilder sbDesc = new StringBuilder();
                    foreach (var item in desceHolders)
                    {
                        sbDesc.Append(" ");
                        sbDesc.Append(item.InnerText.ToString());
                    }
                    jmJob.Description = sbDesc.ToString();
                    HtmlNode JobID = curnode.SelectSingleNode("tr[@class='jobTitle']//div[@style='position:absolute;margin-top:4px;margin-right:-18px;']");
                    string altId = JobID.GetAttributeValue("id", "");
                    jmJob.ID = getJobID(altId);
                    jmJob.URL = "http://www.jobmaster.co.il/code/check/checknum.asp?key=" + jmJob.ID;

                    HtmlNode Company = curnode.SelectSingleNode("tr[@bgcolor='white']//a/u/font");
                    if (Company != null)
                    {
                        jmJob.Company = Company.InnerText;
                    }
                    else
                    {
                        jmJob.Company = "Not Available";
                    }

                    jobs.Add(jmJob);
                }
            }
        

            return jobs;
        }

        override public string createSiteURL()
        {
            SiteURL1 = "http://www.jobmaster.co.il/code/check/search.asp?currPage=";
            SiteURL2 = "&headcatnum=15&ezor=3,%205,%2024,%2020,%2025,%2021,%2044,%2030,%2036,%2026,%2031,%2028,%2027&subcat=803,%20171,%20821,%20805,%20164,%20799,%20999&jobtype=1,%2022,%2015,%2024";
            string pageIdStr = pageId.ToString();
            CompleteSiteURL = SiteURL1 + pageIdStr + SiteURL2;
            pageId++;
            return CompleteSiteURL;
        }

        override public string getJobID(string i_currentTagId)
        {
            int IsCommunResult = -1;//top page - a free job container tag
            string FreeJobPattern = "commun";
            string currentTag = i_currentTagId;
            string ID = "";
            IsCommunResult = i_currentTagId.IndexOf(FreeJobPattern);
            if (IsCommunResult != -1)
            {
                string[] substrings = Regex.Split(i_currentTagId, FreeJobPattern);
                if (substrings.Length > 1)
                {
                    ID = substrings[1];
                }
            }

            return ID;
        }
    }
}

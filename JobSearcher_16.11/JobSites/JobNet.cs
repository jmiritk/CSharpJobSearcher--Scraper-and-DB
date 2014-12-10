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
    public class JobNet:JobSite
    {
        public string SiteURL1 { get; set; }
        public string SiteURL2 { get; set; }
        public string CompleteSiteURL { get; set; }
        public String SpecificJobURL { get; set; }

         public JobNet()
        {
            SpecificJobURL = "http://www.jobnet.co.il/positionresults.aspx?returned=true&positionid=";
            pageId = 0;
            SiteName = "Jobnet";

        }

         override public string createSiteURL()
        {

            SiteURL1 = "http://www.jobnet.co.il/positionresults.aspx?p=";
            SiteURL2 = "&searchpage=searchindex.aspx&subprofid=1472%2c987%2c686%2c689%2c994%2c729%2c1348%2c998%2c750%2c1434%2c1119%2c895%2c902%2c904%2c1433&checkarea=2%2c5%2c4&positiontype=0&checkextent=1%2c2%2c4%2c7&displayrad=fullview&savechoice=true";
            string pageIdStr = pageId.ToString();
            CompleteSiteURL = SiteURL1 + pageIdStr + SiteURL2;
            pageId++;
            return CompleteSiteURL;
        }

         public override List<Job> readJobs(string i_url)
         {
             List<Job> jobs = new List<Job>();

             WebClient GodLikeClient = new WebClient();
             HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();



             JobSearcher_16._11.Helpers.HttpDownloader downloader = new Helpers.HttpDownloader(i_url, null, null);
             document.LoadHtml(downloader.GetPage());

             HtmlNodeCollection HTMLjobs = document.DocumentNode.SelectNodes("//div[@vocab='http://schema.org/']");
             if (HTMLjobs != null)
             {
                 foreach (var Hjob in HTMLjobs)
                 {

                     HtmlNode curnode = Hjob;
                     Job jnJob = new Job();
                     HtmlNode titleHolder = curnode.SelectSingleNode("div//h2");
                     jnJob.Title = titleHolder.InnerText;

                     HtmlNode Company = curnode.SelectSingleNode("div//span");
                     jnJob.Company = Company.InnerText;

                     HtmlNode idHolder = curnode.SelectSingleNode("div//input");
                     jnJob.ID = idHolder.Attributes["value"].Value;
                     jnJob.URL = SpecificJobURL + jnJob.ID;
                     jnJob.Company = Company.InnerText;

                     HtmlNodeCollection desceHolders = curnode.SelectNodes("div//i");
                     StringBuilder sbDesc = new StringBuilder();
                     foreach (var item in desceHolders)
                     {
                         sbDesc.Append(" ");
                         sbDesc.Append(item.InnerText);
                     }

                     jnJob.Description = sbDesc.ToString();




                     jobs.Add(jnJob);

                 }
             }

             return jobs;
         }

    }
}

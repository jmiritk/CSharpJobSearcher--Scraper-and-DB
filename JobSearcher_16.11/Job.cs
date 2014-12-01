using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSearcher_16._11
{
    public class Job
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public string URL { get; set; }
        public string Requirements { get; set; }
        public string companyName { get; set; }
        public string RequestedExperience { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int comment { get; set; }
        public int SiteID { get; set; }
        public bool isRecommended { get; set; }
    

        public Job()
        {
            ID = "";
            Title = "";
            URL = "";
            Requirements = "";
            RequestedExperience = "";
            isRecommended = false;
            Status = "";
            Company = "";
            SiteID = -1;
            Description = "";
            comment = -1;
        }
    }
}

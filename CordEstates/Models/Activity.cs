using System;

namespace CordEstates.Models
{

    //TODO Admin main Dashboard show system activity
    public class Activity
    {
        public string ActivityType { get; set; }
        public string Summary { get; set; }
        public DateTime Date { get; set; }
        public int Id { get; set; }

    }
}

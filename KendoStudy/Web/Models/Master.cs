namespace Web.Models
{
    public class Master
    {
     
        public int AirlineMasterID { get; set; }

      
        public string AirlineCode { get; set; }

      
        public string AirlineName { get; set; }

     
        public string LPC { get; set; }

        public string IsOperational { get; set; } // Yes / NO

        public DateTime LastChangedAt { get; set; }
    }
}

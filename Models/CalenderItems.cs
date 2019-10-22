using System;
using System.ComponentModel.DataAnnotations;

namespace piDash.Models
{
    public class CalenderItem
    {
        [Required]
        public string Summary { get; set; }
        
        public string StartsAt { get; set; }

        public string ColorId { get; set; }
    }

}

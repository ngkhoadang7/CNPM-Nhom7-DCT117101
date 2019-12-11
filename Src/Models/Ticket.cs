using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieTheater.Models
{
    public class Ticket
    {
        public int ID { get; set;}

        public int ShowtimeID { get; set;}

        [Display(Name = "Ghế")]
        public string Seat { get; set; }

        public string UserID { get; set; }

        public string StaffID { get; set; }

        [Display(Name = "Mã lấy vé")]
        public string CodeToGet { get; set; }

        public bool Got {get; set;}

        public float Money {get; set;}

        [Display(Name = "Suất")]
        public Showtime Showtime { get; set; }

    }
}
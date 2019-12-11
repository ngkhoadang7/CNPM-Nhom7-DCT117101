using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace MovieTheater.Models
{
    public class Showtime
    {
        public int ID { get; set; }
        
        public int MovieID { get; set; }

        public int TheaterID { get; set; }
        
        [Required]
        [Display(Name="Suất")]
        public DateTime StartTime { get; set; }

        public int NoSeatBooked { get; set; }

        [Required]
        [Display(Name="Định dạng")]
        public string Format { get; set; }

        [Display(Name = "Phim")]
        public Movie Movie { get; set; }

        [Display(Name = "Rạp")]
        public Theater Theater { get; set; }

        /*public int NoSeatLeft
        {
            get
            {
                return Theater.NoSeat - NoSeatBooked;
            }
        }*/
    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace MovieTheater.Models
{
    public class Theater
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Phòng chiếu")]
        public string Name { get; set; }

        [Display(Name = "Số ghế")]
        public int NoSeat { get; set; }

        [Required]
        [Display(Name = "Định dạng")]
        public string Format { get; set; }
    }
}
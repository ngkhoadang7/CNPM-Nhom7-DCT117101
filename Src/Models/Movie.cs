using System;
using System.ComponentModel.DataAnnotations;

namespace MovieTheater.Models
{
    public class Movie
    {
        public string Poster { get; set; }

        public int ID { get; set; }

        [Required]
        [Display(Name = "Phim")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Đạo diễn")]
        public string Director { get; set; }

        [Required]
        [Display(Name = "Diễn viên")]
        public string Cast { get; set; }

        [Required]
        [Display(Name = "Thể loại")]
        public string Genre { get; set; }

        [Display(Name = "Khởi chiếu")]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [Display(Name = "Thời lượng")]
        public int Duration { get; set; }

        [Required]
        [Display(Name = "Ngôn ngữ")]
        public string Language { get; set; }

        [Required]
        public string Rated { get; set; }
        
        [Required]
        [Display(Name = "Tóm tắt")]
        public string Summary { get; set; }

        [Display(Name = "Tình trạng")]
        public bool Showing { get; set; } = true;

        //public ICollection<Showtime> Showtimes { get; set; }
    }
}
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieTheater.Models;
using System.Globalization;


namespace MovieTheater.Data
{
    public static class DbInitializer
    {
        public static void Initialize(MovieTheaterContext context)
        {
            //context.Database.EnsureCreated();

            // Look for any students.
            if (context.Movies.Any())
            {
                return;   // DB has been seeded
            }

            var movies = new Movie[]
            {
                new Movie
                {
                    Title = "KẺ HỦY DIỆT: VẬN MỆNH ĐEN TỐI",
                    Genre = "Hành Động",
                    Duration = 134,
                    ReleaseDate = new DateTime(2019,11,1),
                    Language = "Tiếng Anh với phụ đề tiếng Việt",
                    Rated = "C18",
                    Director = "Tim Miller",
                    Cast = "Linda Hamilton, Arnold Schwarzenegger, Mackenzie Davis, Gabriel Luna, Natalia Reyes, Diego Boneta",
                    Poster = "M01.jpg",
                    Summary = "Kẻ Hủy Diệt: Vận Mệnh Đen Tối - Terminator: Dark Fate. Chào mừng đến với thế giới hậu Ngày Phán Xét.",
                    Showing = true
                },
                new Movie
                {
                    Title = "MALEFICENT TIÊN HẮC ÁM",
                    Genre = "Phiêu Lưu, Thần thoại",
                    Duration = 125,
                    ReleaseDate = new DateTime(2019,10,18),
                    Language = "Tiếng Anh với phụ đề tiếng Việt",
                    Rated = "C13",
                    Director = "Joachim Rønning",
                    Cast = "Angelina Jolie, Elle Fanning, Michelle Pfeiffer, Ed Skrein, Chiwetel Ejiofor",
                    Poster = "M02.jpg",
                    Summary = "Thời gian trôi qua thật bình yên với Maleficent và Aurora. Mặc dù mối quan hệ của cả hai được tạo dựng từ những tổn thương, thù hận rồi sau đó mới đến tình yêu thương nhưng cuối cùng thì nó cũng đã đơm hoa kết trái. Tuy vậy, xung đột giữa hai giới: loài người và tiên tộc vẫn vẫn luôn hiện hữu. Cuộc hôn nhân vốn bị trì hoãn giữa Aurora và Hoàng tử Phillips chính là cầu nối gắn kết Vương quốc Ulstead và nước láng giềng Moors lại với nhau. Bất ngờ thay, sự xuất hiện của một phe đồng minh hoàn toàn mới sẽ khiến Maleficent và Aurora bị chia cắt về hai chiến tuyến trong trận Đại Chiến. Trận chiến này sẽ thử thách lòng tin lẫn tình cảm của cả hai. Liệu rằng họ có thật sự trở thành một gia đình hay không? Tất cả sẽ được giải đáp trong Maleficent: Mistress of Evil/ Tiên Hắc Ám 2.",
                    Showing = true
                },
                new Movie
                {
                    Title = "ĐẠI DỊCH TỬ THẦN",
                    Genre = "Hồi hộp, Kinh Dị",
                    Duration = 93,
                    ReleaseDate = new DateTime(2019,10,31),
                    Language = "Tiếng Anh với phụ đề tiếng Việt",
                    Rated = "C18",
                    Director = "Flavio Pedota",
                    Cast = "Rubén Guevara, Leonidas Urbina, Magdiel González",
                    Poster = "M03.jpg",
                    Summary = "Sự bùng phát của virus dại đột biến tạo nên một đại dịch, nhấn chìm thành phố trong hỗn loạn và sợ hãi. Một người cha phải tìm cách chiến đấu qua những khu vực nhiễm bệnh để giải cứu đứa con trai duy nhất của mình.",
                    Showing = true
                },
                new Movie
                {
                    Title = "PHÁP SƯ MÙ: AI CHẾT GIƠ TAY",
                    Genre = "Hài, Kinh Dị",
                    Duration = 120,
                    ReleaseDate = new DateTime(2019,11,8),
                    Language = "Tiếng Việt với phụ đề tiếng Anh",
                    Rated = "C18",
                    Director = "Lý Minh Thắng, Huỳnh Lập",
                    Cast = "NSND Ngọc Giàu, Đại Nghĩa, Việt Hương, Huỳnh Lập, Khả Như, Quang Trung, Hạnh Thảo, Lê Giang, Phương Thanh…",
                    Poster = "M04.jpg",
                    Summary = "Với thông điệp: “Thiện Căn ở tại lòng ta - Chữ Tâm kia mới bằng ba chữ Tài”, bộ phim điện ảnh Pháp sư mù sẽ tái hiện hành trình đi tìm lại ánh sáng cho đôi mắt của Tinh lâm và những câu chuyện nhân quả đan xen mà Tinh Lâm phải trải qua để thấu hiểu rõ hơn về cuộc đời. Tinh Lâm bị mù là do tai nạn hay có nguyên nhân bí ẩn nào khác? Ai sẽ cùng đồng hành với Tinh Lâm để tiếp tục con đường “trừ ma diệt quỷ”? Tất cả sẽ được giải đáp thỏa đáng vào ngày 08.11.2019 tới đây.",
                    Showing = true
                }
            };

            foreach (Movie m in movies)
            {
                context.Movies.Add(m);
            }
            context.SaveChanges();

            var theaters = new Theater[]
            {
                new Theater
                {
                    Name = "Cinema 06",
                    NoSeat = 30,
                    Format = "4DX"
                },
                new Theater
                {
                    Name = "Cinema 05",
                    NoSeat = 40,
                    Format = "3D"
                },
                new Theater
                {
                    Name = "Cinema 04",
                    NoSeat = 40,
                    Format = "3D"
                },
                new Theater
                {
                    Name = "Cinema 03",
                    NoSeat = 50,
                    Format = "2D"
                },
                new Theater
                {
                    Name = "Cinema 02",
                    NoSeat = 50,
                    Format = "2D"
                },
                new Theater
                {
                    Name = "Cinema 01",
                    NoSeat = 50,
                    Format = "2D"
                }
            };

            foreach (Theater t in theaters)
            {
                context.Theaters.Add(t);
            }
            context.SaveChanges();

            var showtimes = new Showtime[]
            {
                new Showtime
                {
                    MovieID = movies.Single(m => m.Title == "PHÁP SƯ MÙ: AI CHẾT GIƠ TAY").ID,
                    TheaterID = theaters.Single(t => t.Name =="Cinema 01").ID,
                    StartTime = new DateTime(2019,12,9,9,0,0),
                    NoSeatBooked = 0,
                    Format = "2D"
                },
                new Showtime
                {
                    MovieID = movies.Single(m => m.Title == "PHÁP SƯ MÙ: AI CHẾT GIƠ TAY").ID,
                    TheaterID = theaters.Single(t => t.Name =="Cinema 04").ID,
                    StartTime = new DateTime(2019,12,9,13,0,0),
                    NoSeatBooked = 0,
                    Format = "3D"
                },
                new Showtime 
                {
                    MovieID = movies.Single(m => m.Title == "PHÁP SƯ MÙ: AI CHẾT GIƠ TAY").ID,
                    TheaterID = theaters.Single(t => t.Name =="Cinema 01").ID,
                    StartTime = new DateTime(2019,12,9,17,0,0),
                    NoSeatBooked = 0,
                    Format = "2D"
                },
                new Showtime
                {
                    MovieID = movies.Single(m => m.Title == "KẺ HỦY DIỆT: VẬN MỆNH ĐEN TỐI").ID,
                    TheaterID = theaters.Single(t => t.Name =="Cinema 04").ID,
                    StartTime = new DateTime(2019,12,9,21,0,0),
                    NoSeatBooked = 0,
                    Format = "3D"
                },
            };

            foreach (Showtime s in showtimes)
            {
                context.Showtimes.Add(s);
            }
            context.SaveChanges();

            var tickets = new Ticket[]
            {
                new Ticket
                {
                    ShowtimeID = 4 ,
                    Seat ="1",
                    Money = 95000
                },
                new Ticket
                {
                    ShowtimeID = 4 ,
                    Seat ="2",
                    Money = 95000
                },
                new Ticket
                {
                    ShowtimeID = 3 ,
                    Seat ="2",
                    Money = 125000
                },
            };

            foreach (Ticket t in tickets)
            {
                context.Tickets.Add(t);
            }
            context.SaveChanges();
        }
    }
}
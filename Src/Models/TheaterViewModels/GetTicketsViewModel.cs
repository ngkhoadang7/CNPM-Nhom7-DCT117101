using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTheater.Models.TheaterViewModels
{
    public class GetTicketsViewModel
    {
        public int TicketID { get; set; }
        public bool IsSelected { get; set; }
    }
}
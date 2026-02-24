using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filminurk.Core.Dto
{
    public class OMDbDTO
    {
        public string? Title { get; set; }
        public string? Year { get; set; }
        public string? Plot { get; set; }
        public string? Director { get; set; }
        public string? Actors { get; set; }       // "Actor1, Actor2, Actor3"
        public string? Genre { get; set; }         // "Action, Drama"
        public string? imdbRating { get; set; }    // "9.0"
        public string? Released { get; set; }      // "18 Jul 2008"
        public string? Response { get; set; }      // "True" / "False"
        public string? Error { get; set; }
    }
}

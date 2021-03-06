using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace card_app.Models
{
    public class Deck
    {
        [Key]
        public int DeckId { get; set; }
        public string DeckName { get; set; }

        public ICollection<Card> Cards { get; set; }
    }

    public class DeckDto
    {
        public int DeckId { get; set; }
        public string DeckName { get; set; }
    }
}
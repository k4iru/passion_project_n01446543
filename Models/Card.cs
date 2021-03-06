using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace card_app.Models
{
    public class Card
    {
        [Key]
       
        public int CardId { get; set; }
        public string CardQuestion { get; set; }
        public string CardAnswer { get; set; }

        [ForeignKey("Deck")]
        public int DeckId { get; set; }
        public Deck Deck { get; set; }
    }

    public class CardDto
    {
        public int CardId { get; set; }

        [DisplayName("Card Question")]
        public string CardQuestion { get; set; }

        [DisplayName("Card Answer")]
        public string CardAnswer { get; set; }

        public int DeckID { get; set; }
    }
}
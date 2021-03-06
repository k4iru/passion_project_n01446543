using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace card_app.Models
{
    public class UserxDeck
    {
        [Key]
        public int UserxDeckId { get; set;
        }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("Deck")]
        public int DeckId { get; set; }
        public Deck Deck { get; set; }
    }
}
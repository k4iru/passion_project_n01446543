using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace card_app.Models.ViewModels
{
    public class ShowDeckCards
    {
        public DeckDto Deck { get; set; }

        public IEnumerable<CardDto> DeckCards { get; set; }
    }
}
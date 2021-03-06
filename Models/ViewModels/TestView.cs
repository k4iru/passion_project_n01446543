using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace card_app.Models.ViewModels
{
    public class TestView
    {
        public CardDto CorrectCard { get; set; }

        public IEnumerable<CardDto> AllCards { get; set; }
    }
}
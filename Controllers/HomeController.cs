using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace card_app.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // open to decks automatically
            return RedirectToAction("Index", "Decks");
        }
    }
}
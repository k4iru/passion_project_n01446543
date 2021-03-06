using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using card_app.Models;
using card_app.Models.ViewModels;
using Microsoft.AspNet.Identity;

namespace card_app.Controllers
{
    public class DecksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Decks
        public ActionResult Index()
        {

            string UserId = User.Identity.GetUserId();

            // only show decks that the user created
            var query = from Deck in db.Decks
                        join UserxDeck in db.UserxDecks on Deck.DeckId equals UserxDeck.DeckId
                        where UserxDeck.UserId == UserId
                        select Deck;

            var res = query.ToList();
            return View(res);
        }

        // GET: Decks/Details/5
        /// <summary>
        /// Get all the cards in the deck. From here you can add new cards, or edit / delete existing cards
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A view listing all cards in the deck</returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deck deck = db.Decks.Find(id);

            if (deck == null)
            {
                return HttpNotFound();
            }

            // deck dto
            DeckDto deckDto = new DeckDto
            {
                DeckId = deck.DeckId,
                DeckName = deck.DeckName
            };

            // get cards in deck
            List<Card> Cards = db.Cards.ToList();
            List<CardDto> CardDtos = new List<CardDto> { };

            foreach (var card in Cards)
            {
                if (card.DeckId == id)
                {
                    CardDto newCard = new CardDto
                    {
                        CardId = card.CardId,
                        CardAnswer = card.CardAnswer,
                        CardQuestion = card.CardQuestion,
                        DeckID = card.DeckId
                    };

                    CardDtos.Add(newCard);
                }
            }

            ShowDeckCards model = new ShowDeckCards
            {
                Deck = deckDto,
                DeckCards = CardDtos
            };

            return View(model);
        }

        // GET: Decks/Create
        /// <summary>
        /// create a new deck. Only registered users can create a deck
        /// </summary>
        /// <returns>Returns you to your list of decks</returns>
        [Authorize(Roles = "User")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Decks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DeckId,DeckName")] Deck deck)
        {
            if (ModelState.IsValid)
            {
                db.Decks.Add(deck);
                db.SaveChanges();

                // used as a bridging table to see what decks belong to which users
                // and only return decks that the user is associated with
                UserxDeck UxD = new UserxDeck
                {
                    UserId =  User.Identity.GetUserId(),
                    DeckId = deck.DeckId
                };

                db.UserxDecks.Add(UxD);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(deck);
        }

        // GET: Decks/Edit/5
        /// <summary>
        /// Edit the name of the Deck
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns list of Decks</returns>
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deck deck = db.Decks.Find(id);
            if (deck == null)
            {
                return HttpNotFound();
            }
            return View(deck);
        }

        // POST: Decks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DeckId,DeckName")] Deck deck)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deck).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(deck);
        }

        // GET: Decks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deck deck = db.Decks.Find(id);
            if (deck == null)
            {
                return HttpNotFound();
            }
            return View(deck);
        }

        // POST: Decks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Deck deck = db.Decks.Find(id);
            db.Decks.Remove(deck);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Grab 5 random cards from the deck. Assigns one as the Answer Card and lets the user choose from a list. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Test View</returns>
        public ActionResult Test(int? id)
        {
            // take 5 random cards from the deck
            List<Card> Cards = db.Cards.Where(m => m.DeckId == id).OrderBy(m => Guid.NewGuid()).Take(5).ToList();
            TestView model = new TestView();
            List<CardDto> CardDtos = new List<CardDto>();

            foreach (var card in Cards )
            {
                CardDto newCard = new CardDto
                {
                    CardId = card.CardId,
                    CardAnswer = card.CardAnswer,
                    CardQuestion = card.CardQuestion,
                    DeckID = card.DeckId
                };

                // set first card as correct card
                if (model.CorrectCard == null )
                {
                    model.CorrectCard = newCard;
                }
                CardDtos.Add(newCard);
            }

            // shuffle cards
            var shuffledCards = CardDtos.OrderBy(a => Guid.NewGuid()).ToList();
            model.AllCards = shuffledCards;
            return View(model);
        }

        /// <summary>
        /// once a user chooses an answer, if the guess is correct 
        /// the game continues, else the useris brought back to the home page
        /// </summary>
        /// <param name="GuessId">Guessed card</param>
        /// <param name="ActualId">Correct Answer</param>
        /// <param name="DeckId">Deck Id</param>
        /// <returns></returns>
        public ActionResult TestCard(int GuessId, int ActualId, int DeckId)
        {
            if (GuessId == ActualId )
            {
                return RedirectToAction("Test", "Decks", new { id =DeckId });
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}

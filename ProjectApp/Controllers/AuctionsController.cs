using Microsoft.AspNetCore.Mvc;
using ProjectApp.Core;
using ProjectApp.Core.Interfaces;
using ProjectApp.Models.Auctions;

namespace ProjectApp.Controllers
{
    public class AuctionsController : Controller
    {
        
        private IAuctionService _auctionService;

        public AuctionsController(IAuctionService auctionService)
        {
            _auctionService = auctionService;
        }
        // GET: AuctionsController
        public ActionResult Index()
        {
            List<Auction> auctions = _auctionService.GetAuctions();
            List<AuctionsVm> auctionsVms = new List<AuctionsVm>();

            foreach (Auction auction in auctions)
            {
                auctionsVms.Add(AuctionsVm.FromAuction(auction));
            }

            return View(auctionsVms);
        }
        
        [HttpGet("Auctions/IndexByUserBids")] 
        public ActionResult IndexByBid()
        {
                List<AuctionsFromBidVm> auctionsVms = new List<AuctionsFromBidVm>();

                try
                {
                    // Get auctions where the logged-in user has placed bids
                    List<Auction> auctions = _auctionService.GetAuctionsWhereBid(User.Identity.Name);

                    // Convert auctions to view models
                    foreach (Auction auction in auctions)
                    {
                        auctionsVms.Add(AuctionsFromBidVm.FromAuction(auction));
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "An error occurred while loading auctions. Please try again later.";
                }

                return View(auctionsVms);
            }

        

        // GET: AuctionsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AuctionsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AuctionsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AddAuctionVm addAuctionVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _auctionService.AddAuction(User.Identity.Name, addAuctionVm.Title, addAuctionVm.Price,addAuctionVm.EndDate);
                    return RedirectToAction("Index");
                }
                return View(addAuctionVm);
            }
            catch
            {
                return View(addAuctionVm);
            }
        }

        // GET: AuctionsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AuctionsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ChangeDescriptionAuctionVm changeDescriptionAuctionVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _auctionService.ChangeAuctionDecription(changeDescriptionAuctionVm.Id,changeDescriptionAuctionVm.description,User.Identity.Name);
                    return RedirectToAction("Index");
                }
                return View(changeDescriptionAuctionVm);
            }
            catch
            {
                return View(changeDescriptionAuctionVm);
            }
        }

        // GET: AuctionsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AuctionsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

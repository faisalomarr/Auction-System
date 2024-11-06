using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectApp.Core;
using ProjectApp.Core.Interfaces;
using ProjectApp.Models.Auctions;
using ProjectApp.Models.Bids;

namespace ProjectApp.Controllers
{    
    [Authorize] 
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
        
        [HttpGet("Auctions/MyAuctions")] 
        public ActionResult MyAuctions()
        {
            List<AuctionsVm> auctionsVms = new List<AuctionsVm>();

            try
            {
                // Get auctions that belongs to the logged-in user
                List<Auction> auctions = _auctionService.GetAuctionsOfUser(User.Identity.Name);

                // Convert auctions to view models
                foreach (Auction auction in auctions)
                {
                    auctionsVms.Add(AuctionsVm.FromAuction(auction));
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while loading auctions. Please try again later.";
            }

            return View(auctionsVms);
            
        }
        
        
        [HttpGet("Auctions/Won")] 
        public ActionResult MyAuctionsWon()
        {
            List<AuctionsVm> auctionsVms = new List<AuctionsVm>();

            try
            {
                // Get auctions that belongs to the logged-in user
                List<Auction> auctions = _auctionService.GetAuctionsWon(User.Identity.Name);

                // Convert auctions to view models
                foreach (Auction auction in auctions)
                {
                    auctionsVms.Add(AuctionsVm.FromAuction(auction));
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
            AuctionDetailsVm detailsVm = null;

            try
            {
                // Get the auction details using the provided ID
                Auction auction = _auctionService.GetAuctionById(id);

                if (auction == null)
                {
                    ViewBag.ErrorMessage = "Auction not found.";
                    return View(); // Return the view with ViewBag.ErrorMessage set
                }

                // Convert auction to view model
                detailsVm = AuctionDetailsVm.FromAuction(auction);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while loading auction details. Please try again later.";
            }

            return View(detailsVm);
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
                    _auctionService.AddAuction(User.Identity.Name, addAuctionVm.Title, addAuctionVm.Price,addAuctionVm.EndDate, addAuctionVm.Description);
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
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBid(AddBidVm addBidVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _auctionService.AddBid(User.Identity.Name, addBidVm.Price ,addBidVm.AuctionId);
                    return RedirectToAction("Index");
                }
                return View(addBidVm);
            }
            catch
            {
                return View(addBidVm);
            }
        }
        
        [HttpGet]
        public ActionResult CreateBid(int auctionId)
        {
            var bidVm = new AddBidVm
            {
                AuctionId = auctionId // Set the auction ID in the view model
            };
            return View(bidVm);
        }

    
    }
    
    
}

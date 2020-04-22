using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Subscrib_er.Data;
using Subscrib_er.Entities;
using Subscrib_er.Services.Interface;
using Subscrib_er.Web.ViewModel.Package;

namespace Subscrib_er.Web.Controllers
{
    //[Route("Package")]
    [Authorize]
    public class PackageController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRepository<Package> packageRepo;
        private readonly IRepository<ApplicationUser> subscriberDb;
        private readonly IRepository<Payments> paymentRepo;

        public PackageController(UserManager<ApplicationUser> userManager,
                                 IRepository<Package> packageRepo,
                                 IRepository<ApplicationUser> subscriberDb,
                                 IRepository<Payments> paymentRepo)
        {
            this.userManager = userManager;
            this.packageRepo = packageRepo;
            this.subscriberDb = subscriberDb;
            this.paymentRepo = paymentRepo;
        }

        //get user id
        public string UserId()
        {
            return userManager.GetUserId(User);
        }

        //get all active subscriptions
        //[Route("Active")]
        public ActionResult Active()
        {
            var model = packageRepo.GetAll().Where(u => u.userId == UserId() && u.Status == packagestate.Active).ToList();
            return View(model);
        }

        //[Route("Inactive")]
        //GET all Inactive subscription
        public ActionResult Inactive()
        {
            var model = packageRepo.GetAll().Where(u => u.userId == UserId() && u.Status == packagestate.Inactive).ToList();
            return View(model);
        }

        //[Route("Expired")]
        //GET all Expired subscriptions
        public ActionResult Expired()
        {
            var model = packageRepo.GetAll().Where(u => u.userId == UserId() && u.Status == packagestate.Expired).ToList();
            return View(model);
        }


        // GET: Package
        //[Route("Index")]
        public ActionResult Index()
        {
            var model = packageRepo.GetAll().Where(u => u.userId == UserId()).ToList();
            return View(model);
        }

        // GET: Package/Details/5
        //[Route("Details/{id?}")]
        public ActionResult Details(Guid id) 
        {
            //get the package      
            var package = packageRepo.GetEntityById(id);      
            //get the payment for that package
            var payment = paymentRepo.GetAll().Where(p => p.packageId == package.Id && p.userId == package.userId).FirstOrDefault();

            PackageModel packageModel = new PackageModel()
            {
                package = package,
                payments = payment
            };
            return View(packageModel);
        }


        //[Route("Create")]
        [HttpGet]
        // GET: Package/Create
        public ActionResult Create()
        {
            return View();
        }


        // POST: Package/Create
        //[Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PackageViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //set the package state pending if the user wishes to pay now or later
                    if (model.payNow == true)
                    {
                        model.packagestate = packagestate.Active;
                    }
                    else
                    {
                        model.packagestate = packagestate.Inactive;
                    }

                    Package package = new Package()
                    {
                        PackageName = model.PackageName,
                        DealerName = model.DealerName,
                        //Cost = model.Cost,
                        Status = model.packagestate,
                        Description = model.Description,
                        userId = UserId()
                    };

                    //insert new package to DB
                    Package newPackage =  packageRepo.Insert(package);
                    //redirect the user based on his chosen pay status
                    if (model.payNow == true)
                    {
                        return RedirectToAction("Pay", "Payments", new { id = newPackage.Id});
                    }
                    else
                    {
                        return RedirectToAction("index");
                    }
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // GET: Package/Edit/5
        //[Route("Edit/{id?}")]
        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            Package package = packageRepo.GetEntityById(id);
            PackageViewModel model = new PackageViewModel()
            {
                Id = package.Id,
                PackageName = package.PackageName,
                DealerName = package.DealerName,
                Description = package.Description,
                //Cost = package.Cost
            };
            return View(model);
        }

        // POST: Package/Edit/5
        //[Route("Edit/{id?}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PackageViewModel model)
        {
            try
            {
                // TODO: Add update logic here
                
                if (ModelState.IsValid)
                {
                    if (model.payNow == true)
                    {
                        model.packagestate = packagestate.Active;
                    }
                    else
                    {
                        model.packagestate = packagestate.Inactive;
                    }
                    //get the package to update first
                    Package package = packageRepo.GetEntityById(model.Id);

                    //update each attribute
                    package.PackageName = model.PackageName;
                    package.DealerName = model.DealerName;
                    //package.Cost = model.Cost;
                    package.Status = model.packagestate;
                    package.Description = model.Description;

                    //update the package
                    packageRepo.Update(package);

                    if (package.Status == packagestate.Active)
                    {
                        return RedirectToAction("Edit", "Payments", new { id = model.Id});
                    }
                }
                return RedirectToAction("Active");
            }
            catch
            {
                return View();
            }
        }

        // GET: Package/Delete/5
        //[Route("Delete/{id?}")]
        public ActionResult Delete(Guid id)
        {
            /*******************************
             * cos of the fluent api configuration between package and payment
             * having an object type of package in payment
             * we only have to write the code to delete the package
             * and its resulting payment(s) will also be deleted from their tables
             * *********************************************************************/

                //code to delete the package    
                packageRepo.Delete(id);
                return RedirectToAction("index");
        }        
    }
}
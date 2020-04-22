using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Subscrib_er.Data;
using Subscrib_er.Entities;
using Subscrib_er.Services.Interface;
using Subscrib_er.Web.ViewModel.Payments;

namespace Subscrib_er.Web.Controllers
{
    //[Route("Payments")]
    public class PaymentsController : Controller
    {
        private readonly UserManager<ApplicationUser> AppUser;
        private readonly IRepository<Payments> paymentRepo;
        private readonly IRepository<Package> packageRepo;

        public PaymentsController(UserManager<ApplicationUser> AppUser,
                                  IRepository<Payments> paymentRepo,
                                  IRepository<Package> packageRepo)
        {
            this.AppUser = AppUser;
            this.paymentRepo = paymentRepo;
            this.packageRepo = packageRepo;
        }

        //GET: USER ID
        public string UserID()
        {
            return AppUser.GetUserId(User);
        }
        
        //GET: Expiry date for a package
        public DateTime EndDate(PaymentViewModel model)
        {
            if (model.style == style.Daily)
            {
                model.EndDate = model.StartDate.AddDays(1);
            }
            else if (model.style == style.Weekly)
            {
                model.EndDate = model.StartDate.AddDays(7);
            }
            else if (model.style == style.Monthly)
            {
                model.EndDate = model.StartDate.AddMonths(1);
            }
            else if (model.style == style.Yearly)
            {
                model.EndDate = model.StartDate.AddYears(1);
            }

            return model.EndDate;
        }
        // GET: Payments
        public ActionResult Index()
        {
            return View();
        }

        // GET: Payments/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Payments/Create
        [Route("Pay/{id?}")]
        [HttpGet]
        public ActionResult Pay(Guid id)
        {
            PaymentViewModel payment = new PaymentViewModel()
            {
                packageID = id
            };
            return View(payment);
        }

        // POST: Payments/Create
        //[Route("Pay/{id?}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Pay(PaymentViewModel paymentViewModel)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {                  
                    Payments payments = new Payments()
                    {
                        //packageDuration = paymentViewModel.duration,
                        startDate = paymentViewModel.StartDate,
                        amount = paymentViewModel.Cost,
                        packageId = paymentViewModel.packageID,
                        PackageStyle = paymentViewModel.style,
                        userId = UserID(),
                        endDate = EndDate(paymentViewModel)
                    };

                    paymentRepo.Insert(payments);
                }

                return RedirectToAction("index", "Home");
            }
            catch
            {
                return View();
            }
        }

        // GET: Payments/Edit/5
        //[Route("Edit/{id?}")]
        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            Payments pay = paymentRepo.GetEntityById(id);
            PaymentViewModel model = new PaymentViewModel()
            {
                ID = pay.Id,
                packageID = pay.packageId,
                userID = pay.userId,
                Cost = pay.amount,
                StartDate = pay.startDate,
                style = pay.PackageStyle,
                EndDate = pay.endDate
            };
            return View(model);
        }

        // POST: Payments/Edit/5
        //[Route("Edit/{id?}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PaymentViewModel model)
        {
            //get payment
            Payments pay = paymentRepo.GetEntityById(model.ID);
            if (ModelState.IsValid)
            {
                pay.packageId = model.packageID;
                pay.userId = model.userID;
                pay.PackageStyle = model.style;
                pay.startDate = model.StartDate;
                pay.amount = model.Cost;
                pay.endDate = EndDate(model);

                //update
                paymentRepo.Update(pay);
                return RedirectToAction("Details", "Package");
            }
            return View();
        }

        // GET: Payments/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Payments/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
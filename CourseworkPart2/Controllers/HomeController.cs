using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CourseworkPart2.Models;

namespace CourseworkPart2.Controllers
{
    public class HomeController : Controller
    {
        //Database Instance to be used by whole controller class.
        CauseSiteDB causeSiteDb = new CauseSiteDB();

        //Users will need to log in/Register to access any of the pages other than the home page
        public ActionResult Index()
        {
            CauseSignatureViewModel causeSignatureViewModel = new CauseSignatureViewModel();
            causeSignatureViewModel.Causes = causeSiteDb.Causes.ToList();
            causeSignatureViewModel.Signatures = causeSiteDb.Signatures.ToList();
            return View("index", causeSignatureViewModel);
        }

        public ActionResult StartPetition()
        {

            return RedirectToAction("Login", "Member");
        }


        public ActionResult Register()
        {
            return RedirectToAction("Register", "Member");

        }
        public ActionResult Login()
        {
            return RedirectToAction("Login", "Member");
        }

        //Need to be logged in to search
        public ActionResult SearchCause()
        {
            if (Session["MemberId"] != null)
            {
                
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Member");

            }
            
        }


        //Need to be logged in to view so redirects to log in page if there is no member logged in.
        //else navigates to the CauseExpanded page, of the cause which has a matching id.
        public ActionResult CauseExpanded(int? id)
        {
            if (Session["MemberId"] == null)
            {
                return RedirectToAction("Login", "Member");
            }
            else
            {
                //pass only data relevant to the cause with the parameter id to the new view
                Cause cause = new Cause();
                cause = causeSiteDb.Causes.FirstOrDefault(c => c.Id == id);
                CauseSignatureViewModel causeSignatureViewModel = new CauseSignatureViewModel();

          
                causeSignatureViewModel.Signatures = causeSiteDb.Signatures.ToList();
                causeSignatureViewModel.Causes.Add(cause);
                return RedirectToAction("CauseExpanded", "Cause", causeSignatureViewModel);
                
            }
        }

        

        //This should not be reaches as the link specifies the action result in Cause controller.
        public ActionResult SignCause(int? id)
        {
            if(Session["MemberId"]== null)
            {
                return RedirectToAction("Login", "Member");
            }
            else
            {
                Cause cause = new Cause();
                cause = causeSiteDb.Causes.FirstOrDefault(c => c.Id == id);
                CauseSignatureViewModel causeSignatureViewModel = new CauseSignatureViewModel();

                causeSignatureViewModel.Signatures = causeSiteDb.Signatures.ToList();
                causeSignatureViewModel.Causes.Add(cause);
                return RedirectToAction("Index", "Cause", causeSignatureViewModel);
            }
            
        }
        





    }
}
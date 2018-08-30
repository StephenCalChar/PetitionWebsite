using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CourseworkPart2.Models;


namespace CourseworkPart2.Controllers
{
    public class MemberController : Controller
    {
        //Database Instance to be used by whole controller class.
        CauseSiteDB causeSiteDb = new CauseSiteDB();

        //Get
        public ActionResult Index()
        {
            return View(causeSiteDb.Members.ToList());
        }


        [HttpGet]
        public ActionResult Register(int id = 0)
        {
            //passes an instance of member object into view
            Member memberModel = new Member();
            return View(memberModel);
        }

        //adds the registered user to the DB.
        [HttpPost]
        public ActionResult Register(Member memberModel)
        {
            if (ModelState.IsValid)
            {
                //checks for duplicate usernames
                if (causeSiteDb.Members.Any(x => x.Username == memberModel.Username))
                {
                    ViewBag.DuplicateMessage = "Username Taken, Please Choose Another";
                    return View("Register", memberModel);
                    
                }
                causeSiteDb.Members.Add(memberModel);
                causeSiteDb.SaveChanges();

                //clear down modal for security
                ModelState.Clear();
                //stores various member properties in session
                Session["MemberId"] = memberModel.MemberId.ToString();
                Session["Username"] = memberModel.Username.ToString();

                CauseSignatureViewModel causeSignatureViewModel = new CauseSignatureViewModel();
                causeSignatureViewModel.Causes = causeSiteDb.Causes.ToList();
                causeSignatureViewModel.Signatures = causeSiteDb.Signatures.ToList();
                return RedirectToAction("Index", "Cause", causeSignatureViewModel);

            }
            else
            {
                //if modal has not been properly filled in it returns the same view
                return View();  

            }

        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }



       //recieved a memberModel with the properties of the username and password that have just been typed in.
        [HttpPost]
        public ActionResult Login(Member memberModel)
        {
            CauseSignatureViewModel causeSignatureViewModel = new CauseSignatureViewModel();
            causeSignatureViewModel.Causes = causeSiteDb.Causes.ToList();
            causeSignatureViewModel.Signatures = causeSiteDb.Signatures.ToList();

            //searches for a member with the same properties as the memberModel passed in.
            var member = causeSiteDb.Members.SingleOrDefault(u => u.Username == memberModel.Username && u.Password == memberModel.Password);
            //if a member is found, create a session. If not add a error message to the modal and return the same view
            if (member != null)
            {
                Session["MemberId"] = member.MemberId.ToString();
                Session["Username"] = member.Username.ToString();
                return RedirectToAction("Index", "Cause", causeSignatureViewModel);
            }
            else
            {
                ModelState.AddModelError("", "Username or Password Incorrect");
            }

            return View();

        }


        //Code adapted from Aristos post on 8 March 2012 on https://stackoverflow.com/questions/9621543/how-to-logout-the-user-from-my-asp-net-application 
        //ends session and logs out user, redirecting them to home page
        [HttpGet]
        public ActionResult LogOut()
        {
            Session["MemberId"] = null;
            Session.Abandon();
            Request.Cookies.Clear();
            CauseSignatureViewModel causeSignatureViewModel = new CauseSignatureViewModel();
            causeSignatureViewModel.Causes = causeSiteDb.Causes.ToList();
            causeSignatureViewModel.Signatures = causeSiteDb.Signatures.ToList();
            return RedirectToAction("Index", "Home", causeSignatureViewModel);
            
        }

        //passes a member object into view with the properties of the member which has the ID passed in as a parameter.
        [HttpGet]
        public ActionResult MemberProfile(int id)
        {
            Member member = new Member();
            member = causeSiteDb.Members.FirstOrDefault(c => c.MemberId == id);
            return View(member);
        }

    }
}
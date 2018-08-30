using CourseworkPart2.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace CourseworkPart2.Controllers
{
    public class CauseController : Controller
    {

        //Database Instance to be used by whole controller class.
        CauseSiteDB causeSiteDb = new CauseSiteDB();



        
        // GET: Cause
        public ActionResult Index()
        {
            // returns an instance of the causeSignatureModel containing all Signatures and Causes.
            CauseSignatureViewModel causeSignatureViewModel = new CauseSignatureViewModel();
            causeSignatureViewModel.Causes = causeSiteDb.Causes.ToList();
            causeSignatureViewModel.Signatures = causeSiteDb.Signatures.ToList();
            return View("Index", causeSignatureViewModel);
        }


        [HttpGet]
        public ActionResult NewCause(int id = 0)
        {
            //ensures a member is logged in, if not redirects to the log in page
            if (Session["MemberId"] == null)
            {
                return RedirectToAction("Login", "Member");
            }
            else
            {
                //creates a new cause object and passes it to view
                Cause causeModel = new Cause();
                return View(causeModel);
            }
        }

        [HttpPost]
        public ActionResult NewCause( Cause causeModel)
        {
            //attempt to allow picture uploads did not work and ran out of time.

            /*
            if (causeModel.Description == null || causeModel.CauseTitle ==null)
            {
                ViewBag.MissingItem = "Plese Enter A Title And Description";
                return View(causeModel);
            }
            if(ModelState.IsValid)
            {
                if(file!=null)
                {
                    try
                    {
                        string fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/UploadedImages/"), fileName);
                        file.SaveAs(path);
                        causeModel.ImagePath = path;     
                    }
                    catch
                    {

                    }
                }
                else
                {
                    causeModel.ImagePath = "~/UploadedImages/pension.jpg";
                }
            }

            */

            //sets every image to the same picture.
            causeModel.ImagePath = "~/UploadedImages/pension.jpg";

            //sets the time the cause was made.
            causeModel.CreatedOn = DateTime.Now;

            //finds current user object in database based on the session memberId and passes it to the currentUser variable.
            //So that the Member object can be saved inside the database as the Cause creator.
            int currentSessionId = Convert.ToInt32(Session["MemberId"]);
            Member currentUser = causeSiteDb.Members.FirstOrDefault(x => x.MemberId == currentSessionId);
            causeModel.CreatedBy = currentUser;


            //saves db after adding the cause object
            causeSiteDb.Causes.Add(causeModel);
            causeSiteDb.SaveChanges();


            CauseSignatureViewModel causeSignatureViewModel = new CauseSignatureViewModel();
            causeSignatureViewModel.Causes = causeSiteDb.Causes.ToList();
            causeSignatureViewModel.Signatures = causeSiteDb.Signatures.ToList();
            return RedirectToAction("Index", causeSignatureViewModel);
        }




        [HttpGet]
        public ActionResult CauseExpanded(int id)
        {
            //checks a member is logged in.
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
                return View(causeSignatureViewModel);
            }




        }
       
        public ActionResult SignCause(int? id)
        {
            if (Session["MemberId"] == null)
            {
                return RedirectToAction("Login", "Member");
            }
            //creates instance of member and assigns the current user's properties to it.
            int currentSessionId = Convert.ToInt32(Session["MemberId"]);
            Member currentUser = causeSiteDb.Members.FirstOrDefault(x => x.MemberId == currentSessionId);

            //finds cause based on parameters fed in.
            Cause cause = new Cause();
            cause = causeSiteDb.Causes.FirstOrDefault(c => c.Id == id);


            // check if user has already signed cause
            List<Signature> allSigs = causeSiteDb.Signatures.ToList();
            Boolean alreadySigned = false;
            foreach(Signature signature in allSigs)
            {
                if(signature.MemberId == currentUser.MemberId && signature.CauseId == cause.Id)
                {
                    alreadySigned = true;
                }
            }
            if (alreadySigned == true)
            {
                //if user has already signed cause another signature is not permitted. So they are redirected to the main page.
                //cause creators are permitted to sign their cause once.
                //add some sort of error message here
                CauseSignatureViewModel causeSignatureViewModel = new CauseSignatureViewModel();
                causeSignatureViewModel.Causes = causeSiteDb.Causes.ToList();
                causeSignatureViewModel.Signatures = causeSiteDb.Signatures.ToList();
                return RedirectToAction("Index", causeSignatureViewModel);
            }
            
            else
            {
                //if member has not signed the cause before, creates a new instance of signature.
                //sets the signatures properties based on the cause being signed and the current user.
                Signature signature = new Signature();
                signature.SignatureTime = DateTime.Now;
                signature.Member = currentUser;
                signature.Cause = cause;
                signature.MemberId = currentUser.MemberId;
                signature.CauseId = cause.Id;
                signature.MemberUsername = currentUser.Username;

                //saves db after adding the signature object
                causeSiteDb.Signatures.Add(signature);
                causeSiteDb.SaveChanges();
                
 
                CauseSignatureViewModel causeSignatureViewModel = new CauseSignatureViewModel();
                causeSignatureViewModel.Causes = causeSiteDb.Causes.ToList();
                causeSignatureViewModel.Signatures = causeSiteDb.Signatures.ToList();
                return RedirectToAction("index", causeSignatureViewModel);

            }

  
        }

        //Deletes a Cause.      
        public ActionResult DeleteCause(int id)
        {
            //Creates cause instance based on the id parameter passed in
            Cause cause = new Cause();
            cause = causeSiteDb.Causes.FirstOrDefault(c => c.Id == id);

            //Delete signatures first then the cause
            List<Signature> sigsToRemove = new List<Signature>();
            
            //appends all signatures that need deleting to a list that can be removed.
            //All signatures related to a cauase must be deleted before the cause can be removed.
            foreach(var signature in causeSiteDb.Signatures)
            {
                if(signature.Cause == cause)
                {
                    sigsToRemove.Add(signature);
                }
            }

            //removes the signatures and cause and saves changes.
            causeSiteDb.Signatures.RemoveRange(sigsToRemove);
            causeSiteDb.Causes.Remove(cause);
            causeSiteDb.SaveChanges();

            CauseSignatureViewModel causeSignatureViewModel = new CauseSignatureViewModel();
            causeSignatureViewModel.Causes = causeSiteDb.Causes.ToList();
            causeSignatureViewModel.Signatures = causeSiteDb.Signatures.ToList();
            return RedirectToAction("Index", causeSignatureViewModel);
        }


        [HttpGet]
        public ActionResult LogOut()
        {
            //ends current session.
            Session["MemberId"] = null;
            Session.Abandon();
            Request.Cookies.Clear();
            CauseSignatureViewModel causeSignatureViewModel = new CauseSignatureViewModel();
            causeSignatureViewModel.Causes = causeSiteDb.Causes.ToList();
            causeSignatureViewModel.Signatures = causeSiteDb.Signatures.ToList();
            return RedirectToAction("Index", "Home", causeSignatureViewModel);
        }


        //get search data
        [HttpGet]
        public ActionResult SearchCause(string searchString)
        {

            //Search code adapted from Rick Anderson's guide dated 03/07/2017 from https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/search 

            if (!String.IsNullOrEmpty(searchString))
            {
                List<Cause> searchResults = causeSiteDb.Causes.ToList();
                //searches for any Cause titles which contain the string that has been searched.
                searchResults = searchResults.Where(x => x.CauseTitle.ToString().ToLower().Contains(searchString.ToString().ToLower())).ToList();
                //if no results were found.
                if (searchResults.Count() == 0)
                {
                    ViewBag.NotFoundMessage = "Sorry, No Matches Were Found";
                    return View();
                }
                else
                {
                    //returns the same view and the Jquery displays the results below the search box.
                    return View(searchResults);
                }

                
            }
            else
            {
                return View();
              
            }
        }

        //Updates the signatures shown in the partial view. Refreshed with an AJAX get call.
        [HttpGet]
        public ActionResult updateSigs(int? id)
        {
            //Creates new instance of cause
            Cause cause = new Cause();
            cause = causeSiteDb.Causes.Find(id);
            CauseSignatureViewModel causeSignatureViewModel = new CauseSignatureViewModel();
            //finds all signatures in Database.
            //this would become unmanageable as the website gets bigger but will keep as this for now.
            causeSignatureViewModel.Signatures = causeSiteDb.Signatures.ToList();
            causeSignatureViewModel.Causes.Add(cause);
            return PartialView("_SignaturesPartialView", causeSignatureViewModel);

        }
        



    }
}
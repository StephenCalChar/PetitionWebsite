using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseworkPart2.Models
{
    public class CauseSignatureViewModel
    {
        public List<CourseworkPart2.Models.Signature> Signatures { get; set; }
        public List<CourseworkPart2.Models.Cause> Causes { get; set; }


        public CauseSignatureViewModel()
        {
            //initialises lists
            Signatures = new List<Signature>();
            Causes = new List<Cause>();
        } 

            





    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseworkPart2.Models
{
    public class Cause
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter a Title")]
        public String CauseTitle { get; set; }

        [Required(ErrorMessage = "Please Enter a Desciption")]
        public String Description { get; set; }

        public virtual Member CreatedBy { get; set; }

        public List<Signature> CauseSignees { get; set; }

        public DateTime CreatedOn { get; set; }

        [DisplayName("Upload File (Optional)")]
        public String ImagePath { get; set; }



        public Cause()
        {
            //initialises list
            CauseSignees = new List<Signature>();
        }



    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CourseworkPart2.Models
{
    public class Member
    {
        [Key]
        public int MemberId { get; set; }

        public String Name { get; set; }
        
        // Regular Expression for email check taken from OpenEducation's video posted 16 September 2015 on https://www.youtube.com/watch?v=Uq0y8oxnx-8 
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3]\.)|(([\w-]+\.)+))([a-zA-Z{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter valid email.")]
        [Required(ErrorMessage = "Please Enter an Email")]
        public String Email { get; set; }


        [Required(ErrorMessage ="Please Enter a Username")]
        public String Username { get; set; }


        [Required(ErrorMessage = "Please Enter a Password")]
        [DataType(DataType.Password)]
        public String  Password { get; set; }


        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        [Compare("Password")]
        public String ConfirmPassword { get; set; }

        public List<Signature> SignedCauses { get; set; }

        



        //constructor
        public Member()
        {
            //initialises list
            SignedCauses = new List<Signature>();

        }

    }


 
}
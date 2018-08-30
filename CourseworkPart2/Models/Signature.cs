using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseworkPart2.Models
{
    public class Signature
    {
        //had to de-normalise and store the memberUsername in the signature, so that when displaying signatures in a view the memberUsername
        //could be displayed instead of the memberId which is just a number.
        [Key]
        [Required]
        public int SignatureId { get; set; }

        
        public Cause Cause { get; set; }

        public int CauseId { get; set; }

        public Member Member { get; set; }

        public int MemberId { get; set; }
        public string MemberUsername { get; set; }

        public DateTime SignatureTime { get; set; }

    }
}
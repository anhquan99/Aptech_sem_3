using AptechSem3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AptechSem3.Validate
{
    public class CandidateValidation
    {
        public static bool validate(USR canidate)
        {
            try
            {
                //code here
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
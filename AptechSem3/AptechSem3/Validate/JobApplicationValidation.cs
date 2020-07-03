using AptechSem3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AptechSem3.Validate
{
    public class JobApplicationValidation
    {
        public static bool Validate(JOB_APPLICATION application)
        {
            if (!TextValidation.validateStringNumber(application.PERSONAL_ID)) return false;
            if (!TextValidation.validateStringNumber(application.PHONE)) return false;
            if (!TextValidation.validateStringDigit(application.NAME)) return false;
            if (!TextValidation.validateEmail(application.MAIL)) return false;
            return true;
        }
    }
}
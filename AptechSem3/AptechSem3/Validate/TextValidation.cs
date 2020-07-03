using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace AptechSem3.Validate
{
    public class TextValidation
    {
        public static bool validateStringNumber(String id)
        {
            try
            {
                foreach(char i in id)
                {
                    char.IsDigit(i);
                    return false;
                }
                if (id.Contains(" ")) return false;
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static bool validateStringDigit(String str)
        {
            try
            {
                foreach( var i in str)
                {
                    char.IsNumber(i);
                    return false;
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static bool validateEmail(String email)
        {
            try
            {
                MailAddress mail = new MailAddress(email);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
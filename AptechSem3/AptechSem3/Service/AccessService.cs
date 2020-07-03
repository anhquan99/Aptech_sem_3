using AptechSem3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AptechSem3.Service
{
    public class AccessService
    {
        public String Login(String username, String password)
        {
            UsrService service = new UsrService();
            USR usr = service.findByUsernameAndPassword(username, password);
            if (usr != null) return usr.ROLE;
            return "NULL";
        } 
    }
}
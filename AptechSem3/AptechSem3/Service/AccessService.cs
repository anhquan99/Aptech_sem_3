using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AptechSem3.Service
{
    public class AccessService
    {
        public bool CandidateLogin(String username, String password)
        {
            CandidateService service = new CandidateService();
            if (service.findByUsernameAndPassword(username, password) != null) return true;
            return false;
        } 
    }
}
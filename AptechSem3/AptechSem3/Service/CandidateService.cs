using AptechSem3.Models;
using AptechSem3.Validate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AptechSem3.Service
{
    public class CandidateService : UsrService, ICRUD<USR>
    {
        private APTECH_SEM_3Entities db = new APTECH_SEM_3Entities();
        public List<USR> findAll()
        {
            try
            {
                List<USR> list = (from p in db.USRs where p.ROLE == "CANDIDATE" select p).ToList();
                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
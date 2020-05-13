using AptechSem3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AptechSem3.Service
{
    public class JobPostService : ICRUD<JOB_POST>
    {
        private APTECH_SEM_3Entities db = new APTECH_SEM_3Entities();
        public bool Create(JOB_POST t)
        {
            throw new NotImplementedException();
        }

        public bool deleteById(string id)
        {
            throw new NotImplementedException();
        }

        public List<JOB_POST> findAll()
        {
            try
            {
                List<JOB_POST> list = (from p in db.JOB_POST select p).ToList();
                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public JOB_POST findById(string id)
        {
            throw new NotImplementedException();
        }

        public bool update(JOB_POST t)
        {
            throw new NotImplementedException();
        }
    }
}
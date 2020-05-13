using AptechSem3.Models;
using AptechSem3.Validate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AptechSem3.Service
{
    public class JobApplicationService : ICRUD<JOB_APPLICATION>
    {
        private APTECH_SEM_3Entities db = new APTECH_SEM_3Entities();
        public bool Create(JOB_APPLICATION t)
        {
            try
            {
                if (JobApplicationValidation.Validate(t))
                {
                    db.JOB_APPLICATION.Add(t);
                    if (db.SaveChanges() != 0) return true;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool deleteById(string id)
        {
            throw new NotImplementedException();
        }

        public List<JOB_APPLICATION> findAll()
        {
            throw new NotImplementedException();
        }

        public JOB_APPLICATION findById(string id)
        {
            throw new NotImplementedException();
        }

        public bool update(JOB_APPLICATION t)
        {
            throw new NotImplementedException();
        }
    }
}
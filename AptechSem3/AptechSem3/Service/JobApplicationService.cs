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
                t.CREATED = DateTime.Now;
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
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    var selected = (from p in db.JOB_APPLICATION where p.PHONE == t.PHONE && p.PERSONAL_ID == t.PERSONAL_ID select p).SingleOrDefault();
                    selected.MAIL = t.MAIL;
                    selected.NAME = t.NAME;
                    selected.WORK_EXP = t.WORK_EXP;
                    selected.EDUCATION = t.EDUCATION;
                    selected.ADDRESS = t.ADDRESS;
                    if (db.SaveChanges() == 0) throw new Exception("CAN NOT UPDATE APPLICATION");
                    return true;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static JOB_APPLICATION findByUsername(String username)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    var application = (from p in db.USRs where p.USERNAME == username select p).SingleOrDefault().JOB_APPLICATION;
                    return application;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static JOB_APPLICATION findByPersonalIdAndPhone(String personalId , String phone)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    JOB_APPLICATION application = (from p in db.JOB_APPLICATION where p.PERSONAL_ID == personalId && p.PHONE == phone select p).SingleOrDefault();
                    return application;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
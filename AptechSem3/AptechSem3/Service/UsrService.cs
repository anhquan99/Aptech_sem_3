using AptechSem3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace AptechSem3.Service
{
    public class UsrService : ICRUD<USR>
    {
        private APTECH_SEM_3Entities db = new APTECH_SEM_3Entities();

        public bool Create(USR usr)
        {
            throw new NotImplementedException();
        }

        public bool deleteById(string username)
        {
            try
            {
                var temp = (from p in db.USRs where p.USERNAME == username select p).SingleOrDefault();
                USR usr = (USR)temp;
                db.USRs.Remove(usr);
                if (db.SaveChanges() != 0) return true;
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<USR> findAll()
        {
            try
            {
                List<USR> list = (from p in db.USRs select p).ToList();
                return list;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public USR findById(string username)
        {
            try
            {
                var usr = (from p in db.USRs where p.USERNAME == username select p).SingleOrDefault();
                return usr;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool update(USR t)
        {
            try
            {
                var usr = (from p in db.USRs where p.USERNAME == t.USERNAME select p).SingleOrDefault();
                if (usr != null)
                {
                    usr.PASSWORD = t.PASSWORD;
                    if (db.SaveChanges() != 0) return true;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public USR findByUsernameAndPassword(String username, String password)
        {
            try
            {
                var list = db.USRs.ToList();
                var selected = (from p in db.USRs where p.USERNAME == username && p.PASSWORD == password select p).Single();
                return selected;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public int getApplyIdByUsername(String username)
        {
            try
            {
                int apply_id = (from p in db.USRs where p.USERNAME == "test-name" select p).SingleOrDefault().APPLY_ID ?? 0;
                if (apply_id == 0) throw new Exception("CAN NOT FOUND USER APPLY_ID");
                return apply_id;
            }
            catch (Exception)
            {

                throw;
            }
        }
        //background service to delete all user is finished test
        public static void deletebackgroundUsrNotAvailable()
        {
            try
            {
                while (true)
                {
                    using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                    {
                        DateTime t1 = DateTime.Now;
                        DateTime t2 = t1.AddMinutes(-1);
                        var list = db.FIND_USR_BY_TEST_END(t1, t2);
                        foreach (var i in list)
                        {
                            var temp = (from p in db.USRs where p.USERNAME == i.USERNAME select p).SingleOrDefault();
                            db.USRs.Remove(temp);
                        }
                        if (db.SaveChanges() == 0) throw new Exception("CAN NOT DELETE USR");
                    }
                    //sleep for 1 minute
                    Thread.Sleep(60000);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        
    }
}
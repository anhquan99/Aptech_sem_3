using System;
using AptechSem3.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AptechSem3.Service.ModelService
{
    public class UserService : IService<USR>
    {
        public bool create(USR t)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    db.USRs.Add(t);
                    if (db.SaveChanges() == 0) throw new Exception();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool deleteById(string username)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    var selectedUser = (from p in db.USRs where p.USERNAME == username select p).SingleOrDefault();
                    if (selectedUser != null)
                    {
                        db.USRs.Remove(selectedUser);
                        if (db.SaveChanges() == 0) throw new Exception();
                    }
                    else throw new NotImplementedException();
                    return true;

                }
            }
            catch (Exception ex)
            {
                throw ;
            }
        }

        public List<USR> findAll()
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    return (from p in db.USRs select p).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ;
            }
        }

        public USR findById(string username)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    var selectedUser = (from p in db.USRs where p.USERNAME == username select p).SingleOrDefault();
                    return selectedUser;
                }
            }
            catch (Exception ex)
            {
                throw ;
            }
        }

        public bool update(USR t)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    var selectedUser = (from p in db.USRs
                                        where p.USERNAME == t.USERNAME
                                        select p).SingleOrDefault();

                    if (selectedUser != null)
                    {
                        selectedUser.PASSWORD = t.PASSWORD;
                        selectedUser.APPLY_ID = t.APPLY_ID;
                        selectedUser.ROLE = t.ROLE;
                        if (db.SaveChanges() == 0) return false;
                        return true;
                    }
                    else throw new Exception();
                }
            }
            catch (Exception ex)
            {
                throw ;
            }
        }
        public USR findByUsernameAndPassword(String username, String password)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    var selectedUser = (from p in db.USRs where p.USERNAME == username && p.PASSWORD == password select p).SingleOrDefault();
                    return selectedUser;
                }

            }
            catch (Exception ex)
            {
                throw ;
            }
        }
        public bool findByApplyId(string applyId)
        {
            try
            {
                int id = Int32.Parse(applyId);
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    return (from p in db.USRs where p.APPLY_ID == id select p).Any();
                }

            }
            catch (Exception ex)
            {
                throw ;
            }
        }
        public USR findReportByApplyId(string applyId)
        {
            try
            {
                int id = Int32.Parse(applyId);
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    return (from p in db.USRs where p.APPLY_ID == id select p).SingleOrDefault();
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<USR> findByRole(string role)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    return (from p in db.USRs where p.ROLE == role select p).ToList();
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
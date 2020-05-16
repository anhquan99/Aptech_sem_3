﻿using AptechSem3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

                throw;
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
        
    }
}
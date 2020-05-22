using System;
using AptechSem3.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AptechSem3.Service.ModelService
{
    public class PostService : IService<JOB_POST>
    {
        public bool create(JOB_POST t)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    db.JOB_POST.Add(t);
                    if (db.SaveChanges() == 0) throw new Exception();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public bool deleteById(string postId)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    int id = Int32.Parse(postId);
                    var selectedPost = (from p in db.JOB_POST where p.POST_ID == id select p).SingleOrDefault();
                    if (selectedPost != null)
                    {
                        db.JOB_POST.Remove(selectedPost);
                        if (db.SaveChanges() == 0) throw new Exception();
                    }
                    else throw new NotImplementedException();
                    return true;

                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public List<JOB_POST> findAll()
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    return (from p in db.JOB_POST select p).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public JOB_POST findById(string postId)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    int id = Int32.Parse(postId);
                    var selectedPost = (from p in db.JOB_POST where p.POST_ID == id select p).SingleOrDefault();
                    return selectedPost;
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public bool update(JOB_POST t)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    var selectedPost = (from p in db.JOB_POST
                                        where p.POST_ID == t.POST_ID
                                        select p).SingleOrDefault();

                    if (selectedPost != null)
                    {
                        selectedPost.POST = t.POST;
                        selectedPost.CREATED = t.CREATED;
                        selectedPost.TITLE = t.TITLE;
                        selectedPost.END_DATE = t.END_DATE;
                        if (db.SaveChanges() == 0) return false;
                        return true;
                    }
                    else throw new Exception();
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }


    }
}
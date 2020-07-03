using System;
using AptechSem3.Models;
using System.Collections.Generic;
using AptechSem3.Models;
using System.Linq;
using System.Web;

namespace AptechSem3.Service.ModelService
{
    public class ApplicationService : IService<JOB_APPLICATION>
    {

        public bool create(JOB_APPLICATION t)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    db.JOB_APPLICATION.Add(t);
                    if (db.SaveChanges() == 0) throw new Exception();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool deleteById(string applyId)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    int id = Int32.Parse(applyId);
                    var selectedApplication = (from p in db.JOB_APPLICATION where p.APPLY_ID == id select p).SingleOrDefault();
                    if (selectedApplication != null)
                    {
                        db.JOB_APPLICATION.Remove(selectedApplication);
                        if (db.SaveChanges() == 0) throw new Exception();
                    }
                    else throw new NotImplementedException();
                    return true;

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<JOB_APPLICATION> findAll()
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    return (from p in db.JOB_APPLICATION select p).ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public JOB_APPLICATION findById(string applyId)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    int id = Int32.Parse(applyId);
                    var selectedApplication = (from p in db.JOB_APPLICATION where p.APPLY_ID == id select p).SingleOrDefault();
                    return selectedApplication;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool update(JOB_APPLICATION t)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    var selectedApplication = (from p in db.JOB_APPLICATION
                                               where p.APPLY_ID == t.APPLY_ID
                                               select p).SingleOrDefault();

                    if (selectedApplication != null)
                    {
                        selectedApplication.POST_ID = t.POST_ID;
                        selectedApplication.PERSONAL_ID = t.PERSONAL_ID;
                        selectedApplication.NAME = t.NAME;
                        selectedApplication.ADDRESS = t.ADDRESS;
                        selectedApplication.PHONE = t.PHONE;
                        selectedApplication.MAIL = t.MAIL;
                        selectedApplication.EDUCATION = t.EDUCATION;
                        selectedApplication.WORK_EXP = t.WORK_EXP;
                        selectedApplication.CREATED = t.CREATED;
                        selectedApplication.APPROVE_STATUS = t.APPROVE_STATUS;
                        if (db.SaveChanges() == 0) return false;
                        return true;
                    }
                    else throw new Exception();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<JOB_APPLICATION> findByStatus(int status)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    return (from p in db.JOB_APPLICATION where p.APPROVE_STATUS == status select p).ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        
        public bool apply(string applyId, int status)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    int id = Int32.Parse(applyId);
                    var selectedApplication = (from p in db.JOB_APPLICATION where p.APPLY_ID == id select p).SingleOrDefault();
                    if (selectedApplication != null)
                    {
                        if (status == 1)
                        {
                            selectedApplication.APPROVE_STATUS = 1;
                        }
                        else selectedApplication.APPROVE_STATUS = -1;
                        if (db.SaveChanges() == 0) throw new Exception();
                        return true;
                    }
                    else throw new NotImplementedException();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public string trim(string t)
        {
            try
            {
                char[] trimChars = { '/', ':', ' ' };
                return t.Trim(trimChars);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int getTestIdByApplyId(string applyId)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    int id = Int32.Parse(applyId);
                    var selectedPost = (from p in db.JOB_APPLICATION
                                        where p.APPLY_ID == id
                                        select p).SingleOrDefault();

                    int postId = selectedPost.POST_ID;
                    var selectedTest = (from t in db.TESTs where t.POST_ID == postId select t).SingleOrDefault();
                    if (selectedTest != null)
                    { 
                        return selectedTest.TEST_ID;

                    }
                    else throw new Exception();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public List<CustomApplication> findAllCustomApplications()
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    var selectedApplications = (from p in db.GET_RESULT_WITH_APPLY() select p).ToList();
                    List<CustomApplication> list = new List<CustomApplication>();
                    if (selectedApplications != null)
                    {
                        foreach (var a in selectedApplications)
                        {
                            CustomApplication application = new CustomApplication() { APPLY_ID = a.APPLY_ID, NAME = a.NAME, APPROVE_STATUS = a.APPROVE_STATUS, TEST_INDEX= a.TEST_INDEX, TEST_RESULT_1 = a.TEST_RESULT_1, TEST_RESULT_2 = a.TEST_RESULT_2, TEST_RESULT_3 = a.TEST_RESULT_3 };
                            list.Add(application);
                        }

                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public List<CustomApplication> findCustomApplicationsWithCondition(int condition)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    var selectedApplications = (from p in db.GET_RESULT_WITH_APPLY() select p).ToList();
                    List<CustomApplication> list = new List<CustomApplication>();
                    if (selectedApplications != null)
                    {
                        foreach (var a in selectedApplications)
                        {
                            if (a.APPROVE_STATUS == condition)
                            { 
                                CustomApplication application = new CustomApplication() { APPLY_ID = a.APPLY_ID, NAME = a.NAME, APPROVE_STATUS = a.APPROVE_STATUS, TEST_INDEX = a.TEST_INDEX, TEST_RESULT_1 = a.TEST_RESULT_1, TEST_RESULT_2 = a.TEST_RESULT_2, TEST_RESULT_3 = a.TEST_RESULT_3 };
                                list.Add(application);

                            }
                        }

                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
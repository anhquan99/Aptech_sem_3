using System;
using AptechSem3.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AptechSem3.Service.ModelService
{
    public class ResultService : IService<RESULT>
    {
        public bool create(RESULT t)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    db.RESULTs.Add(t);
                    if (db.SaveChanges() == 0) throw new Exception();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ;
            }
        }

        public bool deleteByApplyIdAndTestId(string applyId, string testId)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    int aid = Int32.Parse(applyId);
                    int tid = Int32.Parse(testId);

                    var selectedResult = (from p in db.RESULTs where p.APPLY_ID == aid && p.TEST_ID == tid select p).SingleOrDefault();
                    if (selectedResult != null)
                    {
                        db.RESULTs.Remove(selectedResult);
                        if (db.SaveChanges() == 0) throw new Exception();
                    }
                    else throw new Exception();
                    return true;

                }
            }
            catch (Exception ex)
            {
                throw ;
            }
        }

        public bool deleteById(string id)
        {
            throw new NotImplementedException();
        }

        public List<RESULT> findAll()
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    return (from p in db.RESULTs select p).ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public RESULT findByApplyIdAndTestId(string applyId, string testId)
        {

            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    int aid = Int32.Parse(applyId);
                    int tid = Int32.Parse(testId);
                    var selectedResult = (from p in db.RESULTs where p.APPLY_ID == aid && p.TEST_ID == tid select p).SingleOrDefault();
                    return selectedResult;
                }
            }
            catch (Exception ex)
            {
                throw ;
            }
        }

        public RESULT findById(string id)
        {
            throw new NotImplementedException();
        }

        public bool update(RESULT t)
        {
            throw new NotImplementedException();
        }

        public bool updateByApplyIdAndTestId(RESULT t)
        {

            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    var selectedResult = (from p in db.RESULTs
                                          where p.APPLY_ID == t.APPLY_ID && p.TEST_ID == t.TEST_ID
                                          select p).SingleOrDefault();

                    if (selectedResult != null)
                    {
                        selectedResult.TEST_INDEX = t.TEST_INDEX;
                        selectedResult.TEST_RESULT_1 = t.TEST_RESULT_1;
                        selectedResult.TEST_RESULT_2 = t.TEST_RESULT_2;
                        selectedResult.TEST_RESULT_3 = t.TEST_RESULT_3;
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

        public List<RESULT> findResultHigh(string category)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    if (category == "GENERAL KNOWLEDGE")
                    {
                        return (from p in db.RESULTs where p.TEST_RESULT_1 >= 80 select p).ToList();

                    }
                    else if (category == "MATHEMATICS")
                    {
                        return (from p in db.RESULTs where p.TEST_RESULT_2 >= 80 select p).ToList();
                    }
                    else return (from p in db.RESULTs where p.TEST_RESULT_3 >= 80 select p).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ;
            }
        }

        public List<RESULT> findResultMiddle(string category)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    if (category == "GENERAL KNOWLEDGE")
                    {
                        return (from p in db.RESULTs where p.TEST_RESULT_1 >= 50 && p.TEST_RESULT_1 < 80 select p).ToList();

                    }
                    else if (category == "MATHEMATICS")
                    {
                        return (from p in db.RESULTs where p.TEST_RESULT_2 >= 50 && p.TEST_RESULT_2 < 80 select p).ToList();
                    }
                    else return (from p in db.RESULTs where p.TEST_RESULT_3 >= 50 && p.TEST_RESULT_3 < 80 select p).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ;
            }
        }
        public List<RESULT> FindResultByTestId(int testId)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    return (from p in db.RESULTs where p.TEST_ID == testId select p).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ;
            }
        }

        public List<JOB_APPLICATION> getApplications(int testId, string key)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    ApplicationService applicationService = new ApplicationService();
                    List<JOB_APPLICATION> applications = new List<JOB_APPLICATION>();

                    var selectedResults = (from p in db.RESULTs where p.TEST_ID == testId select p).ToList();

                    foreach (var r in selectedResults)
                    {
                        if (key == "high")
                        {
                        }
                        var selectedApplication = (from p in db.JOB_APPLICATION where p.APPLY_ID == r.APPLY_ID select p).SingleOrDefault();
                        applications.Add(selectedApplication);
                    }
                    return applications;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public bool deleteResultByApply(int applyId)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {

                    var selectedResult = (from p in db.RESULTs where p.APPLY_ID == applyId select p).SingleOrDefault();
                    db.RESULTs.Remove(selectedResult);
                    if (db.SaveChanges() == 0) throw new Exception();
                    else return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


    }
}
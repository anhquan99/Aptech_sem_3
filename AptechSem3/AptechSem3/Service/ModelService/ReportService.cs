using System;
using System.Collections.Generic;
using AptechSem3.Models;
using System.Linq;
using System.Web;

namespace AptechSem3.Service.ModelService
{
    public class ReportService : IService<REPORT>
    {
        public bool create(REPORT t)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    db.REPORTs.Add(t);
                    if (db.SaveChanges() == 0) throw new Exception();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public bool deleteById(string id)
        {
            throw new NotImplementedException();

        }

        public List<REPORT> findAll()
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    return (from p in db.REPORTs select p).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public REPORT findByCreated(DateTime created)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {

                    var selectedReport = (from p in db.REPORTs where p.CREATED == created select p).SingleOrDefault();
                    return selectedReport;
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public REPORT findById(string id)
        {
            throw new NotImplementedException();
        }

        public bool update(REPORT t)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    var selectedReport = (from p in db.REPORTs
                                          where p.CREATED == t.CREATED
                                          select p).SingleOrDefault();

                    if (selectedReport != null)
                    {
                        selectedReport.PERCENT_ATTEMPT = t.PERCENT_ATTEMPT;
                        selectedReport.PERCENT_FAST_FINISH = t.PERCENT_FAST_FINISH;
                        selectedReport.PERCENT_HIGH_PART_1 = t.PERCENT_HIGH_PART_1;
                        selectedReport.PERCENT_HIGH_PART_2 = t.PERCENT_HIGH_PART_2;
                        selectedReport.PERCENT_HIGH_PART_3 = t.PERCENT_HIGH_PART_3;
                        selectedReport.PERCENT_HIGH_SCORE = t.PERCENT_HIGH_SCORE;
                        selectedReport.PERCENT_MIDDLE_SCORE = t.PERCENT_MIDDLE_SCORE;
                        selectedReport.PERCENT_MID_PART_1 = t.PERCENT_MID_PART_1;
                        selectedReport.PERCENT_MID_PART_2 = t.PERCENT_MID_PART_2;
                        selectedReport.PERCENT_MID_PART_3 = t.PERCENT_MID_PART_3;
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
        public int findTestIdWithCreated(DateTime created)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {

                    var selectedTest = (from p in db.TESTs where p.END_TIME == created select p).SingleOrDefault();
                    return selectedTest.TEST_ID;
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }
    }
}
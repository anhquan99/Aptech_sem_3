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
                throw ;
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
                throw ;
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
                throw ;
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
                throw ;
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
                throw;
            }
        }

        public List<REPORT> findReportWithMonth(DateTime month)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {

                    List<REPORT> reports = (from p in db.REPORTs where p.CREATED.Month == month.Month && p.CREATED.Year == month.Year orderby p.CREATED ascending select p).ToList();
                    System.Diagnostics.Debug.WriteLine("abc");
                    return reports;
                }
            }
            catch (Exception ex)
            {
                throw ;
            }
        }

        /*public List<REPORT> findReportWithYear(DateTime year)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {

                    List<REPORT> reports = (from p in db.REPORTs where p.CREATED.Year == month.Month select p).ToList();
                    System.Diagnostics.Debug.WriteLine("abc");
                    return reports;
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }*/

        public List<REPORT> findReportWithYear(DateTime year)
        {
            try

            {
                List<REPORT> newReports = new List<REPORT>();
                for (int i = 1; i < 13; i++) 
                {
                    using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                    {
                        List<REPORT> reports = (from p in db.REPORTs where p.CREATED.Month == i && p.CREATED.Year == year.Year orderby p.CREATED ascending select p).ToList();
                        double attemp = 0, sum_high = 0, sum_middle = 0, GK_high = 0, GK_middle = 0, M_high = 0, M_middle = 0, CT_high = 0, CT_middle = 0;
                        if (reports != null)
                        {
                            foreach (var r in reports)
                            {
                                attemp = attemp + r.PERCENT_ATTEMPT;
                                sum_high = sum_high + r.PERCENT_HIGH_SCORE;
                                sum_middle = sum_middle + r.PERCENT_MIDDLE_SCORE;
                                GK_high = GK_high + r.PERCENT_HIGH_PART_1;
                                M_high = M_high + r.PERCENT_HIGH_PART_2;
                                CT_high = CT_high + r.PERCENT_HIGH_PART_3;
                                GK_middle = GK_middle + r.PERCENT_MID_PART_1;
                                M_middle = M_middle + r.PERCENT_MID_PART_2;
                                CT_middle = CT_middle + r.PERCENT_MID_PART_3;
                            }
                            
                        }
                        DateTime created = new DateTime(year.Year, i, 1);
                        if (reports.Count != 0)
                        {
                            REPORT report = new REPORT()
                            {
                                CREATED = created,
                                PERCENT_ATTEMPT = attemp / (reports.Count),
                                PERCENT_HIGH_SCORE = sum_high / (reports.Count),
                                PERCENT_MIDDLE_SCORE = sum_middle / (reports.Count),
                                PERCENT_HIGH_PART_1 = GK_high / (reports.Count),
                                PERCENT_HIGH_PART_2 = M_high / (reports.Count),
                                PERCENT_HIGH_PART_3 = CT_high / (reports.Count),
                                PERCENT_MID_PART_1 = GK_middle / (reports.Count),
                                PERCENT_MID_PART_2 = M_middle / (reports.Count),
                                PERCENT_MID_PART_3 = CT_middle / (reports.Count),
                                PERCENT_FAST_FINISH = 0
                            };
                            newReports.Add(report);
                        }
                        else 
                        {
                            REPORT report = new REPORT()
                            {
                                CREATED = created,
                                PERCENT_ATTEMPT = 0,
                                PERCENT_HIGH_SCORE = 0,
                                PERCENT_MIDDLE_SCORE = 0,
                                PERCENT_HIGH_PART_1 = 0,
                                PERCENT_HIGH_PART_2 = 0,
                                PERCENT_HIGH_PART_3 = 0,
                                PERCENT_MID_PART_1 = 0,
                                PERCENT_MID_PART_2 = 0,
                                PERCENT_MID_PART_3 = 0,
                                PERCENT_FAST_FINISH = 0
                            };
                            newReports.Add(report);
                        }
                        
                        

                        
                    }
                }
                return newReports;
            }
            catch (Exception ex)
            {
                throw ;
            }
        }

        public List<JOB_APPLICATION> findApplicationWithResult(int testId,string request)
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    TestService testService = new TestService();
                    ApplicationService applicationService = new ApplicationService();
                    List<JOB_APPLICATION> applications = new List<JOB_APPLICATION>();
                    double sum_score = (double)testService.GetScoreFromTestAndCategory(testId, "none");
                    List<RESULT> results = (from p in db.RESULTs where p.TEST_ID == testId select p).ToList();
                    foreach (var r in results)
                    {
                        if ((request == "high") && ((r.TEST_RESULT_1 + r.TEST_RESULT_2 + r.TEST_RESULT_3) >= sum_score * 0.8))
                        {
                            applications.Add(applicationService.findById(r.APPLY_ID.ToString()));
                        }
                        else if ((request == "mid") && ((r.TEST_RESULT_1 + r.TEST_RESULT_2 + r.TEST_RESULT_3) >= sum_score * 0.5) && ((r.TEST_RESULT_1 + r.TEST_RESULT_2 + r.TEST_RESULT_3) < sum_score * 0.8))
                        {
                            applications.Add(applicationService.findById(r.APPLY_ID.ToString()));
                        }
                        else if ((request == "low") && ((r.TEST_RESULT_1 + r.TEST_RESULT_2 + r.TEST_RESULT_3) < sum_score * 0.5))
                        {
                            applications.Add(applicationService.findById(r.APPLY_ID.ToString()));
                        }
                        else if ((request == "attempt") && (r.TEST_INDEX != 0))
                        {
                            applications.Add(applicationService.findById(r.APPLY_ID.ToString()));
                        }

                    }
                    return applications;
                }
            }
            catch (Exception ex)
            {
                throw;

            }
        }

        public List<REPORT> findAllByDate()
        {
            try
            {
                using (APTECH_SEM_3Entities db = new APTECH_SEM_3Entities())
                {
                    return (from p in db.REPORTs orderby p.CREATED descending select p).ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AptechSem3.Models
{
    public class CustomApplication
    {
        public int APPLY_ID { get; set; }
        public string NAME { get; set; }
        public int APPROVE_STATUS { get; set; }
        public int TEST_INDEX { get; set; }
        public double TEST_RESULT_1 { get; set; }
        public double TEST_RESULT_2 { get; set; }
        public double TEST_RESULT_3 { get; set; }

        /*public CustomApplication(int aPPLY_ID, string nAME, int aPPROVE_STATUS, int tEST_INDEX, double tEST_RESULT_1, double tEST_RESULT_2, double tEST_RESULT_3)
        {
            APPLY_ID = aPPLY_ID;
            NAME = nAME;
            APPROVE_STATUS = aPPROVE_STATUS;
            TEST_INDEX = tEST_INDEX;
            TEST_RESULT_1 = tEST_RESULT_1;
            TEST_RESULT_2 = tEST_RESULT_2;
            TEST_RESULT_3 = tEST_RESULT_3;
        }*/
    }
}
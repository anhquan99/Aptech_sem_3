//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AptechSem3.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TEST
    {
        public TEST()
        {
            this.RESULTs = new HashSet<RESULT>();
            this.QUESTIONs = new HashSet<QUESTION>();
        }
    
        public int POST_ID { get; set; }
        public System.DateTime START_TIME { get; set; }
        public System.DateTime END_TIME { get; set; }
        public string TEST_NAME { get; set; }
        public int TEST_ID { get; set; }
    
        public virtual JOB_POST JOB_POST { get; set; }
        public virtual ICollection<RESULT> RESULTs { get; set; }
        public virtual ICollection<QUESTION> QUESTIONs { get; set; }
    }
}

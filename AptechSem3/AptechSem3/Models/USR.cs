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
    
    public partial class USR
    {
        public Nullable<int> APPLY_ID { get; set; }
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
        public string ROLE { get; set; }
    
        public virtual JOB_APPLICATION JOB_APPLICATION { get; set; }
    }
}

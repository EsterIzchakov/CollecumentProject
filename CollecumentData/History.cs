//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CollecumentData
{
    using System;
    using System.Collections.Generic;
    
    public partial class History
    {
        public string ID { get; set; }
        public int VirsionNum { get; set; }
        public string UpdateID { get; set; }
        public System.DateTime Date_Update { get; set; }
        public string Remarks { get; set; }
    
        public virtual File File { get; set; }
        public virtual User User { get; set; }
    }
}

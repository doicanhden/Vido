//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Vido.Parking
{
    using System;
    using System.Collections.Generic;
    
    public partial class InOutRecord
    {
        public long RecordId { get; set; }
        public string InEmployeeId { get; set; }
        public string InLaneCode { get; set; }
        public string InTime { get; set; }
        public string InBackImg { get; set; }
        public string InFrontImg { get; set; }
        public string OutEmployeeId { get; set; }
        public string OutLaneCode { get; set; }
        public string OutTime { get; set; }
        public string OutBackImg { get; set; }
        public string OutFrontImg { get; set; }
        public string CardId { get; set; }
        public string UserData { get; set; }
        public string Comment { get; set; }
        public Nullable<decimal> FeeValue { get; set; }
    
        public virtual Card Card { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Employee Employee1 { get; set; }
    }
}

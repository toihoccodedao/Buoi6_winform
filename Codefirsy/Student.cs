//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Codefirsy
{
    using System;
    using System.Collections.Generic;
    
    public partial class Student
    {
        public int StudentID { get; set; }
        public string FullName { get; set; }
        public Nullable<decimal> AverageScore { get; set; }
        public Nullable<int> FacultyID { get; set; }
    
        public virtual Faculty Faculty { get; set; }
    }
}

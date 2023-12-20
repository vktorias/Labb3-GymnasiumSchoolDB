using System;
using System.Collections.Generic;

namespace Labb3_GymnasiumSchoolDB.Models
{
    public partial class Enrollment
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int? Grade { get; set; }
        public DateTime? GradeDate { get; set; }

        public virtual Course Course { get; set; } = null!;
        public virtual Student Student { get; set; } = null!;
    }
}

using System;
using System.Collections.Generic;

namespace Labb3_GymnasiumSchoolDB.Models
{
    public partial class Course
    {
        public int CourseId { get; set; }
        public string? CourseName { get; set; }
        public int TeacherId { get; set; }

        public virtual Employee Teacher { get; set; } = null!;
    }
}

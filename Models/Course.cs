using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Labb3_GymnasiumSchoolDB.Models
{
    public partial class Course
    {
        public int CourseId { get; set; }
        public string? CourseName { get; set; }
        public int TeacherId { get; set; }

        public virtual Employee Teacher { get; set; } = null!;

        [NotMapped]
        public object Enrollments { get; internal set; }
    }
}

using System.Collections.Generic;
using ContosoUniversity.Models;

namespace ContosoUniversity.ViewModels
{
    public class InstructorIndexData
    {
        public IEnumerable<Instructor> Instructor { get; set; }
        public IEnumerable<Course> Course { get; set; }
        public IEnumerable<Enrollment> Enrollment { get; set; }
    }
}
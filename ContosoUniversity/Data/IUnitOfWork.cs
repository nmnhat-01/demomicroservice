using ContosoUniversity.Models;

namespace ContosoUniversity.Data
{
    public interface IUnitOfWork : IDisposable
    {
        public GenericRepository<Student> StudentRepository { get; }
        public GenericRepository<Department> DepartmentRepository { get; }
        public GenericRepository<Course> CourseRepository { get; }
        public GenericRepository<Instructor> InstructorRepository { get; }
        public void Save();
        public void Dispose();
    }
}

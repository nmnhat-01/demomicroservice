using ContosoUniversity.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Data
{
    public class StudentRepository : IStudentRepository, IDisposable
    {
        private SchoolContext context;

        public StudentRepository(SchoolContext context)
        {
            this.context = context;
        }

        public List<Student> GetStudents()
        {
            return context.Student.ToList();
        }

        public Student GetStudentByID(int id)
        {
            return context.Student.Find(id);
        }

        public void InsertStudent(Student student)
        {
            context.Student.Add(student);
        }

        public void DeleteStudent(int studentID)
        {
            Student student = context.Student.Find(studentID);
            context.Student.Remove(student);
        }

        public void UpdateStudent(Student student)
        {
            context.Student.Update(student);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public bool Exists(int studentID)
        {
            return context.Student.Any(e => e.StudentID == studentID);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

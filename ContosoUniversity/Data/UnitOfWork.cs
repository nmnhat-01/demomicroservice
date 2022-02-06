using System;
using ContosoUniversity.Models;

namespace ContosoUniversity.Data
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private SchoolContext context;
        private GenericRepository<Student> studentRepository;
        private GenericRepository<Department> departmentRepository;
        private GenericRepository<Course> courseRepository;
        private GenericRepository<Instructor> instructorRepository;

        public UnitOfWork(SchoolContext context)
        {
            this.context = context;
        }

        public GenericRepository<Student> StudentRepository
        {
            get
            {
                if (this.studentRepository == null)
                {
                    this.studentRepository = new GenericRepository<Student>(context);
                }
                return studentRepository;
            }
        }

        public GenericRepository<Department> DepartmentRepository
        {
            get
            {
                if (this.departmentRepository == null)
                {
                    this.departmentRepository = new GenericRepository<Department>(context);
                }
                return departmentRepository;
            }
        }

        public GenericRepository<Course> CourseRepository
        {
            get
            {
                if (this.courseRepository == null)
                {
                    this.courseRepository = new GenericRepository<Course>(context);
                }
                return courseRepository;
            }
        }

        public GenericRepository<Instructor> InstructorRepository
        {
            get
            {
                if (this.instructorRepository == null)
                {
                    this.instructorRepository = new GenericRepository<Instructor>(context);
                }
                return instructorRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
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
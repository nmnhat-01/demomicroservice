using System;
using System.Collections.Generic;
using ContosoUniversity.Models;

namespace ContosoUniversity.Data
{
    public interface IStudentRepository : IDisposable
    {
        List<Student> GetStudents();
        Student GetStudentByID(int studentId);
        void InsertStudent(Student student);
        void DeleteStudent(int studentID);
        void UpdateStudent(Student student);
        public bool Exists(int studentID);
        void Save();
    }
}
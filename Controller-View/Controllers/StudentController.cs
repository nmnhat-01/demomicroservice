#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using System.Data;

namespace Controller_View.Controllers
{
    public class StudentController : Controller
    {
        /*private readonly SchoolContext _context;

        public StudentController(SchoolContext context)
        {
            _context = context;
        }*/
        private readonly IStudentRepository studentRepository;

        public StudentController(SchoolContext context)
        {
            this.studentRepository = new StudentRepository(context);
        }

        /*public StudentController(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
        }*/

        // GET: Student
        public IActionResult Index()
        {
            return View(studentRepository.GetStudents());
        }

        // GET: Student/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            /*var student = await _context.Student
                .FirstOrDefaultAsync(m => m.StudentID == id);*/
            Student student = studentRepository.GetStudentByID((int)id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Student/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentID,LastName,FirstMidName,EnrollmentDate")] Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    /*_context.Student.Add(student);
                    await _context.SaveChangesAsync();*/
                    studentRepository.InsertStudent(student);
                    studentRepository.Save();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(student);
        }

        // GET: Student/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            /*var student = await _context.Student.FindAsync(id);*/
            Student student = studentRepository.GetStudentByID((int)id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentID,LastName,FirstMidName,EnrollmentDate")] Student student)
        {
            if (id != student.StudentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    /*_context.Student.Update(student);
                    await _context.SaveChangesAsync();*/
                    studentRepository.UpdateStudent(student);
                    studentRepository.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StudentID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Student/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            /*var student = await _context.Student
                .FirstOrDefaultAsync(m => m.StudentID == id);*/
            Student student = studentRepository.GetStudentByID((int)id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            /* var student = await _context.Student.FindAsync(id);
             _context.Student.Remove(student);
             await _context.SaveChangesAsync();*/
            Student student = studentRepository.GetStudentByID(id);
            studentRepository.DeleteStudent(id);
            studentRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return studentRepository.Exists(id);
        }

        protected override void Dispose(bool disposing)
        {
            studentRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}

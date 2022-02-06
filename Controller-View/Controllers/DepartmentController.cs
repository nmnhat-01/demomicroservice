#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Models;
using ContosoUniversity.Data;

namespace Controller_View.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly SchoolContext _context;
        private IUnitOfWork unitOfWork;

        public DepartmentController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: Department
        public async Task<IActionResult> Index()
        {
            var schoolContext = unitOfWork.DepartmentRepository.Get(includeProperties: "Administrator");
            /*var schoolContext = _context.Department.Include(d => d.Administrator);*/
            return View(schoolContext.ToList());
        }

        // GET: Department/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            /*var department = await _context.Department
                .Include(d => d.Administrator)
                .FirstOrDefaultAsync(m => m.DepartmentID == id);*/
            var department = unitOfWork.DepartmentRepository.GetByID(id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // GET: Department/Create
        public IActionResult Create()
        {
            /*ViewData["InstructorID"] = new SelectList(_context.Instructor, "InstructorID", "FirstMidName");*/
            PopulateDepartmentsDropDownList();
            return View();
        }

        // POST: Department/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DepartmentID,Name,Budget,StartDate,InstructorID")] Department department)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.DepartmentRepository.Insert(department);
                unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            /*ViewData["InstructorID"] = new SelectList(_context.Instructor, "InstructorID", "FirstMidName", department.InstructorID);*/
            PopulateDepartmentsDropDownList(department.InstructorID);
            return View(department);
        }

        // GET: Department/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Department department = unitOfWork.DepartmentRepository.GetByID(id);
            if (department == null)
            {
                return NotFound();
            }
            /*ViewData["InstructorID"] = new SelectList(_context.Instructor, "InstructorID", "FirstMidName", department.InstructorID);*/
            PopulateDepartmentsDropDownList(department.InstructorID);
            return View(department);
        }

        // POST: Department/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DepartmentID,Name,Budget,StartDate,InstructorID")] Department department)
        {
            if (id != department.DepartmentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    unitOfWork.DepartmentRepository.Update(department);
                    unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    Department department1 = unitOfWork.DepartmentRepository.GetByID(department.DepartmentID);
                    if (department1 == null)
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
            /*ViewData["InstructorID"] = new SelectList(_context.Instructor, "InstructorID", "FirstMidName", department.InstructorID);*/
            PopulateDepartmentsDropDownList(department.InstructorID);
            return View(department);
        }

        // GET: Department/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Department department = unitOfWork.DepartmentRepository.GetByID(id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            unitOfWork.DepartmentRepository.Delete(id);
            unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        private void PopulateDepartmentsDropDownList(object selectedInstructor = null)
        {
            var instructorsQuery = unitOfWork.InstructorRepository.Get(
             orderBy: q => q.OrderBy(d => d.FirstMidName));
            ViewData["InstructorID"] = new SelectList(instructorsQuery, "InstructorID", "FirstMidName", selectedInstructor);
        }
    }
}

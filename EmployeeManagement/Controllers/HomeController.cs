using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private IEmployeeRepository _employeeRepository;
        private readonly IHostingEnvironment _hostingEnvironment;

        public HomeController(IEmployeeRepository employeeRepository, IHostingEnvironment hostingEnvironment)
        {
            _employeeRepository = employeeRepository;
            _hostingEnvironment = hostingEnvironment;
        }

        public ViewResult Index()
        {
            var model = _employeeRepository.GetAllEmployees();
            return View(model);
        }

        public ViewResult Details(int id)
        {
            Employee employee = _employeeRepository.GetEmployee(id);
            if (employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id);
            }
            string pageTitle = "Employee Details";
            EmployeeDetailsVM viewModel = new EmployeeDetailsVM(pageTitle, employee);
            return View(viewModel);
        }

        [Authorize]
        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(EmployeeCreateVM viewModel)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedPhoto(viewModel);
                Employee newEmployee = new Employee
                {
                    Name = viewModel.Name,
                    Email = viewModel.Email,
                    Department = viewModel.Department,
                    PhotoPath = uniqueFileName
                };
                _employeeRepository.Add(newEmployee);
                return RedirectToAction("Details", new { id = newEmployee.Id });
            }

            return View();
        }

        [Authorize]
        [HttpGet]
        public ViewResult Edit(int id)
        {
            Employee employee = _employeeRepository.GetEmployee(id);
            EmployeeEditVM employeeEditVM = EmployeeEditVM.CreateViewModel(employee);
            return View(employeeEditVM);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(EmployeeEditVM viewModel)
        {
            if (ModelState.IsValid)
            {
                Employee employee = _employeeRepository.GetEmployee(viewModel.Id);
                employee.Name = viewModel.Name;
                employee.Email = viewModel.Email;
                employee.Department = viewModel.Department;

                if (viewModel.Photo != null)
                {
                    if (viewModel.ExistingPhotoPath != null)
                    {
                        DeletePhoto(viewModel.ExistingPhotoPath);
                    }
                    employee.PhotoPath = ProcessUploadedPhoto(viewModel);
                }

                _employeeRepository.Update(employee);
                return RedirectToAction("Index");
            }

            return View();
        }

        private void DeletePhoto(string fileName)
        {
            //GC.Collect();
            //GC.WaitForPendingFinalizers();
            string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "Images", fileName);
            System.IO.File.Delete(filePath);
        }

        [NonAction]
        private string ProcessUploadedPhoto(EmployeeCreateVM viewModel)
        {
            string uniqueFileName = null;
            if (viewModel.Photo != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid() + "_" + viewModel.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    viewModel.Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
    }
}

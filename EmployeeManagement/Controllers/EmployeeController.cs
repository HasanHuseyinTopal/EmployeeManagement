using DataAccessLayer.Abstract;
using EntityLayer.DTOs;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class EmployeeController : Controller
    {
        IUnitOfWorkDal _UnitOfWorkDal;
        IWebHostEnvironment _webHostEnvironment;
        public EmployeeController(IUnitOfWorkDal unitOfWorkDal, IWebHostEnvironment webHostEnvironment)
        {
            _UnitOfWorkDal = unitOfWorkDal;
            _webHostEnvironment = webHostEnvironment;

        }
        public IActionResult EmployeeList()
        {
         
            var result = _UnitOfWorkDal.employeeDal.GetAll();
            return View(result);
        }
        public IActionResult EmployeeDetails(int? id)
        {
            
            var result = _UnitOfWorkDal.employeeDal.GetOne(x => x.employeeId == id);
            return View(result);
        }
        [HttpGet]
        public IActionResult EmployeeCreate()
        {
            return View();
        }
        [HttpPost]
        public IActionResult EmployeeCreate(EmployeeCreateViewModel model, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    Employee employee = new Employee()
                    {
                        employeeImageUrl = FileChangeProcess(file, model.employeeImageUrl),
                        employeeName = model.employeeName,
                        employeeEmail = model.employeeEmail,
                        employeeDepartmant = model.employeeDepartmant,
                    };
                    _UnitOfWorkDal.employeeDal.Add(employee);
                    _UnitOfWorkDal.Save();
                    return RedirectToAction("EmployeeDetails", "Employee", new { id = employee.employeeId });
                }

            }
            return View();
        }
        public IActionResult EmployeeDelete(int id)
        {
            var delete = _UnitOfWorkDal.employeeDal.GetOne(x => x.employeeId == id);
            if (delete.employeeImageUrl != null)
            {
                FileDeleteProcess(delete.employeeImageUrl, _webHostEnvironment.WebRootPath);
            }
            _UnitOfWorkDal.employeeDal.Delete(delete);
            _UnitOfWorkDal.Save();
            return RedirectToAction("EmployeeList", "Employee");
        }
        [HttpGet]
        public IActionResult EmployeeUpdate(int id)
        {
            var update = _UnitOfWorkDal.employeeDal.GetOne(x => x.employeeId == id);
            EmployeeUpdateViewModel model = new()
            {
                EmployeeId = update.employeeId,
                employeeName = update.employeeName,
                employeeEmail = update.employeeEmail,
                employeeDepartmant = update.employeeDepartmant,
                employeeImageUrl = update.employeeImageUrl,
            };
            return View(model);

        }
        [HttpPost]
        public IActionResult EmployeeUpdate(EmployeeUpdateViewModel model, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                var update = _UnitOfWorkDal.employeeDal.GetOne(x => x.employeeId == model.EmployeeId);

                if (file != null)
                    update.employeeImageUrl = FileChangeProcess(file, update.employeeImageUrl);
                else
                    update.employeeImageUrl = model.employeeImageUrl;

                update.employeeId = model.EmployeeId;
                update.employeeName = model.employeeName;
                update.employeeEmail = model.employeeEmail;
                update.employeeDepartmant = model.employeeDepartmant;

                _UnitOfWorkDal.employeeDal.Update(update);
                _UnitOfWorkDal.Save();
                return RedirectToAction("EmployeeDetails", "Employee", new { id = update.employeeId });
            }
            return View(model);
        }





        public void FileDeleteProcess(string imageUrl, string wwwRoot)
        {
            string oldFilePath = Path.Combine(wwwRoot, imageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }
        }
        public string FileChangeProcess(IFormFile file, string imageUrl)
        {
            string wwwRoot = _webHostEnvironment.WebRootPath;
            string fileHashedName = Guid.NewGuid().ToString() + "--" + file.FileName;
            string fileUploadPath = Path.Combine(wwwRoot, @"Images");
            string fileStreamPath = Path.Combine(fileUploadPath, fileHashedName);

            if (imageUrl != null)
            {
                FileDeleteProcess(imageUrl, wwwRoot);
            }
            //file.CopyTo(new FileStream(fileStreamPath, FileMode.Create));
            using (var fileStream = new FileStream(fileStreamPath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            string returnUrl = @"\Images\" + fileHashedName;
            return returnUrl;
        }
    }
}

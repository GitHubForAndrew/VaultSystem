using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VaultSystem.Models;
using VaultSystem.Domain.Abstract;
using VaultSystem.Domain.Concrete;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace VaultSystem.Controllers
{
    [Authorize(Roles = "GlobalAdmin")]
    public class UserManageController : Controller
    {
        private IUserRepository userRepository = new AppUserRepository();
        private ApplicationUserManager _userManager;
        private RoleManager<IdentityRole> _rolemanager;


        public UserManageController()
        {
        }

        public UserManageController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }


        public ActionResult Index()
        {
            return View(UserManager.Users.ToList());
        }

        public ActionResult EditUser(string userId)
        {
            var editUserView = new EditUserView();
            var user = UserManager.FindById(userId);

            List<string> roles = new List<string>();
            foreach (var role in RoleManager.Roles)
            {
                roles.Add(role.Name);
            }

            editUserView.UserRoles = UserManager.GetRoles(user.Id).ToList();
            
            editUserView.User = user;
            editUserView.AllRoles = roles;

            return View(editUserView);
        }

        public ActionResult AddUser()
        {
            var editUserView = new EditUserView();
            List<string> roles = new List<string>();
            foreach (var role in RoleManager.Roles)
            {
                roles.Add(role.Name);
            }
            
            editUserView.AllRoles = roles;

            return View(editUserView);
        }

        [HttpPost]
        public ActionResult EditUser(string newPassword, string confirmPassword, string userId, string[] selectRoles)
        {
            if (!ModelState.IsValid)
                return EditUser(userId);

            if (selectRoles != null)
            {
                foreach (var role in selectRoles)
                {
                    var findRole = UserManager.GetRoles(userId).FirstOrDefault(r => r == role);
                    if (findRole == null)
                        UserManager.AddToRole(userId, role);
                }
            }

            if (!string.IsNullOrEmpty(newPassword))
            {
                if (newPassword != confirmPassword)
                {
                    ModelState.AddModelError("", "Введенные пароли не совпадают!");
                    return EditUser(userId);
                }
                UserManager.ResetPassword(userId, UserManager.GeneratePasswordResetToken(userId), newPassword);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddUser(string userName, string newPassword, string confirmPassword, string userId, string[] selectRoles)
        {
            if (!ModelState.IsValid)
                return EditUser(userId);
            if (string.IsNullOrEmpty(newPassword))
            {
                ModelState.AddModelError("", "Пароль не может быть пустым!");
                return AddUser();
            }

            if (newPassword != confirmPassword)
            {
                ModelState.AddModelError("", "Введенные пароли не совпадают!");
                return AddUser();
            }

            var user = new ApplicationUser() { Email = userName, UserName = userName };
            var passValidator = ((PasswordValidator)UserManager.PasswordValidator);
            passValidator.RequireDigit = false;
            passValidator.RequireNonLetterOrDigit = false;
            passValidator.RequireUppercase = false;
            var result = UserManager.Create(user, newPassword);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Ошибка добавления пользователя!");
                ModelState.AddModelError("", result.Errors.FirstOrDefault());
                return Index();
            }
            if (selectRoles != null)
            {
                foreach (var role in selectRoles)
                {
                    UserManager.AddToRole(user.Id, role);
                }
            }
            return RedirectToAction("Index");
        }

        private ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }

        private RoleManager<IdentityRole> RoleManager
        {
            get
            {
                if (_rolemanager == null)
                    _rolemanager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(HttpContext.GetOwinContext().Get<ApplicationDbContext>()));
                return _rolemanager;
            }
            set
            {
                _rolemanager = value;
            }
        }
    }
}
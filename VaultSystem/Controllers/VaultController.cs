using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using VaultSystem.Domain.Abstract;
using VaultSystem.Domain.Concrete;
using VaultSystem.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;

namespace VaultSystem.Controllers
{
    public class VaultController : Controller
    {
        private IVaultRepository vaultRepository = new VaultRepository();
        private ApplicationUserManager _userManager;
        
        public ActionResult Index(int vaultId = 0)
        {
            var vaultInfo = new VaultInfo();
            vaultInfo.User = UserManager.FindById(User.Identity.GetUserId());
            vaultInfo.UserRoles = UserManager.GetRoles(vaultInfo.User.Id).ToList();
            vaultInfo.VaultId = vaultId;
            return View(vaultInfo);
        }

        public PartialViewResult Vault(int vaultId = 0)
        {
            var vaultView = new VaultView();
            var user = UserManager.FindById(User.Identity.GetUserId());
            vaultView.MyVaults = vaultRepository.GetMyVaults(user.Id);
            vaultView.OtherVaults = vaultRepository.GetOtherVaults(user.Id);
            vaultView.UserRoles = UserManager.GetRoles(user.Id).ToList();
            if (vaultId > 0)
                vaultView.VaultId = vaultId;
            //vaultView.User = UserManager.FindById(User.Identity.GetUserId());
            return PartialView(vaultView);
        }

        public PartialViewResult VaultItems(int vaultId = 0)
        {
            var viView = new VaultItemsView();
            var user = UserManager.FindById(User.Identity.GetUserId());
            var vault = vaultRepository.Vault(vaultId);
            viView.VaultId = vaultId;
            var access = GetAccess(vault, user.Id);
            if(!string.IsNullOrEmpty(access))
            {
                viView.VaultItems = vault.VaultItem.ToList();
                viView.VaultAccessLevel = access;
                return PartialView(viView);
            }
            ModelState.AddModelError("", "Access denied");
            return PartialView(viView);
        }

        private string GetAccess(Vault vault, string userId)
        {
            var access = vault.VaultUser.FirstOrDefault(vu => vu.UserId == userId && (vu.DateFrom <= DateTime.Now && (!vu.DateTo.HasValue || vu.DateTo >= DateTime.Now)));

            if (access == null)
                return string.Empty;

            return access.AccessLevel;
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

        [Authorize(Roles = "VaultAdmin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string vaultName)
        {
            if (string.IsNullOrEmpty(vaultName))
            {
                ModelState.AddModelError("", "Name can't be empty!");
                return Create();
            }
            var user = UserManager.FindById(User.Identity.GetUserId());
            var vault = new Vault() { Name = vaultName };
            vaultRepository.CreaetVault(vaultName, user.Id);
            return RedirectToAction("Index");
        }


        public ActionResult AddContent(int vaultId = 0)
        {
            if (vaultId == 0)
                return RedirectToAction("Index");

            var vault = vaultRepository.Vault(vaultId);
            if (vault == null)
                return RedirectToAction("Index", vaultId);

            var user = UserManager.FindById(User.Identity.GetUserId());
            var accessLevel = GetAccess(vault, user.Id);
            if (string.IsNullOrEmpty(accessLevel) || accessLevel == "Read")
            {
                return RedirectToAction("Index", vaultId);
            }

            return View(vaultId);
        }

        [HttpPost]
        public ActionResult AddContent(int vaultId, string name, string content)
        {
            if (vaultId == 0)
                return RedirectToAction("Index");
            var vaultItem = new VaultItem() { Name = name, Content = content };
            vaultRepository.AddVaultItem(vaultId, vaultItem);
            return Index(vaultId);
        }

        public ActionResult GetContent(int? id)
        {

            return View();
        }
    }
}
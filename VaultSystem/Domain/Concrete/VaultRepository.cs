using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaultSystem.Domain.Abstract;
using VaultSystem.Models;

namespace VaultSystem.Domain.Concrete
{
    public class VaultRepository : IVaultRepository
    {
        private VaultEntities context = new VaultEntities();

        public List<Vault> Vaults
        {
            get
            {
                return context.Vault.ToList();
            }
        }

        public void AddVaultItem(int vaultId, VaultItem item)
        {
            item.VaultId = vaultId;
            context.VaultItem.Add(item);
            context.SaveChanges();
        }

        public void RemoveVaultItem(VaultItem item)
        {
            context.VaultItem.Remove(item);
            context.SaveChanges();
        }

        public void CreaetVault(string vaultName, string userId)
        {
            var vault = new Vault() { Name = vaultName, DateCreate = DateTime.Now };
            context.Vault.Add(vault);
            context.SaveChanges();
            context.VaultUser.Add(new VaultUser() { AccessLevel = "Admin", VaultId = vault.Id, UserId = userId, DateFrom = DateTime.Now, DateTo = null});
            context.SaveChanges();            
        }

        public void EditVaultAccess(VaultUser vaultUser)
        {
            var item = context.VaultUser.FirstOrDefault(i => i.UserId == vaultUser.UserId && i.VaultId == vaultUser.VaultId);
            if (item == null)
            {
                context.VaultUser.Add(vaultUser);
            }
            else
            {
                item.UserId = vaultUser.UserId;
                item.VaultId = vaultUser.VaultId;
                item.DateFrom = vaultUser.DateFrom;
                item.DateTo = vaultUser.DateTo;
                item.AccessLevel = vaultUser.AccessLevel;
            }
            context.SaveChanges();
        }

        public void RemoveVaultAccess(VaultUser vaultUser)
        {
            context.VaultUser.Remove(vaultUser);
            context.SaveChanges();
        }

        public Vault Vault(int vaultId)
        {
            return context.Vault.FirstOrDefault(v => v.Id == vaultId);
        }

        public List<Vault> GetMyVaults(string userId)
        {
            var res = from v in context.Vault join vu in context.VaultUser on v.Id equals vu.VaultId where vu.UserId == userId && vu.AccessLevel == "Admin" select v;
            return res.ToList();
        }

        public List<Vault> GetOtherVaults(string userId)
        {
            //TODO: Нужно реализовать выборку хранилищь.
            return new List<Vault>();
        }
    }
}

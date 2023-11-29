using E_mob_shoppy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_mob_shoppy.DataAccess.Repository.IRepository
{
    public interface IApplicationUsersRepository:IRepository<ApplicationUser>
    {
        void Upadte(ApplicationUser obj);
    }
}

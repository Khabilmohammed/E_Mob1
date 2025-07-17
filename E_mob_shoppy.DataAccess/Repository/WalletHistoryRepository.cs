using E_mob_shoppy.DataAccess.Data;
using E_mob_shoppy.DataAccess.Repository.IRepository;
using E_mob_shoppy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_mob_shoppy.DataAccess.Repository
{
    public class WalletHistoryRepository : Repository<WalletHistory>, IWalletHistoryRepository
    {
        private readonly ApplicationDbContext _db;
        public WalletHistoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}

using E_mob_shoppy.DataAccess.Data;
using E_mob_shoppy.DataAccess.Repository.IRepository;
using E_mob_shoppy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_mob_shoppy.DataAccess.Repository
{
    public class OfferRepository : Repository<Offer>, IOfferRepository
    {
        private ApplicationDbContext _db;
        public OfferRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    

        public void Upadte(Offer obj)
        {
            _db.offers.Update(obj);
        }
    }
}

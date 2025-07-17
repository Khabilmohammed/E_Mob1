using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_mob_shoppy.Models.ViewModel
{
    public class WalletViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }
        public IEnumerable<WalletHistory> WalletHistoryList { get; set; }
    }
}

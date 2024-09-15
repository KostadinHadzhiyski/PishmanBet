using PishmanBet.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PishmanBet.Services.Contracts
{
    public interface IMatchService
    {
        public Task<ICollection<FootballMatchViewModel>> GetComingMatches();
    }
}

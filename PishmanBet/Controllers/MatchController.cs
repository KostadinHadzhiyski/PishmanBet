using Microsoft.AspNetCore.Mvc;
using PishmanBet.Data.ViewModels;
using PishmanBet.Services.Contracts;

namespace PishmanBet.Controllers
{
    public class MatchController : Controller
    {
        IMatchService matchService;
        public MatchController(IMatchService service)
        {
            matchService = service;
        }
        public async Task<IActionResult> Coming()
        {
            ICollection<FootballMatchGetModel> matches = matchService.GetComingMatches();

            var savedMatchesCount = await matchService.WriteNewMatchesAsync(matches);
            
            return View(matches);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PishmanBet.Data.ViewModels;
using PishmanBet.Services.Contracts;

namespace PishmanBet.Controllers
{
    [Authorize]
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

            int savedMatchesCount = await matchService.WriteNewMatchesAsync(matches);

            ICollection<FootballMatchViewModel>? notStartedMatches = await matchService.GetNotFinishedMatchViewModels();


            return View(notStartedMatches);
        }
    }
}

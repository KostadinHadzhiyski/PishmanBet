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
        public IActionResult GetComingMatches()
        {
            ICollection<FootballMatchGetModel> matches = matchService.GetComingMatches();
            
            return View();
        }
    }
}

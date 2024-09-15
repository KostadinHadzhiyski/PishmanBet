

using System.ComponentModel.DataAnnotations;

namespace PishmanBet.Data.ViewModels
{
    public class FootballMatchViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public FootballTeamViewModel HomeTeam { get; set; } = null!;

        [Required]
        public FootballTeamViewModel AwayTeam { get; set; } = null!;

        [Required]
        public string StartTime { get; set; } = null!;
    }
}

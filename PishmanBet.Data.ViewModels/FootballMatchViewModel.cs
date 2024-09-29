

using System.ComponentModel.DataAnnotations;

namespace PishmanBet.Data.ViewModels
{
    public class FootballMatchViewModel
    {
        [Required]
        public Guid Id { get; set; }

        public string HomeTeamName { get; set; } = null!;

        public string AwayTeamName { get; set; } = null!;

        public DateTime StartTime { get; set; }

        public string StartTimeString { get; set; } = null!;
    }
}

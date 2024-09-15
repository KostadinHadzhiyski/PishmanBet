
namespace PishmanBet.Data.ViewModels
{
    public class FootballMatchViewModel
    {
        public string HomeTeamName { get; set; } = null!;
        public string AwayTeamName { get; set; } = null!;
        public decimal HomeTeamOdd { get; set; }

        public decimal AwayTeamOdd { get; set; }

        public decimal DrawOdd { get; set; }
    }
}

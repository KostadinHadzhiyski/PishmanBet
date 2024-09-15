
namespace PishmanBet.Data.ViewModels
{
    public class FootballMatchGetModel
    {
        public string HomeTeamName { get; set; } = null!;
        public string AwayTeamName { get; set; } = null!;
        public double HomeTeamOdd { get; set; }

        public double AwayTeamOdd { get; set; }

        public double DrawOdd { get; set; }

        public string StartDateTime { get; set; } = null!;
    }
}

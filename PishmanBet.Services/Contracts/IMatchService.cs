
namespace PishmanBet.Services.Contracts
{
    using PishmanBet.Data.ViewModels;
    public interface IMatchService
    {
        public ICollection<FootballMatchGetModel> GetComingMatches();

        public Task WriteNewMatches(ICollection<FootballMatchGetModel> gettedMatches);

    }
}


namespace PishmanBet.Services.Contracts
{
    using PishmanBet.Data.ViewModels;
    using System.Text.RegularExpressions;

    public interface IMatchService
    {
        public ICollection<FootballMatchGetModel> GetComingMatches();

        public Task WriteNewMatches(ICollection<FootballMatchGetModel> gettedMatches);

        public Task<int> WriteNewMatchesAsync(ICollection<FootballMatchGetModel> gettedMatches);

        public Task<ICollection<FootballMatchViewModel>> GetNotFinishedMatchViewModels();

    }
}

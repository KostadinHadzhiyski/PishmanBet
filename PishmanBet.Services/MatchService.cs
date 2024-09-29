using Microsoft.EntityFrameworkCore;
using PishmanBet.Common.Enums;
using PishmanBet.Data;
using PishmanBet.Data.Models;
using PishmanBet.Data.ViewModels;
using PishmanBet.Services.Contracts;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;


namespace PishmanBet.Services
{
    public class MatchService : IMatchService
    {
        private const string GetComingMatchesURL = "https://sportsbook.draftkings.com/leagues/soccer/champions-league";

        private string matchesString = null!;

        private PishmanBetDbContext context;

        public MatchService(PishmanBetDbContext _context)
        {
            matchesString = GetMatchesString();
            context = _context;
        }

        public ICollection<FootballMatchGetModel> GetComingMatches()
        {

            List<FootballMatchGetModel>? footballMatches = GetFootballMatches().ToList();


            foreach (FootballMatchGetModel footballMatch in footballMatches)
            {

                string currentMatchOddPattern = GetMatchOddsPattern(footballMatch);

                RegexOptions regexOptions = RegexOptions.IgnoreCase;
                Match match = Regex.Match(matchesString, currentMatchOddPattern.ToString(), regexOptions);

                footballMatch.HomeTeamOdd = DecimalOddConvert(match.Groups[1].Value);
                footballMatch.DrawOdd = DecimalOddConvert(match.Groups[2].Value);
                footballMatch.AwayTeamOdd = DecimalOddConvert(match.Groups[3].Value);
            }



            return footballMatches;
        }

        private ICollection<FootballMatchGetModel> GetFootballMatches()
        { 

            string pattern = @"Event Accordion for ([a-zA-z 0-9]*?) vs ([a-zA-Z 0-9]*).*?sportsbook-event-accordion__date.{2}<span>(.*?)\<";


            RegexOptions regexOptions = RegexOptions.IgnoreCase;
   
            
            

            List<FootballMatchGetModel> footballMatches = new List<FootballMatchGetModel>();

            

            foreach (Match match in Regex.Matches(matchesString, pattern, regexOptions))
            {
                FootballMatchGetModel footballMatch = new FootballMatchGetModel();
                footballMatch.HomeTeamName = match.Groups[1].Value;
                footballMatch.AwayTeamName = match.Groups[2].Value;
                footballMatch.StartDateTime = match.Groups[3].Value;
                footballMatches.Add(footballMatch);
            }

            return footballMatches;
        }

        private string GetMatchOddsPattern(FootballMatchGetModel footballMatch)
        {
            StringBuilder currentMatchOddPattern = new StringBuilder();

            currentMatchOddPattern.Append(@"sportsbook-outcome-cell__label.{2}");
            currentMatchOddPattern.Append(footballMatch.HomeTeamName); 
            currentMatchOddPattern.Append(@"[^0-9]*sportsbook-odds american default-color.{2}(.{1}[0-9]{3}).*sportsbook-odds american default-color.{2}(.{1}[0-9]{3}).*sportsbook-outcome-cell__label.{2}");
            currentMatchOddPattern.Append(footballMatch.AwayTeamName);
            currentMatchOddPattern.Append(@"[^0-9]*sportsbook-odds american default-color.{2}(.{1}[0-9]{3})");

            return currentMatchOddPattern.ToString().Trim();
        }

        private  string GetMatchesString()
        {
            HttpClient client = new HttpClient();
            return client.GetStringAsync(GetComingMatchesURL).Result;
        }

        private double DecimalOddConvert(string input)
        {
            string sign = input.Substring(0, 1);
            int americanOddValue = int.Parse(input.Substring(1));
            double result;

            if(sign == "+")
            {
                result = 1 + (americanOddValue / 100.00);
            }
            else
            {
                result = 1 + (100.00 / americanOddValue);
            }

            return Math.Round(result,2);
        }

        public async Task WriteNewMatches(ICollection<FootballMatchGetModel> gettedMatches)
        {
            foreach (var gettedMatch in gettedMatches)
            {
                bool isMatchExist = await IsMatchExist(gettedMatch);

                if (!isMatchExist) 
                {
                    await CreateMatch(gettedMatch);
                }
            }
        }

        private async Task<bool> IsMatchExist(FootballMatchGetModel gettedMatch)
        {
            var findedMatch = await context
                .Matches
                .FirstOrDefaultAsync(m => m.HomeTeam.Name == gettedMatch.HomeTeamName
                    && m.AwayTeam.Name == gettedMatch.AwayTeamName);
            if (findedMatch == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private async Task<FootballMatch> CreateMatch(FootballMatchGetModel gettedMatch)
        {
            FootballTeam? homeTeam =  await context
                .Teams
                .FirstOrDefaultAsync(t => t.Name == gettedMatch.HomeTeamName);

            if (homeTeam == null){
                homeTeam = await CreateFootballTeam(gettedMatch.HomeTeamName);
            }

            FootballTeam? awayTeam = await context
               .Teams
               .FirstOrDefaultAsync(t => t.Name == gettedMatch.AwayTeamName);

            if (awayTeam == null)
            {
                awayTeam = await CreateFootballTeam(gettedMatch.AwayTeamName);
            }


            FootballMatch match = new FootballMatch
            {
                MatchStatus = Common.Enums.MatchStatus.NotStarted,
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
            };

            return match;
        }

        private async Task<FootballTeam> CreateFootballTeam(string footballName)
        {
            FootballTeam team = new FootballTeam
            {
                Name = footballName,
            };

            await context.Teams.AddAsync(team);
            await context.SaveChangesAsync();
            return team;
        }

        private DateTime mapStartDate(string dateString)
        {
            dateString = Regex.Replace(dateString, @"\b(\d+)(st|nd|rd|th)\b", "$1");
            string format = "ddd d MMM h:mmtt";
            DateTime date = DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture);
            return date;
        }

        public async Task<int> WriteNewMatchesAsync(ICollection<FootballMatchGetModel> gettedMatches)
        {
            List<FootballMatch>? matches = gettedMatches
                .Select(gm => new FootballMatch
                {
                      AwayTeam = new FootballTeam{ Name = gm.AwayTeamName },
                      HomeTeam = new FootballTeam{ Name = gm.HomeTeamName },
                      MatchStatus = MatchStatus.NotStarted,
                      StartDateUtc = mapStartDate(gm.StartDateTime),
                      StartDateBg = mapStartDate(gm.StartDateTime).AddHours(3),
                })
                .ToList();

            var writedMatches = context
                .Matches
                .Where(m => m.MatchStatus != MatchStatus.Finished)
                .Include("HomeTeam")
                .Include("AwayTeam")
                .ToList();

            HashSet<FootballMatch> newMatches = new HashSet<FootballMatch>();

            if(matches != null)
            {
                foreach (var match in matches)
                {
                    if (writedMatches.Any(m => m.HomeTeam.Name == match.HomeTeam.Name && m.AwayTeam.Name == match.AwayTeam.Name && m.StartDateUtc == match.StartDateUtc))
                    {
                        continue;
                    }

                    newMatches.Add(match);

                }
            }

            
            if(newMatches.Count > 0)
            {
                await context.AddRangeAsync(newMatches);
                await context.SaveChangesAsync();
            }
            
            return newMatches.Count();
        }

    }

        
}


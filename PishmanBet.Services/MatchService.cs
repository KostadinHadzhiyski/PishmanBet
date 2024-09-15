using PishmanBet.Data.ViewModels;
using PishmanBet.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PishmanBet.Services
{
    public class MatchService : IMatchService
    {
        private const string GetComingMatchesURL = "https://sportsbook.draftkings.com/leagues/soccer/champions-league";

        private string matchesString = null!;

        public MatchService()
        {
            matchesString = GetMatchesString();
        }

        public async Task<ICollection<FootballMatchViewModel>> GetComingMatches()
        {

            List<FootballMatchViewModel>? footballMatches = GetFootballMatches().ToList();


            foreach (FootballMatchViewModel footballMatch in footballMatches)
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

        private ICollection<FootballMatchViewModel> GetFootballMatches()
        { 

            string pattern = @"Event Accordion for ([a-zA-z 0-9]*) vs ([a-zA-Z 0-9]*)";


            RegexOptions regexOptions = RegexOptions.IgnoreCase;

            List<FootballMatchViewModel> footballMatches = new List<FootballMatchViewModel>();

            foreach (Match match in Regex.Matches(matchesString, pattern, regexOptions))
            {
                FootballMatchViewModel footballMatch = new FootballMatchViewModel();
                footballMatch.HomeTeamName = match.Groups[1].Value;
                footballMatch.AwayTeamName = match.Groups[2].Value;

                footballMatches.Add(footballMatch);
            }

            return footballMatches;
        }

        private string GetMatchOddsPattern(FootballMatchViewModel footballMatch)
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

        private decimal DecimalOddConvert(string input)
        {
            string sign = input.Substring(0, 1);
            int americanOddValue = int.Parse(input.Substring(1));

            if(sign == "-")
            {
                return 1 + (100 / americanOddValue);
            }
            else
            {
                return 1 + (americanOddValue / 100);
            }
        }
    }

        
    }


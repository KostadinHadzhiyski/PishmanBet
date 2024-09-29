
using PishmanBet.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PishmanBet.Data.Models
{
    public class FootballMatch
    {
        public FootballMatch()
        {
            Id = Guid.NewGuid();
        }


        [Key]
        public Guid Id { get; set; }


        [Required]
        //[ForeignKey(nameof(HomeTeam))]
        public Guid HomeTeamId { get; set; }

        public virtual FootballTeam HomeTeam { get; set; } = null!;


        [Required]
        //[ForeignKey(nameof(AwayTeam))]
        public Guid AwayTeamId { get; set; }

        public virtual FootballTeam AwayTeam { get; set; } = null!;

        [Required]
        public DateTime StartDateUtc { get; set; }

        public DateTime StartDateBg { get; set; } 

        public int HomeTeamScore { get; set; }

        public int AwayTeamScore { get; set; }

        public int ScoreSign { get; set; }

        public MatchStatus MatchStatus { get; set; }
    }
}

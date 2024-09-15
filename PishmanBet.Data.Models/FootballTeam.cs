
namespace PishmanBet.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using static PishmanBet.Common.ValidationConstants.FootballTeam;
    public class FootballTeam
    {
        public FootballTeam()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(FootballTeamNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(FootballTeamOddNameMaxLength)]
        public string OddName { get; set; } = null!;

        [Required]
        [MaxLength(FootballTeamScoreNameMaxLength)]
        public string ScoreName { get; set; } = null!;

        public ICollection<FootballMatch> Matches { get; set; } = new HashSet<FootballMatch>();
    }
}

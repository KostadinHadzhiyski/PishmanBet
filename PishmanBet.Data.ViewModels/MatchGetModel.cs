using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PishmanBet.Data.ViewModels
{
    public class MatchGetModel
    {

        [Required]
        public string HomeTeamName { get; set; } = null!;


        [Required]
        public string AwayTeamName { get; set; } = null!;

        [Required]
        public string StartDate { get; set; } = null!;


    }
}

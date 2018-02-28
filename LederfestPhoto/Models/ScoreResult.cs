using System.Linq;

namespace LederfestPhoto.Models
{
    public class ScoreResult
    {
        public string Team;
        //public List<Photo> Photos;
        public int Score { get; set; }
        public ScoreResult(IGrouping<Team, Photo> grouping)
        {
            Team = grouping.Key.Name;
            //Photos = grouping.Select(e => e).ToList();
            //Rating = Photos.Where(r => r.Rating > -1).Sum(r => r.Rating);
            
            Score = grouping.Select(e => e).Where(r => r.Rating > -1).Sum(r => r.Rating);

        }

        
    }
}
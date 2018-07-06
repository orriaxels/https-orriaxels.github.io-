namespace API.Models.ViewModels
{
    public class EditPlayerViewModel
    {
        public string name { get; set; }
        public int wins { get; set; }
        public int losses { get; set; }
        public int draws { get; set; }
        public int gamesWon { get; set; }
        public int gamesLost { get; set; }
        public int attented { get; set; }
    }
}
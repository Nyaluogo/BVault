namespace Bingi_Storage.Models
{
    public class Bet
    {
        public Guid Id { get; set; }
        public AppUser User { get; set; }
        public BettingEvent BettingEvent { get; set; }
        public BettingMarket BettingMarket { get; set; }
        public BettingOdds Odds { get; set; }
        public decimal? PotentialPayout { get; set; }
        public string Selection { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

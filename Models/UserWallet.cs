using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bingi_Storage.Models
{
    public class UserWallet
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; }
        public enum Status { ACTIVE, INACTIVE, FROZEN, CLOSED }
        public Status WalletStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

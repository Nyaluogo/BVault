using Microsoft.AspNetCore.Identity;

namespace Bingi_Storage.Models
{
    public class AppUser : IdentityUser
    {
        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; }
        public string? FirstName { get; set; }
        public string? OtherName { get; set; }
        public string PasswordHash { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public UserProfile Profile { get; set; }
        public ICollection<UserWallet> Wallets { get; set; }
        //public UserLibrary Library { get; set; }
        public enum AccountStatus { INACTIVE, ACTIVE, SUSPENDED, BANNED }
        public AccountStatus MyAccStatus { get; set; } = AccountStatus.INACTIVE;
        public enum KycStatus { VERIFIED, LEARNING, REJECTED, UNKNOWN }
        public KycStatus MyKycStatus { get; set; } = KycStatus.UNKNOWN;
        public DateTime DateOfRegistration { get; set; }
        public DateTime DateOfLastLogin { get; set; }
        public bool IsEmailVerified { get; set; } = false;
        public bool IsPhoneVerified { get; set; } = false;
        public bool IsPublisher { get; set; } = false;
        public bool IsAdmin { get; set; } = false;
        public bool IsSuperAdmin { get; set; } = false;

    }
}

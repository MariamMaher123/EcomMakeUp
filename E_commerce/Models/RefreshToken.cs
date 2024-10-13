using Microsoft.EntityFrameworkCore;

namespace EcomMakeUp.Models
{
    [Owned]
    public class RefreshToken
    {
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiresOn;
        public DateTime CreatedOn { get; set; }
        public DateTime? RevokedOn { get; set; }//تم الالغاء
        public bool IsActive => RevokedOn == null && !IsExpired;
    }
}

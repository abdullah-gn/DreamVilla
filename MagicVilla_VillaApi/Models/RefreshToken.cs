using System.ComponentModel.DataAnnotations;

namespace DreamVilla_VillaApi.Models
{
	public class RefreshToken
	{
        [Key]
        public int Id  { get; set; }
        public string UserId { get; set; }
        public string JwtTokenId { get; set; }
        //mark this invalid once consumed for the first time
        public string Refresh_Token { get; set; }
        public bool IsValid { get; set; }
        public DateTime ExpiresAt { get; set; }

	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAuto_API.Infrastructure
{
    [Table("RefreshTokens")]
    public class EntityRefreshTokenEntityBase
    {
        [Required]
        public string Token { get; set; } = default!;

        [Required]
        public string UserName { get; set; } = default!; // Hoặc kiểu phù hợp với hệ thống (Guid, int...)

        [Required]
        public DateTime ExpiryDate { get; set; }

        public string? UserAgent { get; set; } // (Tuỳ chọn) Lưu trình duyệt thiết bị

        public string? IpAddress { get; set; } // (Tuỳ chọn) IP lúc cấp token
    }
}

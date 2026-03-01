using RobotXTest.DataAccess.Core.Enums;

namespace RobotXTest.Core.Models
{
    public class UpdateRequestDto
    {
        public long CardCode { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? SurName { get; set; }
        public string? PhoneMobile { get; set; }
        public string? Email { get; set; }
        public GenderType? GenderId { get; set; }
        public DateTime? Birthday { get; set; }
        public string? City { get; set; }
        public string? Pincode { get; set; }
        public decimal? Bonus { get; set; }
        public decimal? Turnover { get; set; }
    }
}

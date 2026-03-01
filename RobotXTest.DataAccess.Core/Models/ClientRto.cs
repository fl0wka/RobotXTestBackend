using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RobotXTest.DataAccess.Core.Enums;

namespace RobotXTest.DataAccess.Core.Models
{
    public class ClientRto
    {
        [Key]
        [Column("card_code")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long CardCode { get; set; }

        [Column("last_name")]
        [MaxLength(100)]
        public string? LastName { get; set; }

        [Column("first_name")]
        [MaxLength(100)]
        public string? FirstName { get; set; }

        [Column("sur_name")]
        [MaxLength(100)]
        public string? SurName { get; set; }

        [Column("phone_mobile")]
        [MaxLength(20)]
        public string? PhoneMobile { get; set; }

        [Column("email")]
        [MaxLength(100)]
        public string? Email { get; set; }

        [Column("gender_id")]
        public GenderType? GenderId { get; set; }

        [Column("birthday")]
        public DateTime? Birthday { get; set; }

        [Column("city")]
        public string? City { get; set; }

        [Column("pincode")]
        public string? Pincode { get; set; }

        [Column("bonus", TypeName = "decimal(18,2)")]
        public decimal? Bonus { get; set; }

        [Column("turnover", TypeName = "decimal(18,2)")]
        public decimal? Turnover { get; set; }
    }
}

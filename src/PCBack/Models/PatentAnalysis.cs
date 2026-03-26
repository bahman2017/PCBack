using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCBack.Models;

[Table("patent_analyses")]
public class PatentAnalysis
{
    [Key]
    public Guid Id { get; set; }

    [MaxLength(64)]
    public string? PatentNumber { get; set; }

    [MaxLength(1024)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(512)]
    public string PatentOwner { get; set; } = string.Empty;

    [MaxLength(64)]
    public string PatentStatus { get; set; } = string.Empty;

    /// <summary>Comma-separated technology tags (MVP storage).</summary>
    public string TechnologyTags { get; set; } = string.Empty;

    /// <summary>Comma-separated potential markets.</summary>
    public string PotentialMarkets { get; set; } = string.Empty;

    /// <summary>Comma-separated commercial opportunities.</summary>
    public string CommercialOpportunities { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}

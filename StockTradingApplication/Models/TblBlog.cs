using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace StockTradingApplication.Models;

public partial class TblBlog
{
    public string Id { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Html { get; set; } = null!;

    public string? Css { get; set; }

    public string? Data { get; set; }

    public bool IsPublished { get; set; }

    public string? MetaData { get; set; }

    public DateTime CreatedAt { get; set; }

    public string CreatedBy { get; set; } = null!;
 
    public DateTime UpdatedAt { get; set; }

    public string UpdatedBy { get; set; } = null!;

    [JsonIgnore]
    public bool IsDeleted { get; set; }

    public string Route { get; set; } = null!;

    public string CoverImage { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string Description { get; set; } = null!;
}

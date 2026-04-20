using System.Text.Json.Serialization;

namespace StockTradingApplication.Models.ResponseModels
{
    public class BlogResponseModel
    {
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Html { get; set; } = null!;

        public string Css { get; set; } = string.Empty;

        public string Data { get; set; } = string.Empty;

        public bool IsPublished { get; set; } 

        public string MetaData { get; set; } = string.Empty;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? CreatedAt { get; set; }

        public string CreatedBy { get; set; } = string.Empty;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? UpdatedAt { get; set; }

        public string UpdatedBy { get; set; } = string.Empty;

        public string Route { get; set; } = string.Empty;

        public string CoverImage { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        
        public BlogResponseModel(TblBlog blog, bool isDatesIncluded = false)
        {
            Id = blog.Id;
            Title = blog.Title;
            Html = blog.Html;
            Css = blog.Css ?? string.Empty;
            Data = blog.Data ?? string.Empty;
            IsPublished = blog.IsPublished;
            MetaData = blog.MetaData ?? string.Empty;
            CreatedAt = isDatesIncluded ? blog.CreatedAt.ToString("dd-MM-yyyy HH:mm:ss") : null;
            CreatedBy = blog.CreatedBy;
            UpdatedAt = isDatesIncluded ? blog.UpdatedAt.ToString("dd-MM-yyyy HH:mm:ss") : null;
            UpdatedBy = blog.UpdatedBy;
            Route = blog.Route;
            CoverImage = blog.CoverImage;
            Category = blog.Category;
            Description = blog.Description;

        }
    }
}

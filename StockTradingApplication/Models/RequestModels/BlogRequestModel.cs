namespace StockTradingApplication.Models.RequestModels
{
    public class BlogRequestModel
    {
        /// <summary>
        /// Blog Id (keep null to add)
        /// </summary>
        public string? Id { get; set; } = null;

        /// <summary>
        /// Email of user
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Title of Blog
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Html as string
        /// </summary>
        public string? Html { get; set; }

        /// <summary>
        /// Css as string
        /// </summary>
        public string? Css { get; set; }

        /// <summary>
        /// Data as json string
        /// </summary>
        public string? Data { get; set; }

        /// <summary>
        /// Is blog Published
        /// </summary>
        public bool? IsPublished { get; set; }

        /// <summary>
        /// MetaData about blog
        /// </summary>
        public string? MetaData { get; set; }

        /// <summary>
        /// Route for blog
        /// </summary>
        public string? Route { get; set; }

        /// <summary>
        /// Image File (png, jpg, svg)
        /// </summary>
        public IFormFile? CoverImage { get; set; }

        /// <summary>
        /// Category of blog
        /// </summary>
        public string? Category { get; set; }

        /// <summary>
        /// Description of blog
        /// </summary>
        public string? Description { get; set; }
    }
}

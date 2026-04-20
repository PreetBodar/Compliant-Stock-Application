using System.Text.Json.Serialization;

namespace StockTradingApplication.Models.ResponseModels
{
    public class ModuleResponseModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("moduleName")]
        public string ModuleName { get; set; } = null!;

        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; }
    }
}

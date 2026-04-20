namespace StockTradingApplication.Models.ExtraModels
{
    public class NotComplianceSectorsModel
    {
        public bool Firearms { get; set; } = false;
        public bool Alcohol { get; set; } = false;
        public bool Tobacco { get; set; } = false;
        public bool Gambling { get; set; } = false;
        public bool AdultShows { get; set; } = false;
        public bool ImpureFoods { get; set; } = false;
        public bool UsuriousInstitutions { get; set; } = false;
    }
}

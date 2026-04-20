using Microsoft.EntityFrameworkCore;
using StockTradingApplication.Models;
using StockTradingApplication.Models.Enums;
using StockTradingApplication.Models.ExtraModels;
using StockTradingApplication.Models.ExtraModels.ChartApiModel;
using StockTradingApplication.Models.RequestModels;
using StockTradingApplication.Models.ResponseModels;
using StockTradingApplication.Services.IServices;
using System.Net;
using System.Text.Json;

namespace StockTradingApplication.Services
{
    /// <summary>
    /// Stock Service
    /// </summary>
    public class StockService : IStockService
    {
        private readonly StockTradingApplicationContext _context;
        private readonly IExternalApiService _externalApiService;
        private readonly string chartApiUrl;
        private readonly string fmpApiKey;
        private readonly string fmpCompanyProfileUrl , fmpBalanceSheetUrl , fmpIncomeStatementUrl;
        private readonly IConfiguration _config;

        public StockService(StockTradingApplicationContext context, IExternalApiService externalApiService,IConfiguration configuration)
        {
            _context = context;
            _externalApiService = externalApiService;
            chartApiUrl = configuration["API:ChartApiUrl"]!;
            fmpApiKey = configuration["API:FMP_Key"]!;
            fmpCompanyProfileUrl = configuration["API:FMP_CompanyProfileUrl"]!;
            fmpBalanceSheetUrl = configuration["API:FMP_BalanceSheetUrl"]!;
            fmpIncomeStatementUrl = configuration["API:FMP_IncomeStatementUrl"]!;
            _config = configuration;
        }

        /// <summary>
        /// Get Stock By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<StandardResponseModel> GetStock(Guid id)
        {
            try
            {
                TblStock? stock = await _context.TblStocks.FirstOrDefaultAsync(item => item.Id == id && !item.IsDeleted);
                if(stock == null)
                {
                    return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.StockNotFound, null);
                }
                return new StandardResponseModel(HttpStatusCode.OK, AppMessages.DataFetched, new StockResponseModel(stock));
            }
            catch(Exception ex)
            {
                return new StandardResponseModel(ex);    
            }
        }

        /// <summary>
        /// Get All Stocks
        /// </summary>
        /// <returns></returns>
        public async Task<StandardResponseModel> GetAllStocks()
        {
            try
            {
                var stockList = await _context.TblStocks
                                     .Where(stock => !stock.IsDeleted)
                                     .Select(stock => new StockResponseModel(stock, null))
                                     .ToListAsync();
                    
                return new StandardResponseModel(HttpStatusCode.OK, AppMessages.DataFetched, stockList);
            }
            catch (Exception ex)
            {
                return new StandardResponseModel(ex);
            }
        }

        /// <summary>
        /// Add or Edit Stock
        /// </summary>
        /// <param name="stockRequest"></param>
        /// <returns></returns>
        public async Task<StandardResponseModel> AddOrEditStock(StockRequestModel stockRequest)
        {
            try
            {
                //check if user exist
                if(!await _context.TblUsers.AnyAsync(user => user.Id == stockRequest.UserId && !user.IsDelete))
                {
                    return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.UserNotFound, null);
                }

                TblStock? stock;
                bool isForUpdate = stockRequest.Id != null;
                if (isForUpdate)
                {
                    stock = await _context.TblStocks.FirstOrDefaultAsync(item => item.Id.ToString() == stockRequest.Id && !item.IsDeleted); 
                    if(stock == null)
                    {
                        return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.StockNotFound, null);
                    }
                    stock.ModifiedBy = stockRequest.UserId;
                    stock.ModifiedDateTime = DateTime.Now;
                }
                else
                {
                    stock = new TblStock();
                    stock.Id = Guid.NewGuid();
                    stock.CreatedBy = stockRequest.UserId;
                    stock.CreatedDateTime = DateTime.Now;
                }

                stock.Name = stockRequest.Name ?? stock.Name;
                stock.Ticker = stockRequest.Ticker ?? stock.Ticker;
                stock.Sector = stockRequest.Sector ?? stock.Sector;
                stock.Country = stockRequest.Country ?? stock.Country;
                stock.Mcap = stockRequest.Mcap ?? stock.Mcap;
                stock.Currency = stockRequest.Currency ?? stock.Currency;
                stock.Compliance = stockRequest.Compliance == 0 ? stock.Compliance : (int) stockRequest.Compliance;
                stock.IsActive = stockRequest.IsActive ?? stock.IsActive;
                stock.IsDeleted = false;
                stock.Exchange = stockRequest.Exchange ?? stock.Exchange;
                stock.NotComplianceSectors = JsonSerializer.Serialize<NotComplianceSectorsModel>(new NotComplianceSectorsModel());
                stock.IsHaramRevenueOver5Per = stockRequest.IsHaramRevenueOver5Per ?? stock.IsHaramRevenueOver5Per;
                
                //check if stock with same ticker exist
                if(await _context.TblStocks.AnyAsync(item => item.Ticker.ToLower() == stock.Ticker.ToLower() && 
                                                             item.Id != stock.Id && 
                                                             !item.IsDeleted ))
                {
                    return new StandardResponseModel(HttpStatusCode.Conflict,AppMessages.StockAlreadyExist, null);
                }

                if(isForUpdate) _context.TblStocks.Update(stock);
                else await _context.TblStocks.AddAsync(stock);
                await _context.SaveChangesAsync();

                return new StandardResponseModel(isForUpdate ? HttpStatusCode.OK : HttpStatusCode.Created,
                                                 isForUpdate ? AppMessages.Updated : AppMessages.Added,
                                                 new StockResponseModel(stock));
            }
            catch (Exception ex)
            {
                return new StandardResponseModel(ex);
            }
        }

        /// <summary>
        /// Delete Stock By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<StandardResponseModel> DeleteStock(Guid id)
        {
            try
            {
                TblStock? stock = await _context.TblStocks.FirstOrDefaultAsync(item => item.Id == id && !item.IsDeleted);
                if (stock == null)
                {
                    return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.StockNotFound, null);
                }
                stock.IsDeleted = true;
                _context.TblStocks.Update(stock);
                await _context.SaveChangesAsync();
                return new StandardResponseModel(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return new StandardResponseModel(ex);
            }
        }

        /// <summary>
        /// Get Dynamic Stock data with pagination, search and sort functionality
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderByColumn"></param>
        /// <param name="sortDirection"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public async Task<StandardResponseModel> GetDynamicStockData(
            int page,
            int pageSize,
            string? orderByColumn,
            string sortDirection,
            string? searchText )
        {
            try
            {
                var stockList = (from stock in _context.TblStocks
                                 where !stock.IsDeleted
                                 select stock).AsQueryable();
                //search
                if (searchText != null)
                {
                    searchText = searchText.ToLower();

                    stockList = stockList.Where(stock => stock.Name.ToLower().Contains(searchText)
                                                      || stock.Ticker.ToLower().Contains(searchText)
                                                      || stock.Sector.ToLower().Contains(searchText)
                                                      || stock.Country.ToLower().Contains(searchText)
                                                      || stock.Mcap.ToLower().Contains(searchText)
                                                      || stock.Currency.ToLower().Contains(searchText)
                                                      || stock.Exchange.ToLower().Contains(searchText)
                                                      || stock.Compliance.ToString() == searchText);
                }
                //orderby
                if (orderByColumn != null)
                {
                    bool isDescending = sortDirection.ToLower() == "descending";
                    stockList = orderByColumn.ToLower() switch
                    {
                        "name" => isDescending ? stockList.OrderByDescending(stock => stock.Name) : stockList.OrderBy(stock => stock.Name),
                        "ticker" => isDescending ? stockList.OrderByDescending(stock => stock.Ticker) : stockList.OrderBy(stock => stock.Ticker),
                        "sector" => isDescending ? stockList.OrderByDescending(stock => stock.Sector) : stockList.OrderBy(stock => stock.Sector),
                        "country" => isDescending ? stockList.OrderByDescending(stock => stock.Country) : stockList.OrderBy(stock => stock.Country),
                        "currency" => isDescending ? stockList.OrderByDescending(stock => stock.Currency) : stockList.OrderBy(stock => stock.Currency),
                        "exchange" => isDescending ? stockList.OrderByDescending(stock => stock.Exchange) : stockList.OrderBy(stock => stock.Exchange),
                        "compliance" => isDescending ? stockList.OrderByDescending(stock => stock.Compliance) : stockList.OrderBy(stock => stock.Compliance),
                        _ => isDescending ? stockList.OrderByDescending(stock => stock.Id) : stockList.OrderBy(stock => stock.Id)
                    };
                }
                //pagination
                int totalCount = await stockList.CountAsync();
                int pages = (int)Math.Ceiling((decimal)totalCount / pageSize);
                stockList = stockList.Skip((page - 1) * pageSize).Take(pageSize);
                var filteredData = await stockList.Select(stock => new StockResponseModel(stock, null)).ToListAsync();

                return new StandardResponseModel(HttpStatusCode.OK, AppMessages.DataFetched, new
                {
                    page,
                    pages,
                    pageSize,
                    Result = filteredData,
                    totalCount,
                    FilterCount = filteredData.Count,
                });
            }
            catch (Exception ex)
            {
                return new StandardResponseModel(ex);
            }
        }

        /// <summary>
        /// Get Quantity of stock that user has
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="stockId"></param>
        /// <returns></returns>
        public async Task<StandardResponseModel> GetStockQuantity(Guid userId, Guid stockId)
        {
            try
            {
                //check if user exist
                if(!await _context.TblUsers.AnyAsync(user => user.Id == userId && !user.IsDelete))
                {
                    return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.UserNotFound, null);
                }
                //check if stock exist
                if(!await _context.TblStocks.AnyAsync(stock => stock.Id == stockId && !stock.IsDeleted))
                {
                    return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.StockNotFound, null);
                }

                var transactionTypeData = from transaction in _context.TblStockTransactionDetails
                                          where transaction.UserId == userId && transaction.StockId == stockId
                                          group transaction by transaction.TransactionType;
                var countByType = await (from transactionType in transactionTypeData
                                         select new
                                         {
                                             transactionType.Key,
                                             Quantity = transactionType.Sum(item => item.Quantity)
                                         }).ToListAsync();
                
                int buyQuantity = countByType.Where(item => item.Key == "BUY").Select(item =>  item.Quantity).FirstOrDefault(0);
                int sellQuantity = countByType.Where(item => item.Key == "SALE").Select(item => item.Quantity).FirstOrDefault(0);

                return new StandardResponseModel(HttpStatusCode.OK, AppMessages.DataFetched, new
                {
                    userId,
                    stockId,
                    buyQuantity,
                    sellQuantity,
                    totalQuantity = buyQuantity - sellQuantity
                });

            }
            catch(Exception ex)
            {
                return new StandardResponseModel(ex);
            }
        }

        /// <summary>
        /// Get stock details by ticker
        /// </summary>
        /// <param name="ticker"></param>
        /// <returns></returns>
        public async Task<StandardResponseModel> GetStockByTicker(string ticker)
        {
            try
            {
                TblStock? stock = await _context.TblStocks.FirstOrDefaultAsync(item => item.Ticker.ToLower() == ticker.ToLower() && !item.IsDeleted);
                if (stock == null)
                {
                    return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.StockNotFound, null);
                }
                return new StandardResponseModel(HttpStatusCode.OK, AppMessages.DataFetched, new StockResponseModel(stock));
            }
            catch (Exception ex)
            {
                return new StandardResponseModel(ex);
            }
        }

        /// <summary>
        /// Get full stock detail from chart API
        /// </summary>
        /// <param name="ticker"></param>
        /// <returns></returns>
        public async Task<StandardResponseModel> GetFullStockDetailsFromChart(string ticker)
        {
            try
            {
                TblStock? stock = await _context.TblStocks.FirstOrDefaultAsync(item => item.Ticker.ToLower() == ticker.ToLower() && !item.IsDeleted);
                if(stock == null)
                {
                    return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.StockNotFound, null);
                }
                //get data from chart api
                string url = string.Format(chartApiUrl, ticker);
                var response = await _externalApiService.SendGetRequest<ChartApiResponseModel>(url);
                if(!response.Success)
                {
                    if (response.Exception != null)
                    {
                        return new StandardResponseModel(response.Exception);
                    }
                    return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.StockNotFound, null);
                }

                return new StandardResponseModel(HttpStatusCode.OK, AppMessages.DataFetched, new StockResponseModel(stock, response.Data?.Chart?.Result?[0]));
            }
            catch(Exception ex)
            {
                return new StandardResponseModel(ex);
            }
        }

        /// <summary>
        /// Get Company Details from FMP api
        /// </summary>
        /// <param name="ticker"></param>
        /// <returns></returns>
        public async Task<StandardResponseModel> GetCompanyDetailsFromFmpApi(string ticker)
        {
            try
            {
                //get data from FMP Api
                string url = string.Format(fmpCompanyProfileUrl, ticker, fmpApiKey);
                var response = await _externalApiService.SendGetRequest<List<FmpApiCompanyProfileModel>>(url);
                if (!response.Success)
                {
                    if (response.Exception != null)
                    {
                        return new StandardResponseModel(response.Exception);
                    }
                    return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.StockNotFound, null);
                }
                if (response.Data == null || response.Data.Count == 0)
                {
                    return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.StockNotFound, null);
                }
                return new StandardResponseModel(HttpStatusCode.OK, AppMessages.DataFetched, response.Data);
            }
            catch (Exception ex)
            {
                return new StandardResponseModel(ex);
            }
        }

        /// <summary>
        /// Get company details with ratios
        /// </summary>
        /// <param name="ticker"></param>
        /// <returns></returns>
        public async Task<StandardResponseModel> GetCompanyDetailsWithRatios(string ticker)
        {
            try
            {
                //fetch required data from profile, income statement and balance sheet
                string profileUrl = string.Format(fmpCompanyProfileUrl, ticker, fmpApiKey);
                string incomeStatementUrl = string.Format(fmpIncomeStatementUrl, ticker, fmpApiKey);
                string balanceSheetUrl = string.Format(fmpBalanceSheetUrl, ticker, fmpApiKey);
                var companyProfiles = await _externalApiService.SendGetRequest<List<FmpApiCompanyProfileModel>>(profileUrl);
                var incomeStatements = await _externalApiService.SendGetRequest<List<FmpApiIncomeStatementModel>>(incomeStatementUrl);
                var balanceSheets = await _externalApiService.SendGetRequest<List<FmpApiBalanceSheetStatementModel>>(balanceSheetUrl);
                if (!companyProfiles.Success || !incomeStatements.Success || !balanceSheets.Success)
                {
                    if(companyProfiles.Exception != null || incomeStatements.Exception != null || balanceSheets.Exception != null)
                    {
                        return new StandardResponseModel(companyProfiles.Exception ?? incomeStatements.Exception ?? balanceSheets.Exception!);
                    }
                    return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.StockNotFound, null);
                }
                if (companyProfiles.Data == null || companyProfiles.Data.Count == 0 ||
                    incomeStatements.Data == null || incomeStatements.Data.Count == 0 ||
                    balanceSheets.Data == null || balanceSheets.Data.Count == 0)
                {
                    return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.StockNotFound, null);
                }

                var companyProfile = companyProfiles.Data[0];
                var balanceSheet = balanceSheets.Data[0];
                var incomeStatement = incomeStatements.Data[0];

                if(balanceSheet.TotalAssets == 0 || incomeStatement.Revenue == 0 || balanceSheet.TotalLiabilities == 0)
                {
                    return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.NecessaryDataNotFound, null);
                }

                var response = new CompanyDetailsWithRatiosModel
                {
                    CompanyName = companyProfile.CompanyName,
                    Market = companyProfile.Exchange,
                    InitialPrice = companyProfile.Price,
                    Sector = companyProfile.Sector,
                    Industry = companyProfile.Industry,
                    MarketCap = companyProfile.MktCap,
                    Currency = companyProfile.Currency,
                    Country = companyProfile.Country,
                    DebtToAssetRatio = Math.Round((double)balanceSheet.TotalDebt / balanceSheet.TotalAssets , 2),
                    InterestIncomePer = Math.Round((double)incomeStatement.InterestIncome / balanceSheet.TotalAssets * 100, 2),
                    Description = companyProfile.Description,
                    InterestBearingDebt = Math.Round((double)balanceSheet.TotalDebt / balanceSheet.TotalAssets * 100, 2),
                    DebtToRevenuePerc = Math.Round((double)balanceSheet.TotalDebt / incomeStatement.Revenue * 100, 2),
                    CashRatioPerc = Math.Round((double)balanceSheet.CashAndCashEquivalents / balanceSheet.TotalLiabilities * 100, 2),
                    isHaramRevenueOver5Per = false
                };
                response.Sectors = GetNotCompliantSectors(response.Description ?? "");
                response.Compliance = FindCompliance(
                    response.Sector ?? "",
                    balanceSheet.TotalDebt,
                    incomeStatement.Revenue,
                    balanceSheet.CashAndCashEquivalents,
                    balanceSheet.TotalAssets);
                return new StandardResponseModel(HttpStatusCode.OK, AppMessages.DataFetched, response);            
            }
            catch (Exception ex)
            {
                return new StandardResponseModel(ex);
            }
        }

        /// <summary>
        /// generate not compilant sectors model from description
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public NotComplianceSectorsModel GetNotCompliantSectors(string description)
        {
            var sectors = new NotComplianceSectorsModel();
            description = description.ToLower();
            
            //get names of all sectors
            List<string> sectorsList = typeof(NotComplianceSectorsModel).GetProperties().Select(property => property.Name).ToList();

            foreach (var sector in sectorsList)
            {
                //set property true if description has matching word with sector keywords
                if(_config.GetSection($"Keywords:{sector}").Get<List<string>>()!.Any(word => description.Contains(word)))
                {
                    typeof(NotComplianceSectorsModel).GetProperty(sector)!.SetValue(sectors, true);
                }
            }
            return sectors;
        }

        /// <summary>
        /// Find Compliance for company
        /// </summary>
        /// <param name="sector"></param>
        /// <param name="debt"></param>
        /// <param name="revenue"></param>
        /// <param name="cash"></param>
        /// <param name="totalAssets"></param>
        /// <returns></returns>
        public ComplianceEnum FindCompliance(string sector,long debt, long revenue, long cash, long totalAssets)
        {
            ComplianceEnum compliance = ComplianceEnum.FullyCompliant;

            List<string> NonCompliantMainSectors = _config.GetSection("Keywords:notCompliantMainSectors").Get<List<string>>()!;
            //check if main sector is compliant or not
            if (NonCompliantMainSectors.Contains(sector.ToLower()))
            {
                compliance = ComplianceEnum.NotCompliant;
            }
            //check if more than 80% asset is cash
            if ((double)cash / totalAssets > 0.8)
            {
                compliance = ComplianceEnum.NotCompliant;
            }
            //check if debt Revenue ratio is more than 33%
            if((double) debt / revenue > 0.33)
            {
                compliance = ComplianceEnum.NotCompliant;
            }

            return compliance;
        }
    }
}

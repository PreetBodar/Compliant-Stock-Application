using Microsoft.EntityFrameworkCore;
using StockTradingApplication.Models;
using StockTradingApplication.Models.Enums;
using StockTradingApplication.Models.RequestModels;
using StockTradingApplication.Models.ResponseModels;
using StockTradingApplication.Services.IServices;
using System.Net;

namespace StockTradingApplication.Services
{
    /// <summary>
    /// bank service
    /// </summary>
    public class BankService : IBankService
    {
        private readonly StockTradingApplicationContext _context;
        public BankService(StockTradingApplicationContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Add or update bank
        /// </summary>
        /// <param name="bankRequest"></param>
        /// <returns></returns>
        public async Task<StandardResponseModel> AddOrEditBank(BankRequestModel bankRequest)
        {
            try
            {
                //check if user exist
                if (!await _context.TblUsers.AnyAsync(user => user.Id == bankRequest.UserId && !user.IsDelete))
                {
                    return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.UserNotFound, null);
                }
                //check if bank already exist
                if (await _context.TblBanks.AnyAsync(bank => bank.BankName == bankRequest.BankName && bank.Id != bankRequest.Id))
                {
                    return new StandardResponseModel(HttpStatusCode.Conflict, AppMessages.BankAlreadyExist, null);
                }

                bool isForUpdate = bankRequest.Id != Guid.Empty;
                TblBank? bank;
                if (isForUpdate)
                {
                    bank = await _context.TblBanks.FindAsync(bankRequest.Id);
                    if(bank == null)
                    {
                        return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.BankNotFound, null);
                    }
                    bank.ModifyBy = bankRequest.UserId;
                    bank.ModifyDatetime = DateTime.Now;
                }
                else
                {
                    bank = new TblBank();
                    bank.Id = Guid.NewGuid();
                    bank.CreatedBy = bankRequest.UserId;
                    bank.CreatedDatetime = DateTime.Now;
                }

                bank.BankName = bankRequest.BankName ?? bank.BankName;
                bank.IsActive = bankRequest.IsActive ?? bank.IsActive;

                if(isForUpdate) _context.TblBanks.Update(bank);
                else await _context.TblBanks.AddAsync(bank);
                await _context.SaveChangesAsync();
                return new StandardResponseModel(isForUpdate ? HttpStatusCode.OK : HttpStatusCode.Created,
                                                 isForUpdate ? AppMessages.Updated : AppMessages.Added,
                                                 bank);
            }
            catch (Exception ex)
            {
                return new StandardResponseModel(ex);
            }
        }

        /// <summary>
        /// Get bank by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<StandardResponseModel> GetBank(Guid id)
        {
            try
            {
                TblBank? bank = await _context.TblBanks.FindAsync(id);
                if (bank == null)
                {
                    return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.BankNotFound, null);
                }
                return new StandardResponseModel(HttpStatusCode.OK, AppMessages.DataFetched, bank);
            }
            catch (Exception ex)
            {
                return new StandardResponseModel(ex);
            }
        }

        /// <summary>
        /// Get All banks 
        /// </summary>
        /// <returns></returns>
        public async Task<StandardResponseModel> GetAllBanks()
        {
            try
            {
                var allBanks = from bank in _context.TblBanks
                               select new
                               {
                                   bank.Id,
                                   bank.BankName,
                                   bank.IsActive
                               };
                return new StandardResponseModel(HttpStatusCode.OK, AppMessages.DataFetched, allBanks);
            }
            catch (Exception ex)
            {
                return new StandardResponseModel(ex);
            }
        }

        /// <summary>
        /// delete bank by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<StandardResponseModel> DeleteBank(Guid id)
        {
            try
            {
                TblBank? bank = await _context.TblBanks.FindAsync(id);
                if (bank == null)
                {
                    return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.BankNotFound, null);
                }
                _context.TblBanks.Remove(bank);
                await _context.SaveChangesAsync();
                return new StandardResponseModel(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return new StandardResponseModel(ex);
            }
        }

        /// <summary>
        /// Get dynamic bank data with pagination search and sort
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderByColumn"></param>
        /// <param name="sortDirection"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public async Task<StandardResponseModel> GetDynamicBankData(
            int page,
            int pageSize,
            string? orderByColumn,
            string sortDirection,
            string? searchText
            )
        {
            try
            {
                var BankList = from bank in _context.TblBanks
                               select new
                               {
                                   bank.Id,
                                   bank.BankName,
                                   bank.IsActive
                               };
                //search
                if(searchText != null)
                {
                    BankList = BankList.Where(bank => bank.BankName.ToLower().Contains(searchText.ToLower()));
                }
                //sort
                if(orderByColumn != null)
                {
                    bool isDescending = sortDirection.ToLower() == "descending";
                    BankList = orderByColumn.ToLower() switch
                    {
                        "bankname" => isDescending ? BankList.OrderByDescending(bank => bank.BankName) :
                                                     BankList.OrderBy(bank => bank.BankName),
                        _ => BankList
                    };
                }
                //pagination
                int totalCount = await BankList.CountAsync();
                int pages = (int)Math.Ceiling((decimal)totalCount / pageSize);
                var filteredList = await BankList.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

                return new StandardResponseModel(HttpStatusCode.OK, AppMessages.DataFetched, new
                {
                    page,
                    pages,
                    pageSize,
                    Result = filteredList,
                    FilterCount = filteredList.Count,
                    totalCount
                });
            }
            catch (Exception ex)
            {
                return new StandardResponseModel(ex);
            }
        }

        /// <summary>
        /// Get user bank details by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<StandardResponseModel> GetUserBankDetails(Guid id)
        {
            try
            {
                var userBankData = await (from userBankDetail in _context.TblUserBankDetails
                                          join bank in _context.TblBanks on userBankDetail.BankId equals bank.Id
                                          join user in _context.TblUsers on userBankDetail.UserId equals user.Id
                                          join createdByUser in _context.TblUsers on userBankDetail.CreatedBy equals createdByUser.Id
                                          join modifyByUser in _context.TblUsers on userBankDetail.ModifyBy equals modifyByUser.Id into modifiedUserData
                                          from modifiedByUser in modifiedUserData.DefaultIfEmpty()
                                          where userBankDetail.Id == id && !userBankDetail.IsDeleted
                                          select new
                                          {
                                              userBankDetail.Id,
                                              userBankDetail.UserId,
                                              userName = user.FirstName + " " + (user.LastName ?? ""),
                                              userBankDetail.BankId,
                                              bank.BankName,
                                              userBankDetail.BankAccountNo,
                                              userBankDetail.AccountHolderName,
                                              userBankDetail.AccountType,
                                              CreatedBy = createdByUser.FirstName + " " + (createdByUser.LastName ?? ""),
                                              CreatedDateTime = userBankDetail.CreatedDatetime.ToString("dd-MM-yyyy HH:mm:ss"),
                                              ModifiedBy = userBankDetail.ModifyBy != null ? modifiedByUser.FirstName + " " + (modifiedByUser.LastName ?? "") : "",
                                              ModifiedDateTime = userBankDetail.ModifyDatetime != null ? userBankDetail.ModifyDatetime.Value.ToString("dd-MM-yyyy HH:mm:ss") : ""
                                          }).FirstOrDefaultAsync();
                if(userBankData == null)
                {
                    return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.UserBankDetailsNotFound, null);
                }
                return new StandardResponseModel(HttpStatusCode.OK, AppMessages.DataFetched, userBankData);
  
            }
            catch(Exception ex)
            {
                return new StandardResponseModel(ex);
            }

        }

        /// <summary>
        /// Add or Edit user bank details
        /// </summary>
        /// <param name="userBankRequest"></param>
        /// <returns></returns>
        public async Task<StandardResponseModel> AddOrEditUserBankDetails(UserBankRequestModel userBankRequest)
        {
            try
            {
                bool isForUpdate = userBankRequest.Id != Guid.Empty;
                TblUserBankDetail? userBankDetail;
                if (isForUpdate)
                {
                    userBankDetail = await _context.TblUserBankDetails.FirstOrDefaultAsync(item => item.Id == userBankRequest.Id && !item.IsDeleted);
                    if(userBankDetail == null)
                    {
                        return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.UserBankDetailsNotFound, null);
                    }
                    userBankDetail.ModifyBy = userBankRequest.UserId;
                    userBankDetail.ModifyDatetime = DateTime.Now;
                }
                else
                {
                    userBankDetail = new TblUserBankDetail();
                    userBankDetail.Id = Guid.NewGuid();
                    userBankDetail.CreatedBy = userBankRequest.UserId;
                    userBankDetail.CreatedDatetime = DateTime.Now;
                }
                //check if user exist
                if(!await _context.TblUsers.AnyAsync(user => user.Id == userBankRequest.UserId && !user.IsDelete))
                {
                    return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.UserNotFound, null);
                }
                //check if bank exist
                if (userBankRequest.BankId != Guid.Empty && !await _context.TblBanks.AnyAsync(bank => bank.Id == userBankRequest.BankId))
                {
                    return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.BankNotFound, null);
                }
                //check if similar record exist
                if (await _context.TblUserBankDetails.AnyAsync(item => item.BankAccountNo == userBankRequest.BankAccountNo && item.Id != userBankRequest.Id))
                {
                    return new StandardResponseModel(HttpStatusCode.Conflict, AppMessages.UserBankDetailsAlreadyFound, null);
                }

                userBankDetail.UserId = userBankRequest.UserId;
                userBankDetail.BankId = userBankRequest.BankId == Guid.Empty ? userBankDetail.BankId : userBankRequest.BankId;
                userBankDetail.BankAccountNo = userBankRequest.BankAccountNo ?? userBankDetail.BankAccountNo;
                userBankDetail.AccountType = userBankRequest.AccountType == 0 ? userBankDetail.AccountType : userBankRequest.AccountType.ToString().ToLower();
                userBankDetail.AccountHolderName = userBankRequest.AccountHolderName ?? userBankDetail.AccountHolderName;
                userBankDetail.IsDeleted = false;
                userBankDetail.IsActive = userBankRequest.IsActive ?? userBankDetail.IsActive;

                if (isForUpdate) _context.TblUserBankDetails.Update(userBankDetail);
                else await _context.TblUserBankDetails.AddAsync(userBankDetail);
                await _context.SaveChangesAsync();

                return new StandardResponseModel(isForUpdate ? HttpStatusCode.OK : HttpStatusCode.Created,
                                                 isForUpdate ? AppMessages.Updated : AppMessages.Added,
                                                 userBankDetail);

            }
            catch(Exception ex)
            {
                return new StandardResponseModel(ex);   
            }
        }

    }
}

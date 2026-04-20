using System.Net;
using StockTradingApplication.Models;
using StockTradingApplication.Models.Enums;
using StockTradingApplication.Models.ResponseModels;
using StockTradingApplication.Services.IServices;
using Microsoft.EntityFrameworkCore;
using StockTradingApplication.Models.RequestModels;

namespace StockTradingApplication.Services
{
    public class ModuleService : IModuleService
    {
        private readonly StockTradingApplicationContext _context;
        public ModuleService(StockTradingApplicationContext context)
        {
            _context = context;
        }

        /// <summary>
        /// get all modules
        /// </summary>
        /// <returns></returns>
        public async Task<StandardResponseModel> GetAllModules()
        {
            try
            {
                //fetch all roles
                var moduleList = await (from module in _context.TblModules
                                        where module.IsDelete == false
                                        select new
                                        {
                                            module.Id,
                                            module.ModuleName,
                                            module.IsActive
                                        }).ToListAsync();
                return new StandardResponseModel(HttpStatusCode.OK, AppMessages.DataFetched, moduleList);
            }
            catch (Exception ex)
            {
                return new StandardResponseModel(ex);
            }
        }

        /// <summary>
        /// Get module by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<StandardResponseModel> GetModule(int id)
        {
            try
            {
                TblModule? module = await _context.TblModules.FirstOrDefaultAsync(item => item.Id == id && item.IsDelete == false);
                //check if module exist
                if (module == null)
                {
                    return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.ModuleNotFound, null);
                }
                return new StandardResponseModel(HttpStatusCode.OK, AppMessages.DataFetched, new { module.Id, module.ModuleName, module.IsActive });

            }
            catch (Exception ex)
            {
                return new StandardResponseModel(ex);
            }
        }

        /// <summary>
        /// Create Module
        /// </summary>
        /// <param name="moduleRequest"></param>
        /// <returns></returns>
        public async Task<StandardResponseModel> CreateModule(ModuleRequestModel moduleRequest)
        {
            try
            {
                //check if module Name exist
                if (await _context.TblModules.AnyAsync(item => item.ModuleName.ToLower() == moduleRequest.ModuleName.ToLower() && item.IsDelete == false))
                {
                    return new StandardResponseModel(HttpStatusCode.Conflict, AppMessages.ModuleAlreadyExist, null);
                }
                TblModule module = new TblModule
                {
                    ModuleName = moduleRequest.ModuleName,
                    IsActive = moduleRequest.isActive,
                    IsDelete = false
                };
                await _context.TblModules.AddAsync(module);
                await _context.SaveChangesAsync();
                return new StandardResponseModel(HttpStatusCode.Created, AppMessages.Added, new { module.Id, module.ModuleName, module.IsActive});
            }
            catch (Exception ex)
            {
                return new StandardResponseModel(ex);
            }
        }

        /// <summary>
        /// update module
        /// </summary>
        /// <param name="moduleRequest"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<StandardResponseModel> UpdateModule(ModuleRequestModel moduleRequest, int id)
        {
            try
            {
                TblModule? module = await _context.TblModules.FirstOrDefaultAsync(item => item.Id == id && item.IsDelete == false);
                //check if role exist
                if (module == null)
                {
                    return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.ModuleNotFound, null);
                }
                if (await _context.TblModules.AnyAsync(item => item.ModuleName.ToLower() == moduleRequest.ModuleName.ToLower() && item.IsDelete == false && item.Id != id))
                {
                    return new StandardResponseModel(HttpStatusCode.Conflict, AppMessages.ModuleAlreadyExist, null);
                }
                module.ModuleName = moduleRequest.ModuleName;
                module.IsActive = moduleRequest.isActive;
                _context.TblModules.Update(module);
                await _context.SaveChangesAsync();
                return new StandardResponseModel(HttpStatusCode.OK, AppMessages.Updated, new { module.Id, module.ModuleName, module.IsActive });
            }
            catch (Exception ex)
            {
                return new StandardResponseModel(ex);
            }
        }

        /// <summary>
        /// Delete Role
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<StandardResponseModel> DeleteModule(int id)
        {
            try
            {
                TblModule? module = await _context.TblModules.FindAsync(id);
                if (module == null)
                {
                    return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.ModuleNotFound, null);
                }
                module.IsDelete = true;
                _context.TblModules.Update(module);
                await _context.SaveChangesAsync();
                return new StandardResponseModel(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return new StandardResponseModel(ex);
            }
        }
    }
}

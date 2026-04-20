using System.Net;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using StockTradingApplication.Models;
using StockTradingApplication.Models.Enums;
using StockTradingApplication.Models.RequestModels;
using StockTradingApplication.Models.ResponseModels;
using StockTradingApplication.Services.IServices;

namespace StockTradingApplication.Services
{
    /// <summary>
    /// Permission service
    /// </summary>
    public class PermissionService : IPermissionService
    {
        private readonly StockTradingApplicationContext _context;
        private readonly string superAdminId;
        public PermissionService(StockTradingApplicationContext context,IConfiguration configuration)
        {
            _context = context;
            superAdminId = configuration["superAdminId"];
        }

        /// <summary>
        /// Get Permission by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<StandardResponseModel> GetPermission(int id)
        {
            try
            {
                //check if permission exist
                if (!await _context.TblPermissions.AnyAsync(permission => permission.Id == id))
                {
                    return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.PermissionNotFound, null);
                }
                var permissionDetails = await (from permission in _context.TblPermissions
                                               join module in _context.TblModules on permission.ModuleId equals module.Id
                                               join role in _context.AspNetRoles on permission.RoleId equals role.Id
                                               where permission.Id == id
                                               select new
                                               {
                                                   permission.Id,
                                                   permission.IsView,
                                                   permission.IsAdd,
                                                   permission.IsEdit,
                                                   permission.IsDelete,
                                                   Module = new
                                                   {
                                                       module.Id,
                                                       module.ModuleName,
                                                       module.IsActive
                                                   },
                                                   role = new
                                                   {
                                                       role.Id,
                                                       role.Name
                                                   }
                                               }).FirstOrDefaultAsync();
                return new StandardResponseModel(HttpStatusCode.OK, AppMessages.DataFetched, permissionDetails);
            }
            catch (Exception ex)
            {
                return new StandardResponseModel(ex);
            }
        }

        /// <summary>
        /// Get permission BY moduleID
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public async Task<StandardResponseModel> GetPermissionByModuleId(int moduleId)
        {
            try
            {
                var permissionDetails = await (from permission in _context.TblPermissions
                                               join module in _context.TblModules on permission.ModuleId equals module.Id
                                               join role in _context.AspNetRoles on permission.RoleId equals role.Id
                                               where module.Id == moduleId
                                               select new
                                               {
                                                   permission.Id,
                                                   roleId = role.Id,
                                                   roleName = role.Name,
                                                   permission.IsView,
                                                   permission.IsAdd,
                                                   permission.IsEdit,
                                                   permission.IsDelete,
                                                   
                                               }).ToListAsync();
                return new StandardResponseModel(HttpStatusCode.OK, AppMessages.DataFetched, permissionDetails);
            }
            catch (Exception ex)
            {
                return new StandardResponseModel(ex);
            }
        }

        /// <summary>
        /// Add permissions for module
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public async Task<StandardResponseModel> AddPermission(int moduleId)
        {
            try
            {
                //check if module exist
                if(!await _context.TblModules.AnyAsync(module => module.Id == moduleId && module.IsDelete == false))
                {
                    return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.ModuleNotFound, null);
                }
                //check if similar permission already exist
                if (await _context.TblPermissions.AnyAsync(item => item.ModuleId == moduleId))
                {
                    return new StandardResponseModel(HttpStatusCode.Conflict, AppMessages.PermissionAlreadyExist, null);
                }
                List<string> roleIdList = await _context.AspNetRoles.Select(role => role.Id).ToListAsync();
                List<TblPermission> permissionList = new List<TblPermission>();
                foreach (string roleId in roleIdList)
                {
                    bool isPermitted = roleId == superAdminId; 
                    permissionList.Add(new TblPermission
                    {
                        ModuleId = moduleId,
                        RoleId = roleId,
                        IsView = isPermitted,
                        IsAdd = isPermitted ,
                        IsEdit = isPermitted,
                        IsDelete = isPermitted
                    });
                }
                await _context.TblPermissions.AddRangeAsync(permissionList);
                await _context.SaveChangesAsync();
                return new StandardResponseModel(HttpStatusCode.Created, AppMessages.Added, permissionList.Select(permission => new
                {
                    permission.Id,
                    permission.RoleId,
                    permission.IsView,
                    permission.IsAdd,
                    permission.IsEdit,
                    permission.IsDelete
                }));
            }
            catch (Exception ex)
            {
                return new StandardResponseModel(ex);
            }
        }

        /// <summary>
        /// Update permission
        /// </summary>
        /// <param name="permissionRequest"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<StandardResponseModel> UpdatePermission(PermissionRequestModel permissionRequest, int id)
        {
            try
            {
                TblPermission? permission = _context.TblPermissions.Find(id);
                //check if permission exist
                if (permission == null)
                {
                    return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.PermissionNotFound, null);
                }

                permission.IsView = permissionRequest.IsView ?? permission.IsView;
                permission.IsAdd = permissionRequest.IsAdd ?? permission.IsAdd;
                permission.IsEdit = permissionRequest.IsEdit ?? permission.IsEdit;
                permission.IsDelete = permissionRequest.IsDelete ?? permission.IsDelete;

                _context.TblPermissions.Update(permission);
                await _context.SaveChangesAsync();
                return new StandardResponseModel(HttpStatusCode.OK, AppMessages.Updated, new
                {
                    permission.Id,
                    permission.ModuleId,
                    permission.RoleId,
                    permission.IsView,
                    permission.IsAdd,
                    permission.IsEdit,
                    permission.IsDelete,
                });
            }
            catch (Exception ex)
            {
                return new StandardResponseModel(ex);
            }
        }

        /// <summary>
        /// Remove Permission
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public async Task<StandardResponseModel> DeletePermission(int moduleId)
        {
            try
            {
                //check if module exist
                if (!await _context.TblModules.AnyAsync(module => module.Id == moduleId))
                {
                    return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.ModuleNotFound, null);
                }
                List<TblPermission>? permission = await _context.TblPermissions.Where(permission => permission.ModuleId == moduleId).ToListAsync();
                _context.TblPermissions.RemoveRange(permission);
                await _context.SaveChangesAsync();
                return new StandardResponseModel(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return new StandardResponseModel(ex);
            }
        }

        /// <summary>
        /// Update permissions for all modules of given roleId
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="permissionRequest"></param>
        /// <returns></returns>
        public async Task<StandardResponseModel> BulkUpdateByRoleId(string roleId, PermissionRequestModel permissionRequest)
        {
            try
            {
                //check if role exist
                if (!await _context.AspNetRoles.AnyAsync(role => role.Id == roleId))
                {
                    return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.RoleNotFound, null);
                }
                List<TblPermission> permissionList = await _context.TblPermissions.Where(permission => permission.RoleId == roleId).ToListAsync();

                foreach (TblPermission permission in permissionList)
                {
                    permission.IsView = permissionRequest.IsView ?? permission.IsView;
                    permission.IsAdd = permissionRequest.IsAdd ?? permission.IsAdd;
                    permission.IsEdit = permissionRequest.IsEdit ?? permission.IsEdit;
                    permission.IsDelete = permissionRequest.IsDelete ?? permission.IsDelete;
                }
                _context.TblPermissions.UpdateRange(permissionList);
                await _context.SaveChangesAsync();
                return new StandardResponseModel(HttpStatusCode.OK, AppMessages.Updated, null);
            }
            catch (Exception ex)
            {
                return new StandardResponseModel(ex);
            }
        }

        /// <summary>
        /// export permission to XL by roleId
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<StandardResponseModel> ExportPermissionsToXL(string roleId)
        {
            try 
            {
                var permissionList = await (from permission in _context.TblPermissions
                                            join module in _context.TblModules on permission.ModuleId equals module.Id
                                            join role in _context.AspNetRoles on permission.RoleId equals role.Id
                                            where permission.RoleId == roleId
                                            select new
                                            {
                                                permission.Id,
                                                ModuleId = module.Id,
                                                module.ModuleName,
                                                Role = role.Name,
                                                permission.IsView,
                                                permission.IsAdd,
                                                permission.IsEdit,
                                                permission.IsDelete
                                            }).ToListAsync();
                if(permissionList.Count == 0)
                {
                    return new StandardResponseModel(HttpStatusCode.NoContent);
                }

                XLWorkbook xLWorkbook = new XLWorkbook();
                IXLWorksheet worksheet = xLWorkbook.AddWorksheet($"{permissionList[0].Role}_Permissions");
                //insert data in XL file 
                worksheet.FirstCell().InsertTable(permissionList);
                //adjusting column width
                worksheet.Column(3).Width = 15;
                worksheet.Column(4).Width = 12;
                
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    xLWorkbook.SaveAs(memoryStream);
                    byte[] fileBytes = memoryStream.ToArray();
                    return new StandardResponseModel(HttpStatusCode.OK, worksheet.Name, fileBytes);
                }
            }
            catch(Exception ex)
            {
                return new StandardResponseModel(ex);
            }
        }

        /// <summary>
        /// Get pagination data for permission
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderByColumn"></param>
        /// <param name="sortDirection"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public async Task<StandardResponseModel> GetDynamicPermissionData(
            int page,
            int pageSize,
            string? orderByColumn,
            string sortDirection,
            string? searchText
            )
        {
            try
            {
                //get all records
                var permissionList = (from permission in _context.TblPermissions
                                      join module in _context.TblModules on permission.ModuleId equals module.Id
                                      join role in _context.AspNetRoles on permission.RoleId equals role.Id
                                      select new
                                      {
                                          permission.Id,
                                          permission.IsView,
                                          permission.IsAdd,
                                          permission.IsEdit,
                                          permission.IsDelete,
                                          Module = new
                                          {
                                              module.Id,
                                              module.ModuleName,
                                              module.IsActive
                                          },
                                          role = new
                                          {
                                              role.Id,
                                              role.Name
                                          }
                                      }).AsQueryable();
                //Apply search
                if(searchText != null)
                {
                    permissionList = permissionList.Where(permission => permission.role.Name.Contains(searchText) ||
                                                                        permission.Module.ModuleName.Contains(searchText));
                }

                //apply orderByColumn
                if(orderByColumn != null)
                {
                    permissionList = orderByColumn.ToLower() switch
                    {
                        "rolename" => sortDirection.ToLower() == "descending" ? 
                                      permissionList.OrderByDescending(permission => permission.role.Name) :
                                      permissionList.OrderBy(permission => permission.role.Name),
                        "modulename" => sortDirection.ToLower() == "descending" ? 
                                        permissionList.OrderByDescending(permission => permission.Module.ModuleName) :
                                        permissionList.OrderBy(permission => permission.Module.ModuleName),
                        _ => sortDirection.ToLower() == "descending" ? 
                             permissionList.OrderByDescending(permission => permission.Id) :
                             permissionList.OrderBy(permission => permission.Id)
                    };
                }

                int totalCount = await permissionList.CountAsync();
                int pages = (int)Math.Ceiling((decimal)totalCount / pageSize);
                var filteredData = await permissionList.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
                return new StandardResponseModel(HttpStatusCode.OK, AppMessages.DataFetched, new
                {
                    page,
                    pages,
                    pageSize,
                    Result = filteredData,
                    FilterCount = filteredData.Count,
                    totalCount
                });
            }
            catch(Exception ex)
            {
                return new StandardResponseModel(ex);
            }
        }


    }
}

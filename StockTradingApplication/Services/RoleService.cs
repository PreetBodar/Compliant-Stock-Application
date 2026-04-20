using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockTradingApplication.Models;
using StockTradingApplication.Models.Enums;
using StockTradingApplication.Models.ResponseModels;
using StockTradingApplication.Services.IServices;
using System.Net;

namespace StockTradingApplication.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly StockTradingApplicationContext _context;
        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, StockTradingApplicationContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        /// <summary>
        /// Get All roles
        /// </summary>
        /// <returns></returns>
        public async Task<StandardResponseModel> GetAllRoles()
        {
            try
            {
                //fetch all roles
                var roles = await (from role in _roleManager.Roles
                                   select new
                                   {
                                       role.Id,
                                       role.Name
                                   }).ToListAsync();
                return new StandardResponseModel(HttpStatusCode.OK, AppMessages.DataFetched, roles);
            }
            catch (Exception ex)
            {
                return new StandardResponseModel(ex);
            }
        }

        /// <summary>
        /// Get Role by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<StandardResponseModel> GetRole(string id)
        {
            try
            {
                IdentityRole? role = await _roleManager.FindByIdAsync(id);
                //check if role exist
                if (role == null)
                {
                    return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.RoleNotFound, null);
                }
                return new StandardResponseModel(HttpStatusCode.OK, AppMessages.DataFetched, new { role.Id, role.Name });

            }
            catch (Exception ex)
            {
                return new StandardResponseModel(ex);
            }
        }

        /// <summary>
        /// Create Role
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<StandardResponseModel> CreateRole(string name)
        {
            try
            {
                //check if role exist
                if (await _roleManager.RoleExistsAsync(name))
                {
                    return new StandardResponseModel(HttpStatusCode.Conflict, AppMessages.RoleAlreadyExist, null);
                }
                IdentityRole role = new IdentityRole
                {
                    Name = name
                };
                var result = await _roleManager.CreateAsync(role);
                if (!result.Succeeded)
                {
                    return new StandardResponseModel(HttpStatusCode.InternalServerError, AppMessages.AddFailed, result.Errors);
                }
                List<int> moduleIdList = await _context.TblModules
                                               .Where(module => module.IsDelete == false)
                                               .Select(module => module.Id).ToListAsync();
                List<TblPermission> permissionList = new List<TblPermission>();
                foreach (int moduleId in moduleIdList)
                {
                    permissionList.Add(new TblPermission
                    {
                        ModuleId = moduleId,
                        RoleId = role.Id,
                        IsAdd = false,
                        IsView = false,
                        IsEdit = false,
                        IsDelete = false
                    });
                }
                await _context.TblPermissions.AddRangeAsync(permissionList);
                await _context.SaveChangesAsync();
                return new StandardResponseModel(HttpStatusCode.Created, AppMessages.Added, new { role.Id, role.Name });
            }
            catch (Exception ex)
            {
                return new StandardResponseModel(ex);
            }
        }

        /// <summary>
        /// Update Role
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<StandardResponseModel> UpdateRole(string name, string id)
        {
            try
            {
                IdentityRole? role = await _roleManager.FindByIdAsync(id);
                //check if role exist
                if (role == null)
                {
                    return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.RoleNotFound, null);
                }
                if (await _roleManager.RoleExistsAsync(name))
                {
                    return new StandardResponseModel(HttpStatusCode.Conflict, AppMessages.RoleAlreadyExist, null);
                }
                role.Name = name;
                var result = await _roleManager.UpdateAsync(role);
                if (!result.Succeeded)
                {
                    return new StandardResponseModel(HttpStatusCode.InternalServerError, AppMessages.UpdateFailed, result.Errors);
                }
                return new StandardResponseModel(HttpStatusCode.OK, AppMessages.Updated, new { role.Id, role.Name });

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
        public async Task<StandardResponseModel> DeleteRole(string id)
        {
            try
            {
                IdentityRole? role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                {
                    return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.RoleNotFound, null);
                }
                var result = await _roleManager.DeleteAsync(role);
                if (!result.Succeeded)
                {
                    return new StandardResponseModel(HttpStatusCode.InternalServerError, AppMessages.DeleteFailed, result.Errors);
                }
                return new StandardResponseModel(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return new StandardResponseModel(ex);
            }
        }

        /// <summary>
        /// Assign role to user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task<StandardResponseModel> AssignRoleToUser(string userId, string roleName)
        {
            try
            {
                IdentityRole? role = await _roleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.RoleNotFound, null);
                }
                ApplicationUser? user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.UserNotFound, null);
                }
                var result = await _userManager.AddToRoleAsync(user, roleName);
                return new StandardResponseModel(HttpStatusCode.OK, AppMessages.Updated, new { userId, roleName });
            }
            catch (Exception ex)
            {
                return new StandardResponseModel(ex);
            }
        }

        /// <summary>
        /// Get Roles by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<StandardResponseModel> GetRolesByUserId(string userId)
        {
            try
            {
                ApplicationUser? user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return new StandardResponseModel(HttpStatusCode.NotFound, AppMessages.UserNotFound, null);
                }
                var roleList = await _userManager.GetRolesAsync(user);
                return new StandardResponseModel(HttpStatusCode.OK, AppMessages.DataFetched, roleList);
            }
            catch (Exception ex)
            {
                return new StandardResponseModel(ex);
            }
        }
    }
}



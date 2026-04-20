using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using StockTradingApplication.Models.RequestModels;
using StockTradingApplication.Models.ResponseModels;

namespace IntergrationTests.IntegrationTests
{
    public class PermissionIntegrationTests
    {
        private readonly HttpClient client = new HttpClient { BaseAddress = new Uri("https://localhost:40002") };
        private readonly int validPermissionId = 693, invalidPermissionId = 999999;
        private readonly int validModuleId = 20, invalidModuleId = 999999;
        private readonly string adminId = "00f9a6d7-fdf8-48b5-b92a-29e87957eb30";
        private record RoleResponse(string id, string name);
        private record PermissionResponse(int id,bool isEdit, bool isView, bool isAdd, bool isDelete );

        /// <summary>
        /// valid case for get module, should return Ok
        /// </summary>
        [Fact]
        public async void Get_Permission_ShouldReturnOk()
        {
            //act
            var response = await client.GetAsync($"permissions/{validPermissionId}");
            var permission = (await StandardResponseModel<PermissionResponse>.DeserializeResponse(response.Content)).Data;

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            permission.Should().NotBeNull();
        }

        /// <summary>
        /// fetch permission that doesn't exist, should return not found
        /// </summary>
        [Fact]
        public async void GetPermission_FetchPermissionNotExist_ShouldReturnNotFound()
        {
            //act
            var response = await client.GetAsync($"permissions/{invalidPermissionId}");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// valid case for get permission by moduleId, should return Ok
        /// </summary>
        [Fact]
        public async void Get_PermissionByModuleId_ShouldReturnOk()
        {
            //act
            var response = await client.GetAsync($"permissions/getPermissionByModuleId/{validModuleId}");
            var permissions = (await StandardResponseModel<List<PermissionResponse>>.DeserializeResponse(response.Content)).Data;

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            permissions.Should().NotBeNullOrEmpty();
        }

        /// <summary>
        /// fetch permissions by module id that doesn't exist, should return not found
        /// </summary>
        [Fact]
        public async void GetPermissionByModuleId_FetchModuleNotExist_ShouldReturnNotFound()
        {
            //act
            var response = await client.GetAsync($"permissions/module/{invalidModuleId}");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// valid case for add permission, should return created
        /// </summary>
        [Fact]
        public async void Add_Permissions_ShouldReturnCreated()
        {
            //arrange
            var response = await client.PostAsJsonAsync($"v1/modules",new ModuleRequestModel{ModuleName = "permissionTestModule", isActive = true });
            var module = (await StandardResponseModel<ModuleResponseModel>.DeserializeResponse(response.Content)).Data;
            module.Should().NotBeNull();

            //act
            response = await client.PostAsync($"permissions/addPermission/{module.Id}", null);
            var permissions = (await StandardResponseModel<List<PermissionResponse>>.DeserializeResponse(response.Content)).Data;

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            permissions.Should().NotBeNullOrEmpty();

            //dispose
            await client.DeleteAsync($"permissions/deletePermission/{module.Id}");
            await client.DeleteAsync($"v1/modules/{module.Id}");
        }

        /// <summary>
        /// add permissions for module that doesn't exist, should return not found
        /// </summary>
        [Fact]
        public async void AddPermissions_ModuleNotExist_ShouldReturnNotFound()
        {
            //act
            var response = await client.PostAsync($"permissions/addPermissions/{invalidModuleId}", null);
            var permissions = (await StandardResponseModel<List<PermissionResponse>>.DeserializeResponse(response.Content)).Data;

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// valid case for update permission, should return Ok
        /// </summary>
        [Fact]
        public async void Update_Permissions_ShouldReturnOk()
        {
            //arrange
            var permissionRequest = new PermissionRequestModel
            {
                IsAdd = true,
                IsEdit = true,
                IsView = true,
                IsDelete = true
            };

            //act
            var response = await client.PutAsJsonAsync($"permissions/updatePermission/{validPermissionId}", permissionRequest);
            var permission = (await StandardResponseModel<PermissionResponse>.DeserializeResponse(response.Content)).Data;

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            permission.Should().NotBeNull();
            permission.id.Should().Be(validPermissionId);
            permission.isAdd.Should().BeTrue();
            permission.isEdit.Should().BeTrue();
            permission.isView.Should().BeTrue();
            permission.isDelete.Should().BeTrue();
        }

        /// <summary>
        /// update permissions that doesn't exist, should return not found
        /// </summary>
        [Fact]
        public async void UpdatePermissions_UpdatePermissionNotExist_ShouldReturnNotFound()
        {
            //arrange
            var permissionRequest = new PermissionRequestModel
            {
                IsAdd = true,
                IsEdit = true,
                IsView = true,
                IsDelete = true
            };

            //act
            var response = await client.PutAsJsonAsync($"permissions/{invalidPermissionId}", permissionRequest);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// valid case for delete permission, should return no content
        /// </summary>
        [Fact]
        public async void Delete_Permission_ShouldReturnNoContent()
        {
            //arrange
            var response = await client.PostAsJsonAsync($"modules", new ModuleRequestModel { ModuleName = "DeletePermissionModule", isActive = true });
            var module = (await StandardResponseModel<ModuleResponseModel>.DeserializeResponse(response.Content)).Data;
            var permissions = await client.PostAsync($"permissions/module/{module?.Id}", null);

            //act
            if (permissions.IsSuccessStatusCode) { response = await client.DeleteAsync($"permissions/module/{module?.Id}"); }
            else { 
                await client.DeleteAsync($"modules/{module?.Id}");
                response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            }
            //assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            //dispose
            await client.DeleteAsync($"modules/{module?.Id}");
        }

        /// <summary>
        /// delete permission for module that doesnt exist, should return not found
        /// </summary>
        [Fact]
        public async void DeletePermission_GivenModuleNotExist_ShouldReturnNotFound()
        {
            //act
            var response = await client.DeleteAsync($"permissions/{invalidModuleId}");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// valid case for bulk update permission by role id, should return Ok
        /// </summary>
        [Fact]
        public async void Permission_BulkUpdateByRoleId_ShouldReturnOk()
        {
            //arrange
            var response = await client.PostAsJsonAsync("roles", new RoleRequestModel { Name = "bulkUpdateTest" });
            var roleId = ((await StandardResponseModel<RoleResponse>.DeserializeResponse(response.Content)).Data)?.id;
            var permissionRequest = new PermissionRequestModel
            {
                IsAdd = true,
                IsEdit = true,
                IsView = true,
                IsDelete = true
            };
            //act
            response = await client.PatchAsJsonAsync($"permissions/role/{roleId}", permissionRequest);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            //dispose
            await client.DeleteAsync($"roles/{roleId}");
        }

        /// <summary>
        /// bulk update for role id that doesn't exist, should return not found 
        /// </summary>
        [Fact]
        public async void BulkUpdateByRoleId_GivenRoleNotExist_ShouldReturnNotFound()
        {
            //arrange
            var permissionRequest = new PermissionRequestModel
            {
                IsAdd = true,
                IsEdit = true,
                IsView = true,
                IsDelete = true
            };
            //act
            var response = await client.PatchAsJsonAsync($"permissions/role/98a5517a-b2f4-42da-8f26-62d9f2ffefab", permissionRequest);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// valid case for exporting permission to XL, should return Ok
        /// </summary>
        [Fact]
        public async void Export_PermissionToXL_ShouldReturnFile()
        {
            //act
            var response = await client.GetAsync($"permissions/exportRolePermissions/{adminId}");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        /// <summary>
        /// export permission for role with no permissions, should return NoContent
        /// </summary>
        [Fact]
        public async void ExportPermissionToXL_PermissionsNotExist_ShouldReturnNoContent()
        {
            //act
            var response = await client.GetAsync($"permissions/exportRolePermissions/98a5517a-b2f4-42da-8f26-62d9f2ffefab");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// valid case for get dynamic permission data, should return Ok
        /// </summary>
        [Fact]
        public async void Get_DynamicPermissionData_ShouldReturnOk()
        {
            //arrange
            int pageSize = 5;

            //act
            var response = await client.GetAsync($"Permission/paged?page=1&pageSize={pageSize}&searchText=a");
            var data = (await StandardResponseModel<PaginatedResponseModel<PermissionResponse>>.DeserializeResponse(response.Content)).Data;

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            data.Should().NotBeNull();
            data.Page.Should().BeGreaterThan(0);
            data.PageSize.Should().Be(pageSize);
            data.Result.Count.Should().BeLessThanOrEqualTo(pageSize);
            data.FilterCount.Should().Be(data.Result.Count);
        }
    }
}

using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using StockTradingApplication.Models.RequestModels;

namespace IntergrationTests.IntegrationTests
{
    public class RoleIntegrationTests
    {
        private readonly HttpClient client = new HttpClient { BaseAddress = new Uri("https://localhost:40002") };
        private readonly string nonExistingId = "f9f1a23c-144a-4520-a58c-f390b2a09a51";
        private readonly string validUserId = "0aa6c5b4-4c1c-4b5b-bb7a-feea8d0f504e";
        private record RoleResponse(string id,string name);

        /// <summary>
        /// get all roles, should return Ok
        /// </summary>
        [Fact]
        public async void Get_AllRoles_ShouldReturnOk()
        {
            //act
            var response = await client.GetAsync("role/getAllRoles");
            var roleList = (await StandardResponseModel<List<RoleResponse>>.DeserializeResponse(response.Content)).Data;

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            roleList.Should().NotBeNullOrEmpty();
        }

        /// <summary>
        /// valid case for get role, should return Ok.
        /// </summary>
        [Fact]
        public async void Get_Role_ShouldReturnOk()
        {
            //arrange
            var roleRequest = new RoleRequestModel { Name = "getRoleTestRole" };
            var response = await client.PostAsJsonAsync("role/createRole", roleRequest);
            var role = (await StandardResponseModel<RoleResponse>.DeserializeResponse(response.Content)).Data;

            //act
            response = await client.GetAsync($"role/getRole/{role?.id}");
            role = (await StandardResponseModel<RoleResponse>.DeserializeResponse(response.Content)).Data;

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            role.Should().NotBeNull();
            role.id.Should().NotBeNullOrEmpty();
            role.name.Should().Be(roleRequest.Name);

            //dispose
            await client.DeleteAsync($"role/deleteRole/{role.id}");
        }

        /// <summary>
        /// fetch role that doesnt exist, should return not found
        /// </summary>
        [Fact]
        public async void GetRole_FetchRoleNotExist_ShouldReturnNotFound()
        {
            //act
            var response = await client.GetAsync($"role/getRole/{nonExistingId}");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// valid case for create role, should return created
        /// </summary>
        [Fact]
        public async void Create_Role_ShouldReturnCreated()
        {
            //arrange
            var roleRequest = new RoleRequestModel { Name = "testRoleAdd" };
            
            //act
            var response = await client.PostAsJsonAsync("role/createRole", roleRequest);
            var role = (await StandardResponseModel<RoleResponse>.DeserializeResponse(response.Content)).Data;

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            role.Should().NotBeNull();
            role.id.Should().NotBeNull();
            role.name.Should().Be(roleRequest.Name);

            //dispose
            await client.DeleteAsync($"role/deleteRole/{role.id}");
        }

        /// <summary>
        /// create role with rolename that already exist, should return conflict 
        /// </summary>
        [Fact]
        public async void CreateRole_CreatedRoleAlreadyExist_ShouldReturnConflict()
        {
            //arrange
            var roleRequest = new RoleRequestModel { Name = "testSameRoleAdd" };
            var response = await client.PostAsJsonAsync("role/createRole", roleRequest);
            var role = (await StandardResponseModel<RoleResponse>.DeserializeResponse(response.Content)).Data;

            //act
            response = await client.PostAsJsonAsync("role/createRole", roleRequest);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);

            //dispose
            await client.DeleteAsync($"role/deleteRole/{role?.id}");
        }

        /// <summary>
        /// valid case for update role, should return Ok
        /// </summary>
        [Fact]
        public async void Update_Role_ShouldReturnOk()
        {
            //arrange
            var roleRequest = new RoleRequestModel { Name = "testRoleUpdate" };
            var response = await client.PostAsJsonAsync("role/createRole", roleRequest);
            var role = (await StandardResponseModel<RoleResponse>.DeserializeResponse(response.Content)).Data;
            roleRequest.Name = "updatedTestRole";

            //act
            response = await client.PutAsJsonAsync($"role/updateRole/{role?.id}", roleRequest);
            role = (await StandardResponseModel<RoleResponse>.DeserializeResponse(response.Content)).Data;

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            role.Should().NotBeNull();
            role.name.Should().Be(roleRequest.Name);

            //dispose
            await client.DeleteAsync($"role/deleteRole/{role?.id}");
        }

        /// <summary>
        /// update role with rolename that already exist, should return conflict
        /// </summary>
        [Fact]
        public async void UpdateRole_UpdateRoleAlreadyExist_ShouldReturnConflict()
        {
            //arrange
            var roleRequest = new RoleRequestModel { Name = "testSameRoleUpdate" };
            var response = await client.PostAsJsonAsync("role/createRole", roleRequest);
            var role = (await StandardResponseModel<RoleResponse>.DeserializeResponse(response.Content)).Data;
            roleRequest.Name = "superadmin";

            //act
            response = await client.PutAsJsonAsync($"role/updateRole/{role?.id}", roleRequest);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);

            //dispose
            await client.DeleteAsync($"role/deleteRole/{role?.id}");
        }

        /// <summary>
        /// update role that doesnt exist, should return not found
        /// </summary>
        [Fact]
        public async void UpdateRole_UpdateRoleNotExist_ShouldReturnNotFound()
        {
            //arrange
            var roleRequest = new RoleRequestModel { Name = "notFoundForUpdate" };

            //act
            var response = await client.PutAsJsonAsync($"role/updateRole/{nonExistingId}", roleRequest);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// valid case for delete role, should return NoContent
        /// </summary>
        [Fact]
        public async void Delete_Role_ShouldReturnNoContent()
        {
            //arrange
            var roleRequest = new RoleRequestModel { Name = "testRoleDelete" };
            var response = await client.PostAsJsonAsync("roles", roleRequest);
            var role = (await StandardResponseModel<RoleResponse>.DeserializeResponse(response.Content)).Data;

            //act
            response = await client.DeleteAsync($"roles/{role?.id}");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// delete role that doesnt exist, should return not found
        /// </summary>
        [Fact]
        public async void DeleteRole_DeleteRoleNotExist_ShouldReturnNotFound()
        {
            //act
            var response = await client.DeleteAsync($"roles/{nonExistingId}");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// valid case for assign role to user, should return Ok
        /// </summary>
        [Fact]
        public async void Assign_RoleToUser_ShouldReturnOk()
        {
            //arrange
            var roleRequest = new RoleRequestModel { Name = "testRoleForAssign" };
            var response = await client.PostAsJsonAsync("roles", roleRequest);
            var role = (await StandardResponseModel<RoleResponse>.DeserializeResponse(response.Content)).Data;

            //act
            response = await client.PostAsync($"roles/assign?userId={validUserId}&roleName={roleRequest.Name}", null);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            //dispose
            await client.DeleteAsync($"roles/{role?.id}");
        }

        /// <summary>
        /// Assign role to user that doesnt exist, should return not found.
        /// </summary>
        [Fact]
        public async void AssignRoleToUser_AssignUserNotExist_ShouldReturnNotFound()
        {
            //arrange
            var roleRequest = new RoleRequestModel { Name = "userNotFoundForAssign" };
            var response = await client.PostAsJsonAsync("roles", roleRequest);
            var role = (await StandardResponseModel<RoleResponse>.DeserializeResponse(response.Content)).Data;

            //act
            response = await client.PostAsync($"roles/assign?userId={nonExistingId}&roleName={roleRequest.Name}", null);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);

            //dispose
            await client.DeleteAsync($"roles/{role?.id}");
        }

        /// <summary>
        /// assign role that doesnt exist to user, should return not found 
        /// </summary>
        [Fact]
        public async void AssignRoleToUser_AssignRoleNotExist_ShouldReturnNotFound()
        {
            //act
            var response = await client.PostAsync($"roles/assign?userId={validUserId}&roleName=RandomRoleName", null);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// valid case for get role by userId, should return Ok
        /// </summary>
        [Fact]
        public async void Get_RoleByUserId_ShouldReturnOk()
        {
            //act
            var response = await client.GetAsync($"roles/user/{validUserId}");
            var roles = (await StandardResponseModel<List<string>>.DeserializeResponse(response.Content)).Data;

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            roles.Should().NotBeNull();
        }

        /// <summary>
        /// fetch role for userId that doesnt exist, should return Ok
        /// </summary>
        [Fact]
        public async void GetRoleByUserId_FetchUserNotExist_ShouldReturnOk()
        {
            //act
            var response = await client.GetAsync($"roles/user/{nonExistingId}");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}

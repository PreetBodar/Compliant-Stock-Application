using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using StockTradingApplication.Models.RequestModels;
using StockTradingApplication.Models.ResponseModels;

namespace IntergrationTests.IntegrationTests
{
    public class ModuleIntegrationTests
    {
        private readonly HttpClient client = new HttpClient { BaseAddress = new Uri("https://localhost:40002") };

        /// <summary>
        /// get all modules , should return Ok
        /// </summary>
        [Fact]
        public async void Get_AllModules_ShouldReturnOk()
        {
            //act
            var response = await client.GetAsync("modules");
            var moduleList = (await StandardResponseModel<List<ModuleResponseModel>>.DeserializeResponse(response.Content)).Data;

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            moduleList.Should().NotBeNull();
            moduleList.Count.Should().BeGreaterThan(0);
        }

        /// <summary>
        /// Fetch module that doesn't exist, should return Not Found
        /// </summary>
        [Fact]
        public async void GetModule_FetchModuleNotExist_ShouldReturnNotFound()
        {
            //act
            var response = await client.GetAsync("modules/999999");
            //assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// valid case for getModule, should return Ok
        /// </summary>
        [Fact]
        public async void Get_Module_ShouldReturnOk()
        {
            //arrange
            ModuleRequestModel moduleRequest = new ModuleRequestModel { ModuleName = "TestModule53951434", isActive = true};
            var response = await client.PostAsJsonAsync("modules", moduleRequest);
            var createdModule = (await StandardResponseModel<ModuleResponseModel>.DeserializeResponse(response.Content)).Data;
            
            //act
            response = await client.GetAsync($"modules/{createdModule?.Id}");
            var module = (await StandardResponseModel<ModuleResponseModel>.DeserializeResponse(response.Content)).Data;

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            module.Should().NotBeNull();
            module.Id.Should().Be(createdModule?.Id);

            //dispose
            await client.DeleteAsync($"modules/{createdModule?.Id}");
        }
        
        /// <summary>
        /// valid case for addModule , should return Created
        /// </summary>
        [Fact]
        public async void Add_Module_ShouldReturnCreated()
        {
            //arrange
            ModuleRequestModel moduleRequest = new ModuleRequestModel { ModuleName = "TestModule53951439", isActive = true };

            //act
            var response = await client.PostAsJsonAsync("modules", moduleRequest);
            var createdModule = (await StandardResponseModel<ModuleResponseModel>.DeserializeResponse(response.Content)).Data;

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            createdModule.Should().NotBeNull();
            createdModule.ModuleName.Should().Be(moduleRequest.ModuleName);
            createdModule.IsActive.Should().Be(moduleRequest.isActive);

            //dispose
            await client.DeleteAsync($"modules/{createdModule.Id}");
        }

        /// <summary>
        /// Add module with same name as existing module, should return conflict
        /// </summary>
        [Fact]
        public async void AddModule_AddAlreadyExistingModule_ShouldReturnConflict()
        {
            //arrange
            ModuleRequestModel moduleRequest = new ModuleRequestModel { ModuleName = "TestModule53951431", isActive = true };
            var response = await client.PostAsJsonAsync("modules", moduleRequest);
            var createdModule = (await StandardResponseModel<ModuleResponseModel>.DeserializeResponse(response.Content)).Data;

            //act
            response = await client.PostAsJsonAsync($"modules", moduleRequest);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
            createdModule.Should().NotBeNull();
            

            //dispose
            await client.DeleteAsync($"modules/{createdModule.Id}");
        }

        /// <summary>
        /// valid case for update module,should return ok
        /// </summary>
        [Fact]
        public async void Update_Module_ShouldReturnOk()
        {
            //arrange
            ModuleRequestModel moduleRequest = new ModuleRequestModel { ModuleName = "TestModule53951420", isActive = true };
            var response = await client.PostAsJsonAsync("modules", moduleRequest);
            var createdModule = (await StandardResponseModel<ModuleResponseModel>.DeserializeResponse(response.Content)).Data;
            moduleRequest = new ModuleRequestModel { ModuleName = "TestModule53951888", isActive = false };

            //act
            response = await client.PutAsJsonAsync($"modules/{createdModule?.Id}", moduleRequest);
            var updatedModule = (await StandardResponseModel<ModuleResponseModel>.DeserializeResponse(response.Content)).Data;

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            updatedModule.Should().NotBeNull();
            updatedModule.Id.Should().Be(createdModule?.Id);
            updatedModule.ModuleName.Should().Be(moduleRequest.ModuleName);
            updatedModule.IsActive.Should().Be(moduleRequest.isActive);

            //dispose
            await client.DeleteAsync($"modules/{updatedModule?.Id}");
        }

        /// <summary>
        /// updating module that doesn't exist, should return not found
        /// </summary>
        [Fact]
        public async void UpdateModule_UpdateModuleNotExist_ShouldReturnNotFound()
        {
            //arrange
            ModuleRequestModel moduleRequest = new ModuleRequestModel { ModuleName = "TestModule53951455", isActive = true };

            //act
            var response = await client.PutAsJsonAsync($"modules/999999", moduleRequest);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// updating module with same name as existing module, should return conflict
        /// </summary>
        [Fact]
        public async void UpdateModule_UpdateAsAlreadyExistingModule_ShouldReturnConflict()
        {
            //arrange
            ModuleRequestModel moduleRequest = new ModuleRequestModel { ModuleName = "Test5593062595", isActive = true };
            var createResponse = await client.PostAsJsonAsync($"modules", moduleRequest);
            var createdModule = (await StandardResponseModel<ModuleResponseModel>.DeserializeResponse(createResponse.Content)).Data;
            var sameModuleRequest = new ModuleRequestModel { ModuleName = "Test36696908598", isActive = true };
            var updateResponse = await client.PostAsJsonAsync($"modules", sameModuleRequest);
            var updateModule = (await StandardResponseModel<ModuleResponseModel>.DeserializeResponse(updateResponse.Content)).Data;

            //act
            var response = await client.PutAsJsonAsync($"modules/{createdModule?.Id}", sameModuleRequest);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
            createdModule.Should().NotBeNull();
            updateModule.Should().NotBeNull();

            //dispose
            await client.DeleteAsync($"modules/{createdModule.Id}");
            await client.DeleteAsync($"modules/{updateModule.Id}");
        }

        /// <summary>
        /// valid case for delete module, should return NoContent
        /// </summary>
        [Fact]
        public async void Delete_Module_ShouldReturnNoContent()
        {
            //arrange
            ModuleRequestModel moduleRequest = new ModuleRequestModel { ModuleName = "TestModule43951245", isActive = true };
            var response = await client.PostAsJsonAsync("modules", moduleRequest);
            var module = (await StandardResponseModel<ModuleResponseModel>.DeserializeResponse(response.Content)).Data;

            //act
            response = await client.DeleteAsync($"modules/{module?.Id}");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// delete module that doesn't exist, should return not found
        /// </summary>
        [Fact]
        public async void DeleteModule_DeleteModuleNotExist_ShouldReturnNotFound()
        {
            //act
            var response = await client.DeleteAsync("modules/999999");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

    }
}

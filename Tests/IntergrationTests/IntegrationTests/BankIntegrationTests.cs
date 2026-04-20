using FluentAssertions;
using StockTradingApplication.Models;
using StockTradingApplication.Models.RequestModels;
using StockTradingApplication.Models.ResponseModels;
using System.Net;
using System.Net.Http.Json;

namespace IntergrationTests.IntegrationTests
{
    public class BankIntegrationTests
    {
        private readonly HttpClient client = new HttpClient { BaseAddress = new Uri("https://localhost:40002") };
        private readonly Guid userIdForTest = Guid.Parse("C84A9AD9-5A09-4589-9132-76630CA594AF");
        private readonly Guid nonExistingId = Guid.Parse("f9f1a23c-144a-4520-a58c-f395b2a09a51");

        /// <summary>
        /// valid case for Get all banks, should return Ok
        /// </summary>
        [Fact]
        public async void Get_AllBank_ShouldReturnOk()
        {
            //act
            var response = await client.GetAsync("banks");
            var result = await StandardResponseModel<List<TblBank>>.DeserializeResponse(response.Content);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Data.Should().NotBeNull();
            result.Data.Count.Should().BeGreaterThan(0);
        }

        /// <summary>
        /// valid case for get bank by bankid, should return Ok
        /// </summary>
        [Fact]
        public async void Get_Bank_ShouldReturnOk()
        {
            //arrange
            BankRequestModel bankRequest = new BankRequestModel { UserId = userIdForTest, BankName = "Test5586864354", IsActive = true };
            var response = await client.PostAsJsonAsync("banks", bankRequest);
            var createdBank = (await StandardResponseModel<TblBank>.DeserializeResponse(response.Content)).Data;
            //act
            response = await client.GetAsync($"banks/{createdBank?.Id}");
            var bank = (await StandardResponseModel<TblBank>.DeserializeResponse(response.Content)).Data;

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            createdBank.Should().NotBeNull();
            bank.Should().NotBeNull();
            bank.BankName.Should().Be(createdBank.BankName);
            bank.IsActive.Should().Be(createdBank.IsActive);
            bank.Id.Should().NotBeEmpty();

            //dispose
            await client.DeleteAsync($"banks/{bank.Id}");
        }

        /// <summary>
        /// get bank with bankid that doesn't exist, should return not found
        /// </summary>
        [Fact]
        public async void GetBank_FetchBankNotExist_ShouldReturnFound()
        {
            //act
            var response = await client.GetAsync($"banks/{nonExistingId}");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// valid case for Adding bank with AddOrEditBank, should return created
        /// </summary>
        [Fact]
        public async void AddOrEditBank_AddBank_ShouldReturnCreated()
        {
            //arrange
            BankRequestModel bankRequest = new BankRequestModel { UserId = userIdForTest, BankName = "Test5582264354", IsActive = true };
            
            //act
            var response = await client.PostAsJsonAsync("banks", bankRequest);
            var createdBank = (await StandardResponseModel<TblBank>.DeserializeResponse(response.Content)).Data;

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            createdBank.Should().NotBeNull();
            createdBank.Id.Should().NotBeEmpty();
            createdBank.BankName.Should().Be(createdBank.BankName);

            //dispose
            await client.DeleteAsync($"banks/{createdBank.Id}");
        }

        /// <summary>
        /// adding bank with bankname that already exist, should return conflict
        /// </summary>
        [Fact]
        public async void AddOrEditBank_AddedBanknameAlreadyExist_ShouldReturnConflict()
        {
            //arrange
            BankRequestModel bankRequest = new BankRequestModel { UserId = userIdForTest, BankName = "Test55898254350", IsActive = true };
            var response = await client.PostAsJsonAsync("banks", bankRequest);
            var bank = (await StandardResponseModel<TblBank>.DeserializeResponse(response.Content)).Data;

            //act
            response = await client.PostAsJsonAsync("banks", bankRequest);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
            bank.Should().NotBeNull();

            //dispose
            await client.DeleteAsync($"banks/{bank.Id}");
        }

        /// <summary>
        /// valid case for update bank with AddOrEditBank, should return Ok
        /// </summary>
        [Fact]
        public async void AddOrEditBank_UpdateBank_ShouldReturnOk()
        {
            //arrange
            BankRequestModel bankRequest = new BankRequestModel { UserId = userIdForTest, BankName = "Test5586326354", IsActive = true };
            var response = await client.PostAsJsonAsync("banks", bankRequest);
            var bank = (await StandardResponseModel<TblBank>.DeserializeResponse(response.Content)).Data;
            bank.Should().NotBeNull();
            BankRequestModel updateRequest = new BankRequestModel { UserId = userIdForTest, Id = bank.Id, BankName = "Test553925304", IsActive = false };

            //act
            response = await client.PostAsJsonAsync("banks", updateRequest);
            var updatedBank = (await StandardResponseModel<TblBank>.DeserializeResponse(response.Content)).Data;

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            bank.Should().NotBeNull();
            updatedBank.Should().NotBeNull();
            updatedBank.Id.Should().Be(bank.Id);
            updatedBank.BankName.Should().Be(updateRequest.BankName);
            updatedBank.IsActive.Should().Be(updateRequest.IsActive ?? default);
            bank.Should().NotBeNull();

            //dispose
            await client.DeleteAsync($"banks/{bank.Id}");
        }

        /// <summary>
        /// update bank with bankname that already exists, should return conflict
        /// </summary>
        [Fact]
        public async void AddOrEditBank_UpdateBankNotExist_ShouldReturnNotFound()
        {
            //arrange
            BankRequestModel updateRequest = new BankRequestModel { UserId = userIdForTest, Id = nonExistingId, BankName = "Test553925304", IsActive = false };

            //act
            var response = await client.PostAsJsonAsync("banks", updateRequest);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// AddOrEditBank with userId that doesn't exist, should return not found
        /// </summary>
        [Fact]
        public async void AddOrEditBank_UserNotExist_ShouldReturnNotFound()
        {
            //arrange
            BankRequestModel bankRequest = new BankRequestModel { UserId = nonExistingId, BankName = "Test553925304", IsActive = false };

            //act
            var response = await client.PostAsJsonAsync("banks", bankRequest);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// valid case for Delete bank, should return NoContent
        /// </summary>
        [Fact]
        public async void Delete_Bank_ShouldReturnNoContent()
        {
            //arrange
            BankRequestModel bankRequest = new BankRequestModel { UserId = userIdForTest, BankName = "Test558335260", IsActive = true };
            var response = await client.PostAsJsonAsync("banks", bankRequest);
            var bank = (await StandardResponseModel<TblBank>.DeserializeResponse(response.Content)).Data;
            bank.Should().NotBeNull();

            //act
            response = await client.DeleteAsync($"banks/{bank.Id}");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Delete bank with bankId that doesn't exist, should return not found
        /// </summary>
        [Fact]
        public async void DeleteBank_DeleteBankNotExist_ShouldReturnNotFound()
        {
            //act
            var response = await client.DeleteAsync($"banks/{nonExistingId}");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// valid case for GetDynamicBankData, should return Ok
        /// </summary>
        [Fact]
        public async void Get_DynamicBankData_ShouldReturnOk()
        {
            //arrange
            int pageSize = 5;

            //act
            var response = await client.GetAsync($"banks/paged?" +
                $"page=1&" +
                $"pageSize={pageSize}&" +
                $"orderByColumn=bankname&" +
                $"searchText=a");
            var data = (await StandardResponseModel<PaginatedResponseModel<TblBank>>.DeserializeResponse(response.Content)).Data;

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

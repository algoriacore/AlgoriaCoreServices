using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.QueriesAndCommands.Files._1Model;
using AlgoriaCore.Application.QueriesAndCommands.Users._1Model;
using AlgoriaCore.Application.QueriesAndCommands.Users._2Queries;
using AlgoriaCore.Application.QueriesAndCommands.Users._3Commands;
using AlgoriaCore.Application.Tests.ASanitization;
using AlgoriaCore.Application.Tests.Infrastructure;
using AlgoriaCore.Domain.Entities;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AlgoriaCore.Application.Tests.Multitenancy.Users
{
    [Collection("TestsCollection")]
    public class UsersTests : TestBaseTenantDefault
    {
        public UsersTests(QueryTestFixture fixture) : base(fixture) { }

        #region SUCCESSFUL TESTS

        [Fact]
        public async Task CreateUserTest()
        {
            long response = await Mediator.Send(new UserCreateCommand()
            {
                Name = "Prueba usuario",
                LastName = "Apellido P prueb",
                SecondLastName = "Apellido M prueba",
                EmailAddress = "correonew@dominio.com",
                PhoneNumber = "3515345",
                UserName ="usertest01",
                Password = "contrasenia",
                ShouldChangePasswordOnNextLogin = true,
                IsActive = true
            });

            response.ShouldBeGreaterThan(0);

            User User = Context.User.FirstOrDefault(p => p.TenantId == SessionContext.TenantId && p.Id == response);

            User.ShouldNotBeNull();
        }

        [Fact]
        public async Task UpdateUserTest()
        {
            User entity = Context.User.FirstOrDefault(p => p.TenantId == SessionContext.TenantId && p.UserLogin.ToUpper() == "UPD");
            string name = entity.Name + " MOD";

            long response = await Mediator.Send(new UserUpdateCommand()
            {
                Id = entity.Id,
                Name = name,
                LastName = "Apellido P prueb",
                SecondLastName = "Apellido M prueba",
                EmailAddress = "correoupdnn@dominio.com",
                PhoneNumber = "3515345",
                UserName = "UPD",
                //Password = "contrasenia",
                ShouldChangePasswordOnNextLogin = true,
                IsActive = true
            });

            response.ShouldBeGreaterThan(0);

            entity = Context.User.FirstOrDefault(p => p.Id == entity.Id);

            entity.Name.ShouldBe(name);
        }

        [Fact]
        public async Task UpdateUserProfileTest()
        {
            User entity = Context.User.FirstOrDefault(p => p.TenantId == SessionContext.TenantId && p.Id == SessionContext.UserId);
            string name = entity.Name + " MOD PROFILE";
            Dictionary<string, string> preferences = new Dictionary<string, string>();
            preferences.Add("appTheme", "purple-blue");

            long response = await Mediator.Send(new UserUpdateProfileCommand()
            {
                Name = name,
                LastName = entity.Lastname,
                SecondLastName = entity.SecondLastname,
                EmailAddress = entity.EmailAddress,
                PhoneNumber = entity.PhoneNumber,
                Preferences = preferences
            });

            response.ShouldBeGreaterThan(0);

            entity = Context.User.FirstOrDefault(p => p.Id == entity.Id);

            entity.Name.ShouldBe(name);
        }

        [Fact]
        public async Task ResetUserPasswordTest()
        {
            User entity = Context.User.FirstOrDefault(p => p.TenantId == SessionContext.TenantId && p.UserLogin.ToUpper() == "UPD");
            Tenant tenant = Context.Tenant.FirstOrDefault(p => p.Id == entity.TenantId);

            string response = await Mediator.Send(new UserResetPasswordCommand()
            {
                TenancyName = tenant.TenancyName,
                UserName = entity.UserLogin
            });

            response.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task ConfirmPasswordTest()
        {
            User entity = Context.User.FirstOrDefault(p => p.TenantId == SessionContext.TenantId && p.UserLogin.ToUpper() == "UPD");
            Tenant tenant = Context.Tenant.FirstOrDefault(p => p.Id == entity.TenantId);

            string response = await Mediator.Send(new UserResetPasswordCommand()
            {
                TenancyName = tenant.TenancyName,
                UserName = entity.UserLogin
            });

            response.ShouldNotBeEmpty();

            long responseConfirm = await Mediator.Send(new ConfirmPasswordCommandReset()
            {
                ConfirmationCode = response,
                Password = "123456a",
                ConfirmPassword = "123456a"
            });

            responseConfirm.ShouldBeGreaterThan(0);
            responseConfirm.ShouldBe(entity.Id);
        }

        [Fact]
        public async Task GetUserListTest()
        {
            PagedResultDto<UserListResponse> response = await Mediator.Send(new UserGetListQuery());

            response.ShouldNotBeNull();
            response.Items.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task GetUserProfilePictureTest()
        {
            User entity = Context.User.FirstOrDefault(p => p.TenantId == SessionContext.TenantId && p.UserLogin.ToUpper() == "GET");

            entity.ShouldNotBeNull();

            GetFileResponse response = await Mediator.Send(new UserProfilePictureQuery() { Id = entity.Id });

            response.ShouldNotBeNull();
            response.FileArray.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task GetUserProfileTest()
        {
            User entity = Context.User.FirstOrDefault(p => p.TenantId == SessionContext.TenantId && p.UserLogin.ToUpper() == "GET");

            entity.ShouldNotBeNull();

            UserForEditResponse response = await Mediator.Send(new UserProfileQuery() { Id = entity.Id, ClientType = "web" });

            response.ShouldNotBeNull();
        }

        #endregion

        #region FAILED TESTS

        [Fact]
        public async Task CreateUserSanitize()
        {
            var ac = new ASanitizeTest<UserCreateCommand, long>(new UserCreateCommand()
            {
                Name = "Prueba usuario",
                LastName = "Apellido P prueb",
                SecondLastName = "Apellido M prueba",
                EmailAddress = "correocreate@dominio.com",
                PhoneNumber = "3515345",
                UserName = "usertest01",
                Password = "contrasenia",
                ShouldChangePasswordOnNextLogin = true,
                IsActive = true
            });

            ac.Mediator = Mediator;

            ac.RuleFor(x => x.Name).NotEmpty();
            ac.RuleFor(x => x.Name).MaxLength(50);
            ac.RuleFor(x => x.LastName).NotEmpty();
            ac.RuleFor(x => x.LastName).MaxLength(50);
            ac.RuleFor(x => x.SecondLastName).MaxLength(50);
            ac.RuleFor(x => x.EmailAddress).NotEmpty();
            ac.RuleFor(x => x.EmailAddress).MaxLength(250);
            ac.RuleFor(x => x.EmailAddress).EmailAddress();
            ac.RuleFor(x => x.PhoneNumber).MaxLength(20);
            ac.RuleFor(x => x.UserName).NotEmpty();
            ac.RuleFor(x => x.UserName).MaxLength(32);

            await ac.RunTest();

            if (ac.TotalErrors > 0)
            {
                throw new System.Exception(string.Join("\n\n", ac.ErrorMessages));
            }

			Assert.Equal(0, ac.TotalErrors);
		}

        [Fact]
        public async Task UpdateUserSanitize()
        {
            var ac = new ASanitizeTest<UserUpdateCommand, long>(new UserUpdateCommand()
            {
                Id = 2,
                Name = "Prueba usuario",
                LastName = "Apellido P prueb",
                SecondLastName = "Apellido M prueba",
                EmailAddress = "correoupdate@dominio.com",
                PhoneNumber = "3515345",
                UserName = "usertest01",
                Password = "contrasenia",
                PasswordRepeat = "contrasenia",
                ShouldChangePasswordOnNextLogin = true,
                IsActive = true
            });
            ac.Mediator = Mediator;

            ac.RuleFor(x => x.Id).NotEmpty();
            ac.RuleFor(x => x.Name).NotEmpty();
            ac.RuleFor(x => x.Name).MaxLength(50);
            ac.RuleFor(x => x.LastName).NotEmpty();
            ac.RuleFor(x => x.LastName).MaxLength(50);
            ac.RuleFor(x => x.SecondLastName).MaxLength(50);
            ac.RuleFor(x => x.EmailAddress).NotEmpty();
            ac.RuleFor(x => x.EmailAddress).MaxLength(250);
            ac.RuleFor(x => x.EmailAddress).EmailAddress();
            ac.RuleFor(x => x.PhoneNumber).MaxLength(20);
            ac.RuleFor(x => x.UserName).NotEmpty();
            ac.RuleFor(x => x.UserName).MaxLength(32);
            ac.RuleFor(x => x.PasswordRepeat).Equal(x => x.Password);

            await ac.RunTest();

            if (ac.TotalErrors > 0)
            {
                throw new System.Exception(string.Join("\n\n", ac.ErrorMessages));
            }

			Assert.Equal(0, ac.TotalErrors);
		}

        #endregion
    }
}
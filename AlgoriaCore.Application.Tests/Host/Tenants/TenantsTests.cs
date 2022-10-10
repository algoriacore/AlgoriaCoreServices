using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.Exceptions;
using AlgoriaCore.Application.QueriesAndCommands.Registration._3Commands;
using AlgoriaCore.Application.QueriesAndCommands.Tenants;
using AlgoriaCore.Application.QueriesAndCommands.Tenants._1Model;
using AlgoriaCore.Application.QueriesAndCommands.Users._2Queries;
using AlgoriaCore.Application.Tests.ASanitization;
using AlgoriaCore.Application.Tests.Infrastructure;
using AlgoriaCore.Domain.Entities;
using AlgoriaCore.Domain.Exceptions;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AlgoriaCore.Application.Tests.Host.Tenants
{
    [Collection("TestsCollection")]
    public class TenantsTests : TestBaseHost
    {
        public TenantsTests(QueryTestFixture fixture) : base(fixture) {}

        #region SUCCESSFUL TESTS

        [Fact]
        public async Task CreateTenantTest()
        {
            int response = await Mediator.Send(new TenantCreateCommand()
            {
                TenancyName = "test",
                TenantName = "TEST",
                Password = "123456a",
                PasswordConfirm = "123456a",
                Name = "Admin",
                LastName = "Test",
                SecondLastName = "Test",
                EmailAddress = "ediths.tostado@gmail.com"
            });

            response.ShouldBeGreaterThan(0);

            Tenant entity = Context.Tenant.FirstOrDefault(p => p.Id == response);

            entity.ShouldNotBeNull();
        }

        [Fact]
        public async Task UpdateTenantTest()
        {
            Tenant entity = Context.Tenant.FirstOrDefault(p => p.TenancyName.ToUpper() == "UPD");
            string name = entity.Name + " MOD";

            int response = await Mediator.Send(new UpdateTenantCommand()
            {
                Id = entity.Id,
                TenancyName = entity.TenancyName,
                Name = name
            });

            response.ShouldBeGreaterThan(0);

            entity = Context.Tenant.FirstOrDefault(p => p.Id == entity.Id);

            entity.Name.ShouldBe(name);
        }

        [Fact]
        public async Task GetTenantTest()
        {
            Tenant entity = Context.Tenant.FirstOrDefault();

            entity.ShouldNotBeNull();

            TenantResponse response = await Mediator.Send(new GetTenantQuery() { Id = entity.Id });

            response.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetTenantListTest()
        {
            PagedResultDto<TenantListResponse> response = await Mediator.Send(new TenantGetListQuery());

            response.ShouldNotBeNull();
            response.Items.Count.ShouldBeGreaterThan(0);
        }

        #endregion

        #region FAILED TESTS

        [Fact]
        public async Task CreateTenantSanitizeTest()
        {
            var ac = new ASanitizeTest<TenantCreateCommand, int>(new TenantCreateCommand()
            {
                TenancyName = "test",
                TenantName = "TEST",
                Password = "123456",
                PasswordConfirm = "123456",
                Name = "Admin",
                LastName = "Test",
                SecondLastName = "Test",
                EmailAddress = "ediths.tostado@gmail.com"
            });

            ac.Mediator = Mediator;

            ac.RuleFor(x => x.TenancyName).NotEmpty();
            ac.RuleFor(x => x.TenancyName).MinLength(2);
            ac.RuleFor(x => x.TenancyName).MaxLength(50);
            ac.RuleFor(x => x.TenantName).NotEmpty();
            ac.RuleFor(x => x.TenantName).MaxLength(150);
            ac.RuleFor(x => x.Name).NotEmpty();
            ac.RuleFor(x => x.Name).MaxLength(50);
            ac.RuleFor(x => x.LastName).NotEmpty();
            ac.RuleFor(x => x.LastName).MaxLength(50);
            ac.RuleFor(x => x.SecondLastName).MaxLength(50);
            ac.RuleFor(x => x.EmailAddress).NotEmpty();
            ac.RuleFor(x => x.EmailAddress).MaxLength(250);
            ac.RuleFor(x => x.EmailAddress).EmailAddress();

            await ac.RunTest();

            if (ac.TotalErrors > 0)
            {
                throw new System.Exception(string.Join("\n\n", ac.ErrorMessages));
            }

			Assert.Equal(0, ac.TotalErrors);
		}

        [Fact]
        public async Task UpdateTenantSanitizeTest()
        {
            Tenant entity = Context.Tenant.FirstOrDefault(p => p.TenancyName.ToUpper() == "UPD");

            var ac = new ASanitizeTest<UpdateTenantCommand, int>(new UpdateTenantCommand()
            {
                Id = entity.Id,
                TenancyName = entity.TenancyName,
                Name = entity.Name
            });

            ac.Mediator = Mediator;

            ac.RuleFor(x => x.Id).NotEmpty();
            ac.RuleFor(x => x.Name).NotEmpty();
            ac.RuleFor(x => x.Name).MaxLength(150);
            ac.RuleFor(x => x.TenancyName).NotEmpty();
            ac.RuleFor(x => x.TenancyName).MaxLength(50);

            await ac.RunTest();

            if (ac.TotalErrors > 0)
            {
                throw new System.Exception(string.Join("\n\n", ac.ErrorMessages));
            }

			Assert.Equal(0, ac.TotalErrors);
		}

        [Fact]
        public async Task CreateTenantFailedTenancyNameDuplicatedTest()
        {
            await Assert.ThrowsAsync<TenantRegistrationDuplicatedTenancyNameException>(async () =>
                    await Mediator.Send(new TenantCreateCommand()
                    {
                        TenancyName = "Default",
                        TenantName = "TEST",
                        Password = "123456",
                        PasswordConfirm = "123456",
                        Name = "Admin",
                        LastName = "Test",
                        SecondLastName = "Test",
                        EmailAddress = "ediths.tostado@gmail.com"
                    })
                );
        }

        [Fact]
        public async Task CreateTenantFailedPasswordEmptyTest()
        {
            await Assert.ThrowsAsync<ValidationException>(async () =>
                    await Mediator.Send(new TenantCreateCommand()
                    {
                        TenancyName = "test2",
                        TenantName = "TEST2",
                        Password = "",
                        PasswordConfirm = "123456",
                        Name = "Admin",
                        LastName = "Test2",
                        SecondLastName = "Test2",
                        EmailAddress = "ediths.tostado@gmail.com"
                    })
                );
        }

        [Fact]
        public async Task CreateTenantFailedPasswordMaxLengthTest()
        {
            string password = new String('A', 201);

            await Assert.ThrowsAsync<ValidationException>(async () =>
                    await Mediator.Send(new TenantCreateCommand()
                    {
                        TenancyName = "test2",
                        TenantName = "TEST2",
                        Password = password,
                        PasswordConfirm = password,
                        Name = "Admin",
                        LastName = "Test2",
                        SecondLastName = "Test2",
                        EmailAddress = "ediths.tostado@gmail.com"
                    })
                );
        }

        [Fact]
        public async Task CreateTenantFailedPasswordDontMatchTest()
        {
            await Assert.ThrowsAsync<ValidationException>(async () =>
                    await Mediator.Send(new TenantCreateCommand()
                    {
                        TenancyName = "test2",
                        TenantName = "TEST2",
                        Password = "123456",
                        PasswordConfirm = "12345678",
                        Name = "Admin",
                        LastName = "Test2",
                        SecondLastName = "Test2",
                        EmailAddress = "ediths.tostado@gmail.com"
                    })
                );
        }

        #endregion

    }
}
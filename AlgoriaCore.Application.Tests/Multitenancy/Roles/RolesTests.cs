using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.QueriesAndCommands.Roles._1Model;
using AlgoriaCore.Application.QueriesAndCommands.Roles._2Queries;
using AlgoriaCore.Application.QueriesAndCommands.Roles._3Commands;
using AlgoriaCore.Application.Tests.ASanitization;
using AlgoriaCore.Application.Tests.Infrastructure;
using AlgoriaCore.Domain.Entities;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AlgoriaCore.Application.Tests.Multitenancy.Roles
{
    [Collection("TestsCollection")]
    public class RolesTests : TestBaseTenantDefault
    {
        private Role RoleDefault { get; set; }

        public RolesTests(QueryTestFixture fixture) : base(fixture) {}

        #region SUCCESSFUL TESTS

        [Fact]
        public async Task CreateRoleTest()
        {
            long response = await Mediator.Send(new RolCreateCommand()
            {
                Name = "ROLNEW",
                DisplayName = "ROLNEW",
                IsActive = true
            });

            response.ShouldBeGreaterThan(0);

            Role entity = Context.Role.FirstOrDefault(p => p.TenantId == SessionContext.TenantId && p.Id == response);

            entity.ShouldNotBeNull();
        }

        [Fact]
        public async Task UpdateRoleTest()
        {
            Role entity = Context.Role.FirstOrDefault(p => p.TenantId == SessionContext.TenantId && p.Name.ToUpper() == "UPD");
            string name = entity.Name + " MOD";

            long response = await Mediator.Send(new RolUpdateCommand()
            {
                Id = entity.Id,
                Name = name,
                DisplayName = entity.DisplayName + " MOD",
                IsActive = true
            });

            response.ShouldBeGreaterThan(0);

            entity = Context.Role.FirstOrDefault(p => p.TenantId == SessionContext.TenantId && p.Id == entity.Id);

            entity.Name.ShouldBe(name);
        }

        [Fact]
        public async Task DeleteRoleTest()
        {
            Role entity = Context.Role.FirstOrDefault(p => p.TenantId == SessionContext.TenantId && p.Name.ToUpper() == "DEL");
            long id = entity.Id;

            long response = await Mediator.Send(new RolDeleteCommand()
            {
                Id = id
            });

            response.ShouldBeGreaterThan(0);

            entity = Context.Role.FirstOrDefault(p => p.TenantId == SessionContext.TenantId && p.Id == id && p.IsDeleted != true);

            entity.ShouldBeNull();
        }

        [Fact]
        public async Task GetRoleTest()
        {
            Role entity = Context.Role.FirstOrDefault(p => p.TenantId == SessionContext.TenantId);

            entity.ShouldNotBeNull();

            RolResponse response = await Mediator.Send(new RolGetByIdQuery() { Id = entity.Id });

            response.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetRoleListTest()
        {
            PagedResultDto<RolForListResponse> response = await Mediator.Send(new RolGetListQuery());

            response.ShouldNotBeNull();
            response.Items.Count.ShouldBeGreaterThan(0);
        }

        #endregion

        #region FAILED TESTS

        [Fact]
        public async Task CreateRoleSanitizeTest()
        {
            var ac = new ASanitizeTest<RolCreateCommand, long>(new RolCreateCommand()
            {
                Name = "ROLNEW",
                DisplayName = "ROLNEW",
                IsActive = true
            });

            ac.Mediator = Mediator;

            ac.RuleFor(x => x.Name).NotEmpty();
            ac.RuleFor(x => x.Name).MaxLength(50);
            ac.RuleFor(x => x.DisplayName).NotEmpty();
            ac.RuleFor(x => x.DisplayName).MaxLength(100);

            await ac.RunTest();

            if (ac.TotalErrors > 0)
            {
                throw new System.Exception(string.Join("\n\n", ac.ErrorMessages));
            }

			Assert.Equal(0, ac.TotalErrors);
		}

        [Fact]
        public async Task UpdateRoleSanitizeTest()
        {
            Role entity = Context.Role.FirstOrDefault(p => p.TenantId == SessionContext.TenantId && p.Name.ToUpper() == "UPDSAN");

            var ac = new ASanitizeTest<RolUpdateCommand, long>(new RolUpdateCommand()
            {
                Id = entity.Id,
                Name = entity.Name,
                DisplayName = entity.DisplayName,
                IsActive = entity.IsActive == true
            });

            ac.Mediator = Mediator;

            ac.RuleFor(x => x.Id).NotEmpty();
            ac.RuleFor(x => x.Name).NotEmpty();
            ac.RuleFor(x => x.Name).MaxLength(50);
            ac.RuleFor(x => x.DisplayName).NotEmpty();
            ac.RuleFor(x => x.DisplayName).MaxLength(100);

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
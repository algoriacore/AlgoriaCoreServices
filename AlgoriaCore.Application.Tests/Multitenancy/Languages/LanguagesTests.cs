using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.QueriesAndCommands.Languages.Languages;
using AlgoriaCore.Application.QueriesAndCommands.Languages.Texts;
using AlgoriaCore.Application.Tests.ASanitization;
using AlgoriaCore.Application.Tests.Infrastructure;
using AlgoriaCore.Domain.Entities;
using AlgoriaCore.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AlgoriaCore.Application.Tests.Multitenancy.Languages
{
    [Collection("TestsCollection")]
    public class LanguagesTests : TestBaseTenantDefault
    {
        private Language LanguageDefault { get; set; }

        public LanguagesTests(QueryTestFixture fixture) : base(fixture) { }

        #region LANGUAGES

        #region SUCCESSFUL TESTS

        [Fact]
        public async Task CreateLanguageTest()
        {
            int response = await Mediator.Send(new LanguageCreateCommand()
            {
                Name = "es-ES",
                DisplayName = "Español (España)",
                IsActive = true
            });

            response.ShouldBeGreaterThan(0);

            Language language = Context.Language.FirstOrDefault(p => p.TenantId == SessionContext.TenantId && p.Id == response);

            language.ShouldNotBeNull();
        }

        [Fact]
        public async Task UpdateLanguageTest()
        {
            Language language = Context.Language.FirstOrDefault(p => p.TenantId == SessionContext.TenantId && p.Name.ToUpper() == "UPD");
            string name = language.Name + " MOD";

            int response = await Mediator.Send(new LanguageUpdateCommand()
            {
                Id = language.Id,
                Name = name,
                DisplayName = language.DisplayName + " MOD",
                IsActive = true
            });

            response.ShouldBeGreaterThan(0);

            language = Context.Language.FirstOrDefault(p => p.TenantId == SessionContext.TenantId && p.Id == language.Id);

            language.Name.ShouldBe(name);
        }

        [Fact]
        public async Task DeleteLanguageTest()
        {
            Language language = Context.Language.FirstOrDefault(p => p.TenantId == SessionContext.TenantId && p.Name.ToUpper() == "DEL");
            int id = language.Id;

            int response = await Mediator.Send(new LanguageDeleteCommand()
            {
                Id = language.Id
            });

            response.ShouldBeGreaterThan(0);

            language = Context.Language.FirstOrDefault(p => p.TenantId == SessionContext.TenantId && p.Id == id);

            language.ShouldBeNull();
        }

        [Fact]
        public async Task GetLanguageTest()
        {
            Language language = Context.Language.FirstOrDefault(p => p.TenantId == SessionContext.TenantId);

            language.ShouldNotBeNull();

            LanguageResponse response = await Mediator.Send(new LanguageGetByIdQuery() { Id = language.Id });

            response.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetLanguageListTest()
        {
            PagedResultDto<LanguageForListResponse> response = await Mediator.Send(new LanguageGetListQuery());

            response.ShouldNotBeNull();
            response.Items.Count.ShouldBeGreaterThan(0);
        }

        #endregion

        #region FAILED TESTS

        [Fact]
        public async Task CreateLanguageSanitizeTest()
        {
            var ac = new ASanitizeTest<LanguageCreateCommand, int>(new LanguageCreateCommand()
            {
                Name = "es-ES2",
                DisplayName = "Español (España)",
                IsActive = false
            });

            ac.Mediator = Mediator;

            ac.RuleFor(x => x.Name).NotEmpty();
            ac.RuleFor(x => x.Name).MaxLength(10);
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
        public async Task UpdateLanguageSanitizeTest()
        {
            Language entity = Context.Language.FirstOrDefault(p => p.TenantId == SessionContext.TenantId && p.Name.ToUpper() == "UPDSAN");

            var ac = new ASanitizeTest<LanguageUpdateCommand, int>(new LanguageUpdateCommand()
            {
                Id = entity.Id,
                Name = entity.Name,
                DisplayName = entity.DisplayName,
                IsActive = entity.IsActive == true
            });

            ac.Mediator = Mediator;

            ac.RuleFor(x => x.Id).NotEmpty();
            ac.RuleFor(x => x.Name).NotEmpty();
            ac.RuleFor(x => x.Name).MaxLength(10);
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
        public async Task CreateLanguageFailedNameDuplicatedTest()
        {
            await Assert.ThrowsAsync<EntityDuplicatedException>(async () =>
                    await Mediator.Send(new LanguageCreateCommand()
                    {
                        Name = "es-MX",
                        DisplayName = "FAILED TEST NAME DUPLICATED",
                        IsActive = false
                    })
                );
        }

        [Fact]
        public async Task UpdateLanguageFailedNameDuplicatedTest()
        {
            Language entity = Context.Language.FirstOrDefault(p => p.TenantId == SessionContext.TenantId && p.Name.ToUpper() == "UPDDUP");

            await Assert.ThrowsAsync<EntityDuplicatedException>(async () =>
                    await Mediator.Send(new LanguageUpdateCommand()
                    {
                        Id = entity.Id,
                        Name = "es-MX",
                        DisplayName = "FAILED TEST NAME DUPLICATED",
                        IsActive = false
                    })
                );
        }

        #endregion

        #endregion

        #region LANGUAGES TEXTS

        #region SUCCESSFUL TESTS

        [Fact]
        public async Task CreateLanguageTextTest()
        {
            Language language = await GetLanguageDefault();

            language.ShouldNotBeNull();

            long response = await Mediator.Send(new LanguageTextCreateCommand()
            {
                LanguageId = language.Id,
                Key = "Status",
                Value = "Estatus"
            });

            response.ShouldBeGreaterThan(0);

            LanguageText languageText = Context.LanguageText.FirstOrDefault(p => p.Language.TenantId == SessionContext.TenantId && p.Id == response);

            languageText.ShouldNotBeNull();
        }

        [Fact]
        public async Task UpdateLanguageTextTest()
        {
            Language language = await GetLanguageDefault();
            LanguageText languageText = Context.LanguageText.FirstOrDefault(p => p.Language.TenantId == SessionContext.TenantId && p.LanguageId == language.Id && p.Key.ToUpper() == "UPD");
            string value = languageText.Value + " MOD";

            long response = await Mediator.Send(new LanguageTextUpdateCommand()
            {
                Id = languageText.Id,
                LanguageId = (int)languageText.LanguageId,
                Key = languageText.Key,
                Value = value
            });

            response.ShouldBeGreaterThan(0);

            languageText = Context.LanguageText.FirstOrDefault(p => p.Language.TenantId == SessionContext.TenantId && p.Id == languageText.Id);

            languageText.Value.ShouldBe(value);
        }

        [Fact]
        public async Task GetLanguageTextTest()
        {
            LanguageText languageText = Context.LanguageText.FirstOrDefault(p => p.Language.TenantId == SessionContext.TenantId);

            languageText.ShouldNotBeNull();

            LanguageTextResponse response = await Mediator.Send(new LanguageTextGetByIdQuery() { Id = languageText.Id });

            response.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetLanguageTextListTest()
        {
            Language language = await GetLanguageDefault();
            PagedResultDto<LanguageTextForListResponse> response = await Mediator.Send(new LanguageTextGetListQuery() { LanguageId = language.Id });

            response.ShouldNotBeNull();
            response.Items.Count.ShouldBeGreaterThan(0);
        }

        #endregion

        #region FAILED TESTS

        [Fact]
        public async Task CreateLanguageTextSanitizeTest()
        {
            Language language = await GetLanguageDefault();

            language.ShouldNotBeNull();

            var ac = new ASanitizeTest<LanguageTextCreateCommand, long>(new LanguageTextCreateCommand()
            {
                LanguageId = language.Id,
                Key = "Status",
                Value = "Estatus"
            });

            ac.Mediator = Mediator;

            ac.RuleFor(x => x.Value).NotEmpty();

            await ac.RunTest();

            if (ac.TotalErrors > 0)
            {
                throw new System.Exception(string.Join("\n\n", ac.ErrorMessages));
            }
        }

        [Fact]
        public async Task UpdateLanguageTextSanitizeTest()
        {
            Language language = await GetLanguageDefault();
            LanguageText entity = Context.LanguageText.FirstOrDefault(p => p.Language.TenantId == SessionContext.TenantId && p.LanguageId == language.Id && p.Key.ToUpper() == "UPD");

            var ac = new ASanitizeTest<LanguageTextUpdateCommand, long>(new LanguageTextUpdateCommand()
            {
                Id = entity.Id,
                LanguageId = entity.LanguageId.Value,
                Key = entity.Key,
                Value = entity.Value
            });

            ac.Mediator = Mediator;

            ac.RuleFor(x => x.LanguageId).NotEmpty();
            ac.RuleFor(x => x.Value).NotEmpty();

            await ac.RunTest();

            if (ac.TotalErrors > 0)
            {
                throw new System.Exception(string.Join("\n\n", ac.ErrorMessages));
            }

			Assert.Equal(0, ac.TotalErrors);
		}

        #endregion

        #endregion

        #region PRIVATE METHODS

        private async Task<Language> GetLanguageDefault()
        {
            if (LanguageDefault == null)
            {
                LanguageDefault = await Context.Language.FirstOrDefaultAsync(p => p.TenantId == SessionContext.TenantId && p.Name == "es-MX");
            }

            return LanguageDefault;
        }

        #endregion

    }
}
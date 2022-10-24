using AlgoriaCore.Application.QueriesAndCommands.SessionLogin._2Queries;
using AlgoriaCore.Application.QueriesAndCommands.SessionLogin._3Commands;
using AlgoriaCore.Application.QueriesAndCommands.Users._3Commands;
using AlgoriaCore.Application.Tests.ASanitization;
using AlgoriaCore.Application.Tests.Host.Settings;
using AlgoriaCore.Application.Tests.Infrastructure;
using AlgoriaCore.Domain.Exceptions;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace AlgoriaCore.Application.Tests.Host.Login
{
    [Collection("TestsCollection")]
    public class LoginTests : TestBaseHost
    {
		readonly QueryTestFixture _fixture;

        public LoginTests(QueryTestFixture fixture) : base(fixture) { _fixture = fixture; }

        #region SUCCESSFUL TESTS

        [Fact]
        public async Task LoginUserTest()
        {
            await Mediator.Send(new UserCreateCommand()
            {
                Name = "Prueba usuario",
                LastName = "Apellido P prueb",
                SecondLastName = "Apellido M prueba",
                EmailAddress = "correo7@dominio.com",
                PhoneNumber = "3515345",
                UserName = "usertest07",
                Password = "contrAsen1",
                ShouldChangePasswordOnNextLogin = false,
                IsActive = true
            });

            var lResp = await Mediator.Send(new UserLoginQuery()
            {
                UserName = "usertest07",
                Password = "contrAsen1"
			});

            lResp.ShouldNotBeNull();
        }

        [Fact]
        public async Task UserChangePasswordTest()
        {
            await Mediator.Send(new UserCreateCommand()
            {
                Name = "Prueba usuario",
                LastName = "Apellido P prueb",
                SecondLastName = "Apellido M prueba",
                EmailAddress = "correo6@dominio.com",
                PhoneNumber = "3515345",
                UserName = "usertest06",
                Password = "contrAsen1",
                ShouldChangePasswordOnNextLogin = false,
                IsActive = true
            });

            await Mediator.Send(new UserChangePasswordCommand()
            {
                UserName = "usertest06",
                CurrentPassword = "contrAsen1",
                NewPassword = "123qwe",
                ConfirmPassword = "123qwe",
                TenancyName = null
            });

            var lResp = await Mediator.Send(new UserLoginQuery()
            {
                TenancyName = null,
                UserName = "usertest06",
                Password = "123qwe"
            });

            lResp.ShouldNotBeNull();
        }

        [Fact]
        public async Task UserChangePasswordSanitizeTest()
        {
            await Mediator.Send(new UserCreateCommand()
            {
                Name = "Prueba usuario",
                LastName = "Apellido P prueb",
                SecondLastName = "Apellido M prueba",
                EmailAddress = "correo5@dominio.com",
                PhoneNumber = "3515345",
                UserName = "usertest05",
                Password = "contrAsen1",
                //PasswordRepeat = "contrasenia",
                ShouldChangePasswordOnNextLogin = false,
                IsActive = true
            });

            var ac = new ASanitizeTest<UserChangePasswordCommand, long>(new UserChangePasswordCommand()
            {
                UserName = "usertest01",
                CurrentPassword = "contrasenia",
                NewPassword = "contrasenia01",
                ConfirmPassword = "contrasenia01"
            });

            ac.Mediator = Mediator;

            ac.RuleFor(x => x.CurrentPassword).NotEmpty();
            ac.RuleFor(x => x.NewPassword).NotEmpty();
            ac.RuleFor(x => x.ConfirmPassword).Equal(x => x.NewPassword);

            await ac.RunTest();

            if (ac.TotalErrors > 0)
            {
                throw new System.Exception(string.Join("\n\n", ac.ErrorMessages));
            }

			Assert.Equal(0, ac.TotalErrors);
		}

        #endregion

        #region FAILED TESTS

        [Fact]
        public async Task LoginUserWrongUserNameTest()
        {
            await Mediator.Send(new UserCreateCommand()
            {
                Name = "Prueba usuario",
                LastName = "Apellido P prueb",
                SecondLastName = "Apellido M prueba",
                EmailAddress = "correo4@dominio.com",
                PhoneNumber = "3515345",
                UserName = "usertest04",
                Password = "contrAsen1",
                ShouldChangePasswordOnNextLogin = false,
                IsActive = true
            });

            await Assert.ThrowsAsync<WrongUserNameOrPasswordException>(async () =>
                    await Mediator.Send(new UserLoginQuery()
                    {
                        UserName = "usertest",
                        Password = "contrasenia"
                    })
                );
        }

        [Fact]
        public async Task LoginUserWrongPasswordTest()
        {
            await Mediator.Send(new UserCreateCommand()
            {
                Name = "Prueba usuario",
                LastName = "Apellido P prueb",
                SecondLastName = "Apellido M prueba",
                EmailAddress = "correo3@dominio.com",
                PhoneNumber = "3515345",
                UserName = "usertest03",
                Password = "contrAsen1",
                ShouldChangePasswordOnNextLogin = false,
                IsActive = true
            });

            await Assert.ThrowsAsync<WrongUserNameOrPasswordException>(async () =>
                    await Mediator.Send(new UserLoginQuery()
                    {
                        UserName = "usertest03",
                        Password = "contrAsen1aa"
					})
                );
        }

        [Fact]
        public async Task LoginUserMustChangePassowrdTest()
        {
            await Mediator.Send(new UserCreateCommand()
            {
                Name = "Prueba usuario",
                LastName = "Apellido P prueb",
                SecondLastName = "Apellido M prueba",
                EmailAddress = "correo2@dominio.com",
                PhoneNumber = "3515345",
                UserName = "usertest02",
                Password = "contrAsen1",
                ShouldChangePasswordOnNextLogin = true,
                IsActive = true
            });

            await Assert.ThrowsAsync<UserMustChangePasswordException>(async () =>
                    await Mediator.Send(new UserLoginQuery()
                    {
                        UserName = "usertest02",
                        Password = "contrAsen1"
					})
                );
        }

        [Fact]
        public async Task LoginUserLockedTest()
        {
            SettingsTests settingTests = new SettingsTests(_fixture);

            await settingTests.UpdateSettingsTest();

            await Mediator.Send(new UserCreateCommand()
            {
                Name = "Prueba usuario",
                LastName = "Apellido P prueb",
                SecondLastName = "Apellido M prueba",
                EmailAddress = "correo1@dominio.com",
                PhoneNumber = "3515345",
                UserName = "userToLockedTest",
                Password = "contrAsen1",
                ShouldChangePasswordOnNextLogin = false,
                IsActive = true
            });

            await Assert.ThrowsAsync<WrongUserNameOrPasswordException>(async () =>
                    await Mediator.Send(new UserLoginQuery()
                    {
                        UserName = "userToLockedTest",
                        Password = "contraseniaXXX"
                    })
                );

            await Assert.ThrowsAsync<WrongUserNameOrPasswordException>(async () =>
                    await Mediator.Send(new UserLoginQuery()
                    {
                        UserName = "userToLockedTest",
                        Password = "contraseniaXXX"
                    })
                );

            await Assert.ThrowsAsync<UserLockedException>(async () =>
                    await Mediator.Send(new UserLoginQuery()
                    {
                        UserName = "userToLockedTest",
                        Password = "contraseniaXXX"
                    })
                );
        }

        #endregion
    }
}
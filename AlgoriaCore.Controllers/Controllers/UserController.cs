using AlgoriaCore.Application.BaseClases.Dto;
using AlgoriaCore.Application.QueriesAndCommands.Users._1Model;
using AlgoriaCore.Application.QueriesAndCommands.Users._2Queries;
using AlgoriaCore.Application.QueriesAndCommands.Users._3Commands;
using AlgoriaCore.Domain.Authorization;
using AlgoriaCore.WebUI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlgoriaCore.WebUI.Controllers
{
    [AlgoriaCoreAuthorizationFilter]
    public class UserController : BaseController
    {
        [HttpPost]
        public async Task<PagedResultDto<UserListResponse>> GetUserList([FromBody]UserGetListQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<FileDto> ExportUser([FromBody] UserExportQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<FileDto> ExportCSVUser([FromBody] UserExportCSVQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<FileDto> ExportPDFUser([FromBody] UserExportPDFQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
		public async Task<List<UserListResponse>> GetUserAutocompleteList(string term)
		{
			UserGetListQuery query = new UserGetListQuery();
			query.Filter = term;
			query.PageNumber = 1;
			query.PageSize = 50;

			var ll = await Mediator.Send(query);
			return ll.Items;
		}

		[HttpPost]
		public async Task<List<object>> GetUserForEditorAutocompleteList(string term)
		{
			UserGetListQuery query = new UserGetListQuery();
			query.Filter = term;
			query.PageNumber = 1;
			query.PageSize = 50;

			var ll = await Mediator.Send(query);
			return ll.Items.Select(m => new { Id = m.Id, Login = m.Login, FullName = m.FullName }).ToList<object>();
		}

		[AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_Users_Create)]
        [HttpPost]
        public async Task<long> CreateUser([FromBody]UserCreateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_Users_Edit)]
        [HttpPost]
        public async Task<long> UpdateUser([FromBody]UserUpdateCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [HttpPost]
        public async Task<UserForEditResponse> GetUserForEdit(UserGetForEditQuery dto)
        {
            return await Mediator.Send(dto);
        }

        [HttpPost]
        public async Task<UserForEditResponse> GetUserProfile(UserProfileQuery dto)
        {
            return await Mediator.Send(dto);
        }

        [HttpPost]
        public async Task<long> UpdateUserProfile(UserUpdateProfileCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetPictureProfile(long id)
        {
            var uM = new UserProfilePictureQuery();
            uM.Id = id;

            var f = await Mediator.Send(uM);

            if (f.FileArray == null)
            {
                var fPath = System.IO.Directory.GetCurrentDirectory();
                var fil = System.IO.Path.Combine(fPath, "Content/static/default-picture.png");
                f.FileArray = System.IO.File.ReadAllBytes(fil);
                f.ContentType = "application/octet-stream";
            }

            return File(f.FileArray, f.ContentType);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_Users_Delete)]
        [HttpPost]
        public async Task<long> DeleteUser([FromBody]UserDeleteCommand dto)
        {
            return await Mediator.Send(dto);
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_Users_Edit)]
        [HttpPost("{id}")]
        public async Task<long> LockUser(long id)
        {
            return await Mediator.Send(new UserLockCommand() { Id = id });
        }

        [AlgoriaCoreAuthorizationFilter(AppPermissions.Pages_Administration_Users_Edit)]
        [HttpPost("{id}")]
        public async Task<long> UnlockUser(long id)
        {
            return await Mediator.Send(new UserUnlockCommand() { Id = id });
        }
    }
}
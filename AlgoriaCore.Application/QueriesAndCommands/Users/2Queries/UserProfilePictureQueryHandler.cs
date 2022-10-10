using AlgoriaCore.Application.Managers.BinaryObjects;
using AlgoriaCore.Application.Managers.Users;
using AlgoriaCore.Application.QueriesAndCommands.Files._1Model;
using AlgoriaPersistence.Interfaces.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AlgoriaCore.Application.QueriesAndCommands.Users._2Queries
{
    public class UserProfilePictureQueryHandler : IRequestHandler<UserProfilePictureQuery, GetFileResponse>
    {
        private readonly UserManager _userManager;
        private readonly BinaryObjectManager _binaryManager;

        public UserProfilePictureQueryHandler(UserManager userManager,
                                            BinaryObjectManager binaryManager)
        {
            _userManager = userManager;
            _binaryManager = binaryManager;
        }

        public async Task<GetFileResponse> Handle(UserProfilePictureQuery request, CancellationToken cancellationToken)
        {
            using (_userManager.CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MayHaveTenant))
            {
                using (_userManager.CurrentUnitOfWork.DisableFilter(AlgoriaCoreDataFilters.MustHaveTenant))
                {
                    GetFileResponse r = new GetFileResponse();

                    var ud = await _userManager.GetUserById(request.Id);

                    if (ud != null && ud.ProfilePictureId != null)
                    {
                        r.FileArray = await _binaryManager.GetFileAsync(ud.ProfilePictureId ?? Guid.NewGuid());
                        r.ContentType = "application/octet-stream";
                    }

                    return r;
                }
            }
        }
    }
}
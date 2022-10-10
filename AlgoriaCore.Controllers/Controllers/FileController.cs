using AlgoriaCore.Application.QueriesAndCommands.Files._1Model;
using AlgoriaCore.Application.QueriesAndCommands.Files._2Queries;
using AlgoriaCore.Application.QueriesAndCommands.Files._3Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AlgoriaCore.WebUI.Controllers
{
    public class FileController : BaseController
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetFile(string uuid)
        {
            var fg = new GetFile();
            fg.UUID = uuid.ToUpper();

            var f = await Mediator.Send(fg);

            return File(f.FileArray, f.ContentType);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> DownloadFileByUUID(string uuid)
        {
            var fg = new GetFile();
            fg.UUID = uuid.ToUpper();

            var f = await Mediator.Send(fg);
            f.ContentType = "application/octet-stream";

            return File(f.FileArray, f.ContentType, f.FileName);
        }

        [HttpGet]
        public async Task<string> GetFileB64(string uuid)
        {
            var fg = new GetFile();
            fg.UUID = uuid.ToUpper();

            var f = await Mediator.Send(fg);

            return Convert.ToBase64String(f.FileArray);
        }

        [HttpPost]
        public async Task<FileUploadResponse> UploadTemp(IFormFile file)
        {
            var fs = new UploadTempFile();
            fs.ContentDisposition = file.ContentDisposition;
            fs.ContentType = file.ContentType;
            fs.FileName = file.FileName;
            fs.Name = file.Name;
            fs.Length = (int)file.Length;

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                fs.FileArray = stream.ToArray();
            }

            return await Mediator.Send(fs);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetFileTemp(string tempFileName)
        {
            var fg = new GetTempFile();
            fg.FileName = tempFileName;

            var f = await Mediator.Send(fg);

            return File(f.FileArray, f.ContentType);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> DownloadTempFile(string fileName, string fileType, string fileToken)
        {
            var tempFileName = new DownloadTempFile();
            tempFileName.FileName = fileName;
            tempFileName.FileType = fileType;
            tempFileName.FileToken = fileToken;

            var file = await Mediator.Send(tempFileName);

            return File(file.FileArray, file.ContentType, file.FileName);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> DownloadTempFilePDF(string tempFileName, string downloadName)
        {
            var fg = new GetTempFile();
            fg.FileName = tempFileName;

            var f = await Mediator.Send(fg);

            return File(f.FileArray, f.ContentType, downloadName);
        }
    }
}

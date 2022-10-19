using AlgoriaCore.Application;
using AlgoriaCore.Application.Configuration;
using AlgoriaCore.Application.Localization;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaCore.Domain.FileStorage;
using AlgoriaCore.Domain.Interfaces.FileStorage;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Autofac;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AlgoriaInfrastructure.FileStorage
{
    public class FileStorageS3Service : IFileStorageService
    {
        public IAppLocalizationProvider AppLocalizationProvider { get; set; }

        private readonly string _bucketName;
        private readonly RegionEndpoint bucketRegion = RegionEndpoint.USWest2;
        private readonly IAmazonS3 _client;

        public FileStorageS3Service(
            ILifetimeScope lifeTimeScope,
            IOptions<FileStorageOptions> fileStorageOptions
        )
        {
            _bucketName = fileStorageOptions.Value.S3.Bucket;

            _client = new AmazonS3Client(
                fileStorageOptions.Value.S3.Id,
                fileStorageOptions.Value.S3.SecretKey,
                bucketRegion
            );
        }
        public string L(string key)
        {
            return AppLocalizationProvider.L(key);
        }

        public string CreateTempFile(IFileStorageDto request, string path)
        {
            #region GUID AND NAMING
            var uid = Guid.NewGuid().ToString();
            string tempFileName = "";
            if (request.FileName.IndexOf('.') >= 0)
            {
                tempFileName = string.Format("{0}.{1}", uid, request.FileName.Substring(request.FileName.LastIndexOf('.') + 1));
            }
            else
            {
                tempFileName = uid;
            }
            string tempPath = string.Format("{0}\\{1}", path, tempFileName);
            #endregion

            WritingAnObjectAsync(request.FileArray, tempFileName, request.ContentType).Wait();

            return tempFileName;
        }

        public async Task<IFileStorageDto> GetTempFileAsync(string tempFileName, string tempPath)
        {
            string keyName = tempFileName;
            FileStorageDto fResp = await ReadObjectDataAsync(keyName);
            return fResp;
        }

        public async Task<IFileStorageDto> DownloadTempFile(string filePath, string fileName, string fileType)
        {
            string keyName = fileName;
            FileStorageDto fResp = await ReadObjectDataAsync(keyName);
            await DeleteObjectAsync(keyName);
            return fResp;
        }


        #region FileHandling S3
        public Stream GetStreamFromFile(byte[] fileArray)
        {
            Stream stream = new MemoryStream(fileArray);
            return stream;
        }
        public byte[] GetFileFromStream(Stream inputStream)
        {
            using (var streamReader = new MemoryStream())
            {
                inputStream.CopyTo(streamReader);
                return streamReader.ToArray();
            }
        }

        public async Task WritingAnObjectAsync(byte[] fileArray, string keyName, string contentType)
        {
            try
            {
                var putRequest2 = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    InputStream  = GetStreamFromFile(fileArray),
                    Key = keyName,
                    ContentType = contentType
                };
                putRequest2.Metadata.Add("x-amz-meta-title", keyName);
                PutObjectResponse response2 = await _client.PutObjectAsync(putRequest2);
            }
            catch (AmazonS3Exception e)
            {
                string message = e.Message;
                throw new AlgoriaCoreGeneralException(L("UploadingFileToS3Error : {message}"));
            }
            catch (Exception e)
            {
                string message = e.Message;
                throw new AlgoriaCoreGeneralException(L("UploadingFileToS3Error : Unknown encountered on server: {message}"));
            }
        }

        public async Task<FileStorageDto> ReadObjectDataAsync(string keyName)
        {
            try
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = _bucketName,
                    Key = keyName
                };
                GetObjectResponse response = await _client.GetObjectAsync(request);

                var fResp = new FileStorageDto();
                fResp.FileName = response.Key;
                fResp.Length = (int)response.Headers.ContentLength;
                fResp.FileArray = GetFileFromStream(response.ResponseStream);
                fResp.ContentType = response.Headers.ContentType;
                return fResp;

            }
            catch (AmazonS3Exception e)
            {
                string message = e.Message;
                throw new AlgoriaCoreGeneralException(L("UploadingFileToS3Error : {message}"));
            }
            catch (Exception e)
            {
                string message = e.Message;
                throw new AlgoriaCoreGeneralException(L("UploadingFileToS3Error : Unknown encountered on server: {message}"));
            }
        }

        private async Task DeleteObjectAsync(string keyName)
        {
            try
            {
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = keyName
                };

                await _client.DeleteObjectAsync(deleteObjectRequest);
            }
            catch (AmazonS3Exception e)
            {
                string message = e.Message;
                throw new AlgoriaCoreGeneralException(L("UploadingFileToS3Error : {message}"));
            }
            catch (Exception e)
            {
                string message = e.Message;
                throw new AlgoriaCoreGeneralException(L("UploadingFileToS3Error : Unknown encountered on server: {message}"));
            }
        }

        #endregion


    }
}

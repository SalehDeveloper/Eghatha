using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Eghatha.Application.Common.Services;
using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.Storage
{
    public class CloudinaryService : ICloudinaryService
    { 
        private readonly CloudinaryOptions _options;
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinaryOptions> cloudinaryOptions)
        {
            _options = cloudinaryOptions.Value;

            var account = new Account(_options.Name, _options.ApiKey, _options.ApiSecret);

            _cloudinary = new Cloudinary(account);
        }



        public async Task<ErrorOr<string>> UploadUserPhotoAsync(string email, IFormFile photoFile)
        {
            if (IsEmptyFile(photoFile))
                return CloudinaryErrors.EmptyFile;

            if (!IsValidImageType(photoFile.ContentType))
                return CloudinaryErrors.UnsupportedPhotoType;


            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(photoFile.FileName, photoFile.OpenReadStream()),
                PublicId = $"users/{email}/profile",
                Overwrite = true
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            if (result.StatusCode == HttpStatusCode.OK)
                return result.SecureUrl.ToString();

            return CloudinaryErrors.FailedToUpload;
        }

        public async Task<ErrorOr<string>> UploadVolunteerCvAsync(string email, IFormFile pdfFile)
        {
            if (IsEmptyFile(pdfFile))
                return ErrorOr.Error.Validation("File", "cv file is required.");

            var ext = Path.GetExtension(pdfFile.FileName).ToLowerInvariant();
            var mime = pdfFile.ContentType.ToLowerInvariant();

            var isPdf = mime == "application/pdf" && ext == ".pdf";
            if (!isPdf)
                return ErrorOr.Error.Validation("File", "Certification must be a valid PDF file.");

            string path = $"volunteers/{email}/cv";

            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(pdfFile.FileName, pdfFile.OpenReadStream()),
                PublicId = path,
                Overwrite = true,

            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
                return ErrorOr.Error.Failure("Cloudinary", "Failed to upload cv.");

            return result.SecureUrl.ToString();
        }



        private static readonly List<string> AllowedPhotoTypes = new()
        {
        "image/jpeg",
        "image/png",
        "image/jpg"
        };

        private bool IsValidImageType(string imageType)
        {
            if (AllowedPhotoTypes.Contains(imageType))
                return true;
            return false;
        }

        private bool IsEmptyFile(IFormFile file)
        {
            if (file is null || file.Length == 0)
                return true;

            return false;
        }
    }
}

using ErrorOr;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Application.Common.Services
{
    public interface ICloudinaryService
    {
        Task<ErrorOr<string>> UploadUserPhotoAsync(string email, IFormFile photoFile);

        Task<ErrorOr<string>> UploadVolunteerCvAsync(string email, IFormFile pdfFile);


    }
}

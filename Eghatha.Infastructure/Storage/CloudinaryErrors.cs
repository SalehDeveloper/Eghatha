using ErrorOr;

namespace Eghatha.Infastructure.Storage
{
    public static class CloudinaryErrors
    {
        public static readonly Error EmptyFile = Error.Validation(code: "Cloudinary.EmptyFile", description: "File is empty");

        public static readonly Error UnsupportedPhotoType = Error.Validation(code: "Cloudinary.UnsupportedPhotoType", description: "Invalid photo type. Only JPG,JPEG and PNG are allowed.");

        public static readonly Error UnsupportedFileType = Error.Validation(code: "Cloudinary.UnsupportedFileType",
            description: "Invalid file type.Only PDF or Word documents are allowed");

        public static readonly Error FailedToUpload = Error.Failure(code: "Cloudinary.FailedToUpload", description: "Failed to upload photo.");
        public static readonly Error FailedToDelete = Error.Failure(code: "Cloudinary.FailedToDelete", description: "Failed to delete photo.");


    }
}

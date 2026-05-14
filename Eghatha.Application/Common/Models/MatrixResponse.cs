namespace Eghatha.Application.Common.Models
{
    public sealed class MatrixResponse
    {
        public double?[][] durations { get; set; } = [];

        public double?[][] distances { get; set; } = [];
    }
}

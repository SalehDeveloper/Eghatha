namespace Eghatha.Application.Features.Disasters.Dtos
{
    public sealed record ReportDto(
      Guid Id,
      string Summary,
      string PdfUrl,
      DateTimeOffset IssuedAt
  );
}

namespace MesaApi.Application.Common.Interfaces;

public interface IPdfGeneratorService
{
    Task<byte[]> GenerateRequestPdfAsync(int requestId, CancellationToken cancellationToken = default);
    Task<byte[]> GenerateReportPdfAsync(object reportData, string reportTitle, CancellationToken cancellationToken = default);
}
using PCBack.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PCBack.Services;

public class PdfService : IPdfService
{
    static PdfService()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public byte[] GenerateReportPdf(CommercialReport report)
    {
        ArgumentNullException.ThrowIfNull(report);

        return Document.Create(document =>
        {
            document.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(11));

                page.Content().Column(column =>
                {
                    column.Spacing(10);

                    column.Item().Text("PatentClarity — Commercial Report")
                        .SemiBold().FontSize(16);

                    column.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                    column.Item().Text(text =>
                    {
                        text.Span("Title: ").SemiBold();
                        text.Span(report.Title ?? string.Empty);
                    });

                    column.Item().Text(text =>
                    {
                        text.Span("Patent Owner: ").SemiBold();
                        text.Span(report.PatentOwner ?? string.Empty);
                    });

                    column.Item().Text(text =>
                    {
                        text.Span("Patent Status: ").SemiBold();
                        text.Span(report.PatentStatus ?? string.Empty);
                    });

                    column.Item().PaddingTop(8).Text("Technology Tags").SemiBold().FontSize(13);
                    AddBulletList(column, report.TechnologyTags);

                    column.Item().PaddingTop(8).Text("Potential Markets").SemiBold().FontSize(13);
                    AddBulletList(column, report.PotentialMarkets);

                    column.Item().PaddingTop(8).Text("Commercial Opportunities").SemiBold().FontSize(13);
                    AddBulletList(column, report.CommercialOpportunities);
                });
            });
        }).GeneratePdf();
    }

    private static void AddBulletList(ColumnDescriptor column, IReadOnlyList<string>? items)
    {
        if (items == null || items.Count == 0)
        {
            column.Item().Text("—").FontColor(Colors.Grey.Medium);
            return;
        }

        foreach (var line in items.Where(s => !string.IsNullOrWhiteSpace(s)))
            column.Item().Text($"• {line.Trim()}");
    }
}

using Eghatha.Application.Common.Services;
using Eghatha.Domain.Disasters;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eghatha.Infastructure.Services
{
    using Eghatha.Application.Common.Interfaces;
    using Eghatha.Domain.Teams;
    using QuestPDF.Fluent;
    using QuestPDF.Helpers;
    using QuestPDF.Infrastructure;

    public class DisasterReportPdfService : IDisasterReportPdfService
    {
        private readonly ITeamRepository _teamRepository;

        public DisasterReportPdfService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<byte[]> Generate(Disaster disaster, CancellationToken cancellationToken)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var teamIds = disaster.Teams.Select(t => t.TeamId).ToList();
            var teams = await _teamRepository.GetTeamsByIdsAsync(teamIds, cancellationToken);
            var teamDict = teams.ToDictionary(x => x.Id);

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

                    // ---------------- HEADER ----------------
                    page.Header()
                        .PaddingBottom(10)
                        .Column(column =>
                        {
                            column.Item().Background(Colors.Red.Darken2).Padding(15).Column(c =>
                            {
                                c.Item().Text("DISASTER INCIDENT REPORT")
                                    .FontSize(22)
                                    .Bold()
                                    .FontColor(Colors.White);

                                c.Item().Text(disaster.Title)
                                    .FontSize(14)
                                    .FontColor(Colors.White);
                            });
                        });

                    // ---------------- CONTENT ----------------
                    page.Content()
                        .Column(column =>
                        {
                            column.Spacing(18);

                            ComposeGeneralInformation(column.Item(), disaster);
                            ComposeTeamsSection(column.Item(), disaster, teamDict);
                            ComposeResourcesSection(column.Item(), disaster);
                            ComposeAffectedPeopleSection(column.Item(), disaster);
                        });

                    // ---------------- FOOTER ----------------
                    page.Footer()
                        .BorderTop(1)
                        .BorderColor(Colors.Grey.Lighten2)
                        .PaddingTop(8)
                        .Row(row =>
                        {
                            row.RelativeItem()
                                .Text($"Generated: {DateTime.UtcNow:u}")
                                .FontSize(9)
                                .FontColor(Colors.Grey.Darken1);

                            row.ConstantItem(120)
                                .AlignRight()
                                .DefaultTextStyle(x => x.FontSize(9).FontColor(Colors.Grey.Darken1))
                                .Text(text =>
                                {
                                    text.Span("Page ");
                                    text.CurrentPageNumber();
                                });
                        });
                });
            });

            return document.GeneratePdf();
        }

        // ================= GENERAL INFO =================
        private void ComposeGeneralInformation(IContainer container, Disaster disaster)
        {
            container
                .Background(Colors.Grey.Lighten4)
                .Padding(15)
                .Border(1)
                .BorderColor(Colors.Grey.Lighten2)
                .Column(column =>
                {
                    column.Spacing(6);

                    column.Item().Text("GENERAL INFORMATION")
                        .FontSize(14)
                        .Bold()
                        .FontColor(Colors.Red.Darken2);

                    column.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten1);

                    column.Item().Text($"Type: {disaster.Type.Name}");
                    column.Item().Text($"Location: {disaster.City}, {disaster.Province}");
                    column.Item().Text($"Status: {disaster.Status.Name}");
                    column.Item().Text($"Start: {disaster.StartTime:u}");
                    column.Item().Text($"End: {(disaster.EndTime?.ToString("u") ?? "N/A")}");

                    column.Item().PaddingTop(5);

                    column.Item().Text("Description")
                        .Bold();

                    column.Item().Text(disaster.Description);
                });
        }

        // ================= TEAMS =================
        private void ComposeTeamsSection(
      IContainer container,
      Disaster disaster,
      Dictionary<Guid, Team> teamDict)
        {
            container.Column(column =>
            {
                column.Spacing(8);

                column.Item().Text("ASSIGNED TEAMS")
                    .FontSize(14)
                    .Bold()
                    .FontColor(Colors.Blue.Darken2);

                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(250); // Team Name
                        columns.RelativeColumn();    // Team Id
                    });

                    table.Header(header =>
                    {
                        header.Cell()
                            .Background(Colors.Blue.Lighten3)
                            .Padding(6)
                            .Text("Team Name")
                            .Bold();

                        header.Cell()
                            .Background(Colors.Blue.Lighten3)
                            .Padding(6)
                            .Text("Team Id")
                            .Bold();
                    });

                    foreach (var team in disaster.Teams)
                    {
                        var teamName = teamDict.TryGetValue(team.TeamId, out var t)
                            ? t.Name
                            : "Unknown";

                        table.Cell()
                            .BorderBottom(1)
                            .BorderColor(Colors.Grey.Lighten3)
                            .Padding(6)
                            .Text(teamName);

                        table.Cell()
                            .BorderBottom(1)
                            .BorderColor(Colors.Grey.Lighten3)
                            .Padding(6)
                            .Text(team.TeamId.ToString());
                    }
                });
            });
        }

        // ================= RESOURCES =================
        private void ComposeResourcesSection(IContainer container, Disaster disaster)
        {
            container.Column(column =>
            {
                column.Spacing(8);

                column.Item().Text("RESOURCES")
                    .FontSize(14)
                    .Bold()
                    .FontColor(Colors.Green.Darken2);

                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3);
                        columns.ConstantColumn(50);
                        columns.ConstantColumn(60);
                        columns.ConstantColumn(60);
                        columns.ConstantColumn(60);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.Green.Lighten3).Padding(5).Text("Resource").Bold();
                        header.Cell().Background(Colors.Green.Lighten3).Padding(5).Text("Sent").Bold();
                        header.Cell().Background(Colors.Green.Lighten3).Padding(5).Text("Consumed").Bold();
                        header.Cell().Background(Colors.Green.Lighten3).Padding(5).Text("Returned").Bold();
                        header.Cell().Background(Colors.Green.Lighten3).Padding(5).Text("Damaged").Bold();
                    });

                    foreach (var r in disaster.Resources)
                    {
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(r.ResourceType.Name);
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(r.QuantitySent.ToString());
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(r.QuantityConsumed.ToString());
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(r.QuantityReturned.ToString());
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(r.QuantityDamaged.ToString());
                    }
                });
            });
        }

        // ================= AFFECTED PEOPLE =================
        private void ComposeAffectedPeopleSection(IContainer container, Disaster disaster)
        {
            container.Column(column =>
            {
                column.Spacing(8);

                column.Item().Text("AFFECTED PEOPLE")
                    .FontSize(14)
                    .Bold()
                    .FontColor(Colors.Orange.Darken2);

                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3);   // Name
                        columns.ConstantColumn(50);  // Age
                        columns.RelativeColumn(2);   // Phone
                        columns.RelativeColumn(2);   // Status
                    });

                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.Orange.Lighten3).Padding(5).Text("Name").Bold();
                        header.Cell().Background(Colors.Orange.Lighten3).Padding(5).Text("Age").Bold();
                        header.Cell().Background(Colors.Orange.Lighten3).Padding(5).Text("Phone").Bold();
                        header.Cell().Background(Colors.Orange.Lighten3).Padding(5).Text("Status").Bold();
                    });

                    foreach (var p in disaster.AffectedPeople)
                    {
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(p.Name);
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(p.Age.ToString());
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(p.Phone);
                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(p.Status.Name);
                    }
                });
            });
        }
    }
}
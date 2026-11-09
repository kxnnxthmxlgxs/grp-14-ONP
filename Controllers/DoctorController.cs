using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NMBDHL.DataAccess.SQLDataBaseAccess;
using NMBDHL.Doctor.Model;
using NMBDHL.Doctor.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

namespace WebApplication2.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorRepository _doctorRepo;

        public DoctorController(IConfiguration config)
        {
            var db = new SQLDataBaseAccess(config);
            _doctorRepo = new DoctorRepository(db, config);
        }

        public IActionResult DoctorDashboard()
        {
            return View("~/Views/Home/DoctorDashboard.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> SubmitTestRequest(int patientId, List<string> testPanels, string urgency, string clinicalNotes, string barcode)
        {
            try
            {
                // Defaulting to DoctorID 1 for the session
                int doctorId = 1;

                // Convert the multiple selected panels into a single comma-separated string for the 'Type' column
                string combinedPanels = testPanels != null && testPanels.Any() ? string.Join(", ", testPanels) : "";

                // Send the new barcode parameter to the repository
                bool success = await _doctorRepo.CreateTestRequestAsync(patientId, doctorId, combinedPanels, urgency, clinicalNotes, barcode);

                if (success)
                    TempData["SuccessMessage"] = "Test request submitted successfully.";
                else
                    TempData["ErrorMessage"] = "Failed to submit test request.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
            }

            return RedirectToAction("DoctorDashboard");
        }

        [HttpPost]
        public async Task<IActionResult> CancelRequest(int requestId, string cancelReason)
        {
            try
            {
                // This calls the repository method
                await _doctorRepo.CancelTestRequestAsync(requestId, cancelReason);

                // Return a JSON success message to the frontend
                return Json(new { success = true, message = $"Order #{requestId} has been successfully revoked." });
            }
            catch (Exception ex)
            {
                // If SQL Server throws an error, send it back to the UI
                return Json(new { success = false, message = "Database Error: " + ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> ReleaseRequest(int requestId, string releaseNote)
        {
            try
            {
                await _doctorRepo.ReleaseTestRequestAsync(requestId, releaseNote);
                return Json(new { success = true, message = $"Results for Order #{requestId} securely released to patient portal." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Database Error: " + ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpdatePassword(string newPass)
        {
            try
            {
                int doctorId = 1; // Mock session ID
                string hashedPwd;

                // ONP400 Requirement: Passwords must be hashed in the database
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(newPass));
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        builder.Append(bytes[i].ToString("x2"));
                    }
                    hashedPwd = builder.ToString();
                }

                // Send the hash to the repository
                await _doctorRepo.UpdatePasswordAsync(doctorId, hashedPwd);

                return Json(new { success = true, message = "Security credentials securely hashed and updated." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Database Error: " + ex.Message });
            }
        }


        [HttpGet]
        public async Task<IActionResult> DownloadPdfReport(DateTime startDate, DateTime endDate)
        {
            try
            {
                int doctorId = 1; // Mock session ID
                var reportData = await _doctorRepo.GetDoctorReportAsync(doctorId, startDate, endDate);

                // 1. Build the HTML template dynamically in memory
                StringBuilder html = new StringBuilder();
                html.Append("<html><head><style>");
                html.Append("body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; padding: 20px; color: #333; }");
                html.Append("h2 { color: #071D37; border-bottom: 2px solid #071D37; padding-bottom: 10px; }");
                html.Append("table { width: 100%; border-collapse: collapse; margin-top: 20px; font-size: 12px; }");
                html.Append("th, td { border: 1px solid #ccc; padding: 8px; text-align: left; }");
                html.Append("th { background-color: #f8f9fa; color: #071D37; }");
                html.Append("tr:nth-child(even) { background-color: #fbfcfd; }");
                html.Append("</style></head><body>");

                html.Append("<h2>Biomedical Activity Requisition Log</h2>");
                html.Append($"<p><strong>Clinical Scope:</strong> {startDate:dd MMMM yyyy} to {endDate:dd MMMM yyyy}</p>");
                html.Append($"<p><strong>Generated:</strong> {DateTime.Now:dd MMM yyyy HH:mm} SAST</p>");

                if (reportData != null && reportData.Any())
                {
                    html.Append("<table><thead><tr><th>Ref #</th><th>Date</th><th>Patient</th><th>ID Number</th><th>Priority</th><th>Panels</th><th>Status</th></tr></thead><tbody>");
                    foreach (var row in reportData)
                    {
                        html.Append($"<tr><td>#{row.RequestNumber}</td><td>{row.RequestDate:yyyy-MM-dd}</td><td>{row.PatientName}</td><td>{row.IDNumber}</td><td>{row.Urgency}</td><td>{row.TestPanels}</td><td>{row.Status}</td></tr>");
                    }
                    html.Append("</tbody></table>");
                }
                else
                {
                    html.Append("<p style='text-align:center; padding: 30px;'>No diagnostic requisitions found for this timeframe.</p>");
                }
                html.Append("</body></html>");

                // 2. Convert the HTML directly to a PDF document
                SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();
                converter.Options.PdfPageSize = SelectPdf.PdfPageSize.A4;
                converter.Options.MarginLeft = 20;
                converter.Options.MarginRight = 20;
                converter.Options.MarginTop = 20;
                converter.Options.MarginBottom = 20;

                SelectPdf.PdfDocument doc = converter.ConvertHtmlString(html.ToString());

                // 3. Save to a byte array and trigger the physical file download
                byte[] pdfBytes;
                using (var stream = new System.IO.MemoryStream())
                {
                    doc.Save(stream);
                    pdfBytes = stream.ToArray();
                }
                doc.Close();

                string fileName = $"Diagnostic_Audit_{startDate:yyyyMMdd}_to_{endDate:yyyyMMdd}.pdf";

                // This physically downloads the file to the user's browser
                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                return Content("Error generating document: " + ex.Message);
            }
        }


        [HttpGet]
        public async Task<IActionResult> LoadDashboardSection(string section)
        {
            if (string.IsNullOrEmpty(section)) section = "alerts";
            section = section.ToLower();

            int doctorId = 1;

            switch (section)
            {
                case "alerts":
                    try
                    {
                        var metrics = await _doctorRepo.GetDashboardMetricsAsync(doctorId);
                        if (metrics != null)
                        {
                            ViewBag.TotalActiveRequests = metrics.TotalActiveRequests;
                            ViewBag.CriticalAlertsCount = metrics.CriticalAlertsCount;
                            ViewBag.PendingCollections = metrics.PendingCollections;
                            ViewBag.TurnaroundCompliance = metrics.TurnaroundCompliance;

                            ViewBag.SubmittedCount = metrics.SubmittedCount;
                            ViewBag.SamplesReceivedCount = metrics.SamplesReceivedCount;
                            ViewBag.InProgressCount = metrics.InProgressCount;
                            ViewBag.CompletedCount = metrics.CompletedCount;
                            ViewBag.ReleasedCount = metrics.ReleasedCount;
                            ViewBag.CancelledCount = metrics.CancelledCount;
                        }
                        else
                        {
                            ViewBag.TotalActiveRequests = 0;
                            ViewBag.CriticalAlertsCount = 0;
                            ViewBag.PendingCollections = 0;
                            ViewBag.TurnaroundCompliance = "0%";
                        }

                        var dbRecent = await _doctorRepo.GetRecentRequestsAsync(doctorId);
                        var dbAlerts = await _doctorRepo.GetAbnormalAlertsAsync(doctorId);

                        ViewBag.RecentRequests = dbRecent ?? new List<RecentRequestModel>();
                        return PartialView("~/Views/Doctor/_AbnormalAlertsPartial.cshtml", dbAlerts ?? new List<AbnormalAlertModel>());
                    }
                    catch
                    {
                        ViewBag.TotalActiveRequests = 0;
                        ViewBag.CriticalAlertsCount = 0;
                        ViewBag.PendingCollections = 0;
                        ViewBag.TurnaroundCompliance = "0%";
                        ViewBag.RecentRequests = new List<RecentRequestModel>();
                        return PartialView("~/Views/Doctor/_AbnormalAlertsPartial.cshtml", new List<AbnormalAlertModel>());
                    }

                case "managepatients":
                    try
                    {
                        var dbPatients = await _doctorRepo.GetManagePatientsAsync(doctorId);
                        return PartialView("~/Views/Doctor/_ManagePatientsPartial.cshtml", dbPatients ?? new List<PatientRecordModel>());
                    }
                    catch
                    {
                        return PartialView("~/Views/Doctor/_ManagePatientsPartial.cshtml", new List<PatientRecordModel>());
                    }

                case "new-request":
                case "createrequest":
                    try
                    {
                        var dbDropdown = await _doctorRepo.GetPatientsDropdownAsync();
                        if (dbDropdown != null)
                        {
                            // Explicitly map the database results to the model the HTML expects
                            ViewBag.PatientsList = dbDropdown.Select(x => new PatientDropdownModel
                            {
                                PatientID = (int)x.PatientID,
                                IdNumber = (string)x.IdNumber,
                                FullName = (string)x.FullName
                            }).ToList();
                        }
                        else
                        {
                            ViewBag.PatientsList = new List<PatientDropdownModel>();
                        }
                    }
                    catch
                    {
                        ViewBag.PatientsList = new List<PatientDropdownModel>();
                    }
                    return PartialView("~/Views/Doctor/_CreateRequestPartial.cshtml");

                case "trackprogress":
                    try
                    {
                        var dbProgress = await _doctorRepo.GetTrackProgressAsync(doctorId);
                        return PartialView("~/Views/Doctor/_TrackProgressPartial.cshtml", dbProgress ?? new List<TrackProgressModel>());
                    }
                    catch
                    {
                        return PartialView("~/Views/Doctor/_TrackProgressPartial.cshtml", new List<TrackProgressModel>());
                    }

                case "reports":
                    return PartialView("~/Views/Doctor/_ReportsPortalPartial.cshtml");

                case "profilesettings":
                    return PartialView("~/Views/Doctor/_ProfileSettingsPartial.cshtml");

                default:
                    return PartialView("~/Views/Doctor/_AbnormalAlertsPartial.cshtml");
            }
        }
    }
}
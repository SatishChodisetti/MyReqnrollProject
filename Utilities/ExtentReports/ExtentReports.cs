using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Config;
using MyReqnrollProject.Base;
//using iText.Html2pdf;
using System.IO;
using OpenQA.Selenium;
//using OpenQA.Selenium.DevTools.V136.HeadlessExperimental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reqnroll;


namespace MyReqnrollProject.Utilities.ExtentReport
{
    /// <summary>
    /// Class to handle extent reports
    /// </summary>
    public class ExtentReport
    {
        /// <summary>
        /// extent reports object declarations
        /// </summary>
        public static ExtentReports _extentReports = new ExtentReports();
        public static ExtentTest? _feature;
        public static ExtentTest? _scenario;
        private IWebDriver? _driver;
        public static String dir = AppDomain.CurrentDomain.BaseDirectory;
        public static String testResultPath = dir.Replace("bin/Debug/net8.0", "TestReports");

        /// <summary>
        /// To initialize extent report objects
        /// </summary>
        public static void ExtentReportInit()
        {
            if (_extentReports == null)
            {
                _extentReports = new ExtentReports();
            }

            var sparkReporter = new ExtentSparkReporter(testResultPath + "ExtentSparkReporter.html");
            sparkReporter.Config.ReportName = "Sentimant Analysis - Smoke Test Report";
            sparkReporter.Config.DocumentTitle = "Sentimant Analysis - Smoke Test Report";
            sparkReporter.Config.Theme = Theme.Dark;

            _extentReports = new ExtentReports();
            _extentReports.AttachReporter(sparkReporter);
            _extentReports.AddSystemInfo("Application", "Sentimant Analysis");
            _extentReports.AddSystemInfo("Browser", "Chrome");
            _extentReports.AddSystemInfo("OS", "Windows");
        }

        /// <summary>
        /// Method to perform teardown operaions
        /// </summary>
        public static void ExtentReportTearDown()
        {
            _extentReports?.Flush();
            //Coverting HTML to PDF 
            // var htmlFilePath = Path.Combine(testResultPath, "ExtentSparkReporter.html");
            // var pdfFilePath = Path.Combine(testResultPath, "ExtentSparkReporter.pdf");

            // try
            // {
            //     if (!File.Exists(htmlFilePath))
            //     {
            //         Console.WriteLine("HTML File not found at:" + htmlFilePath);
            //         Directory.CreateDirectory(Path.GetDirectoryName(pdfFilePath));
            //         return;
            //     }
            //     string htmlContent = File.ReadAllText(htmlFilePath, Encoding.UTF8);
            //     // Check if the HTML file is not empty
            //     // var htmlContent = File.ReadAllText(htmlFilePath);
            //     if (string.IsNullOrWhiteSpace(htmlContent))
            //     {
            //         Console.WriteLine("HTML content is empty.");
            //         return;
            //     }
            //     Directory.CreateDirectory(Path.GetDirectoryName(pdfFilePath));

            //     using (FileStream htmlSource = File.Open(htmlFilePath, FileMode.Open, FileAccess.Read))
            //     using (FileStream pdfDest = File.Open(pdfFilePath, FileMode.Create, FileAccess.Write))
            //     {
            //         // HtmlConverter.ConvertToPdf(htmlSource, pdfDest);
            //     }
            //     // Using StringReader for converting HTML to PDF
            //     //using (MemoryStream pdfDest = new MemoryStream())
            //     //{
            //     //    using (StringReader sr = new StringReader(htmlContent))
            //     //    {
            //     //        HtmlConverter.ConvertToPdf(sr, pdfDest);
            //     //    }

            //     //    // Save the converted PDF to a file
            //     //    File.WriteAllBytes(pdfFilePath, pdfDest.ToArray());
            //     //}
            //     Console.WriteLine("Pdf Generated successfully at:" + pdfFilePath);
            // }
            // catch (Exception ex)
            // {
            //     Console.WriteLine(ex.Message);
            // }
        }

        /// <summary>
        /// Method to add screen shot to steps
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="scenarioContext"></param>
        /// <returns></returns>
        public static string addScreenshot(IWebDriver? _driver, ScenarioContext? _scenarioContext)
        {
            string errorfileName = string.Format("error_{0}_{1}", _scenarioContext.ScenarioInfo.Title, DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            ITakesScreenshot? iTakesScreenshot = (ITakesScreenshot)_driver;
            Screenshot screenshot = iTakesScreenshot.GetScreenshot();
            string screenshotLocation = Path.Combine(testResultPath, errorfileName + ".png");
            screenshot.SaveAsFile(screenshotLocation);
            return screenshotLocation;
        }


    }
}

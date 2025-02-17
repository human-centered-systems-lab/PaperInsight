using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PaperInsight
{
    public static class Extensions
    {
        /// <summary>
        ///     Joins two Lists. Will work with null.
        /// </summary>
        /// <typeparam name="T">Type of List elements</typeparam>
        /// <param name="first">first list or null</param>
        /// <param name="second">second list or null</param>
        /// <returns>List of type T containing all elements from First and second in order</returns>
        public static List<T> Join<T>(this List<T>? first, List<T> second)
        {
            if (first is null) return second;
            if (second is null) return first;

            return first.Concat(second).ToList();
        }

        private const string CSV_SEPARATOR = ";";

        public static string PaperText { get; private set; }

        /// <summary>
        ///     Seperates all elememets of array with given seperator
        /// </summary>
        /// <param name="array">array of elements</param>
        /// <param name="separator">string used as separator</param>
        /// <returns>string containing all elemets separated by the separator</returns>
        public static string ToCSVString<T>(this T[] array)
        {
            return string.Join(CSV_SEPARATOR, array);
        }
        /// <summary>
        ///     Seperates all elememets of array with given seperator
        /// </summary>
        /// <param name="array">2D nested array of elements</param>
        /// <param name="separator">string used as separator</param>
        /// <returns>string containing all elemets separated by the separator</returns>
        public static string ToCSVString<T>(this T[,] array)
        {
            var one = new List<T>();
            foreach (var item in array)
            {
                one.Add(item);
            };
            return one.ToArray().ToCSVString();
        }

        public static async Task loadPaperTextHTMLAsync(string URL)
        {
            PaperText = string.Empty;
            PaperText = await CrawlWebPageAsync(URL);
            //PaperText = await CrawlWebPageAsync("https://dl.acm.org/doi/fullHtml/10.1145/3563359.3597444");
            //System.Windows.MessageBox.Show(PaperText);
            string content = PaperText;
            string filePath = @"C:\Users\iq4341\Downloads\test.txt";

            try
            {
                File.WriteAllText(filePath, content);
                Console.WriteLine("String has been written to the file.");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An error occurred while writing to the file: {ex.Message}");
            }
        }


        public static async Task<string> CrawlWebPageAsync(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Fetch the HTML content from the webpage
                    string html = await client.GetStringAsync(url);

                    // Create an HtmlDocument instance and load the HTML content
                    HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
                    htmlDocument.LoadHtml(html);

                    // Extract the text content
                    string text = htmlDocument.DocumentNode.InnerText;

                    // Return the extracted text
                    return text;
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the crawling process
                Console.WriteLine("Error while crawling webpage: " + ex.Message);
                return string.Empty;
            }
        }
        public static void loadPaperTextPDF(string path)
        {
            PaperText = string.Empty;

            using (PdfReader pdfReader = new PdfReader(path))
            {
                using (PdfDocument pdfDocument = new PdfDocument(pdfReader))
                {
                    for (int page = 1; page <= pdfDocument.GetNumberOfPages(); page++)
                    {
                        StringBuilder sb = new StringBuilder();
                        ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                        PdfCanvasProcessor parser = new PdfCanvasProcessor(strategy);

                        // Process the content of the page and extract text
                        parser.ProcessPageContent(pdfDocument.GetPage(page));
                        sb.AppendLine(strategy.GetResultantText());

                        PaperText += sb.ToString();
                    }
                }
            }
        }
    }
}

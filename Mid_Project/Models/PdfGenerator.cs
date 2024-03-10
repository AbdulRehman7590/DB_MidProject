using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Windows;

namespace Mid_Project.Models
{
    public static class PDFGenerator
    {
        public static void GeneratePDF(string sqlQuery, string title)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (sender, e) =>
            {
                try
                {
                    DataTable dt = GetDataTableFromSql(sqlQuery);

                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
                    saveFileDialog.Title = "Save PDF File";
                    saveFileDialog.FileName = $"{title} At {DateTime.Now:yyyy-MM-dd-HH=mm-ss}.pdf";

                    bool? dialogResult = saveFileDialog.ShowDialog();

                    if (dialogResult == true)
                    {
                        if (!string.IsNullOrWhiteSpace(saveFileDialog.FileName))
                        {
                            Document document = new Document(PageSize.A4);
                            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(saveFileDialog.FileName, FileMode.Create));
                            document.Open();
                            AddTitle(document, title);
                            AddDataTable(document, dt);
                            document.Close();

                            MessageBox.Show("PDF created successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Invalid file path. Please provide a valid file path.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error in creating PDF: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };
            worker.RunWorkerAsync();
        }

        private static DataTable GetDataTableFromSql(string sqlQuery)
        {
            try
            {
                using (var con = Configuration.getInstance().getConnection())
                {
                    SqlCommand cmd = new SqlCommand(sqlQuery, con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        private static void AddTitle(Document document, string title)
        {
            Paragraph titleParagraph = new Paragraph(title, FontFactory.GetFont(FontFactory.HELVETICA, 16, Font.BOLD));
            titleParagraph.Alignment = Element.ALIGN_CENTER;
            titleParagraph.SpacingAfter = 10f;
            document.Add(titleParagraph);
        }

        private static void AddDataTable(Document document, DataTable dt)
        {
            Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.BOLD);
            Font regularFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);

            PdfPTable table = new PdfPTable(dt.Columns.Count);
            float[] widths = new float[dt.Columns.Count];
            for (int i = 0; i < dt.Columns.Count; i++)
                widths[i] = 4f;

            table.SetWidths(widths);
            table.WidthPercentage = 100;

            foreach (DataColumn c in dt.Columns)
            {
                PdfPCell headerCell = new PdfPCell(new Phrase(c.ColumnName, boldFont));
                headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(headerCell);
            }

            foreach (DataRow r in dt.Rows)
            {
                for (int h = 0; h < dt.Columns.Count; h++)
                {
                    table.AddCell(new Phrase(r[h].ToString(), regularFont));
                }
            }

            document.Add(table);
        }
    }
}
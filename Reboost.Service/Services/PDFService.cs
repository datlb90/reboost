using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Reboost.Service.Services
{
    public interface IPDFService
    {
        byte[] WriteParagraph(string paragraph);
    }
    public class PDFService: IPDFService
    {
        public byte[] WriteParagraph(string paragraph) {
            using (var ms = new MemoryStream())
            using (Document doc = new Document(PageSize.A4, 25, 25, 30, 30))
            using (PdfWriter writer = PdfWriter.GetInstance(doc, ms))
            {
                doc.Open();
                doc.Add(new Paragraph(paragraph));
                doc.Close();
                writer.Close();

                var bytes = ms.ToArray();
                return bytes;
            }
        }
    }
}

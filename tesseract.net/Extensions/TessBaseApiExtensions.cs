namespace Tesseract.Extensions
{
    public static class TessBaseApiExtensions
    {
        public static ResultIterator Process(this TessBaseAPI tessBaseAPI, string inputFile, bool createPdf = false)
        {
            tessBaseAPI.SetPageSegMode(PageSegmentationMode.AUTO);
            var pix = tessBaseAPI.SetImage(inputFile);
            tessBaseAPI.Recognize();

            //if create pdf export pdf
            if (createPdf)
            {
                //ensure input name is set
                tessBaseAPI.SetInputName(inputFile);

                var fileInfo = new System.IO.FileInfo(inputFile);
                string tessDataPath = string.Format("{0}", tessBaseAPI.GetDatapath());
                string outputName = fileInfo.FullName.Replace(fileInfo.Extension, string.Empty); //input name.pdf

                // ensure the data directory exist
                if (!System.IO.Directory.Exists(tessDataPath))
                {
                    throw new System.IO.DirectoryNotFoundException(string.Format("tessData path {0} does not exist", tessDataPath));
                }

                // call pdf renderer and export pdf
                using (var pdfRenderer = new PdfRenderer(outputName, tessDataPath, false))
                {
                    pdfRenderer.BeginDocument("tesseract.net searchable Pdf generation");
                    pdfRenderer.AddImage(tessBaseAPI);
                    pdfRenderer.EndDocument();
                }
            }

            pix.Dispose();
            return tessBaseAPI.GetIterator();
        }
    }
}

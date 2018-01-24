using Leptonica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace tesseract.net.console
{
    class Program
    {
        public static void Main(string[] args)
        {
            TessBaseAPI tessBaseAPI = new TessBaseAPI();

            System.Console.WriteLine("The current version is {0}", tessBaseAPI.GetVersion());

            System.Console.WriteLine("Press any key to quit...");
            System.Console.ReadKey();
        }
          
        static void example1()
        {
            string dataPath = "./tessdata/";
            string language = "eng";
            OcrEngineMode oem = OcrEngineMode.DEFAULT;
            PageSegmentationMode psm = PageSegmentationMode.AUTO_OSD;

            TessBaseAPI tessBaseAPI = new TessBaseAPI();

            // Initialize tesseract-ocr 
            if (!tessBaseAPI.Init(dataPath, language, oem))
            {
                throw new Exception("Could not initialize tesseract.");
            }

            // Set the Page Segmentation mode
            tessBaseAPI.SetPageSegMode(psm);
        }

        static void example2()
        {
            string dataPath = "./tessdata/";
            string language = "eng";
            OcrEngineMode oem = OcrEngineMode.DEFAULT;
            PageSegmentationMode psm = PageSegmentationMode.AUTO_OSD;

            TessBaseAPI tessBaseAPI = new TessBaseAPI(dataPath, language, oem, psm);
        }

        static void example3()
        {
            string dataPath = "./tessdata/";
            string language = "eng";
            string inputFile = "./input.png";
            OcrEngineMode oem = OcrEngineMode.DEFAULT;
            PageSegmentationMode psm = PageSegmentationMode.AUTO_OSD;

            TessBaseAPI tessBaseAPI = new TessBaseAPI();

            // Initialize tesseract-ocr 
            if (!tessBaseAPI.Init(dataPath, language, oem))
            {
                throw new Exception("Could not initialize tesseract.");
            }

            // Set the Page Segmentation mode
            tessBaseAPI.SetPageSegMode(psm);

            // Set the input image
            Pix pix = tessBaseAPI.SetImage(inputFile);

            // Recognize image
            tessBaseAPI.Recognize();

            ResultIterator resultIterator = tessBaseAPI.GetIterator();

            // extract text from result iterator
            StringBuilder stringBuilder = new StringBuilder();
            PageIteratorLevel pageIteratorLevel = PageIteratorLevel.RIL_PARA;
            do
            {
                stringBuilder.Append(resultIterator.GetUTF8Text(pageIteratorLevel));
            } while (resultIterator.Next(pageIteratorLevel));

            tessBaseAPI.Dispose();
            pix.Dispose();
        }

        static void example4()
        {
            string dataPath = "./tessdata/";
            string language = "eng";
            string inputFile = "./input.png";
            OcrEngineMode oem = OcrEngineMode.DEFAULT;
            PageSegmentationMode psm = PageSegmentationMode.AUTO_OSD;

            TessBaseAPI tessBaseAPI = new TessBaseAPI();

            // Initialize tesseract-ocr 
            if (!tessBaseAPI.Init(dataPath, language, oem))
            {
                throw new Exception("Could not initialize tesseract.");
            }

            // Set the Page Segmentation mode
            tessBaseAPI.SetPageSegMode(psm);

            // Set the input image
            Pix pix = tessBaseAPI.SetImage(inputFile);

            // Recognize image
            tessBaseAPI.Recognize();

            //ensure input name is set
            tessBaseAPI.SetInputName(inputFile);

            var fileInfo = new System.IO.FileInfo(inputFile);
            string tessDataPath = tessBaseAPI.GetDatapath();
            string outputName = fileInfo.FullName.Replace(fileInfo.Extension, string.Empty); //input name.pdf
             
            // call pdf renderer and export pdf
            using (var pdfRenderer = new PdfRenderer(outputName, tessDataPath, false))
            {
                pdfRenderer.BeginDocument("tesseract.net searchable Pdf generation");
                pdfRenderer.AddImage(tessBaseAPI);
                pdfRenderer.EndDocument();
            }

            tessBaseAPI.Dispose();
            pix.Dispose();
        }
    }
}

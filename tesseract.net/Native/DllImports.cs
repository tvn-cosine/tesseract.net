using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Tesseract.Native
{
    public class DllImports
    {
        private const string pattern = @"pvt.cppan.demo.";
        private const string x64 = @"x64";
        private const string x86 = @"x86";
        private const string tesseractDllName = "pvt.cppan.demo.google.tesseract.libtesseract400.dll";

        static DllImports()
        {
            if (string.IsNullOrWhiteSpace(TesseractDirectory))
            {
                TesseractDirectory = Environment.CurrentDirectory;
            }
            CopyDlls();
        }

        private static string tesseractDirectory;
        public static string TesseractDirectory
        {
            get
            {
                return tesseractDirectory;
            }
            set
            {
                if (value != tesseractDirectory)
                {
                    if (Directory.Exists(value))
                    {
                        tesseractDirectory = value;
                        CopyDlls();
                    }
                }
            }
        }

        public static void CopyDlls()
        {
            string directory = string.Format("{0}\\{1}", TesseractDirectory, x86);

            if (Architecture.Is64BitProcess)
            {
                directory = string.Format("{0}\\{1}", TesseractDirectory, x64);
            }

            if (Directory.Exists(directory))
            {
                foreach (string file in Directory.GetFiles(directory))
                {
                    FileInfo fi = new FileInfo(file);
                    if (fi.Name.StartsWith(pattern)) // must copy
                    {
                        string newLocation = string.Format("{0}\\{1}",
                                                    Environment.CurrentDirectory,
                                                    fi.Name);
                        if (!File.Exists(newLocation))
                        {
                            File.Copy(file, newLocation, true);
                        }
                    }
                }
            }
        }
         
        // General free functions 
        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessVersion")]
        internal static extern IntPtr TessVersion();

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessDeleteText")]
        internal static extern void TessDeleteText(IntPtr text);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessDeleteTextArray")]
        internal static extern void TessDeleteTextArray(ref string[] arr);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessDeleteIntArray")]
        internal static extern void TessDeleteIntArray(ref int[] arr);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessDeleteBlockList")]
        internal static extern void TessDeleteBlockList(HandleRef block_list);

        // Renderer API 
        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessTextRendererCreate")]
        internal static extern IntPtr TessTextRendererCreate([MarshalAs(UnmanagedType.AnsiBStr)]string outputbase);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessHOcrRendererCreate")]
        internal static extern IntPtr TessHOcrRendererCreate([MarshalAs(UnmanagedType.AnsiBStr)] string outputbase);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessHOcrRendererCreate2")]
        internal static extern IntPtr TessHOcrRendererCreate2([MarshalAs(UnmanagedType.AnsiBStr)] string outputbase, int font_info);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPDFRendererCreate")]
        internal static extern IntPtr TessPDFRendererCreate([MarshalAs(UnmanagedType.AnsiBStr)] string outputbase, [MarshalAs(UnmanagedType.AnsiBStr)] string datadir, int textonly);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessUnlvRendererCreate")]
        internal static extern IntPtr TessUnlvRendererCreate([MarshalAs(UnmanagedType.AnsiBStr)] string outputbase);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBoxTextRendererCreate")]
        internal static extern IntPtr TessBoxTextRendererCreate([MarshalAs(UnmanagedType.AnsiBStr)] string outputbase);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessDeleteResultRenderer")]
        internal static extern void TessDeleteResultRenderer(IntPtr renderer);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultRendererInsert")]
        internal static extern void TessResultRendererInsert(HandleRef renderer, IntPtr next);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultRendererNext")]
        internal static extern IntPtr TessResultRendererNext(HandleRef renderer);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultRendererBeginDocument")]
        internal static extern int TessResultRendererBeginDocument(HandleRef renderer, [MarshalAs(UnmanagedType.LPStr)] string title);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultRendererAddImage")]
        internal static extern int TessResultRendererAddImage(HandleRef renderer, HandleRef api);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultRendererEndDocument")]
        internal static extern int TessResultRendererEndDocument(HandleRef renderer);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultRendererExtention")]
        internal static extern IntPtr TessResultRendererExtention(HandleRef renderer);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultRendererTitle")]
        internal static extern IntPtr TessResultRendererTitle(HandleRef renderer);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultRendererImageNum")]
        internal static extern int TessResultRendererImageNum(HandleRef renderer);

        // Base API   
        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPICreate")]
        internal static extern IntPtr TessBaseAPICreate();

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIDelete")]
        internal static extern void TessBaseAPIDelete(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetOpenCLDevice")]
        internal static extern UIntPtr TessBaseAPIGetOpenCLDevice(HandleRef handle, IntPtr device);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetInputName")]
        internal static extern void TessBaseAPISetInputName(HandleRef handle, [MarshalAs(UnmanagedType.AnsiBStr)] string name);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetInputName")]
        internal static extern IntPtr TessBaseAPIGetInputName(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetInputImage")]
        internal static extern void TessBaseAPISetInputImage(HandleRef handle, IntPtr pix);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetInputImage")]
        internal static extern IntPtr TessBaseAPIGetInputImage(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetSourceYResolution")]
        internal static extern int TessBaseAPIGetSourceYResolution(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetDatapath")]
        internal static extern IntPtr TessBaseAPIGetDatapath(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetOutputName")]
        internal static extern void TessBaseAPISetOutputName(HandleRef handle, [MarshalAs(UnmanagedType.AnsiBStr)] string name);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetVariable")]
        internal static extern int TessBaseAPISetVariable(HandleRef handle, [MarshalAs(UnmanagedType.AnsiBStr)] string name, [MarshalAs(UnmanagedType.AnsiBStr)] string value);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetDebugVariable")]
        internal static extern int TessBaseAPISetDebugVariable(HandleRef handle, [MarshalAs(UnmanagedType.AnsiBStr)] string name, [MarshalAs(UnmanagedType.AnsiBStr)] string value);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetIntVariable")]
        internal static extern int TessBaseAPIGetIntVariable(HandleRef handle, [MarshalAs(UnmanagedType.AnsiBStr)] string name, out int value);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetBoolVariable")]
        internal static extern int TessBaseAPIGetBoolVariable(HandleRef handle, [MarshalAs(UnmanagedType.AnsiBStr)] string name, out int value);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetDoubleVariable")]
        internal static extern int TessBaseAPIGetDoubleVariable(HandleRef handle, [MarshalAs(UnmanagedType.AnsiBStr)] string name, out double value);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetStringVariable")]
        internal static extern IntPtr TessBaseAPIGetStringVariable(HandleRef handle, [MarshalAs(UnmanagedType.AnsiBStr)] string name);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIPrintVariables")]
        internal static extern void TessBaseAPIPrintVariables(HandleRef handle, UIntPtr fp);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIPrintVariablesToFile")]
        internal static extern int TessBaseAPIPrintVariablesToFile(HandleRef handle, IntPtr filename);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetVariableAsString")]
        internal static extern int TessBaseAPIGetVariableAsString(HandleRef handle, IntPtr name, IntPtr val);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIInit")]
        internal static extern int TessBaseAPIInit(HandleRef tessBasiApiHandle, [MarshalAs(UnmanagedType.AnsiBStr)]string datapath, [MarshalAs(UnmanagedType.AnsiBStr)]string language,
            OcrEngineMode mode, string[] configs, int configs_size, string[] vars_vec, UIntPtr vars_vec_size,
            string[] vars_values, UIntPtr vars_values_size, int set_only_init_params);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIInit1")]
        internal static extern int TessBaseAPIInit1(HandleRef tessBasiApiHandle, [MarshalAs(UnmanagedType.AnsiBStr)] string datapath, [MarshalAs(UnmanagedType.AnsiBStr)] string language,
            OcrEngineMode oem, string[] configs, int configs_size);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIInit2")]
        internal static extern int TessBaseAPIInit2(HandleRef tessBasiApiHandle, [MarshalAs(UnmanagedType.AnsiBStr)] string datapath, [MarshalAs(UnmanagedType.AnsiBStr)] string language,
            OcrEngineMode oem);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIInit3")]
        internal static extern int TessBaseAPIInit3(HandleRef tessBasiApiHandle, [MarshalAs(UnmanagedType.AnsiBStr)] string datapath, [MarshalAs(UnmanagedType.AnsiBStr)] string language);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIInit4")]
        internal static extern int TessBaseAPIInit4(HandleRef tessBasiApiHandle, [MarshalAs(UnmanagedType.AnsiBStr)] string datapath, [MarshalAs(UnmanagedType.AnsiBStr)] string language,
                                    OcrEngineMode mode, string[] configs, int configs_size,
                                    string[] vars_vec, string[] vars_values, UIntPtr vars_vec_size,
                                    int set_only_non_debug_params);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetInitLanguagesAsString")]
        internal static extern IntPtr TessBaseAPIGetInitLanguagesAsString(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetLoadedLanguagesAsVector")]
        internal static extern IntPtr TessBaseAPIGetLoadedLanguagesAsVector(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetAvailableLanguagesAsVector")]
        internal static extern IntPtr TessBaseAPIGetAvailableLanguagesAsVector(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIInitLangMod")]
        internal static extern int TessBaseAPIInitLangMod(HandleRef handle, [MarshalAs(UnmanagedType.AnsiBStr)] string datapath, [MarshalAs(UnmanagedType.AnsiBStr)] string language);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIInitForAnalysePage")]
        internal static extern void TessBaseAPIInitForAnalysePage(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIReadConfigFile")]
        internal static extern void TessBaseAPIReadConfigFile(HandleRef handle, [MarshalAs(UnmanagedType.AnsiBStr)] string filename);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIReadDebugConfigFile")]
        internal static extern void TessBaseAPIReadDebugConfigFile(HandleRef handle, [MarshalAs(UnmanagedType.AnsiBStr)] string filename);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetPageSegMode")]
        internal static extern void TessBaseAPISetPageSegMode(HandleRef handle, PageSegmentationMode mode);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetPageSegMode")]
        internal static extern PageSegmentationMode TessBaseAPIGetPageSegMode(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIRect")]
        internal static extern IntPtr TessBaseAPIRect(HandleRef handle, UIntPtr imagedata, int bytes_per_pixel, int bytes_per_line, int left, int top, int width, int height);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIClearAdaptiveClassifier")]
        internal static extern void TessBaseAPIClearAdaptiveClassifier(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetImage")]
        internal static extern void TessBaseAPISetImage(HandleRef handle, UIntPtr imagedata, int width, int height, int bytes_per_pixel, int bytes_per_line);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetImage2")]
        internal static extern void TessBaseAPISetImage2(HandleRef handle, HandleRef pix);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetSourceResolution")]
        internal static extern void TessBaseAPISetSourceResolution(HandleRef handle, int ppi);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetRectangle")]
        internal static extern void TessBaseAPISetRectangle(HandleRef handle, int left, int top, int width, int height);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetThresholder")]
        internal static extern void TessBaseAPISetThresholder(HandleRef handle, IntPtr thresholder);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetThresholdedImage")]
        internal static extern IntPtr TessBaseAPIGetThresholdedImage(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetRegions")]
        internal static extern IntPtr TessBaseAPIGetRegions(HandleRef handle, out IntPtr pixa);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetTextlines")]
        internal static extern IntPtr TessBaseAPIGetTextlines(HandleRef handle, out IntPtr pixa, out int[] blockids);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetTextlines1")]
        internal static extern IntPtr TessBaseAPIGetTextlines1(HandleRef handle, int raw_image, int raw_padding, out IntPtr pixa, out int[] blockids, out int[] paraids);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetStrips")]
        internal static extern IntPtr TessBaseAPIGetStrips(HandleRef handle, out IntPtr pixa, out int[] blockids);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetWords")]
        internal static extern IntPtr TessBaseAPIGetWords(HandleRef handle, out IntPtr pixa);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetConnectedComponents")]
        internal static extern IntPtr TessBaseAPIGetConnectedComponents(HandleRef handle, IntPtr cc);
        //internal static extern IntPtr TessBaseAPIGetConnectedComponents(HandleRef handle, out Pixa cc); //commented out deliberately, must test 

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetComponentImages")]
        internal static extern IntPtr TessBaseAPIGetComponentImages(HandleRef handle, PageIteratorLevel level,
            int text_only, IntPtr pixa, IntPtr blockids);
        //internal static extern IntPtr TessBaseAPIGetComponentImages(HandleRef handle, PageIteratorLevel level, int text_only, out Pixa pixa, out int[] blockids); //commented out deliberately, must test 

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetComponentImages1")]
        internal static extern IntPtr TessBaseAPIGetComponentImages1(HandleRef handle, PageIteratorLevel level, int text_only, int raw_image, int raw_padding, IntPtr pixa, IntPtr blockids, IntPtr paraids);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetThresholdedImageScaleFactor")]
        internal static extern int TessBaseAPIGetThresholdedImageScaleFactor(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIDumpPGM")]
        internal static extern void TessBaseAPIDumpPGM(HandleRef handle, [MarshalAs(UnmanagedType.AnsiBStr)] string filename);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIAnalyseLayout")]
        internal static extern IntPtr TessBaseAPIAnalyseLayout(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIRecognize")]
        internal static extern int TessBaseAPIRecognize(HandleRef handle, IntPtr monitor);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIRecognizeForChopTest")]
        internal static extern int TessBaseAPIRecognizeForChopTest(HandleRef handle, IntPtr monitor);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIProcessPages")]
        internal static extern int TessBaseAPIProcessPages(HandleRef handle, IntPtr filename, IntPtr retry_config, int timeout_millisec, IntPtr renderer);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIProcessPage")]
        internal static extern int TessBaseAPIProcessPage(HandleRef handle, IntPtr pix, int page_index, IntPtr filename, IntPtr retry_config, int timeout_millisec, IntPtr renderer);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetIterator")]
        internal static extern IntPtr TessBaseAPIGetIterator(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetMutableIterator")]
        internal static extern IntPtr TessBaseAPIGetMutableIterator(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetUTF8Text")]
        internal static extern IntPtr TessBaseAPIGetUTF8Text(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetHOCRText")]
        internal static extern IntPtr TessBaseAPIGetHOCRText(HandleRef handle, int page_number);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetBoxText")]
        internal static extern IntPtr TessBaseAPIGetBoxText(HandleRef handle, int page_number);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetUNLVText")]
        internal static extern IntPtr TessBaseAPIGetUNLVText(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIMeanTextConf")]
        internal static extern int TessBaseAPIMeanTextConf(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIAllWordConfidences")]
        internal static extern IntPtr TessBaseAPIAllWordConfidences(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIAdaptToWordStr")]
        internal static extern int TessBaseAPIAdaptToWordStr(HandleRef handle, PageSegmentationMode mode, IntPtr wordstr);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIClear")]
        internal static extern void TessBaseAPIClear(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIEnd")]
        internal static extern void TessBaseAPIEnd(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIIsValidWord")]
        internal static extern int TessBaseAPIIsValidWord(HandleRef handle, [MarshalAs(UnmanagedType.AnsiBStr)] string word);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetTextDirection")]
        internal static extern int TessBaseAPIGetTextDirection(HandleRef handle, out int out_offset, out float out_slope);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIClearPersistentCache")]
        internal static extern void TessBaseAPIClearPersistentCache(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIDetectOrientationScript")]
        internal static extern int TessBaseAPIDetectOrientationScript(HandleRef handle, out int orient_deg, out float orient_conf, [MarshalAs(UnmanagedType.AnsiBStr)] out string script_name, out float script_conf);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetFeaturesForBlob")]
        internal static extern void TessBaseAPIGetFeaturesForBlob(HandleRef handle, IntPtr blob, IntPtr int_features, IntPtr num_features, IntPtr FeatureOutlineIndex);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessFindRowForBox")]
        internal static extern IntPtr TessFindRowForBox(HandleRef blocks, int left, int top, int right, int bottom);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIRunAdaptiveClassifier")]
        internal static extern void TessBaseAPIRunAdaptiveClassifier(HandleRef handle, IntPtr blob, int num_max_matches, IntPtr unichar_ids, IntPtr ratings, IntPtr num_matches_returned);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetUnichar")]
        internal static extern IntPtr TessBaseAPIGetUnichar(HandleRef handle, int unichar_id);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIGetDawg")]
        internal static extern IntPtr TessBaseAPIGetDawg(HandleRef handle, int i);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPINumDawgs")]
        internal static extern int TessBaseAPINumDawgs(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessMakeTessOCRRow")]
        internal static extern IntPtr TessMakeTessOCRRow(float baseline, float xheight, float descender, float ascender);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessMakeTBLOB")]
        internal static extern IntPtr TessMakeTBLOB(HandleRef pix);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessNormalizeTBLOB")]
        internal static extern void TessNormalizeTBLOB(HandleRef tblob, IntPtr row, int numeric_mode);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIOem")]
        internal static extern OcrEngineMode TessBaseAPIOem(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIInitTruthCallback")]
        internal static extern void TessBaseAPIInitTruthCallback(HandleRef handle, IntPtr cb);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPISetMinOrientationMargin")]
        internal static extern void TessBaseAPISetMinOrientationMargin(HandleRef handle, double margin);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseGetBlockTextOrientations")]
        internal static extern void TessBaseGetBlockTextOrientations(HandleRef handle, IntPtr block_orientation, IntPtr vertical_writing);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessBaseAPIFindLinesCreateBlockList")]
        internal static extern IntPtr TessBaseAPIFindLinesCreateBlockList(HandleRef handle);

        // Page iterator 

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorDelete")]
        internal static extern void TessPageIteratorDelete(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorCopy")]
        internal static extern IntPtr TessPageIteratorCopy(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorBegin")]
        internal static extern void TessPageIteratorBegin(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorNext")]
        internal static extern int TessPageIteratorNext(HandleRef handle, PageIteratorLevel level);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorIsAtBeginningOf")]
        internal static extern int TessPageIteratorIsAtBeginningOf(HandleRef handle, PageIteratorLevel level);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorIsAtFinalElement")]
        internal static extern int TessPageIteratorIsAtFinalElement(HandleRef handle, PageIteratorLevel level, PageIteratorLevel element);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorBoundingBox")]
        internal static extern int TessPageIteratorBoundingBox(HandleRef handle, PageIteratorLevel level, out int left, out int top, out int right, out int bottom);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorBlockType")]
        internal static extern PolyBlockType TessPageIteratorBlockType(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorGetBinaryImage")]
        internal static extern IntPtr TessPageIteratorGetBinaryImage(HandleRef handle, PageIteratorLevel level);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorGetImage")]
        internal static extern IntPtr TessPageIteratorGetImage(HandleRef handle, PageIteratorLevel level, int padding, HandleRef original_image, out int left, out int top);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorBaseline")]
        internal static extern int TessPageIteratorBaseline(HandleRef handle, PageIteratorLevel level, out int x1, out int y1, out int x2, out int y2);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorOrientation")]
        internal static extern void TessPageIteratorOrientation(HandleRef handle, out Orientation orientation, out WritingDirection writing_direction, out TextlineOrder textline_order, out float deskew_angle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessPageIteratorParagraphInfo")]
        internal static extern void TessPageIteratorParagraphInfo(HandleRef handle, out ParagraphJustification justification, out int is_list_item, out int is_crown, out int first_line_indent);
        // Result iterator 

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorDelete")]
        internal static extern void TessResultIteratorDelete(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorCopy")]
        internal static extern IntPtr TessResultIteratorCopy(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorGetPageIterator")]
        internal static extern IntPtr TessResultIteratorGetPageIterator(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorGetPageIteratorConst")]
        internal static extern IntPtr TessResultIteratorGetPageIteratorConst(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorGetChoiceIterator")]
        internal static extern IntPtr TessResultIteratorGetChoiceIterator(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorNext")]
        internal static extern int TessResultIteratorNext(HandleRef handle, PageIteratorLevel level);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorGetUTF8Text")]
        internal static extern IntPtr TessResultIteratorGetUTF8Text(HandleRef handle, PageIteratorLevel level);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorConfidence")]
        internal static extern float TessResultIteratorConfidence(HandleRef handle, PageIteratorLevel level);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorWordRecognitionLanguage")]
        internal static extern IntPtr TessResultIteratorWordRecognitionLanguage(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorWordFontAttributes")]
        internal static extern IntPtr TessResultIteratorWordFontAttributes(HandleRef handle, out int is_bold, out int is_italic, out int is_underlined,
            out int is_monospace, out int is_serif, out int is_smallcaps, out int pointsize, out int font_id);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorWordIsFromDictionary")]
        internal static extern int TessResultIteratorWordIsFromDictionary(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorWordIsNumeric")]
        internal static extern int TessResultIteratorWordIsNumeric(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorSymbolIsSuperscript")]
        internal static extern int TessResultIteratorSymbolIsSuperscript(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorSymbolIsSubscript")]
        internal static extern int TessResultIteratorSymbolIsSubscript(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessResultIteratorSymbolIsDropcap")]
        internal static extern int TessResultIteratorSymbolIsDropcap(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessChoiceIteratorDelete")]
        internal static extern void TessChoiceIteratorDelete(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessChoiceIteratorNext")]
        internal static extern int TessChoiceIteratorNext(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessChoiceIteratorGetUTF8Text")]
        internal static extern IntPtr TessChoiceIteratorGetUTF8Text(HandleRef handle);

        [DllImport(tesseractDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TessChoiceIteratorConfidence")]
        internal static extern float TessChoiceIteratorConfidence(HandleRef handle);
    }
}

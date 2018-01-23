using System;
using System.Runtime.InteropServices;
using Leptonica;
using Tesseract.Native;

namespace Tesseract
{
    /// <summary>
    /// Base class for all tesseract APIs.
    /// Specific classes can add ability to work on different inputs or produce
    /// different outputs.
    /// This class is mostly an interface layer on top of the Tesseract instance
    /// class to hide the data types so that users of this class don't have to
    /// include any other Tesseract headers.
    /// </summary>
    public class TessBaseAPI : IDisposable
    {
        private readonly string dataPath;
        private readonly string language;

        internal readonly HandleRef handleRef;

        private Pix localPix;

        internal Pix LocalPix
        {
            get
            {
                return localPix;
            }
            set
            {
                if (localPix != null)
                {
                    localPix.Dispose();
                }

                localPix = value;
            }
        }

        #region Ctors

        private TessBaseAPI()
        {
            handleRef = new HandleRef(this, Native.DllImports.TessBaseAPICreate());
        }

        public TessBaseAPI(string dataPath, string language = "eng", OcrEngineMode oem = OcrEngineMode.DEFAULT,
            PageSegmentationMode psm = PageSegmentationMode.AUTO_OSD)
            : this()
        {
            //ensure the data directory exist
            if (!System.IO.Directory.Exists(dataPath))
            {
                throw new System.IO.DirectoryNotFoundException("Datapath does not exist");
            }

            // Initialize tesseract-ocr with language and datapath and OEM_TESSERACT_CUBE_COMBINED
            if (!Init(dataPath, language, oem))
            {
                throw new Exception("Could not initialize tesseract.");
            }

            // Set the Page Segmentation mode
            SetPageSegMode(psm);

            // Set the local data path and language
            this.dataPath = dataPath;
            this.language = language;
        }

        #endregion Ctors

        public void DisposeImage()
        {
            if (LocalPix != null)
            {
                LocalPix.Dispose();
            }
            Clear();
            ClearPersistentCache();
            ClearAdaptiveClassifier();
        }

        public ResultIterator Process(string inputFile, bool createPdf = false)
        {
            SetPageSegMode(PageSegmentationMode.AUTO);
            SetInputImage(inputFile);
            Recognize();

            //if create pdf export pdf
            if (createPdf)
            {
                //ensure input name is set
                SetInputName(inputFile);

                var fileInfo = new System.IO.FileInfo(inputFile);
                string tessDataPath = string.Format("{0}", dataPath);
                string outputName = fileInfo.FullName.Replace(fileInfo.Extension, string.Empty); //input name.pdf

                // ensure the data directory exist
                if (!System.IO.Directory.Exists(tessDataPath))
                {
                    throw new System.IO.DirectoryNotFoundException("tessData Path does not exist");
                }

                // call pdf renderer and export pdf
                using (var pdfRenderer = new PdfRenderer(outputName, tessDataPath, false))
                {
                    pdfRenderer.BeginDocument("Newsclip Searchable PDF Generation");
                    pdfRenderer.AddImage(this);
                    pdfRenderer.EndDocument();
                }
            }

            return GetIterator();
        }

        #region Tesseract Methods

        /// <summary>
        ///  Returns the version identifier as a static string. Do not delete.
        /// </summary>
        public string GetVersion()
        {
            IntPtr pointer = Native.DllImports.TessVersion();
            if (pointer == null || pointer == IntPtr.Zero)
            {
                return null;
            }
            else
            {
                return Marshal.PtrToStringAnsi(pointer);
            }
        }

        /// <summary>
        /// Set the name of the input file.Needed for training and
        /// reading a UNLV zone file, and for searchable PDF output.
        /// </summary>
        public void SetInputName(string value)
        {
            Native.DllImports.TessBaseAPISetInputName(handleRef, value);
        }

        /// <summary>
        /// These functions are required for searchable PDF output.
        /// We need our hands on the input file so that we can include
        /// it in the PDF without transcoding.If that is not possible,
        /// we need the original image. Finally, resolution metadata
        /// is stored in the PDF so we need that as well.
        /// </summary>
        public string GetInputName()
        {
            IntPtr pointer = Native.DllImports.TessBaseAPIGetInputName(handleRef);
            if (pointer == null || pointer == IntPtr.Zero)
            {
                return null;
            }
            else
            {
                string returnObject = Marshal.PtrToStringAnsi(pointer);
                Native.DllImports.TessDeleteText(pointer);
                return returnObject;
            }
        }

        public string GetDatapath()
        {
            IntPtr pointer = Native.DllImports.TessBaseAPIGetDatapath(handleRef);
            if (pointer == null || pointer == IntPtr.Zero)
            {
                return null;
            }
            else
            {
                return Marshal.PtrToStringAnsi(pointer);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imagedata"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="bytes_per_pixel"></param>
        /// <param name="bytes_per_line"></param>
        public void SetImage(UIntPtr imagedata, int width, int height, int bytes_per_pixel, int bytes_per_line)
        {
            Clear();
            ClearPersistentCache();
            ClearAdaptiveClassifier();
            if (LocalPix != null)
            {
                LocalPix.Dispose();
            }
             
            Native.DllImports.TessBaseAPISetImage(handleRef, imagedata, width, height, bytes_per_pixel, bytes_per_line); 
        }

        /// <summary>
        /// Takes ownership of the input pix.
        /// </summary>
        public void SetInputImage(Pix value)
        {
            Clear();
            ClearPersistentCache();
            ClearAdaptiveClassifier();
            if (LocalPix != null)
            {
                LocalPix.Dispose();
            }

            LocalPix = value;
            Native.DllImports.TessBaseAPISetImage2(handleRef, (HandleRef)value);
        }

        /// <summary>
        /// Takes ownership of the input pix.
        /// </summary>
        public void SetInputImage(string inputFile)
        {
            //ensure the file exist
            if (!System.IO.File.Exists(inputFile))
            {
                throw new System.IO.FileNotFoundException("File does not exist");
            }

            //set the input image
            SetInputImage(Pix.Read(inputFile));
        }

        /// <summary>
        /// Takes ownership of the input pix.
        /// </summary>
        public Pix GetInputImage()
        {
            IntPtr pointer = Native.DllImports.TessBaseAPIGetInputImage(handleRef);

            if (IntPtr.Zero != pointer)
            {
                return new Pix(pointer);
            }

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        public int GetSourceYResolution()
        {
            return Native.DllImports.TessBaseAPIGetSourceYResolution(handleRef);
        }

        /// <summary>
        ///  Set the name of the bonus output files. Needed only for debugging.
        /// </summary>
        public void SetOutputName(string name)
        {
            Native.DllImports.TessBaseAPISetOutputName(handleRef, name);
        }

        /// <summary>
        /// Set the value of an internal "parameter."
        /// Supply the name of the parameter and the value as a string, just as
        /// you would in a config file.
        /// Returns false if the name lookup failed.
        /// Eg SetVariable("tessedit_char_blacklist", "xyz"); to ignore x, y and z.
        /// Or SetVariable("classify_bln_numeric_mode", "1"); to set numeric-only mode.
        /// SetVariable may be used before Init, but settings will revert to
        /// defaults on End().
        ///
        /// Note: Must be called after Init(). Only works for non-init variables
        /// (init variables should be passed to Init()).
        /// </summary>
        /// <returns></returns>
        public bool SetVariable(string name, string value)
        {
            return Native.DllImports.TessBaseAPISetVariable(handleRef, name, value) == 1 ? true : false;
        }

        public bool SetDebugVariable(string name, string value)
        {
            return Native.DllImports.TessBaseAPISetDebugVariable(handleRef, name, value) == 1 ? true : false;
        }

        /// <summary>
        /// Returns true if the parameter was found among Tesseract parameters.
        ///  Fills in value with the value of the parameter.
        /// </summary>
        public bool GetIntVariable(string name, out int value)
        {
            return Native.DllImports.TessBaseAPIGetIntVariable(handleRef, name, out value) == 1 ? true : false;
        }

        /// <summary>
        /// Returns true if the parameter was found among Tesseract parameters.
        ///  Fills in value with the value of the parameter.
        /// </summary>
        public bool GetBoolVariable(string name, out bool value)
        {
            //execute the dll import function.  it accepts int not bool
            int tempValue;
            var answer = Native.DllImports.TessBaseAPIGetBoolVariable(handleRef, name, out tempValue) == 1 ? true : false;
            //cast the int to bool
            value = tempValue == 1 ? true : false;
            //return whether function succeeded
            return answer;
        }

        /// <summary>
        /// Returns true if the parameter was found among Tesseract parameters.
        ///  Fills in value with the value of the parameter.
        /// </summary>
        public bool GetDoubleVariable(string name, out double value)
        {
            return Native.DllImports.TessBaseAPIGetDoubleVariable(handleRef, name, out value) == 1 ? true : false;
        }

        /// <summary>
        ///  Returns the pointer to the string that represents the value of the
        ///  parameter if it was found among Tesseract parameters.
        /// </summary>
        public string GetStringVariable(string name)
        {
            IntPtr pointer = Native.DllImports.TessBaseAPIGetStringVariable(handleRef, name);
            if (pointer == null || pointer == IntPtr.Zero)
            {
                return null;
            }
            else
            {
                string returnObject = Marshal.PtrToStringAnsi(pointer);
                Native.DllImports.TessDeleteText(pointer);
                return returnObject;
            }
        }

        /// <summary>
        /// Instances are now mostly thread-safe and totally independent,
        /// but some global parameters remain.Basically it is safe to use multiple
        /// TessBaseAPIs in different threads in parallel, UNLESS:
        /// you use SetVariable on some of the Params in classify and textord.
        /// If you do, then the effect will be to change it for all your instances.
        ///
        /// Start tesseract.Returns zero on success and -1 on failure.
        /// NOTE that the only members that may be called before Init are those
        /// listed above here in the class definition.
        ///
        /// The datapath must be the name of the parent directory of tessdata and
        /// must end in / . Any name after the last / will be stripped.
        /// The language is (usually) an ISO 639-3 string or NULL will default to eng.
        /// It is entirely safe (and eventually will be efficient too) to call
        /// Init multiple times on the same instance to change language, or just
        /// to reset the classifier.
        /// The language may be a string of the form[~]<lang>[+[~]<lang>]* indicating
        /// that multiple languages are to be loaded.Eg hin+eng will load Hindi and
        /// English. Languages may specify internally that they want to be loaded
        /// with one or more other languages, so the ~sign is available to override
        ///
        /// that.Eg if hin were set to load eng by default, then hin+~eng would force
        /// loading only hin.The number of loaded languages is limited only by
        /// memory, with the caveat that loading additional languages will impact
        /// both speed and accuracy, as there is more work to do to decide on the
        /// applicable language, and there is more chance of hallucinating incorrect
        /// words.
        /// WARNING: On changing languages, all Tesseract parameters are reset
        /// back to their default values. (Which may vary between languages.)
        /// If you have a rare need to set a Variable that controls
        /// initialization for a second call to Init you should explicitly
        /// call End() and then use SetVariable before Init.This is only a very
        /// rare use case, since there are very few uses that require any parameters
        /// to be set before Init.
        ///
        /// If set_only_non_debug_params is true, only params that do not contain
        /// "debug" in the name will be set.
        /// </summary>
        public bool Init(string dataPath, string language, OcrEngineMode tessOcrEngineMode, string[] configs)
        {
            int configsSize = 0;
            if (configs != null)
                configsSize = configs.Length;

            return Native.DllImports.TessBaseAPIInit1(handleRef, dataPath, language, tessOcrEngineMode, configs, configsSize) == 0;
        }

        /// <summary>
        /// Instances are now mostly thread-safe and totally independent,
        /// but some global parameters remain.Basically it is safe to use multiple
        /// TessBaseAPIs in different threads in parallel, UNLESS:
        /// you use SetVariable on some of the Params in classify and textord.
        /// If you do, then the effect will be to change it for all your instances.
        ///
        /// Start tesseract.Returns zero on success and -1 on failure.
        /// NOTE that the only members that may be called before Init are those
        /// listed above here in the class definition.
        ///
        /// The datapath must be the name of the parent directory of tessdata and
        /// must end in / . Any name after the last / will be stripped.
        /// The language is (usually) an ISO 639-3 string or NULL will default to eng.
        /// It is entirely safe (and eventually will be efficient too) to call
        /// Init multiple times on the same instance to change language, or just
        /// to reset the classifier.
        /// The language may be a string of the form[~]<lang>[+[~]<lang>]* indicating
        /// that multiple languages are to be loaded.Eg hin+eng will load Hindi and
        /// English. Languages may specify internally that they want to be loaded
        /// with one or more other languages, so the ~sign is available to override
        ///
        /// that.Eg if hin were set to load eng by default, then hin+~eng would force
        /// loading only hin.The number of loaded languages is limited only by
        /// memory, with the caveat that loading additional languages will impact
        /// both speed and accuracy, as there is more work to do to decide on the
        /// applicable language, and there is more chance of hallucinating incorrect
        /// words.
        /// WARNING: On changing languages, all Tesseract parameters are reset
        /// back to their default values. (Which may vary between languages.)
        /// If you have a rare need to set a Variable that controls
        /// initialization for a second call to Init you should explicitly
        /// call End() and then use SetVariable before Init.This is only a very
        /// rare use case, since there are very few uses that require any parameters
        /// to be set before Init.
        ///
        /// If set_only_non_debug_params is true, only params that do not contain
        /// "debug" in the name will be set.
        /// </summary>
        public bool Init(string dataPath, string language, OcrEngineMode tessOcrEngineMode)
        {
            return Native.DllImports.TessBaseAPIInit2(handleRef, dataPath, language, tessOcrEngineMode) == 0;
        }

        /// <summary>
        /// Instances are now mostly thread-safe and totally independent,
        /// but some global parameters remain.Basically it is safe to use multiple
        /// TessBaseAPIs in different threads in parallel, UNLESS:
        /// you use SetVariable on some of the Params in classify and textord.
        /// If you do, then the effect will be to change it for all your instances.
        ///
        /// Start tesseract.Returns zero on success and -1 on failure.
        /// NOTE that the only members that may be called before Init are those
        /// listed above here in the class definition.
        ///
        /// The datapath must be the name of the parent directory of tessdata and
        /// must end in / . Any name after the last / will be stripped.
        /// The language is (usually) an ISO 639-3 string or NULL will default to eng.
        /// It is entirely safe (and eventually will be efficient too) to call
        /// Init multiple times on the same instance to change language, or just
        /// to reset the classifier.
        /// The language may be a string of the form[~]<lang>[+[~]<lang>]* indicating
        /// that multiple languages are to be loaded.Eg hin+eng will load Hindi and
        /// English. Languages may specify internally that they want to be loaded
        /// with one or more other languages, so the ~sign is available to override
        ///
        /// that.Eg if hin were set to load eng by default, then hin+~eng would force
        /// loading only hin.The number of loaded languages is limited only by
        /// memory, with the caveat that loading additional languages will impact
        /// both speed and accuracy, as there is more work to do to decide on the
        /// applicable language, and there is more chance of hallucinating incorrect
        /// words.
        /// WARNING: On changing languages, all Tesseract parameters are reset
        /// back to their default values. (Which may vary between languages.)
        /// If you have a rare need to set a Variable that controls
        /// initialization for a second call to Init you should explicitly
        /// call End() and then use SetVariable before Init.This is only a very
        /// rare use case, since there are very few uses that require any parameters
        /// to be set before Init.
        ///
        /// If set_only_non_debug_params is true, only params that do not contain
        /// "debug" in the name will be set.
        /// </summary>
        public bool Init(string dataPath, string language)
        {
            return Native.DllImports.TessBaseAPIInit3(handleRef, dataPath, language) == 0;
        }

        /// <summary>
        /// Instances are now mostly thread-safe and totally independent,
        /// but some global parameters remain.Basically it is safe to use multiple
        /// TessBaseAPIs in different threads in parallel, UNLESS:
        /// you use SetVariable on some of the Params in classify and textord.
        /// If you do, then the effect will be to change it for all your instances.
        ///
        /// Start tesseract.Returns zero on success and -1 on failure.
        /// NOTE that the only members that may be called before Init are those
        /// listed above here in the class definition.
        ///
        /// The datapath must be the name of the parent directory of tessdata and
        /// must end in / . Any name after the last / will be stripped.
        /// The language is (usually) an ISO 639-3 string or NULL will default to eng.
        /// It is entirely safe (and eventually will be efficient too) to call
        /// Init multiple times on the same instance to change language, or just
        /// to reset the classifier.
        /// The language may be a string of the form[~]<lang>[+[~]<lang>]* indicating
        /// that multiple languages are to be loaded.Eg hin+eng will load Hindi and
        /// English. Languages may specify internally that they want to be loaded
        /// with one or more other languages, so the ~sign is available to override
        ///
        /// that.Eg if hin were set to load eng by default, then hin+~eng would force
        /// loading only hin.The number of loaded languages is limited only by
        /// memory, with the caveat that loading additional languages will impact
        /// both speed and accuracy, as there is more work to do to decide on the
        /// applicable language, and there is more chance of hallucinating incorrect
        /// words.
        /// WARNING: On changing languages, all Tesseract parameters are reset
        /// back to their default values. (Which may vary between languages.)
        /// If you have a rare need to set a Variable that controls
        /// initialization for a second call to Init you should explicitly
        /// call End() and then use SetVariable before Init.This is only a very
        /// rare use case, since there are very few uses that require any parameters
        /// to be set before Init.
        ///
        /// If set_only_non_debug_params is true, only params that do not contain
        /// "debug" in the name will be set.
        /// </summary>
        public bool Init(string dataPath, string language, OcrEngineMode tessOcrEngineMode,
            string[] configs, string[] varsVec, string[] varsValues, bool setOnlyNonDebugParams = false)
        {
            int configsSize = 0;
            if (configs != null)
                configsSize = configs.Length;

            UIntPtr varsVecSize = new UIntPtr(0);
            if (varsVec != null)
                varsVecSize = new UIntPtr((uint)varsVec.Length);

            UIntPtr varsValuesSize = new UIntPtr(0);
            if (varsValues != null)
                varsValuesSize = new UIntPtr((uint)varsValues.Length);

            return Native.DllImports.TessBaseAPIInit4(handleRef, dataPath, language, tessOcrEngineMode,
                configs, configsSize, varsVec, varsValues, varsVecSize, setOnlyNonDebugParams ? 1 : 0) == 0;
        }

        /// <summary>
        /// Returns the languages string used in the last valid initialization.
        /// If the last initialization specified "deu+hin" then that will be
        /// returned.If hin loaded eng automatically as well, then that will
        /// not be included in this list.To find the languages actually
        /// loaded use GetLoadedLanguagesAsVector.
        /// The returned string should NOT be deleted.
        /// </summary>
        public string GetInitLanguagesAsString()
        {
            IntPtr pointer = Native.DllImports.TessBaseAPIGetInitLanguagesAsString(handleRef);
            if (pointer == null || pointer == IntPtr.Zero)
            {
                return null;
            }
            else
            {
                string returnObject = Marshal.PtrToStringAnsi(pointer);
                Native.DllImports.TessDeleteText(pointer);
                return returnObject;
            }
        }

        /// <summary>
        /// Init only the lang model component of Tesseract. The only functions
        /// that work after this init are SetVariable and IsValidWord.
        /// WARNING: temporary! This function will be removed from here and placed
        /// in a separate API at some future time.
        /// </summary>
        public int InitLangMod(string dataPath, string language)
        {
            return Native.DllImports.TessBaseAPIInitLangMod(handleRef, dataPath, language);
        }

        /// <summary>
        /// Init only for page layout analysis. Use only for calls to SetImage and
        ///  AnalysePage.Calls that attempt recognition will generate an error.
        /// </summary>
        public void InitForAnalysePage()
        {
            Native.DllImports.TessBaseAPIInitForAnalysePage(handleRef);
        }

        /// <summary>
        /// Set the current page segmentation mode. Defaults to PSM_SINGLE_BLOCK.
        /// The mode is stored as an IntParam so it can also be modified by
        /// ReadConfigFile or SetVariable("tessedit_pageseg_mode", mode as string).
        /// </summary>
        public void SetPageSegMode(PageSegmentationMode value)
        {
            Native.DllImports.TessBaseAPISetPageSegMode(handleRef, value);
        }

        /// <summary>
        /// Return the current page segmentation mode.
        /// </summary>
        public PageSegmentationMode GetPageSegMode()
        {
            return Native.DllImports.TessBaseAPIGetPageSegMode(handleRef);
        }

        /// <summary>
        ///  Call between pages or documents etc to free up memory and forget
        /// adaptive data.
        /// </summary>
        public void ClearAdaptiveClassifier()
        {
            Native.DllImports.TessBaseAPIClearAdaptiveClassifier(handleRef);
        }

        /// <summary>
        /// Set the resolution of the source image in pixels per inch so font size
        /// information can be calculated in results.Call this after SetImage().
        /// </summary>
        public void SetSourceResolution(int ppi)
        {
            Native.DllImports.TessBaseAPISetSourceResolution(handleRef, ppi);
        }

        /// <summary>
        /// Restrict recognition to a sub-rectangle of the image.Call after SetImage.
        /// Each SetRectangle clears the recogntion results so multiple rectangles
        /// can be recognized with the same image.
        /// </summary>
        public void SetRectangle(int left, int top, int width, int height)
        {
            Native.DllImports.TessBaseAPISetRectangle(handleRef, left, top, width, height);
        }

        /// <summary>
        /// Get a copy of the internal thresholded image from Tesseract.
        ///  Caller takes ownership of the Pix and must pixDestroy it.
        ///  May be called any time after SetImage, or after TesseractRect.
        /// </summary>
        public Pix GetThresholdedImage()
        {
            IntPtr pointer = Native.DllImports.TessBaseAPIGetThresholdedImage(handleRef);

            if (IntPtr.Zero != pointer)
            {
                return new Pix(pointer);
            }

            return null;
        }

        public Boxa GetRegions(out Pixa pixa)
        {
            IntPtr pixaPnt;
            IntPtr pointer = Native.DllImports.TessBaseAPIGetRegions(handleRef, out pixaPnt);

            if (pixaPnt != IntPtr.Zero)
            {
                pixa = new Pixa(pixaPnt);
            }
            else
                pixa = null;

            if (IntPtr.Zero != pointer)
            {
                return new Boxa(pointer);
            }

            return null;
        }

        public Boxa GetTextlines(out Pixa pixa, out int[] blockids)
        {
            IntPtr pixaPnt;
            IntPtr pointer = Native.DllImports.TessBaseAPIGetTextlines(handleRef, out pixaPnt, out blockids);

            if (pixaPnt != IntPtr.Zero)
            {
                pixa = new Pixa(pixaPnt);
            }
            else
                pixa = null;

            if (IntPtr.Zero != pointer)
            {
                return new Boxa(pointer);
            }

            return null;
        }

        public Boxa GetStrips(out Pixa pixa, out int[] blockids)
        {
            IntPtr pixaPntr;
            IntPtr pointer = Native.DllImports.TessBaseAPIGetStrips(handleRef, out pixaPntr, out blockids);

            if (pixaPntr != IntPtr.Zero)
            {
                pixa = new Pixa(pixaPntr);
            }
            else
                pixa = null;

            if (IntPtr.Zero != pointer)
            {
                return new Boxa(pointer);
            }

            return null;
        }

        public Boxa GetTextlines(bool rawImage, int rawPadding, out Pixa pixa, out int[] blockids, out int[] paraids)
        {
            IntPtr pixaPntr;
            IntPtr pointer = Native.DllImports.TessBaseAPIGetTextlines1(handleRef, rawImage ? 1 : 0, rawPadding, out pixaPntr, out blockids, out paraids);

            if (pixaPntr != IntPtr.Zero)
            {
                pixa = new Pixa(pixaPntr);
            }
            else
                pixa = null;

            if (IntPtr.Zero != pointer)
            {
                return new Boxa(pointer);
            }

            return null;
        }

        public Boxa GetWords(out Pixa pixa)
        {
            IntPtr pixaPntr;
            IntPtr pointer = Native.DllImports.TessBaseAPIGetWords(handleRef, out pixaPntr);

            if (pixaPntr != IntPtr.Zero)
            {
                pixa = new Pixa(pixaPntr);
            }
            else
                pixa = null;

            if (IntPtr.Zero != pointer)
            {
                return new Boxa(pointer);
            }

            return null;
        }

        /// <summary>
        /// Helper function to get binary images with no padding (most common usage).
        /// </summary>
        public Boxa GetComponentImages(PageIteratorLevel tessPageIteratorLevel, bool textOnly)
        {
            ////    TODO: Implement last two Pixa** pixa, int** blockids
            //// Pixa pixa;
            //// int[] blockids;
            //// DllImports.TessBaseAPIGetComponentImages(handleRef, tessPageIteratorLevel, textOnly ? 1 : 0, out pixa, out blockids);

            IntPtr pointer = Native.DllImports.TessBaseAPIGetComponentImages(handleRef, tessPageIteratorLevel, textOnly ? 1 : 0, IntPtr.Zero, IntPtr.Zero);

            if (IntPtr.Zero != pointer)
            {
                return new Boxa(pointer);
            }

            return null;
        }

        /// <summary>
        /// Returns the scale factor of the thresholded image that would be returned by
        ///  GetThresholdedImage() and the various GetX() methods that call
        ///  GetComponentImages().
        ///  Returns 0 if no thresholder has been set.
        /// </summary>
        public int GetThresholdedImageScaleFactor()
        {
            return Native.DllImports.TessBaseAPIGetThresholdedImageScaleFactor(handleRef);
        }

        /// <summary>
        /// Dump the internal binary image to a PGM file.
        ///  @deprecated Use GetThresholdedImage and write the image using pixWrite
        ///  instead if possible.
        /// </summary>
        public void DumpPGM(string fileName)
        {
            Native.DllImports.TessBaseAPIDumpPGM(handleRef, fileName);
        }

        /// <summary>
        /// Runs page layout analysis in the mode set by SetPageSegMode.
        /// May optionally be called prior to Recognize to get access to just
        /// the page layout results.Returns an iterator to the results.
        /// If merge_similar_words is true, words are combined where suitable for use
        /// with a line recognizer.Use if you want to use AnalyseLayout to find the
        /// textlines, and then want to process textline fragments with an external
        /// line recognizer.
        /// Returns NULL on error or an empty page.
        /// The returned iterator must be deleted after use.
        /// WARNING! This class points to data held within the TessBaseAPI class, and
        /// therefore can only be used while the TessBaseAPI class still exists and
        /// has not been subjected to a call of Init, SetImage, Recognize, Clear, End
        /// DetectOS, or anything else that changes the internal PAGE_RES.
        /// </summary>
        public PageIterator AnalyseLayout()
        {
            var pointer = Native.DllImports.TessBaseAPIAnalyseLayout(handleRef);
            return new PageIterator(pointer);
        }

        /// <summary>
        ///  Recognize the image from SetAndThresholdImage, generating Tesseract
        ///  internal structures.Returns 0 on success.
        ///  Optional.The Get*Text functions below will call Recognize if needed.
        ///  After Recognize, the output is kept internally until the next SetImage.
        /// </summary>
        public int Recognize()
        {
            return Native.DllImports.TessBaseAPIRecognize(handleRef, IntPtr.Zero); //TODO: Implement last parameter ETEXT_DESC*
        }

        /// <summary>
        ///  Variant on Recognize used for testing chopper.
        /// </summary>
        /// <returns></returns>
        public int RecognizeForChopTest()
        {
            return Native.DllImports.TessBaseAPIRecognizeForChopTest(handleRef, IntPtr.Zero); //TODO: Implement last parameter ETEXT_DESC*
        }

        /// <summary>
        /// Get a reading-order iterator to the results of LayoutAnalysis and/or
        /// Recognize.The returned iterator must be deleted after use.
        /// WARNING! This class points to data held within the TessBaseAPI class, and
        /// therefore can only be used while the TessBaseAPI class still exists and
        /// has not been subjected to a call of Init, SetImage, Recognize, Clear, End
        /// DetectOS, or anything else that changes the internal PAGE_RES.
        /// </summary>
        /// <returns></returns>
        public ResultIterator GetIterator()
        {
            ResultIterator iterator = null;
            try
            {
                iterator = new ResultIterator(this);
            }
            catch (ArgumentNullException)
            { }

            return iterator;
        }

        /// <summary>
        /// Get a mutable iterator to the results of LayoutAnalysis and/or Recognize.
        /// The returned iterator must be deleted after use.
        /// WARNING! This class points to data held within the TessBaseAPI class, and
        /// therefore can only be used while the TessBaseAPI class still exists and
        /// has not been subjected to a call of Init, SetImage, Recognize, Clear, End
        /// DetectOS, or anything else that changes the internal PAGE_RES.
        /// </summary>
        public MutableIterator GetMutableIterator()
        {
            MutableIterator iterator = null;
            try
            {
                iterator = new MutableIterator(this);
            }
            catch (ArgumentNullException)
            { }

            return iterator;
        }

        /// <summary>
        /// The recognized text is returned as a char* which is coded
        /// as UTF8 and must be freed with the delete[] operator.
        /// </summary>
        public string GetUTF8Text()
        {
            IntPtr pointer = Native.DllImports.TessBaseAPIGetUTF8Text(handleRef);
            if (pointer == null || pointer == IntPtr.Zero)
            {
                return null;
            }
            else
            {
                string returnObject = Marshaling.PtrToStringUTF8(pointer);
                Native.DllImports.TessDeleteText(pointer); //Delete the text pointer to clear memory
                return returnObject;
            }
        }

        /// <summary>
        /// Make a HTML-formatted string with hOCR markup from the internal
        /// data structures.
        /// page_number is 0-based but will appear in the output as 1-bas
        /// </summary>
        public string GetHOCRText(int pageNumber)
        {
            IntPtr pointer = Native.DllImports.TessBaseAPIGetHOCRText(handleRef, pageNumber);
            if (pointer == null || pointer == IntPtr.Zero)
            {
                return null;
            }
            else
            {
                string returnObject = Marshal.PtrToStringAnsi(pointer);
                Native.DllImports.TessDeleteText(pointer); //Delete the text pointer to clear memory
                return returnObject;
            }
        }

        /// <summary>
        /// The recognized text is returned as a char* which is coded in the same
        ///  format as a box file used in training.Returned string must be freed with
        ///  the delete[] operator.
        ///  Constructs coordinates in the original image - not just the rectangle.
        ///  page_number is a 0-based page index that will appear in the box file.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public string GetBoxText(int pageNumber)
        {
            IntPtr pointer = Native.DllImports.TessBaseAPIGetBoxText(handleRef, pageNumber);
            if (pointer == null || pointer == IntPtr.Zero)
            {
                return null;
            }
            else
            {
                string returnObject = Marshal.PtrToStringAnsi(pointer);
                Native.DllImports.TessDeleteText(pointer); //Delete the text pointer to clear memory
                return returnObject;
            }
        }

        /// <summary>
        /// The recognized text is returned as a char* which is coded
        /// as UNLV format Latin-1 with specific reject and suspect codes
        /// and must be freed with the delete[] operator.
        /// </summary>
        public string GetUNLVText()
        {
            IntPtr pointer = Native.DllImports.TessBaseAPIGetUNLVText(handleRef);
            if (pointer == null || pointer == IntPtr.Zero)
            {
                return null;
            }
            else
            {
                string returnObject = Marshal.PtrToStringAnsi(pointer);
                Native.DllImports.TessDeleteText(pointer); //Delete the text pointer to clear memory
                return returnObject;
            }
        }

        /// <summary>
        ///    * Detect the orientation of the input image and apparent script (alphabet).
        /// orient_deg is the detected clockwise rotation of the input image in degrees
        /// (0, 90, 180, 270)
        /// orient_conf is the confidence(15.0 is reasonably confident)
        /// script_name is an ASCII string, the name of the script, e.g. "Latin"
        /// script_conf is confidence level in the script
        /// Returns true on success and writes values to each parameter as an output
        /// </summary>
        public bool DetectOrientationScript(out int orient_deg, out float orient_conf, out string script_name, out float script_conf)
        {
            return Native.DllImports.TessBaseAPIDetectOrientationScript(handleRef, out orient_deg, out orient_conf, out script_name, out script_conf) == 1 ? true : false;
        }

        /// <summary>
        /// Returns the (average) confidence value between 0 and 100.
        /// </summary>
        public int MeanTextConf
        {
            get
            {
                return Native.DllImports.TessBaseAPIMeanTextConf(handleRef);
            }
        }

        /// <summary>
        /// Free up recognition results and any stored image data, without actually
        ///  freeing any recognition data that would be time-consuming to reload.
        ///  Afterwards, you must call SetImage or TesseractRect before doing
        ///  any Recognize or Get* operation.
        /// </summary>
        public void Clear()
        {
            Native.DllImports.TessBaseAPIClear(handleRef);
        }

        /// <summary>
        /// Close down tesseract and free up all memory. End() is equivalent to
        ///  destructing and reconstructing your TessBaseAPI.
        ///  Once End() has been used, none of the other API functions may be used
        ///  other than Init and anything declared above it in the class definition.
        /// </summary>
        public void End()
        {
            Native.DllImports.TessBaseAPIEnd(handleRef);
        }

        /// <summary>
        /// Clear any library-level memory caches.
        /// There are a variety of expensive-to-load constant data structures(mostly
        /// language dictionaries) that are cached globally -- surviving the Init()
        /// and End() of individual TessBaseAPI's.  This function allows the clearing
        /// of these caches.
        /// </summary>
        public void ClearPersistentCache()
        {
            Native.DllImports.TessBaseAPIClearPersistentCache(handleRef);
        }

        /// <summary>
        ///Check whether a word is valid according to Tesseract's language model
        /// @return 0 if the word is invalid, non-zero if valid.
        /// @warning temporary! This function will be removed from here and placed
        /// in a separate API at some future time.
        /// </summary>
        public bool IsValidWord(string word)
        {
            return Native.DllImports.TessBaseAPIIsValidWord(handleRef, word) != 0;
        }

        /// <summary>
        ///
        /// </summary>
        public bool GetTextDirection(string word, out int outOffset, out float outSlope)
        {
            return Native.DllImports.TessBaseAPIGetTextDirection(handleRef, out outOffset, out outSlope) == 1 ? true : false;
        }

        /// <summary>
        /// This method returns the string form of the specified unichar.
        /// </summary>
        public string GetUnichar(int uniCharId)
        {
            IntPtr pointer = Native.DllImports.TessBaseAPIGetUnichar(handleRef, uniCharId);
            if (pointer == null || pointer == IntPtr.Zero)
            {
                return null;
            }
            else
            {
                string returnObject = Marshal.PtrToStringAnsi(pointer);
                Native.DllImports.TessDeleteText(pointer); //Delete the text pointer to clear memory
                return returnObject;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public OcrEngineMode Oem
        {
            get
            {
                return Native.DllImports.TessBaseAPIOem(handleRef);
            }
        }

        /// <summary>
        /// Read a "config" file containing a set of param, value pairs.
        ///  Searches the standard places: tessdata/configs, tessdata/tessconfigs
        ///  and also accepts a relative or absolute path name.
        ///  Note: only non-init params will be set (init params are set by Init()).
        /// </summary>
        public void ReadConfigFile(string fileName)
        {
            Native.DllImports.TessBaseAPIReadConfigFile(handleRef, fileName);
        }

        /// <summary>
        /// Same as above, but only set debug params from the given config file.
        /// </summary>
        public void ReadDebugConfigFile(string fileName)
        {
            Native.DllImports.TessBaseAPIReadDebugConfigFile(handleRef, fileName);
        }

        public void SetMinOrientationMargin(double margin)
        {
            Native.DllImports.TessBaseAPISetMinOrientationMargin(handleRef, margin);
        }

        #endregion Tesseract Methods

        #region IDisposable Support

        public void Dispose()
        {
            if (handleRef.Handle != null && handleRef.Handle != IntPtr.Zero)
            {
                if (localPix != null)
                {
                    localPix.Dispose();
                }
                Clear();
                ClearAdaptiveClassifier();
                End();
                ClearPersistentCache();
                Native.DllImports.TessBaseAPIDelete(handleRef);
            }
        }

        #endregion IDisposable Support
    }
}

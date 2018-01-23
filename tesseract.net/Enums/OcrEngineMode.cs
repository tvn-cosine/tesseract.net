namespace Tesseract
{
    /// <summary>
    /// When Tesseract/Cube is initialized we can choose to instantiate/load/run
    ///  only the Tesseract part, only the Cube part or both along with the combiner.
    ///  The preference of which engine to use is stored in tessedit_ocr_engine_mode.
    /// 
    ///  ATTENTION: When modifying this enum, please make sure to make the
    ///  appropriate changes to all the enums mirroring it(e.g.OCREngine in
    ///  cityblock/workflow/detection/detection_storage.proto). Such enums will
    ///  mention the connection to OcrEngineMode in the comments.
    /// </summary>
    public enum OcrEngineMode
    {
        /// <summary>
        /// Run Tesseract only - fastest
        /// </summary>
        TESSERACT_ONLY,
        /// <summary>
        /// Run just the LSTM line recognizer.
        /// </summary>
        LSTM_ONLY,
        /// <summary>
        /// Run the LSTM recognizer, but allow fallback to Tesseract when things get difficult.
        /// </summary>
        TESSERACT_LSTM_COMBINED,
        /// <summary>
        /// Specify this mode when calling init,
        /// to indicate that any of the above modes
        /// should be automatically inferred from the
        /// variables in the language-specific config,
        /// command-line configs, or if not specified
        /// in any of the above should be set to the
        /// default OEM_TESSERACT_ONLY.
        /// </summary>
        DEFAULT,
        /// <summary>  
        /// Run Cube only - better accuracy, but slower
        /// </summary>
        CUBE_ONLY,
        /// <summary>   
        /// Run both and combine results - best accuracy
        /// </summary>
        TESSERACT_CUBE_COMBINED,
    }
}

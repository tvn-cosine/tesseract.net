namespace Tesseract
{
    /// <summary>
    ///  Possible modes for page layout analysis. These *must* be kept in order
    ///  of decreasing amount of layout analysis to be done, except for OSD_ONLY,
    ///  so that the inequality test macros below work.
    /// </summary>
    public enum PageSegmentationMode
    {
        /// <summary>
        /// Orientation and script detection only.
        /// </summary>
        OSD_ONLY,
        /// <summary>
        /// Automatic page segmentation with orientation and script detection. (OSD)
        /// </summary>
        AUTO_OSD,
        /// <summary>
        /// Automatic page segmentation, but no OSD, or OCR.
        /// </summary>
        AUTO_ONLY,
        /// <summary>
        /// Fully automatic page segmentation, but no OSD.
        /// </summary>
        AUTO,
        /// <summary>
        /// Assume a single column of text of variable sizes.
        /// </summary>
        SINGLE_COLUMN,
        /// <summary>
        /// Assume a single uniform block of vertically aligned text.
        /// </summary>
        SINGLE_BLOCK_VERT_TEXT,
        /// <summary>
        /// Assume a single uniform block of text. (Default.)
        /// </summary>
        SINGLE_BLOCK,
        /// <summary>
        /// Treat the image as a single text line.
        /// </summary>
        SINGLE_LINE,
        /// <summary>
        ///  Treat the image as a single word.
        /// </summary>
        SINGLE_WORD,
        /// <summary>
        /// Treat the image as a single word in a circle.
        /// </summary>
        CIRCLE_WORD,
        /// <summary>
        /// Treat the image as a single character.
        /// </summary>
        SINGLE_CHAR,
        /// <summary>
        /// Find as much text as possible in no particular order.
        /// </summary>
        SPARSE_TEXT,
        /// <summary>
        ///  Sparse text with orientation and script det.
        /// </summary>
        SPARSE_TEXT_OSD,
        /// <summary>
        /// Treat the image as a single text line, bypassing hacks that are Tesseract-specific.
        /// </summary>
        RAW_LINE,
        /// <summary>
        /// Number of enum entries.
        /// </summary>
        COUNT
    }
}

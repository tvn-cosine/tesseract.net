namespace Tesseract
{
    /// <summary>
    /// Fully justified paragraphs (text aligned to both left and right
    /// margins) are marked by Tesseract with JUSTIFICATION_LEFT if their text
    ///  is written with a left-to-right script and with JUSTIFICATION_RIGHT if
    /// their text is written in a right-to-left script.
    /// </summary>
    public enum ParagraphJustification
    {
        /// <summary>
        /// The alignment is not clearly one of the other options.  This could happen
        ///    for example if there are only one or two lines of text or the text looks
        ///    like source code or poetry
        /// </summary>
        UNKNOWN,
        /// <summary>
        ///  Each line, except possibly the first, is flush to the same left tab stop.
        /// </summary>
        LEFT,
        /// <summary>
        /// The text lines of the paragraph are centered about a line going
        ///   down through their middle of the text lines.
        /// </summary>
        CENTER,
        /// <summary>
        /// Each line, except possibly the first, is flush to the same right tab stop.
        /// </summary>
        RIGHT,
    }
}

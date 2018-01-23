namespace Tesseract
{
    /// <summary>
    /// enum of the elements of the page hierarchy, used in ResultIterator
    /// to provide functions that operate on each level without having to
    /// have 5x as many functions.
    /// </summary>
    public enum PageIteratorLevel
    {
        /// <summary>
        /// Block of text/image/separator line.
        /// </summary>
        RIL_BLOCK,
        /// <summary>
        /// Paragraph within a block.
        /// </summary>
        RIL_PARA,
        /// <summary>
        ///  Line within a paragraph.
        /// </summary>
        RIL_TEXTLINE,
        /// <summary>
        ///  Word within a textline.
        /// </summary>
        RIL_WORD,
        /// <summary>
        ///  Symbol/character within a word.
        /// </summary>
        RIL_SYMBOL
    }
}

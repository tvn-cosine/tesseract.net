namespace Tesseract
{
    /// <summary>
    /// The grapheme clusters within a line of text are laid out logically
    /// in this direction, judged when looking at the text line rotated so that
    /// its Orientation is "page up". 
    /// For English text, the writing direction is left-to-right.For the
    /// Chinese text in the above example, the writing direction is top-to-bottom.
    /// </summary>
    public enum WritingDirection
    {
        LEFT_TO_RIGHT = 0,
        RIGHT_TO_LEFT = 1,
        TOP_TO_BOTTOM = 2,
    }
}

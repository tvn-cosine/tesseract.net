namespace Tesseract
{
    /// <summary>
    /// The text lines are read in the given sequence.
    /// In English, the order is top-to-bottom.
    /// In Chinese, vertical text lines are read right-to-left.Mongolian is
    /// written in vertical columns top to bottom like Chinese, but the lines
    /// order left-to right.
    /// Note that only some combinations make sense.For example,
    /// WRITING_DIRECTION_LEFT_TO_RIGHT implies TEXTLINE_ORDER_TOP_TO_BOTTOM
    /// </summary>
    public enum TextlineOrder
    {
        LEFT_TO_RIGHT = 0,
        RIGHT_TO_LEFT = 1,
        TOP_TO_BOTTOM = 2,
    }
}

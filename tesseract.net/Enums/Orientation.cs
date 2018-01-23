namespace Tesseract
{
    /// <summary>
    /// 
    /// +------------------+  Orientation Example:
    /// | 1 Aaaa Aaaa Aaaa |  ====================
    /// | Aaa aa aaa aa    |  To left is a diagram of some(1) English and
    /// | aaaaaa A aa aaa. |  (2) Chinese text and a(3) photo credit.
    /// |                2 |
    /// |   #######  c c C |  Upright Latin characters are represented as A and a.
    /// |   #######  c c c |   represents a latin character rotated
    /// |   #######  c c c |      anti-clockwise 90 degrees.
    /// |   #######  c   c |
    /// |   #######  .   c |  Upright Chinese characters are represented C and c.
    /// | 3 #######      c |
    /// +------------------+  NOTA BENE: enum values here should match goodoc.proto
    /// If you orient your head so that "up" aligns with Orientation,
    ///  then the characters will appear "right side up" and readable.
    /// 
    ///  In the example above, both the English and Chinese paragraphs are oriented
    /// so their "up" is the top of the page (page up).  The photo credit is read
    ///  with one's head turned leftward ("up" is to page left).
    /// 
    ///  The values of this enum match the convention of Tesseract's osdetect.h
    /// </summary>
    public enum Orientation
    {
        PAGE_UP = 0,
        PAGE_RIGHT = 1,
        PAGE_DOWN = 2,
        PAGE_LEFT = 3,
    }
}

namespace Tesseract
{
    /// <summary>
    /// Possible types for a POLY_BLOCK or ColPartition.
    /// Must be kept in sync with kPBColors in polyblk.cpp and PTIs* Type functions
    /// below, as well as kPolyBlockNames in publictypes.cpp. 
    ///  Used extensively by ColPartition, and POLY_BLOCK.
    /// </summary>
    public enum PolyBlockType
    {
        /// <summary>
        /// Type is not yet known. Keep as the first element.
        /// </summary>
        UNKNOWN,
        /// <summary>
        /// Text that lives inside a column.
        /// </summary>
        FLOWING_TEXT,
        /// <summary>
        /// Text that spans more than one column.
        /// </summary>
        HEADING_TEXT,
        /// <summary>
        /// Text that is in a cross-column pull-out region.
        /// </summary>
        PULLOUT_TEXT,
        /// <summary>
        /// Partition belonging to an equation region.
        /// </summary>
        EQUATION,
        /// <summary>
        /// Partition has inline equation.
        /// </summary>
        INLINE_EQUATION,
        /// <summary>
        /// Partition belonging to a table region.
        /// </summary>
        TABLE,
        /// <summary>
        /// Text-line runs vertically.
        /// </summary>
        VERTICAL_TEXT,
        /// <summary>
        /// Text that belongs to an image.
        /// </summary>
        CAPTION_TEXT,
        /// <summary>
        /// Image that lives inside a column.
        /// </summary>
        FLOWING_IMAGE,
        /// <summary>
        /// Image that spans more than one column.
        /// </summary>
        HEADING_IMAGE,
        /// <summary>
        /// Image that is in a cross-column pull-out region.
        /// </summary>
        PULLOUT_IMAGE,
        /// <summary>
        /// Horizontal Line.
        /// </summary>
        HORZ_LINE,
        /// <summary>
        /// Vertical Line.
        /// </summary>
        VERT_LINE,
        /// <summary>
        /// Lies outside of any column.
        /// </summary>
        NOISE,
        /// <summary>
        /// Count
        /// </summary>
        COUNT
    }
}

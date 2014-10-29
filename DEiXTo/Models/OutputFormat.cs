public struct OutputFormat
{
    #region Instance Variables
    private Format _format;
    private string _caption;
    #endregion

    #region Constructors
    public OutputFormat(Format format, string caption)
    {
        _format = format;
        _caption = caption;
    }
    #endregion

    #region Properties
    public Format Format
    {
        get { return _format; }
    }

    public string Caption
    {
        get { return _caption; }
    }
    #endregion

    #region Public Methods
    public override string ToString()
    {
        return _caption;
    }
    #endregion
}
public enum Format
{
    Text,
    XML,
    RSS
}

public struct OutputFormat
{
    private Format _format;
    private string _caption;

    public OutputFormat(Format format, string caption)
    {
        _format = format;
        _caption = caption;
    }

    public Format Format
    {
        get { return _format; }
    }

    public string Caption
    {
        get { return _caption; }
    }

    public override string ToString()
    {
        return _caption;
    }
}
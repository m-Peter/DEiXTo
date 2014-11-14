namespace DEiXTo.Services
{
    public class RecordsWriterFactory
    {
        public static ExtractedRecordsWriter GetWriterFor(Format outputFormat, string filename)
        {
            switch (outputFormat)
            {
                case Format.Text:
                    return new TextRecordsWriter(filename);
                case Format.XML:
                    return new XmlRecordsWriter(filename);
                default:
                    return null;
            }
        }
    }
}

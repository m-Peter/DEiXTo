namespace DEiXTo.Services
{
    public class DialogBuilderFactory
    {
        public IDialogBuilder CreateBuilder(Format format)
        {
            switch (format)
            {
                case Format.Text:
                    return new TextDialogBuilder();
                case Format.XML:
                    return new XmlDialogBuilder();
                case Format.RSS:
                    return new RssDialogBuilder();
            }

            return null;
        }
    }
}

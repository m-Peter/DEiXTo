namespace DEiXTo.Services
{
    public class RssDialogBuilder : IDialogBuilder
    {
        public void Build(ISaveFileDialog dialog)
        {
            dialog.Filter = "RSS Files (*.rss)|";
            dialog.Extension = "rss";
        }
    }
}

namespace DEiXTo.Services
{
    public class XmlDialogBuilder : IDialogBuilder
    {
        public void Build(ISaveFileDialog dialog)
        {
            dialog.Filter = "XML Files (*.xml)|";
            dialog.Extension = "xml";
        }
    }
}

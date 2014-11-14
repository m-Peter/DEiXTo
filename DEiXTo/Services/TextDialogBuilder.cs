namespace DEiXTo.Services
{
    public class TextDialogBuilder : IDialogBuilder
    {
        public void Build(ISaveFileDialog dialog)
        {
            dialog.Filter = "Text Files (*.txt)|";
            dialog.Extension = "txt";
        }
    }
}

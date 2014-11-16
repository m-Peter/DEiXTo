using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace DEiXTo.Services
{
    public class StatesImageLoader
    {
        public ImageList LoadImages()
        {
            var myAssembly = Assembly.GetExecutingAssembly();
            ImageList imageList = new ImageList();

            var myStream = myAssembly.GetManifestResourceStream("DEiXTo.Images.mec.gif");
            imageList.Images.Add(Image.FromStream(myStream));
            myStream = myAssembly.GetManifestResourceStream("DEiXTo.Images.meo.gif");
            imageList.Images.Add(Image.FromStream(myStream));
            myStream = myAssembly.GetManifestResourceStream("DEiXTo.Images.mes.gif");
            imageList.Images.Add(Image.FromStream(myStream));
            myStream = myAssembly.GetManifestResourceStream("DEiXTo.Images.mn.gif");
            imageList.Images.Add(Image.FromStream(myStream));
            myStream = myAssembly.GetManifestResourceStream("DEiXTo.Images.mno.gif");
            imageList.Images.Add(Image.FromStream(myStream));
            myStream = myAssembly.GetManifestResourceStream("DEiXTo.Images.x.gif");
            imageList.Images.Add(Image.FromStream(myStream));

            myStream.Close();

            return imageList;
        }
    }
}

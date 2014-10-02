using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DEiXTo.Services
{
    public class ElementStyling
    {
        private IDictionary<HtmlElement, string> elementStyles = new Dictionary<HtmlElement, string>();

        public void Style(HtmlElement element)
        {
            if (!elementStyles.ContainsKey(element))
            {
                string style = element.Style;
                elementStyles.Add(element, style);
                element.Style = style + "; background-color: yellow; border: 2px solid red";
            }
            else
            {
                string style = element.Style;
                element.Style = style + "; background-color: yellow; border: 2px solid red";
            }
        }

        public void Unstyle(HtmlElement element)
        {
            if (elementStyles.ContainsKey(element))
            {
                element.Style = elementStyles[element];
            }
        }

        public void UnstyleElements()
        {
            foreach (var item in elementStyles)
            {
                Unstyle(item.Key);
            }
        }
    }
}

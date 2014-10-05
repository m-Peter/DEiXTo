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
        private IDictionary<HtmlElement, string> _elementStyles = new Dictionary<HtmlElement, string>();

        public void Style(HtmlElement element)
        {
            if (!containsKey(element))
            {
                string style = element.Style;
                add(element, style);
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
            if (containsKey(element))
            {
                element.Style = _elementStyles[element];
            }
        }

        public void UnstyleElements()
        {
            foreach (var item in _elementStyles)
            {
                Unstyle(item.Key);
            }
        }

        private void add(HtmlElement key, string value)
        {
            _elementStyles.Add(key, value);
        }

        private bool containsKey(HtmlElement key)
        {
            return _elementStyles.ContainsKey(key);
        }
    }
}

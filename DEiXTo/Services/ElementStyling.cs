using System.Collections.Generic;
using System.Windows.Forms;

namespace DEiXTo.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ElementStyling
    {
        private IDictionary<HtmlElement, string> _elementStyles = new Dictionary<HtmlElement, string>();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return _elementStyles.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            _elementStyles.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public void Unstyle(HtmlElement element)
        {
            if (containsKey(element))
            {
                element.Style = _elementStyles[element];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void UnstyleElements()
        {
            foreach (var item in _elementStyles)
            {
                Unstyle(item.Key);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void add(HtmlElement key, string value)
        {
            if (containsKey(key))
            {
                return;
            }

            _elementStyles.Add(key, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool containsKey(HtmlElement key)
        {
            return _elementStyles.ContainsKey(key);
        }
    }
}

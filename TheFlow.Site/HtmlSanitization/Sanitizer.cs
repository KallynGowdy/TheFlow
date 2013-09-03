using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;


namespace TheFlow.Site.HtmlSanitization
{
    /// <summary>
    /// Defines an interface that defines the contract for an html sanitizer.
    /// </summary>
    public interface IHtmlSanitizer
    {
        /// <summary>
        /// Sanitizes the given string of html into a string of 'safe' html based on the current settings.
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        string GetHtml(string html);
    }

    /// <summary>
    /// Defines a class that is used to sanitize html into valid html that is XSS free.
    /// </summary>
    public class HtmlSanitizer : IHtmlSanitizer
    {
        /// <summary>
        /// Gets or sets the filter for html nodes.
        /// </summary>
        public IElementFilter ElementFilter
        {
            get;
            set;
        }

        /// <summary>
        /// Sanitizes the given string of html into a string of 'safe' html based on the current settings.
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public string GetHtml(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            if (doc.DocumentNode != null)
            {
                if (doc.DocumentNode.HasChildNodes)
                {
                    for (int i = 0; i < doc.DocumentNode.ChildNodes.Count; i++)
                    {
                        if (filterElement(doc.DocumentNode.ChildNodes[i]))
                        {
                            i--;
                        }
                    }
                }
                return doc.DocumentNode.InnerHtml;
            }
            return string.Empty;
        }

        /// <summary>
        /// Filters the element, returns whether the element was removed.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private bool filterElement(HtmlNode node)
        {
            if (ElementFilter.IsValid(node))
            {
                foreach (HtmlNode child in node.ChildNodes)
                {
                    filterElement(child);
                }
                return false;
            }
            else
            {
                node.RemoveAllChildren();
                node.Remove();
                return true;
            }
        }
    }

    /// <summary>
    /// Defines several different filter types that determine how a filter should handle itself.
    /// </summary>
    public enum FilterType
    {
        /// <summary>
        /// Defines that the filter should remove all of the offending objects.
        /// </summary>
        RemoveBad,
        /// <summary>
        /// Defines that the filter should fail on the first offence. 
        /// </summary>
        FailOnFirst
    }

    /// <summary>
    /// Defines the different map types for an element to a filter.
    /// </summary>
    public enum ElementMapType
    {
        Allow,
        Disallow
    }

    /// <summary>
    /// Defines an interface that defines the contract for an element filter.
    /// </summary>
    public interface IElementFilter
    {
        /// <summary>
        /// Gets or sets the list of elements that are either allowed or not allowed based on the mapped value.
        /// </summary>
        IDictionary<string, Tuple<ElementMapType, IAttributeFilter>> MappedElements
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the default way to map an element if it is not referenced in the dictionary.
        /// </summary>
        ElementMapType DefaultMapType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets how the filter should operate.
        /// </summary>
        FilterType FilterType
        {
            get;
            set;
        }

        /// <summary>
        /// Determines if the given HtmlNode is valid as determined by this filter.
        /// </summary>
        /// <param name="node">The HtmlNode to determin validity for.</param>
        /// <returns></returns>
        bool IsValid(HtmlNode node);
    }

    /// <summary>
    /// Defines a filter for an html element that determines whether an html element should be removed.
    /// </summary>
    public class ElementFilter : IElementFilter
    {
        /// <summary>
        /// Gets or sets how the filter type for the attributes of an element.
        /// </summary>
        public FilterType FilterType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the list of elements that are either allowed or not allowed based on the mapped value.
        /// </summary>
        public IDictionary<string, Tuple<ElementMapType, IAttributeFilter>> MappedElements
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the default way to map an element if it is not referenced in the dictionary.
        /// </summary>
        public ElementMapType DefaultMapType
        {
            get;
            set;
        }

        /// <summary>
        /// Determines if the given HtmlNode is valid as determined by this filter.
        /// </summary>
        /// <param name="node">The HtmlNode to determin validity for.</param>
        /// <returns></returns>
        public bool IsValid(HtmlNode node)
        {
            if (MappedElements.ContainsKey(node.Name))
            {
                //if we should remove all of the bad attributes
                if (FilterType == HtmlSanitization.FilterType.RemoveBad)
                {
                    Tuple<ElementMapType, IAttributeFilter> elementFilter = MappedElements[node.Name];
                    if (elementFilter.Item1 == ElementMapType.Allow)
                    {
                        IAttributeFilter attributeFilter = elementFilter.Item2;
                        foreach (HtmlAttribute attr in node.Attributes)
                        {
                            if (!attributeFilter.IsValid(attr))
                            {
                                attr.Remove();
                            }
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    Tuple<ElementMapType, IAttributeFilter> elementFilter = MappedElements[node.Name];
                    if (elementFilter.Item1 == ElementMapType.Allow)
                    {
                        IAttributeFilter attributeFilter = elementFilter.Item2;

                        if (node.Attributes.Any(a => !attributeFilter.IsValid(a)))
                        {
                            //return invalid node because we are supposed to fail if an attribute does not pass.
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else if (node.NodeType == HtmlNodeType.Text)
            {
                return true;
            }
            else
            {
                return DefaultMapType == ElementMapType.Allow;
            }
        }
    }

    /// <summary>
    /// Defines an interface that defines the contract that all AttributeFilter objects should adhere to.
    /// </summary>
    public interface IAttributeFilter
    {
        /// <summary>
        /// Determines if the given attribute is valid as determined by this filter.
        /// </summary>
        /// <param name="attribute">The attribute to verify.</param>
        /// <returns></returns>
        bool IsValid(HtmlAttribute attribute);
    }

    /// <summary>
    /// Defines a filter for an html attribute that determines whether an html attribute should be removed.
    /// </summary>
    public class AttributeFilter : IAttributeFilter
    {
        /// <summary>
        /// Gets or sets the dictionary of attribute names that map to regular expressions that determine whether to keep or discard the attribue.
        /// </summary>
        public IDictionary<string, Tuple<ElementMapType, Predicate<HtmlAttribute>>> Attributes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the default mapping type that determines how to handle unmapped attributes.
        /// </summary>
        public ElementMapType DefaultMapType
        {
            get;
            set;
        }

        /// <summary>
        /// Determines if the given attributeText is valid as determined by this filter.
        /// </summary>
        /// <param name="attributeText">The attribute to verify.</param>
        /// <returns></returns>
        public bool IsValid(HtmlAttribute attribute)
        {
            if (Attributes.ContainsKey(attribute.Name))
            {
                if (Attributes[attribute.Name].Item2(attribute))
                {
                    //allow the attribute if we should
                    return Attributes[attribute.Name].Item1 == ElementMapType.Allow;
                }
                else
                {
                    //discard the attribute if we should
                    return Attributes[attribute.Name].Item1 != ElementMapType.Allow;
                }
            }
            else
            {
                return DefaultMapType == ElementMapType.Allow;
            }
        }
    }
}
using System;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace PlexLander.TagHelpers
{
    [HtmlTargetElement("li", Attributes = NavigationActiveClassAttributeName)]
    [HtmlTargetElement("li", Attributes = ActionAttributeName)]
    [HtmlTargetElement("li", Attributes = ControllerAttributeName)]
    public class BootstrapNavigationListTagHelper : TagHelper
    {
        private const string ActionAttributeName = "bs-action";
        private const string ControllerAttributeName = "bs-controller";
        private const string AreaAttributeName = "bs-area";
        private const string NavigationActiveClassAttributeName = "bs-navigation-active-class";
        private const string AttributeName = "class";

        /// <summary>
        /// The name of the action method.
        /// </summary>
        [HtmlAttributeName(ActionAttributeName)]
        public string Action { get; set; }

        /// <summary>
        /// The name of the controller.
        /// </summary>
        [HtmlAttributeName(ControllerAttributeName)]
        public string Controller { get; set; }

        /// <summary>
        /// The CSS class to set if the menu item is the current active menu item
        /// </summary>
        [HtmlAttributeName(NavigationActiveClassAttributeName)]
        public string ActiveClass { get; set; }


        [HtmlAttributeName(AreaAttributeName)]
        public string Area { get; set; } = String.Empty;

        /// <summary>
        /// Gets or sets the <see cref="Rendering.ViewContext"/> for the current request.
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public IHtmlGenerator Generator { get; set; }

        public BootstrapNavigationListTagHelper(IHtmlGenerator generator) : base()
        {
            Generator = generator;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            TagHelperContent content = await output.GetChildContentAsync();

            RouteValueDictionary currentRouteValues = ViewContext.RouteData.Values;
            RouteValueDictionary targetRouteValues = RouteValuesFromAttributes();

            if (!RouteValuesAreEqual(currentRouteValues, targetRouteValues))
                return; //we are done, this is not the active item

            if (!String.IsNullOrWhiteSpace(Area) && !AreAreasEqual(Area))
                return; //we are done, this is not the active item

            string classValue;
            if (output.Attributes.ContainsName(AttributeName))
            {
                //the user predefined one or more classes
                classValue = String.Format("{0} {1}", output.Attributes[AttributeName].Value, ActiveClass);
            }
            else
            {
                classValue = ActiveClass;
            }

            output.Attributes.SetAttribute(AttributeName, classValue);
            output.Content = content;
        }

        /// <summary>
        /// Check if the current anchor/area from the URL matches the given anchor/area
        /// </summary>
        /// <param name="expectedAnchor"></param>
        /// <returns></returns>
        private bool AreAreasEqual(string expectedAnchor)
        {
            PathString currentPath = ViewContext.HttpContext.Request.Path; //TODO: find better method, this does not actually include the area
            string area = String.Empty;
            if (currentPath.HasValue) //there is a path
            {
                var splitPath = currentPath.Value.Split('#');
                if (splitPath.Count() > 1)
                {
                    area = splitPath[1];
                    return area.Equals(Area, StringComparison.OrdinalIgnoreCase); //current path contains area, return if it equals to expected area
                }
            }

            return String.IsNullOrWhiteSpace(expectedAnchor); //no area defined an no area in current path
        }

        /// <summary>
        /// Gets route values from attributes on the tag
        /// </summary>
        /// <returns>A RouteValueDictionary</returns>
        private RouteValueDictionary RouteValuesFromAttributes()
        {
            RouteValueDictionary dictionary = new RouteValueDictionary();

            if (!String.IsNullOrEmpty(Controller))
            {
                dictionary.Add("controller", Controller);
            }

            if (!String.IsNullOrEmpty(Action))
            {
                dictionary.Add("action", Action);
            }

            return dictionary;
        }

        private bool RouteValuesAreEqual(RouteValueDictionary dictionaryA, RouteValueDictionary dictionaryB)
        {
            //do both have controllers defined?
            bool bothHaveControllers = DictionariesHaveMatchingKeys(dictionaryA, dictionaryB, "controller");

            //do both controllers have actions?
            bool bothHaveActions = DictionariesHaveMatchingKeys(dictionaryA, dictionaryB, "action");

            return bothHaveControllers && bothHaveActions;
        }

        /// <summary>
        /// Checks if two dictionaries have the same key and if so, if both keys .ToString()'s are equal
        /// </summary>
        /// <param name="dictionaryA">The first RouteValueDictionary</param>
        /// <param name="dictionaryB">The second RouteValueDictionary</param>
        /// <param name="key">The key to check for</param>
        /// <returns>true if the dictionaries both give a key</returns>
        private bool DictionariesHaveMatchingKeys(RouteValueDictionary dictionaryA, RouteValueDictionary dictionaryB, string key)
        {
            if (dictionaryA == null)
                throw new ArgumentNullException("dictionaryA");

            if (dictionaryB == null)
                throw new ArgumentNullException("dictionaryB");

            bool bothHaveKeys = dictionaryA.ContainsKey(key) && dictionaryB.ContainsKey(key);
            return bothHaveKeys && dictionaryA[key].ToString().Equals(dictionaryB[key].ToString(), StringComparison.OrdinalIgnoreCase);
        }
    }
}

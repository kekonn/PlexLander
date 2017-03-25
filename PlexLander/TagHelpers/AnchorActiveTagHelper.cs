using System;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using System.Threading.Tasks;

namespace PlexLander.TagHelpers
{
    [HtmlTargetElement("li", Attributes = NavigationActiveClassAttributeName)]
    [HtmlTargetElement("li", Attributes = ActionAttributeName)]
    [HtmlTargetElement("li", Attributes = ControllerAttributeName)]
    public class BootstrapNavigationListTagHelper : TagHelper
    {
        private const string ActionAttributeName = "bs-action";
        private const string ControllerAttributeName = "bs-controller";
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

        [HtmlAttributeName(NavigationActiveClassAttributeName)]
        public string ActiveClass { get; set; }

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

            if (!RouteValuesAreEqual(currentRouteValues,targetRouteValues))
                return; //we are done, this is not the active item

            string classValue;
            if (output.Attributes.ContainsName(AttributeName))
            {
                //the user predefined one or more classes
                classValue = String.Format("{0} {1}",output.Attributes[AttributeName].Value,ActiveClass);
            } else
            {
                classValue = ActiveClass;
            }

            output.Attributes.SetAttribute(AttributeName, classValue);
            output.Content = content;
        }

        private RouteValueDictionary RouteValuesFromAttributes()
        {
            RouteValueDictionary dictionary = new RouteValueDictionary();

            if (!String.IsNullOrEmpty(Controller))
            {
                dictionary.Add("controller", Controller);
            }

            if(!String.IsNullOrEmpty(Action))
            {
                dictionary.Add("action", Action);
            }


            return dictionary;
        }

        private bool RouteValuesAreEqual(RouteValueDictionary dictionaryA, RouteValueDictionary dictionaryB)
        {
            bool areEqual = false;

            //do both have controllers defined?
            bool bothHaveControllers = dictionaryA.ContainsKey("controller") && dictionaryB.ContainsKey("controller");
            if (bothHaveControllers)
            {
                areEqual = dictionaryA["controller"].ToString().Equals(dictionaryB["controller"].ToString(), StringComparison.OrdinalIgnoreCase);
            }

            bool bothHaveActions = dictionaryA.ContainsKey("action") && dictionaryB.ContainsKey("action");
            
            if (bothHaveActions)
                areEqual &= dictionaryA["action"].ToString().Equals(dictionaryB["action"].ToString(), StringComparison.OrdinalIgnoreCase);

            return areEqual;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace LpcThang
{
    public class LivePersonChat
    {
        public const string StateKey = "__livepersonchat__";
        public static IStateStorage StateStorage = new HttpContextStateStorage();
        private readonly IDictionary<string, object> _pageVariables = new Dictionary<string, object>();

        /// <summary>
        /// Page-scoped variables to be included in the request.
        /// </summary>
        public IDictionary<string, object> PageVariables
        {
            get { return _pageVariables; }
        }

        /// <summary>
        /// Signifies that an order (conversion) has been completed.
        /// </summary>
        public bool OrderCompleted { get; set; }

        /// <summary>
        /// LivePersonChat state for the current request.
        /// </summary>
        public static LivePersonChat Current
        {
            get
            {
                var current = StateStorage.Get<LivePersonChat>(StateKey);
                if (current == null)
                {
                    current = new LivePersonChat();
                    StateStorage.Set(StateKey, current);
                }
                return current;
            }
        }

        /// <summary>
        /// Adds a LivePerson page-scoped variable to the request.
        /// </summary>
        /// <param name="name">Name of the variable. <example>OrderTotal</example></param>
        /// <param name="value">Value of the variable. <example>12.99</example></param>
        public static void AddPageVariable(string name, object value)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("name");
            Current.PageVariables[name] = value;
        }

        /// <summary>
        /// Page variable name which is set with a value of 1 when an order is completed.
        /// Can be overridden by setting this field.
        /// </summary>
        public static string SalesOrderCompletedVariable = "OrderTotal";
        /// <summary>
        /// Page variable name used for eCommerce order totals.
        /// Can be overridden by setting this field.
        /// </summary>
        public static string SalesOrderTotalVariable = "sales_OrderTotal";
        /// <summary>
        /// Page variable name used for the order number when an order is completed.
        /// Can be overridden by setting this field.
        /// </summary>
        public static string SalesOrderNumberVariable = "OrderNumber";

        /// <summary>
        /// Signifies that an order (conversion) has been completed.
        /// This adds page variables for the order total and order number, and includes a script
        /// at the end of the page notifying LivePerson of the conversion.
        /// </summary>
        /// <param name="orderNumber">Your order number.</param>
        /// <param name="orderTotal">Total value of the order.</param>
        public static void SetOrderCompleted(string orderNumber, decimal orderTotal)
        {
            AddPageVariable(SalesOrderCompletedVariable, 1);
            AddPageVariable(SalesOrderNumberVariable, orderNumber);
            AddPageVariable(SalesOrderTotalVariable, orderTotal);
            Current.OrderCompleted = true;
        }

        /// <summary>
        /// Renders the required JavaScript onto the page for LivePerson chat, including all specified LP variables.
        /// Ensure that you include your client-specific mtagconfig.js script reference before this.
        /// Recommended to include this inside the body element at the bottom of the page.
        /// </summary>
        public static IHtmlString Render()
        {
            if (Current.PageVariables.Count == 0)
                return new HtmlString(string.Empty);

            var script = new StringBuilder();
            script.AppendLine(@"<script type=""text/javascript"">");

            addPageVariableScripts(script);

            if (Current.OrderCompleted)
                addTagLoadedScript(script);

            script.AppendLine(@"</script>");

            return new HtmlString(script.ToString());
        }

        private static void addPageVariableScripts(StringBuilder script)
        {
            const string lpAddVars = "lpAddVars(\"page\",\"{0}\",\"{1}\");";
            foreach (var kvp in Current.PageVariables)
                script.AppendLine(string.Format(lpAddVars, kvp.Key, kvp.Value));
        }

        private static void addTagLoadedScript(StringBuilder script)
        {
            const string tagLoadedScript =
                @"if(typeof lpMTagConfig.lpTagLoaded != 'undefined' && (!lpMTagConfig.lpTagLoaded)) " +
                @"{ lpMTagConfig.sendCookies=false; lpAddMonitorTag(); lpMTagConfig.lpTagLoaded=true; }";
            script.AppendLine(tagLoadedScript);
        }
    }
}
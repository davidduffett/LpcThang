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

            script.AppendLine(@"</script>");

            return new HtmlString(script.ToString());
        }

        private static void addPageVariableScripts(StringBuilder script)
        {
            const string lpAddVars = "lpAddVars(\"page\",\"{0}\",\"{1}\");";
            foreach (var kvp in Current.PageVariables)
                script.AppendLine(string.Format(lpAddVars, kvp.Key, kvp.Value));
        }
    }
}
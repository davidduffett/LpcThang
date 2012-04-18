using System;
using System.Web.Mvc;

namespace LpcThang
{
    /// <summary>
    /// Action filter attribute that can be applied to an entire controller or a single action method,
    /// specifying a LivePerson Chat page variable that should be included in the request.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class LivePersonChatPageVariableAttribute : ActionFilterAttribute
    {
        private readonly string _name;
        private readonly object _value;

        public LivePersonChatPageVariableAttribute(string name, object value)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("name");
            _name = name;
            _value = value;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.IsChildAction)
                LivePersonChat.AddPageVariable(_name, _value);
        }
    }
}

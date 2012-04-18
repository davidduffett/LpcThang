using System.Web.Mvc;

namespace LpcThang
{
    /// <summary>
    /// Action filter attribute that can be applied to an entire controller or a single action method,
    /// specifying which LivePerson Chat section the current action belongs to.
    /// </summary>
    public class LivePersonChatSectionAttribute : ActionFilterAttribute
    {
        private readonly string _section;

        public LivePersonChatSectionAttribute(string section)
        {
            _section = section;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.IsChildAction && !string.IsNullOrWhiteSpace(_section))
                LivePersonChat.AddPageVariable(LpcVariables.Section, _section);
        }
    }
}

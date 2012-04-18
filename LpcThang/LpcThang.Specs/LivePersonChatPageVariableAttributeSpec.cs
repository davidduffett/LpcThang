using System.Web.Mvc;
using Machine.Fakes;
using Machine.Specifications;

namespace LpcThang.Specs
{
    public abstract class LivePersonChatPageVariableAttributeContext : LivePersonChatContext
    {
        Establish context = () =>
        {
            ActionContext = An<ActionExecutingContext>();
            SUT = new LivePersonChatPageVariableAttribute(Name, Value);
        };

        Because of = () =>
            SUT.OnActionExecuting(ActionContext);

        protected static LivePersonChatPageVariableAttribute SUT;
        protected static ActionExecutingContext ActionContext;
        protected static string Name = "OrderTotal";
        protected static object Value = 12.99m;
    }

    [Subject(typeof(LivePersonChatPageVariableAttribute))]
    public class When_action_is_executing_with_live_person_chat_page_variable : LivePersonChatPageVariableAttributeContext
    {
        It should_set_live_person_chat_page_variable_for_the_request = () =>
            LivePersonChat.Current.PageVariables[Name].ShouldEqual(Value);
    }

    [Subject(typeof(LivePersonChatPageVariableAttribute))]
    public class When_child_action_is_executing_with_live_person_chat_page_variable : LivePersonChatPageVariableAttributeContext
    {
        It should_not_set_live_person_chat_page_variable_for_the_request = () =>
            LivePersonChat.Current.PageVariables.Keys.ShouldNotContain(Name);

        Establish context = () =>
            ActionContext.WhenToldTo(x => x.IsChildAction).Return(true);
    }
}

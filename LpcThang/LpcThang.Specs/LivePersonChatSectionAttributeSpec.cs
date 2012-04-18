using System.Web.Mvc;
using Machine.Fakes;
using Machine.Specifications;

namespace LpcThang.Specs
{
    public abstract class LivePersonChatSectionAttributeContext : LivePersonChatContext
    {
        Establish context = () =>
        {
            ActionContext = An<ActionExecutingContext>();
            SUT = new LivePersonChatSectionAttribute(Section);
        };

        Because of = () =>
            SUT.OnActionExecuting(ActionContext);

        protected static LivePersonChatSectionAttribute SUT;
        protected static ActionExecutingContext ActionContext;
        protected static string Section = "Account";
    }

    [Subject(typeof(LivePersonChatSectionAttribute))]
    public class When_action_is_executing_with_live_person_chat_section : LivePersonChatSectionAttributeContext
    {
        It should_set_live_person_chat_section_for_the_request = () =>
            LivePersonChat.Current.PageVariables["Section"].ShouldEqual(Section);
    }

    [Subject(typeof(LivePersonChatSectionAttribute))]
    public class When_child_action_is_executing_with_live_person_chat_section : LivePersonChatSectionAttributeContext
    {
        It should_not_set_live_person_chat_section_for_the_request = () =>
            LivePersonChat.Current.PageVariables.Keys.ShouldNotContain("Section");

        Establish context = () =>
            ActionContext.WhenToldTo(x => x.IsChildAction).Return(true);
    }
}

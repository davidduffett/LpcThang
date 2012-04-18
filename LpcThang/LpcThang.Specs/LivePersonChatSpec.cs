using System;
using Machine.Fakes;
using Machine.Specifications;

namespace LpcThang.Specs
{
    public abstract class LivePersonChatContext : WithFakes
    {
        Establish context = () =>
            LivePersonChat.StateStorage = new InMemoryStateStorage();

        Cleanup after = () =>
            LivePersonChat.StateStorage = new HttpContextStateStorage();
    }

    [Subject(typeof (LivePersonChat))]
    public class When_adding_a_page_scoped_variable : LivePersonChatContext
    {
        It should_add_it_to_the_current_live_person_chat = () =>
            LivePersonChat.Current.PageVariables["Section"].ShouldEqual("Accounts");

        Because of = () =>
            LivePersonChat.AddPageVariable("Section", "Accounts");
    }

    [Subject(typeof(LivePersonChat))]
    public class When_adding_a_page_scoped_variable_with_empty_name : LivePersonChatContext
    {
        It should_throw_an_argument_null_exception = () =>
            Exception.ShouldBeOfType<ArgumentNullException>();

        Because of = () =>
            Exception = Catch.Exception(() => LivePersonChat.AddPageVariable("", "Accounts"));

        static Exception Exception;
    }

    public abstract class LivePersonChatRenderContext : LivePersonChatContext
    {
        Because of = () =>
            Result = LivePersonChat.Render().ToString();

        protected static string Result;
    }

    [Subject(typeof (LivePersonChat))]
    public class When_rendering_live_person_javascript_with_no_variables : LivePersonChatRenderContext
    {
        It should_be_blank = () =>
            Result.ShouldBeEmpty();
    }

    [Subject(typeof(LivePersonChat))]
    public class When_rendering_live_person_javascript : LivePersonChatRenderContext
    {
        It should_include_lpaddvar_for_each_page_variable = () =>
        {
            foreach (var kvp in LivePersonChat.Current.PageVariables)
                Result.ShouldContain("lpAddVars(\"page\",\"" + kvp.Key + "\",\"" + kvp.Value);
        };

        Establish context = () =>
        {
            LivePersonChat.AddPageVariable("Section", "Accounts");
            LivePersonChat.AddPageVariable("OrderTotal", 12.99m);
        };
    }
}
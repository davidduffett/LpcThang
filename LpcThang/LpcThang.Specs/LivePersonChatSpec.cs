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

    [Subject(typeof(LivePersonChat))]
    public class When_setting_order_completed : LivePersonChatContext
    {
        It should_set_order_completed_to_true = () =>
            LivePersonChat.Current.OrderCompleted.ShouldBeTrue();

        It should_add_page_variable_for_order_completion_flag = () =>
            LivePersonChat.Current.PageVariables[LivePersonChat.SalesOrderCompletedVariable].ShouldEqual(1);

        It should_add_page_variable_for_order_total = () =>
            LivePersonChat.Current.PageVariables[LivePersonChat.SalesOrderTotalVariable].ShouldEqual(12.99m);

        It should_add_page_variable_for_order_number = () =>
            LivePersonChat.Current.PageVariables[LivePersonChat.SalesOrderNumberVariable].ShouldEqual("123456");

        Because of = () =>
            LivePersonChat.SetOrderCompleted("123456", 12.99m);
    }

    [Subject(typeof(LivePersonChat))]
    public class When_rendering_live_person_javascript_and_order_is_completed : LivePersonChatRenderContext
    {
        It should_include_lpTagLoaded_script = () =>
            Result.ShouldContain("lpMTagConfig.lpTagLoaded=true;");

        Establish context = () =>
            LivePersonChat.SetOrderCompleted("123456", 12.99m);
    }
}
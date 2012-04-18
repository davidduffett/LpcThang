using System;

namespace LpcThang
{
    /// <summary>
    /// Action filter attribute that can be applied to an entire controller or a single action method,
    /// specifying which LivePerson Chat sectionName the current action belongs to.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class LivePersonChatSectionAttribute : LivePersonChatPageVariableAttribute
    {
        public LivePersonChatSectionAttribute(string sectionName)
            : base("Section", sectionName)
        {
        }
    }
}

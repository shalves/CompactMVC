namespace System.Web
{
    public class ActionEventArgs : EventArgs
    {
        public string ControllerName { get; private set; }
        public string ActionName { get; private set; }

        public ActionEventArgs(string controllerName, string actionName)
        {
            this.ControllerName = controllerName;
            this.ActionName = actionName;
        }
    }

    public class ActionExecutionErrorEventArgs : ActionEventArgs
    {
        public Exception BaseException { get; private set; }

        public ActionExecutionErrorEventArgs(string controllerName, string actionName, Exception baseException) :
            base(controllerName, actionName)
        {
            this.BaseException = baseException;
        }
    }

    public class ActionAttributeValidateFailedEventArgs : ActionEventArgs
    {
        public ActionAttribute InvalidAttribute { get; private set; }

        public ActionAttributeValidateFailedEventArgs(string controllerName, string actionName, ActionAttribute attr)
            : base(controllerName, actionName)
        {
            this.InvalidAttribute = attr;
        }
    }
}

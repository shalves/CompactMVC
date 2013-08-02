namespace System.Web
{
    public class EventArgsBase : EventArgs
    {
        readonly string _ControllerName;
        public string ControllerName {
            get { return _ControllerName; }
        }

        public EventArgsBase(string controllerName) {
            this._ControllerName = controllerName;
        }
    }

    public class ActionEventArgs : EventArgsBase
    {
        readonly string _ActionName;
        public string ActionName { 
            get { return _ActionName; } 
        }

        public ActionEventArgs(string controllerName, string actionName) : base(controllerName) {
            this._ActionName = actionName;
        }
    }

    public class ActionExecutionErrorEventArgs : ActionEventArgs
    {
        readonly Exception _BaseException;
        public Exception BaseException {
            get { return _BaseException; }
        }

        public ActionExecutionErrorEventArgs(
            string controllerName, string actionName, Exception baseException) : base(controllerName, actionName) {
            this._BaseException = baseException;
        }
    }

    public class ActionAttributeValidateFailedEventArgs : ActionEventArgs
    {
        readonly AttributeBase _FailedAttribute;
        public AttributeBase FailedAttribute {
            get { return _FailedAttribute; }
        }

        public ActionAttributeValidateFailedEventArgs(
            string controllerName, string actionName, AttributeBase faildAttribute) : base(controllerName, actionName) {
            this._FailedAttribute = faildAttribute;
        }
    }

    public class ControllerAttributeValidateFailedEventArgs : EventArgsBase
    {
        readonly AttributeBase _FailedAttribute;
        public AttributeBase FailedAttribute {
            get { return _FailedAttribute; }
        }

        public ControllerAttributeValidateFailedEventArgs(string controllerName, AttributeBase faildAttribte) : base(controllerName) {
            this._FailedAttribute = faildAttribte;
        }
    }
}

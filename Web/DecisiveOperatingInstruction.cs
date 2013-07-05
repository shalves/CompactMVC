namespace System.Web
{
    /// <summary>
    /// 指示应用程序在执行完成某个重要执行点后，如何进行后续操作
    /// </summary>
    public class DecisiveOperatingInstruction
    {
        public enum Operation
        {
            NoOperation, Abort, ThrowException
        }

        Operation _ToDo;
        public Operation ToDo
        {
            get { return _ToDo; }
        }

        Exception _ExcpetionToThrow;
        public Exception ExcpetionToThrow
        {
            get { return _ExcpetionToThrow; }
        }

        DecisiveOperatingInstruction(Operation toDo)
        {
            _ToDo = toDo;
        }

        public static DecisiveOperatingInstruction NoOperation()
        {
            return new DecisiveOperatingInstruction(Operation.NoOperation); 
        }

        public static DecisiveOperatingInstruction Abort()
        {
            return new DecisiveOperatingInstruction(Operation.Abort);
        }

        public static DecisiveOperatingInstruction ThrowException(Exception e)
        {
            return new DecisiveOperatingInstruction(Operation.ThrowException)
            {
                _ExcpetionToThrow = e
            };
        }
    }
}

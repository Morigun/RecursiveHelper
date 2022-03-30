using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RecursiveHelper
{
    class RecursiveHelper
    {
        public byte RecursivDeepLimit { get; set; }
        public int WaitBetweenRecurseLevel { get; set; }
        public RecursiveHelper()
        {
            RecursivDeepLimit = 3;
            WaitBetweenRecurseLevel = 1000;
        }
        public RecursiveHelper(byte deepLimit, int waitBeetweenRecurseLevel)
        {
            RecursivDeepLimit = deepLimit;
            WaitBetweenRecurseLevel = waitBeetweenRecurseLevel;
        }
        public RecursiveResult<TResult> GetRecursiveRepeatMethodWrapper<TResult>(IMethodInvoker<TResult> method)
        {
            return StartRecursiveRepeatMethodWrapper(method);
        }
        private RecursiveResult<TResult> StartRecursiveRepeatMethodWrapper<TResult>(IMethodInvoker<TResult> method, int deepLevel = 0)
        {
            RecursiveResult<TResult> invokeResult = method.Invoke();
            if (invokeResult.IsSuccess)
                return invokeResult;
            if (CheckRecursivDeepLimit(deepLevel))
                return RecursionDeeper(method, deepLevel);
            else
                return invokeResult;
        }
        private bool CheckRecursivDeepLimit(int deepLevel)
        {
            return deepLevel < RecursivDeepLimit;
        }
        private RecursiveResult<TResult> RecursionDeeper<TResult>(IMethodInvoker<TResult> method, int deepLevel)
        {
            Thread.Sleep(WaitBetweenRecurseLevel);
            return StartRecursiveRepeatMethodWrapper(method, deepLevel + 1);
        }

    }

    interface IMethodInvoker<TResult>
    {
        RecursiveResult<TResult> Invoke();
    }

    class ActionInvoker<TResult> : IMethodInvoker<TResult>
    {
        public Action Method { get; set; }
        public RecursiveResult<TResult> Invoke()
        {
            RecursiveResult<TResult> result = new RecursiveResult<TResult>();
            try
            {
                InnerInvoke();
                result.IsSuccess = true;
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.CatchException = ex;
            }
            return result;
        }
        public virtual void InnerInvoke()
        {
            Method();
        }
    }
    class ActionInvoker<T1, TResult> : ActionInvoker<TResult>
    {
        public new Action<T1> Method { get; set; }
        public T1 Arg1 { get; set; }
        public override void InnerInvoke()
        {
            Method(Arg1);
        }
    }
    class ActionInvoker<T1, T2, TResult> : ActionInvoker<T1, TResult>
    {
        public new Action<T1, T2> Method { get; set; }
        public T2 Arg2 { get; set; }
        public override void InnerInvoke()
        {
            Method(Arg1, Arg2);
        }
    }
    class ActionInvoker<T1, T2, T3, TResult> : ActionInvoker<T1, T2, TResult>
    {
        public new Action<T1, T2, T3> Method { get; set; }
        public T3 Arg3 { get; set; }
        public override void InnerInvoke()
        {
            Method(Arg1, Arg2, Arg3);
        }
    }

    class FuncInvoker<TResult> : IMethodInvoker<TResult>
    {
        public Func<TResult> Method { get; set; }
        public Func<TResult, bool> CheckResult { get; set; }
        public RecursiveResult<TResult> Invoke()
        {
            RecursiveResult<TResult> result = new RecursiveResult<TResult>();
            try
            {
                result.Result = InnerInvoke();
                if (CheckResult?.Invoke(result.Result) ?? true)
                    result.IsSuccess = true;
                else
                    result.IsSuccess = false;
            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                result.CatchException = ex;
            }
            return result;
        }
        public virtual TResult InnerInvoke()
        {
            return Method();
        }
    }
    class FuncInvoker<T1, TResult> : FuncInvoker<TResult>
    {
        public new Func<T1, TResult> Method { get; set; }
        public T1 Arg1 { get; set; }
        public override TResult InnerInvoke()
        {
            return Method(Arg1);
        }
    }
    class FuncInvoker<T1, T2, TResult> : FuncInvoker<T1, TResult>
    {
        public new Func<T1, T2, TResult> Method { get; set; }
        public T2 Arg2 { get; set; }
        public override TResult InnerInvoke()
        {
            return Method(Arg1, Arg2);
        }
    }
    class FuncInvoker<T1, T2, T3, TResult> : FuncInvoker<T1, T2, TResult>
    {
        public new Func<T1, T2, T3, TResult> Method { get; set; }
        public T3 Arg3 { get; set; }
        public override TResult InnerInvoke()
        {
            return Method(Arg1, Arg2, Arg3);
        }
    }

    class RecursiveResult<TResult>
    {
        public bool IsSuccess { get; set; }
        public TResult Result { get; set; }
        public Exception CatchException { get; set; }
    }
}

# RecursiveHelper
A script that wraps a method(Action/Func) in recursion with depth control and waiting between recursive calls. Used to call unsupervised and crash-prone code.

An attempt to call the method(Method) again for Func occurs when an exception or incorrect result (CheckResult) occurs, for Action when an exception occurs.

Example:
```CS
//Example Func
int AnyArg = 1;

RecursiveHelper recursive = new RecursiveHelper(5, 3000);

FuncInvoker<int, bool> invoker = new FuncInvoker<int, bool> 
{
    Method = AnyMethod,
    Arg1 = AnyArg,
    CheckResult = AnyPredicate
}

bool result = recursive.GetRecursiveRepeatMethodWrapper(invoker).Result;
// Method for repeating
bool AnyMethod(int input)
{
    if (input == 1)
        return true;
    else
        return false;
}
// Method for checking the result
bool AnyPredicate(bool result)
{
    return result == true;
}
```

```CS
//Example Action
int AnyArg = 1;

RecursiveHelper recursive = new RecursiveHelper(5, 3000);

ActionInvoker<int> invoker = new ActionInvoker<int>
{
    Method = AnyMethod,
    Arg1 = AnyArg
}

recursive.GetRecursiveRepeatMethodWrapper(invoker);
// Method for repeating
void AnyMethod(int input)
{
    Console.WriteLine($"{nameof(AnyMethod)} - {input}");
}
```

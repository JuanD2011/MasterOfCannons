namespace Delegates
{
    public delegate void Action();
    public delegate void Action<in T1>(T1 arg);
    public delegate void Action<in T1, in T2>(T1 arg, T2 arg2);
    public delegate void Action<in T1, in T2, in T3>(T1 arg, T2 arg2, T3 arg3);
    public delegate void Action<in T1, in T2, in T3, in T4>(T1 arg, T2 arg2, T3 arg3, T4 arg4);

    public delegate TOut Func<out TOut>();
    public delegate TOut Func<in T1, out TOut>(T1 arg);
    public delegate TOut Func<in T1, in T2, out TOut>(T1 arg, T2 arg2);
    public delegate TOut Func<in T1, in T2, in T3, out TOut>(T1 arg, T2 arg2, T3 arg3);
}
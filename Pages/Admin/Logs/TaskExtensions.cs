namespace CodeMechanic.Async;

/// <summary>
/// Task Extensions
/// Source (or blame, if you prefer): https://jonlabelle.com/snippets/view/csharp/task-extensions
/// </summary>
public static class TaskExtensions
{
    /// <summary>
    /// From: https://github.com/facebook-csharp-sdk/facebook-csharp-sdk/blob/master/Source/Facebook/TaskExtensions.cs
    /// </summary>
    public static Task<T2> Then<T1, T2>(this Task<T1> first, Func<T1, T2> next)
    {
        if (first == null)
        {
            throw new ArgumentNullException("first");
        }
        if (next == null)
        {
            throw new ArgumentNullException("next");
        }

        TaskCompletionSource<T2> tcs = new TaskCompletionSource<T2>();

        first.ContinueWith(
            delegate
            {
                if (first.IsFaulted)
                {
                    tcs.TrySetException(first.Exception.InnerExceptions);
                }
                else if (first.IsCanceled)
                {
                    tcs.TrySetCanceled();
                }
                else
                {
                    try
                    {
                        T2 result = next(first.Result);
                        tcs.TrySetResult(result);
                    }
                    catch (Exception ex)
                    {
                        tcs.TrySetException(ex);
                    }
                }
            }
        );

        return tcs.Task;
    }

    public static Task ContinueInBackground<T>(this Task<T> task, Action<T> action)
    {
        return task.ContinueWith(
            t =>
            {
                if (t.IsFaulted || t.IsCanceled || t.Exception != null)
                {
                    return;
                }
                action(t.Result);
            },
            TaskScheduler.FromCurrentSynchronizationContext()
        );
    }

    // https://github.com/thedillonb/CodeHub/blob/master/CodeHub.Core/Utils/FireAndForgetTask.cs
    public static Task FireAndForget(this Task task)
    {
        return task.ContinueWith(
            t =>
            {
                if (t.IsFaulted)
                {
                    AggregateException aggException = t.Exception.Flatten();
                    foreach (Exception exception in aggException.InnerExceptions)
                    {
                        System.Diagnostics.Debug.WriteLine(
                            "Fire and Forget failed: "
                            + exception.Message
                            + " - "
                            + exception.StackTrace
                        );
                    }
                }
                else if (t.IsCanceled)
                {
                    System.Diagnostics.Debug.WriteLine("Fire and forget canceled.");
                }
            }
        );
    }

    public static async Task<T> Catch<T>(this Task<T> source, Func<Exception, T> handler = null)
    {
        try
        {
            return await source;
        }
        catch (Exception ex)
        {
            if (handler != null)
            {
                return handler(ex);
            }
            return default(T);
        }
    }

    /// <summary>
    /// This properly registers and unregisters the token when one of the operations completes.
    /// </summary>
    /// <remarks>https://github.com/davidfowl/AspNetCoreDiagnosticScenarios/blob/master/Scenarios/Infrastructure/TaskExtensions.cs</remarks>
    public static async Task<T> WithCancellation<T>(
        this Task<T> task,
        CancellationToken cancellationToken
    )
    {
        var tcs = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously); // deprecated???
        // TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();

        // This disposes the registration as soon as one of the tasks trigger
        using (
            cancellationToken.Register(
                state =>
                {
                    ((TaskCompletionSource<object>)state).TrySetResult(null);
                },
                tcs
            )
        )
        {
            Task resultTask = await Task.WhenAny(task, tcs.Task);
            if (resultTask == tcs.Task)
            {
                // Operation cancelled
                throw new OperationCanceledException(cancellationToken);
            }

            return await task;
        }
    }

    /// <summary>
    /// This method cancels the timer if the operation succesfully completes.
    /// </summary>
    /// <remarks>https://github.com/davidfowl/AspNetCoreDiagnosticScenarios/blob/master/Scenarios/Infrastructure/TaskExtensions.cs</remarks>
    public static async Task<T> TimeoutAfter<T>(this Task<T> task, TimeSpan timeout)
    {
        using (CancellationTokenSource cts = new CancellationTokenSource())
        {
            Task delayTask = Task.Delay(timeout, cts.Token);

            Task resultTask = await Task.WhenAny(task, delayTask);
            if (resultTask == delayTask)
            {
                // Operation cancelled
                throw new OperationCanceledException();
            }
            else
            {
                // Cancel the timer task so that it does not fire
                cts.Cancel();
            }

            return await task;
        }
    }

    /// <summary>
    /// Catches all Exceptions from a Task.WhenAll() invocation, not just the first one.
    /// 
    /// Usage & Credit: https://youtu.be/gW19LaAYczI?list=TLPQMTAxMDIwMjIqUi6MTLc6dg
    /// </summary>
    public static async Task<IEnumerable<T>> WhenAll<T>(params Task<T>[] tasks)
    {
        var all_tasks = Task.WhenAll(tasks);

        try
        {
            return await all_tasks;
        }
        catch (Exception)
        {
            // ignore exception entirely
        }

        throw all_tasks.Exception ?? throw new Exception("This can't possibly throw");
    }

    public static bool IsCausedBy(this Exception ex, CancellationToken cancellationToken) =>
        ex is OperationCanceledException && cancellationToken.IsCancellationRequested;
}
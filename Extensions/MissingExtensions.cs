namespace CodeMechanic.Extensions;

/// <summary>
/// credit: Insight.Database
/// </summary>
public static class MissingExtensions
{
    /// <summary>Determines if a string is null or all whitespace.</summary>
    /// <param name="value">The string to test.</param>
    /// <returns>False if the string contains at least one non-whitespace character.</returns>
    public static bool IsEmpty(this string value) => string.IsNullOrWhiteSpace(value);

    /// <summary>
    /// Returns the maximum value in a sequence or the default.
    /// </summary>
    /// <typeparam name="TSequence">The type of the sequence.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="sequence">The list to evaluate.</param>
    /// <param name="selector">A function to select the value.</param>
    /// <returns>The maximum selected value or the default.</returns>
    public static TValue MaxOrDefault<TSequence, TValue>(
        this IEnumerable<TSequence> sequence
        , Func<TSequence, TValue> selector
        , TValue fallback = default
    ) =>
        !sequence.Any<TSequence>()
            ? fallback
            : sequence.Max<TSequence, TValue>(selector);
    
    
    // public static T DestructureTo<T>(this )
}
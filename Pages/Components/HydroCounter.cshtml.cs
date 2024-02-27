// ~/Pages/Components/Counter.cshtml.cs

using Hydro;

public class HydroCounter : HydroComponent
{
    public int Count { get; set; }

    public void Add()
    {
        Count++;
    }
}
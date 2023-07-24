using Microsoft.AspNetCore.Mvc;

namespace TPOT_Links.Controllers;

[Produces("application/json")]
[Route("api/Car")]
public class CarController : Controller
{
    private readonly ICarService _carService;

    public CarController(ICarService carService)
    {
        _carService = carService;
    }

    // GET: api/Car
    [HttpGet]
    public IEnumerable<Car> Get()
    {
        return _carService.ReadAll();
    }
}

public interface ICarService
{
    IEnumerable<Car> ReadAll();
}

public class Car
{
    public string Make { get; set; } = "Acura";
    public string Model { get; set; } = "TL";
}
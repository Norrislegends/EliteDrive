using Microsoft.AspNetCore.Mvc;
using OP_Project.Repositories;
using OP_Project.Models;

namespace OP_Project.Controllers;

public class HomeController : Controller
{
    private readonly ICarRepository _carRepository;

    public HomeController(ICarRepository carRepository)
    {
        _carRepository = carRepository;
    }

    public async Task<IActionResult> Index(string? searchString, string? carType, decimal? maxPrice)
    {
        var cars = await _carRepository.GetAllAsync();

        if (!string.IsNullOrEmpty(searchString))
        {
            cars = cars.Where(c => c.Brand.Contains(searchString, StringComparison.OrdinalIgnoreCase) 
                                || c.Model.Contains(searchString, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrEmpty(carType))
        {
            cars = cars.Where(c => c.Type == carType);
        }

        if (maxPrice.HasValue)
        {
            cars = cars.Where(c => c.DailyRate <= maxPrice.Value);
        }

        ViewBag.CarTypes = (await _carRepository.GetAllAsync()).Select(c => c.Type).Distinct().ToList();
        
        return View(cars);
    }

    public IActionResult Privacy()
    {
        return View();
    }
}

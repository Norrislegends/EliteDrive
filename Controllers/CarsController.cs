using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OP_Project.Models;
using OP_Project.Repositories;

namespace OP_Project.Controllers;

public class CarsController : Controller
{
    private readonly ICarRepository _carRepository;

    public CarsController(ICarRepository carRepository)
    {
        _carRepository = carRepository;
    }

    // GET: Cars/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var car = await _carRepository.GetByIdAsync(id.Value);
        if (car == null) return NotFound();

        return View(car);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Index()
    {
        var cars = await _carRepository.GetAllAsync();
        return View(cars);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Create() => View();

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Car car)
    {
        if (ModelState.IsValid)
        {
            await _carRepository.AddAsync(car);
            await _carRepository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(car);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var car = await _carRepository.GetByIdAsync(id.Value);
        if (car == null) return NotFound();
        return View(car);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Car car)
    {
        if (id != car.Id) return NotFound();

        if (ModelState.IsValid)
        {
            _carRepository.Update(car);
            await _carRepository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(car);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var car = await _carRepository.GetByIdAsync(id.Value);
        if (car == null) return NotFound();
        return View(car);
    }

    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var car = await _carRepository.GetByIdAsync(id);
        if (car != null)
        {
            _carRepository.Remove(car);
            await _carRepository.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}

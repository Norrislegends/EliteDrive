using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OP_Project.Models;
using OP_Project.Repositories;
using System.Security.Claims;

namespace OP_Project.Controllers;

[Authorize]
public class RentalsController : Controller
{
    private readonly IRentalRepository _rentalRepository;
    private readonly ICarRepository _carRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public RentalsController(
        IRentalRepository rentalRepository,
        ICarRepository carRepository,
        UserManager<ApplicationUser> userManager)
    {
        _rentalRepository = rentalRepository;
        _carRepository = carRepository;
        _userManager = userManager;
    }

    // GET: Rentals/Create?carId=5&startDate=...&endDate=...
    public async Task<IActionResult> Create(int carId, DateTime startDate, DateTime endDate)
    {
        var car = await _carRepository.GetByIdAsync(carId);
        if (car == null) return NotFound();

        var totalDays = (endDate - startDate).Days;
        if (totalDays <= 0) totalDays = 1;

        var rental = new Rental
        {
            CarId = carId,
            Car = car,
            StartDate = startDate,
            EndDate = endDate,
            TotalDays = totalDays,
            TotalPrice = totalDays * car.DailyRate,
            UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? ""
        };

        return View(rental);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Confirm(Rental rental)
    {
        // Re-calculate to prevent manipulation
        var car = await _carRepository.GetByIdAsync(rental.CarId);
        if (car == null) return NotFound();

        rental.TotalDays = (rental.EndDate - rental.StartDate).Days;
        rental.TotalPrice = rental.TotalDays * car.DailyRate;
        rental.Status = RentalStatus.Pending;
        rental.CreatedAt = DateTime.UtcNow;
        rental.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

        await _rentalRepository.AddAsync(rental);
        await _rentalRepository.SaveChangesAsync();

        return RedirectToAction(nameof(MyRentals));
    }

    public async Task<IActionResult> MyRentals()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Challenge();

        var rentals = await _rentalRepository.GetUserRentalsAsync(userId);
        return View(rentals);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id)
    {
        var rental = await _rentalRepository.GetByIdAsync(id);
        if (rental == null) return NotFound();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (rental.UserId != userId) return Forbid();

        if (rental.Status == RentalStatus.Pending)
        {
            rental.Status = RentalStatus.Cancelled;
            _rentalRepository.Update(rental);
            await _rentalRepository.SaveChangesAsync();
        }

        return RedirectToAction(nameof(MyRentals));
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OP_Project.Models;
using OP_Project.Models.ViewModels;
using OP_Project.Repositories;

namespace OP_Project.Controllers;

[Authorize(Roles = "Admin")]
public class DashboardController : Controller
{
    private readonly ICarRepository _carRepository;
    private readonly IRentalRepository _rentalRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public DashboardController(
        ICarRepository carRepository,
        IRentalRepository rentalRepository,
        UserManager<ApplicationUser> userManager)
    {
        _carRepository = carRepository;
        _rentalRepository = rentalRepository;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var cars = await _carRepository.GetAllAsync();
        var rentals = await _rentalRepository.GetAllRentalsWithDetailsAsync();
        var users = await _userManager.Users.ToListAsync();

        var viewModel = new DashboardViewModel
        {
            TotalCars = cars.Count(),
            TotalRentals = rentals.Count(),
            ActiveRentals = rentals.Count(r => r.Status == RentalStatus.Active),
            TotalUsers = users.Count,
            TotalRevenue = rentals.Where(r => r.Status != RentalStatus.Cancelled).Sum(r => r.TotalPrice),
            RecentRentals = rentals.Take(5)
        };

        return View(viewModel);
    }
}

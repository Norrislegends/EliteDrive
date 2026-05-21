namespace OP_Project.Models;

public class Car
{
    public int car_id { get; set; }

    public string make { get; set; }

    public string model { get; set; }

    public int year { get; set; }

    public string car_type { get; set; }

    public int seat_capacity { get; set; }

    public string transmission { get; set; }

    public decimal gas_per_hour { get; set; }

    public decimal rental_price_per_day { get; set; }
}
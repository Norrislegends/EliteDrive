namespace OP_Project.Models;

public class Rental
{
    public int rental_id { get; set; }

    public int user_id { get; set; }

    public int car_id { get; set; }

    public DateTime start_date { get; set; }

    public DateTime end_date { get; set; }

    public decimal total_price { get; set; }

    public string status { get; set; }

    public DateTime created_at { get; set; }
}
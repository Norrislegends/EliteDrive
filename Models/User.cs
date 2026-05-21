namespace OP_Project.Models;

public class User
{
    public int user_id { get; set; }

    public string full_name { get; set; }

    public string email { get; set; }

    public string password { get; set; }

    public string role { get; set; }

    public string phone_number { get; set; }

    public string gender { get; set; }

    public DateTime date_of_birth { get; set; }

    public DateTime created_at { get; set; }
}
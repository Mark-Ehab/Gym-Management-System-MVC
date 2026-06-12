using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DataAccess.Models;

public sealed class Session : BaseEntity
{
    public string Description { get; set; } = default!;
    public byte Capacity { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid TrainerId { get; set; }
    public Trainer Trainer { get; set; } = default!;
    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = default!;
    public ICollection<Booking> Bookings { get; set; } = [];
    public string Status 
    {
        get 
        {
            if(StartDate > DateTime.Now)
            {
                return "Upcoming";
            }

            if(EndDate > DateTime.Now)
            {
                return "Ongoing";
            }

            return "Completed";
        }
    }
}

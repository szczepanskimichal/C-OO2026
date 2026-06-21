namespace BookingApp;

public class Booking
{
    public int Id { get; private set; }
    public string CustomerName { get; private set; }
    public string Room { get; private set; }
    public DateOnly Date { get; private set; }
    public TimeOnly Start { get; private set; }
    public TimeOnly End { get; private set; }
    public string Description { get; private set; }

    public Booking(
        int id,
        string customerName,
        string room,
        DateOnly date,
        TimeOnly startTime,
        TimeOnly endTime,
        string description)
    {
        Id = id;
        CustomerName = customerName;
        Room = room;
        Date = date;
        Start = startTime;
        End = endTime;
        Description = description;
    }

    public void Update(
        string customerName,
        string room,
        DateOnly date,
        TimeOnly startTime,
        TimeOnly endTime,
        string description
    )
    {
        CustomerName = customerName;
        Room = room;
        Date = date;
        Start = startTime;
        End = endTime;
        Description = description;
    }
}
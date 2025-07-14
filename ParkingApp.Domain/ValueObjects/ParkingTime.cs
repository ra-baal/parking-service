namespace ParkingApp.Domain.ValueObjects;

public class ParkingTime
{
    public DateOnly Date { get; init; }
    public TimeOnly Start { get; init; }
    public TimeOnly End { get; init; }

    public ParkingTime(DateTimeOffset start, DateTimeOffset end) 
        : this(DateOnly.FromDateTime(start.Date), TimeOnly.FromDateTime(start.DateTime), TimeOnly.FromDateTime(end.DateTime))
    { 
        if (start.Date != end.Date)
        {
            throw new ArgumentException("Start and ends must be same day");
        }
    }

    public ParkingTime(DateOnly date, TimeOnly start, TimeOnly end)
    {
        if (end <= start)
        {
            throw new ArgumentException("End must be after start");
        }

        Date = date;
        Start = start;
        End = end;
    }

    public TimeSpan TotalDuration => End - Start;
    public bool IsWeekend => Date.DayOfWeek == DayOfWeek.Saturday || Date.DayOfWeek == DayOfWeek.Sunday;
}

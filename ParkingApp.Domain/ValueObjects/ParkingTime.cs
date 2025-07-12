namespace ParkingApp.Domain.ValueObjects;

public class ParkingTime
{
    public DateTime Start { get; init; }
    public DateTime End { get; init; }
    public DayOfWeek Day { get; init; }


    public ParkingTime(DateTime start, DateTime end)
    {
        if (end <= start) throw new ArgumentException("End must be after start");
        Start = start;
        End = end;
        Day = start.DayOfWeek;
    }

    public int TotalHoursRoundedUp() => (int)Math.Ceiling(TotalDuration.TotalHours);
    public TimeSpan TotalDuration => End - Start;
    public bool IsWeekend => Day == DayOfWeek.Saturday || Day == DayOfWeek.Sunday;
    
}

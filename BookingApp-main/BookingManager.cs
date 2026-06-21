using System.Text.Json;

namespace BookingApp;

public class BookingManager
{
    private readonly List<Booking> _bookings = [];

    public BookingManager()
    {
        LoadFromFile();
    }

    public List<Booking> GetAll() => _bookings;

    public List<int> GetAllIds()
    {
        return _bookings.Select(b => b.Id).ToList();
    }

    public string AddBooking(
        string customerName,
        string room,
        string date,
        string startTime,
        string endTime,
        string description)
    {
        if (!DateOnly.TryParse(date, out DateOnly parsedDate))
            return "Ugyldig dato. Bruk format yyyy-mm-dd.";

        if (!TimeOnly.TryParse(startTime, out TimeOnly parsedStartTime))
            return "Ugyldig starttid. Bruk format HH:mm.";

        if (!TimeOnly.TryParse(endTime, out TimeOnly parsedEndTime))
            return "Ugyldig sluttid. Bruk format HH:mm.";

        if (parsedStartTime >= parsedEndTime)
            return "Starttid må være før sluttid.";

        bool roomIsBusy = _bookings.Any(b =>
            b.Room == room &&
            b.Date == parsedDate &&
            IsOverlapping(parsedStartTime, parsedEndTime, b.Start, b.End));

        if (roomIsBusy)
            return "Rommet er opptatt i dette tidsrommet.";

        int newId = _bookings.Count == 0 ? 1 : _bookings.Max(b => b.Id) + 1;

        var booking = new Booking(
            newId,
            customerName,
            room,
            parsedDate,
            parsedStartTime,
            parsedEndTime,
            description);

        _bookings.Add(booking);
        SaveToFile();

        return "Bookingen ble lagt til.";
    }

    public bool DeleteBooking(int id)
    {
        var booking = _bookings.FirstOrDefault(b => b.Id == id);

        if (booking == null)
            return false;

        _bookings.Remove(booking);
        SaveToFile();

        return true;
    }

    public string EditBooking(
        int id,
        string customerName,
        string room,
        string date,
        string startTime,
        string endTime,
        string description)
    {
        var booking = _bookings.FirstOrDefault(b => b.Id == id);

        if (booking == null)
            return "Fant ingen booking med denne ID-en.";

        if (!DateOnly.TryParse(date, out DateOnly parsedDate))
            return "Ugyldig dato. Bruk format yyyy-mm-dd.";

        if (!TimeOnly.TryParse(startTime, out TimeOnly parsedStartTime))
            return "Ugyldig starttid. Bruk format HH:mm.";

        if (!TimeOnly.TryParse(endTime, out TimeOnly parsedEndTime))
            return "Ugyldig sluttid. Bruk format HH:mm.";

        if (parsedStartTime >= parsedEndTime)
            return "Starttid må være før sluttid.";

        bool roomIsBusy = _bookings.Any(b =>
            b.Id != id &&
            b.Room == room &&
            b.Date == parsedDate &&
            IsOverlapping(parsedStartTime, parsedEndTime, b.Start, b.End));

        if (roomIsBusy)
            return "Rommet er opptatt i dette tidsrommet.";

        booking.Update(
            customerName,
            room,
            parsedDate,
            parsedStartTime,
            parsedEndTime,
            description);

        SaveToFile();

        return "Bookingen ble oppdatert.";
    }

    private void LoadFromFile()
    {
        var dataList = BookingData.LoadFromFile();

        if (dataList == null)
            return;

        foreach (var data in dataList)
        {
            var booking = new Booking(
                data.Id,
                data.CustomerName,
                data.Room,
                data.Date,
                data.StartTime,
                data.EndTime,
                data.Description);

            _bookings.Add(booking);
        }
    }

    private void SaveToFile()
    {
        var dataList = _bookings.Select(b => new BookingData
        {
            Id = b.Id,
            CustomerName = b.CustomerName,
            Room = b.Room,
            Date = b.Date,
            StartTime = b.Start,
            EndTime = b.End,
            Description = b.Description
        }).ToList();

        var json = JsonSerializer.Serialize(dataList, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText("bookings.json", json);
    }

    private bool IsOverlapping(
        TimeOnly newStart,
        TimeOnly newEnd,
        TimeOnly existingStart,
        TimeOnly existingEnd)
    {
        return newStart < existingEnd && newEnd > existingStart;
    }
}
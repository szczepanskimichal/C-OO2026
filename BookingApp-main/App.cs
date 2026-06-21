namespace BookingApp;

public class App
{
    private readonly BookingManager _manager = new BookingManager();

    public void Run()
    {
        while (true)
        {
            var choice = BookingConsole.AskForMenuChoice();

            switch (choice)
            {
                case "1":
                    BookingConsole.ShowBookings(_manager.GetAll());
                    break;

                case "2":
                {
                    string customerName = BookingConsole.AskForString("Navn");
                    string room = BookingConsole.AskForString("Rom");
                    string date = BookingConsole.AskForString("Dato yyyy-mm-dd");
                    string startTime = BookingConsole.AskForString("Starttid HH:mm");
                    string endTime = BookingConsole.AskForString("Sluttid HH:mm");
                    string description = BookingConsole.AskForString("Beskrivelse");

                    string message = _manager.AddBooking(
                        customerName,
                        room,
                        date,
                        startTime,
                        endTime,
                        description);

                    BookingConsole.ShowMessage(message);
                    break;
                }

                case "3":
                {
                    var ids = _manager.GetAllIds();

                    if (ids.Count == 0)
                    {
                        BookingConsole.ShowMessage("Ingen bookinger å redigere.");
                        break;
                    }

                    int id = BookingConsole.AskForId(
                        ids,
                        "Velg bookingen du vil redigere");

                    var booking = _manager.GetAll().First(b => b.Id == id);

                    string customerName = BookingConsole.AskForString("Navn", booking.CustomerName);
                    string room = BookingConsole.AskForString(booking.Room);
                    string date = BookingConsole.AskForString("Dato yyyy-mm-dd", booking.Date.ToString("yyyy-MM-dd"));
                    string startTime = BookingConsole.AskForString("Starttid HH:mm", booking.Start.ToString("HH:mm"));
                    string endTime = BookingConsole.AskForString("Sluttid HH:mm", booking.End.ToString("HH:mm"));
                    string description = BookingConsole.AskForString("Beskrivelse", booking.Description);

                    string message = _manager.EditBooking(
                        id,
                        customerName,
                        room,
                        date,
                        startTime,
                        endTime,
                        description);

                    BookingConsole.ShowMessage(message);
                    break;
                }

                case "4":
                {
                    var ids = _manager.GetAllIds();

                    if (ids.Count == 0)
                    {
                        BookingConsole.ShowMessage("Ingen bookinger å slette.");
                        break;
                    }

                    int id = BookingConsole.AskForId(
                        ids,
                        "Velg bookingen du vil slette");

                    bool deleted = _manager.DeleteBooking(id);

                    BookingConsole.ShowMessage(
                        deleted
                            ? "Bookingen ble slettet."
                            : "Fant ingen booking med denne ID-en.");

                    break;
                }

                case "0":
                    return;

                default:
                    BookingConsole.ShowMessage("Ugyldig valg. Prøv igjen.");
                    break;
            }
        }
    }
}
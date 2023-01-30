using ServiceErrors;

namespace Domain;
using ErrorOr;


public class Event
{
    public const int MinNameLength = 1;
    public const int MaxDetailsLength = 1000;
    
    
    public Guid Id { get; private set; }
    public string Name { get; private set; } = "";
    public DateTime StartTime  { get; private set; }
    public string Location { get; private set; } = "";
    public string Details { get; private set; } = "";

    public List<Participant> Participants { get; private set; } = new List<Participant>();
    public List<Corporation> Corporations { get; private set; } = new List<Corporation>();

    private Event() {}
    
    private Event(Guid id, string name, DateTime startTime, string location, string details, List<Participant> participants, List<Corporation> corporations)
    {
        Id = id;
        Name = name;
        StartTime = startTime;
        Location = location;
        Details = details;
        Participants = participants;
        Corporations = corporations;
    }

    public static ErrorOr<Event> Create(string name, DateTime startTime, string location, string details,
        List<Participant> participants, List<Corporation> corporations, Guid? id = null)
    {
        List<Error> errors = new List<Error>();
        if (name.Length < MinNameLength)
        {
            errors.Add( Errors.Event.NameTooShort(MinNameLength));
        }

        if (details.Length > MaxDetailsLength)
        {
            errors.Add(Errors.Event.DetailsTooLong(MaxDetailsLength));
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return new Event(
            id ?? Guid.NewGuid(),
            name,
            startTime,
            location,
            details,
            participants,
            corporations);
    }

}
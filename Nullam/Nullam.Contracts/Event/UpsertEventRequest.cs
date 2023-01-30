using Domain;

namespace Nullam.Contracts.Event;

public record UpsertEventRequest(
    string Name,
    DateTime  StartTime,
    string Location,
    string Details,
    List<Domain.Participant> Participants,
    List<Domain.Corporation> Corporations);
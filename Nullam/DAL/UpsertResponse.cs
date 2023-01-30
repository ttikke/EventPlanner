namespace DAL.db;

public record struct UpsertedEvent(bool IsNewEvent);
public record struct UpsertedParticipant(bool IsNewParticipant);
public record struct UpsertedCorporation(bool IsNewCorporation);
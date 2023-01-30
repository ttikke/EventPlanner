using DAL.db;
using Domain;
using ErrorOr;

namespace DAL;

public interface IEventRepository
{
    ErrorOr<Created> CreateEvent(Event eventT);
    ErrorOr<Event> GetEvent(Guid id);
    ErrorOr<List<Event>> GetEvents();
    ErrorOr<UpsertedEvent> UpsertEvent(Event request);
    ErrorOr<Deleted> DeleteEvent(Guid id);
    ErrorOr<UpsertedEvent> AddParticipantToEvent(Event eventToBeAddedTo, Participant participant);
    ErrorOr<UpsertedEvent> AddCorporationToEvent(Event eventToBeAddedTo, Corporation corporation);
    ErrorOr<UpsertedEvent> DeleteCorpFromEvent(Event eventToBeDeletedFrom, Guid corpId);
    ErrorOr<UpsertedEvent> DeleteParticipantFromEvent(Event eventToBeDeletedFrom, Guid particId);
    ErrorOr<UpsertedEvent> UpsertCorpInEvent(Event eventToBeUpdated, Corporation updatedCorporation);
    ErrorOr<UpsertedEvent> UpsertParticipantInEvent(Event eventToBeUpdated, Participant updatedParticipant);
}
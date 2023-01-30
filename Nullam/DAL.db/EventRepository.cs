using Domain;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using ServiceErrors;

namespace DAL.db;

public class EventRepository : IEventRepository
{
    private readonly NullamDbContext _dbContext;

    public EventRepository(NullamDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public ErrorOr<Created> CreateEvent(Event eventT)
    {
        _dbContext.Add(eventT);
        _dbContext.SaveChanges();
        return Result.Created;
    }

    public ErrorOr<Event> GetEvent(Guid id)
    {
        if (_dbContext.Events.Find(id) is Event eventT)
        {
            return _dbContext.Events
                .Include(e => e.Participants)
                .Include(e => e.Corporations)
                .First(e=> e.Id == id);
        }
        return Errors.Event.NotFound;
    }

    public ErrorOr<List<Event>> GetEvents()
    {
        return _dbContext.Events
            .Include(e => e.Participants)
            .Include(e => e.Corporations)
            .ToList();
    }

    public ErrorOr<UpsertedEvent> UpsertEvent(Event request)
    {
        var isNew = !_dbContext.Events.Any(e => e.Id == request.Id);

        if (isNew)
        {
            _dbContext.Events.Add(request);
        }
        else
        {
            _dbContext.Events.Update(request);
        }

        _dbContext.SaveChanges();

        return new UpsertedEvent(isNew);
    }

    public ErrorOr<Deleted> DeleteEvent(Guid id)
    {
        var eventOrError = GetEvent(id);
        if ( eventOrError.IsError)
        {
            return Errors.Event.NotFound;
        }

        Event eventT = eventOrError.Value;
        
        _dbContext.Participants.RemoveRange(eventT.Participants);
        _dbContext.Corporations.RemoveRange(eventT.Corporations);

        _dbContext.Remove(eventT);
        _dbContext.SaveChanges();

        return Result.Deleted;
    }

    
    //Participant and Corporation in event manipulation
    
    //Get event corporation, participant
    public ErrorOr<Participant> GetEventParticipant(Guid id, Guid participantId)
    {
        var eventOrError = GetEvent(id);
        if ( eventOrError.IsError)
        {
            return Errors.Event.NotFound;
        }

        Event eventT = eventOrError.Value;

        var participant =eventT.Participants.Find(p => p.Id == participantId);

        if (participant != null)
        {
            return participant;
        }
        
        return Errors.Event.NotFound;
    }
    
    public ErrorOr<Corporation> GetEventCorporation(Guid id, Guid corporationId)
    {
        var eventOrError = GetEvent(id);
        if ( eventOrError.IsError)
        {
            return Errors.Event.NotFound;
        }

        Event eventT = eventOrError.Value;

        var corporation =eventT.Corporations.Find(c => c.Id == corporationId);

        if (corporation != null)
        {
            return corporation;
        }
        
        return Errors.Event.NotFound;
    }
    
    
    //Add corporation, participant to event
    public ErrorOr<UpsertedEvent> AddParticipantToEvent(Event eventToBeAddedTo, Participant participant)
    {
        var eventOrError = GetEvent(eventToBeAddedTo.Id);

        if (eventOrError.IsError)
        {
            return eventOrError.Errors;
        }

        var eventT = eventOrError.Value;
        _dbContext.Add(participant);

        eventT.Participants.Add(participant);
        var upsertEventOrError = UpsertEvent(eventT);
        return upsertEventOrError;
    }
    
    public ErrorOr<UpsertedEvent> AddCorporationToEvent(Event eventToBeAddedTo, Corporation corporation)
    {
        var eventOrError = GetEvent(eventToBeAddedTo.Id);

        if (eventOrError.IsError)
        {
            return eventOrError.Errors;
        }

        var eventT = eventOrError.Value;
        _dbContext.Add(corporation);

        eventT.Corporations.Add(corporation);
        var upsertEventOrError = UpsertEvent(eventT);
        return upsertEventOrError;
    }
    
    //Delete corporation, participant from event
    public ErrorOr<UpsertedEvent> DeleteCorpFromEvent(Event eventToBeDeletedFrom, Guid corpId)
    {
        var eventOrError = GetEvent(eventToBeDeletedFrom.Id);
        
        if (eventOrError.IsError)
        {
            return eventOrError.Errors;
        }
        
        var eventT = eventOrError.Value;

        var corporation = eventT.Corporations.Find(c => c.Id == corpId);
        if (corporation != null)
        {
            eventT.Corporations.Remove(corporation);
            _dbContext.Remove(corporation);
            _dbContext.SaveChanges();
        }

        var upsertEventOrError = UpsertEvent(eventT);
        return upsertEventOrError;
    }
    
    public ErrorOr<UpsertedEvent> DeleteParticipantFromEvent(Event eventToBeDeletedFrom, Guid particId)
    {
        var eventOrError = GetEvent(eventToBeDeletedFrom.Id);
        
        if (eventOrError.IsError)
        {
            return eventOrError.Errors;
        }
        
        var eventT = eventOrError.Value;

        var participant = eventT.Participants.Find(p => p.Id == particId);
        if (participant != null)
        {
            eventT.Participants.Remove(participant);
            _dbContext.Remove(participant);
            _dbContext.SaveChanges();
        }

        var upsertEventOrError = UpsertEvent(eventT);
        return upsertEventOrError;
    }
    
    //Upsert corporation, participant in event
    public ErrorOr<UpsertedEvent> UpsertCorpInEvent(Event eventToBeUpdated, Corporation corp)
    {
        var eventOrError = GetEvent(eventToBeUpdated.Id);
        
        if (eventOrError.IsError)
        {
            return eventOrError.Errors;
        }
        
        var eventT = eventOrError.Value;

        var oldCorporation = eventT.Corporations.Find(c => c.Id == corp.Id);
        if (oldCorporation != null)
        {
            eventT.Corporations.Remove(oldCorporation);
            _dbContext.Remove(oldCorporation);
            _dbContext.SaveChanges();
        }
        eventT.Corporations.Add(corp);

        var upsertEventOrError = UpsertEvent(eventT);
        return upsertEventOrError;
    }
    
    public ErrorOr<UpsertedEvent> UpsertParticipantInEvent(Event eventToBeUpdated, Participant participant)
    {
        var eventOrError = GetEvent(eventToBeUpdated.Id);
        
        if (eventOrError.IsError)
        {
            return eventOrError.Errors;
        }
        
        var eventT = eventOrError.Value;

        var oldParticipant = eventT.Participants.Find(p => p.Id == participant.Id);
        if (oldParticipant != null)
        {
            eventT.Participants.Remove(oldParticipant);
            _dbContext.Remove(oldParticipant);
            _dbContext.SaveChanges();
        }
        eventT.Participants.Add(participant);

        var upsertEventOrError = UpsertEvent(eventT);
        return upsertEventOrError;
    }
}
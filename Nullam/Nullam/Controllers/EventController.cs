using DAL;
using Domain;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Nullam.Contracts.Corporation;
using Nullam.Contracts.Event;
using Nullam.Contracts.Participant;
using ServiceErrors;

namespace Nullam.Controllers;
public class EventController : ApiController
{
    private readonly IEventRepository _eventRepository;

    public EventController(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    [HttpPost]
    public IActionResult CreateEvent(CreateEventRequest request)
    {
        var eventOrError = EventFromRequest(request);

        if (eventOrError.IsError)
        {
            return Problem(eventOrError.Errors);
        }

        var eventT = eventOrError.Value;
        
        
        var createEventOrError = _eventRepository.CreateEvent(eventT);

        return createEventOrError.Match(created => CreatedAtGetEvent(eventT),
            errors => Problem(errors)) ;
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetEvent(Guid id)
    {
        var getEventOrError = _eventRepository.GetEvent(id);

        return getEventOrError.Match(eventT => Ok(EventResponseMapper(eventT)),
            errors => Problem(errors));
        
    }
    
    [HttpGet]
    public IActionResult GetEvents()
    {
        var getEventsOrError = _eventRepository.GetEvents();

        return getEventsOrError.Match(events => Ok(EventsListToResponse(events)),
            errors => Problem(errors));
        
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpsertEvent(Guid id, UpsertEventRequest request)
    {
        var eventOrError = EventFromRequest(request, id);

        if (eventOrError.IsError)
        {
            return Problem(eventOrError.Errors);
        }

        var eventT = eventOrError.Value;
        
        var upsertedEventOrError = _eventRepository.UpsertEvent(eventT);
        
        //return 201 if new event was created
        return upsertedEventOrError.Match(upserted => upserted.IsNewEvent ? CreatedAtGetEvent(eventT) : NoContent(),
            errors => Problem(errors));
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteEvent(Guid id)
    {
        var deleteEventOrError = _eventRepository.DeleteEvent(id);

       return deleteEventOrError.Match(deleted => NoContent(), errors => Problem(errors));
    }

    
    
    //Adding participants and corporations
    [HttpPost("{id:guid}/participants")]
    public IActionResult AddParticipantToEvent(Guid id, CreateParticipantRequest participantRequest)
    {
        var getEventOrError = _eventRepository.GetEvent(id);
        
        if (getEventOrError.IsError)
        {
            return Problem(getEventOrError.Errors);
        }
        
        var participantOrError = ParticipantFromRequest(participantRequest);
        if (participantOrError.IsError)
        {
            return Problem(participantOrError.Errors);
        }

        var participant = participantOrError.Value;
        
        var eventT = getEventOrError.Value;

        var upsertedEventOrError = _eventRepository.AddParticipantToEvent(eventT, participant);
        
        return upsertedEventOrError.Match(_ => Ok(EventResponseMapper(eventT)),
            errors => Problem(errors));
        
    }
    
    [HttpPost("{id:guid}/corporations")]
    public IActionResult AddCorporationToEvent(Guid id, CreateCorporationRequest corporationRequest)
    {
        var getEventOrError = _eventRepository.GetEvent(id);
        
        if (getEventOrError.IsError)
        {
            return Problem(getEventOrError.Errors);
        }

        var corporationOrError = CorporationFromRequest(corporationRequest);
        if (corporationOrError.IsError)
        {
            return Problem(corporationOrError.Errors);
        }

        var corporation = corporationOrError.Value;
        
        var eventT = getEventOrError.Value;

        var upsertedEventOrError = _eventRepository.AddCorporationToEvent(eventT, corporation);
        
        return upsertedEventOrError.Match(_ => Ok(EventResponseMapper(eventT)),
            errors => Problem(errors));
        
    }
    
    
    //Deleting corporations and participants
    [HttpDelete("{id:guid}/corporations/{corporationId:guid}")]
    public IActionResult DeleteCorpFromEvent(Guid id, Guid corporationId)
    {
        
        var getEventOrError = _eventRepository.GetEvent(id);
        
        if (getEventOrError.IsError)
        {
            return Problem(getEventOrError.Errors);
        }

        var eventT = getEventOrError.Value;

        var deletedCorpOrError = _eventRepository.DeleteCorpFromEvent(eventT, corporationId);
        
        return deletedCorpOrError.Match(_ => NoContent(),
            errors => Problem(errors));
    }
    
    [HttpDelete("{id:guid}/participants/{participantId:guid}")]
    public IActionResult DeleteParticipantFromEvent(Guid id, Guid participantId)
    {
        var getEventOrError = _eventRepository.GetEvent(id);

        if (getEventOrError.IsError)
        {
            return Problem(getEventOrError.Errors);
        }

        var eventT = getEventOrError.Value;

        var deletedParticipantOrError = _eventRepository.DeleteParticipantFromEvent(eventT, participantId);
        
        return deletedParticipantOrError.Match(_ => NoContent(),
            errors => Problem(errors));
    }
    
    
    //Upsert Corporations and Participants
    [HttpPut("{id:guid}/corporations/{corporationId:guid}")]
    public IActionResult UpsertCorporationToEvent(Guid id, Guid corporationId, UpsertCorporationRequest updatedCorpRequest)
    {
        var getEventOrError = _eventRepository.GetEvent(id);
        
        if (getEventOrError.IsError)
        {
            return Problem(getEventOrError.Errors);
        }
        
        var corporationOrError = CorporationFromRequest(updatedCorpRequest, corporationId);
        if (corporationOrError.IsError)
        {
            return Problem(corporationOrError.Errors);
        }

        var corporation = corporationOrError.Value;

        var eventT = getEventOrError.Value;

        var upsertCorpOrError = _eventRepository.UpsertCorpInEvent(eventT, corporation);
        
        return upsertCorpOrError.Match(_ => NoContent(),
            errors => Problem(errors));
    }
    
    [HttpPut("{id:guid}/participants/{participantId:guid}")]
    public IActionResult UpsertEventParticipant(Guid id, Guid participantId, UpsertParticipantRequest updatedParticRequest)
    {
        var getEventOrError = _eventRepository.GetEvent(id);
        
        if (getEventOrError.IsError)
        {
            return Problem(getEventOrError.Errors);
        }
        
        var participantOrError = ParticipantFromRequest(updatedParticRequest, participantId);
        if (participantOrError.IsError)
        {
            return Problem(participantOrError.Errors);
        }

        var participant = participantOrError.Value;

        var eventT = getEventOrError.Value;

        var upsertParticipantOrError = _eventRepository.UpsertParticipantInEvent(eventT, participant);
        
        return upsertParticipantOrError.Match(_ => NoContent(),
            errors => Problem(errors));
    }
    
    
    
    
    //Event helper methods

    private static EventResponse EventResponseMapper(Event eventT)
    {
        var response = new EventResponse(eventT.Id, eventT.Name, eventT.StartTime, eventT.Location, eventT.Details,
            eventT.Participants, eventT.Corporations);
        return response;
    }
    
    private static EventsListResponse EventsListToResponse(List<Event> events)
    {
        var responseList = new List<EventResponse>();
        foreach (var eventT in events)
        {
            responseList.Add(new EventResponse(eventT.Id, eventT.Name, eventT.StartTime, eventT.Location,
                eventT.Details,
                eventT.Participants, eventT.Corporations));
        }
        
        return new EventsListResponse(responseList);
    }
    
    private IActionResult CreatedAtGetEvent(Event eventT)
    {
        return CreatedAtAction(actionName: nameof(GetEvent),
            routeValues: new { id = eventT.Id }, 
            value: EventResponseMapper(eventT));
    }
    
    private static ErrorOr<Event> EventFromRequest(CreateEventRequest request)
    {
        return Event.Create(request.Name, request.StartTime, request.Location, request.Details,
            request.Participants, request.Corporations);
    }
    
    private static ErrorOr<Event> EventFromRequest(UpsertEventRequest request, Guid id)
    {
        return Event.Create(request.Name, request.StartTime, request.Location, request.Details,
            request.Participants, request.Corporations, id);
    }
    
    
    
    //Corporation helper methods
    private static ErrorOr<Corporation> CorporationFromRequest(CreateCorporationRequest request)
    {
        return Corporation.Create(request.Name, request.RegistrationCode, request.NumberOfParticipants,
            request.PaymentType, request.Details);
    }
    
    private static ErrorOr<Corporation> CorporationFromRequest(UpsertCorporationRequest request, Guid id)
    {
        return Corporation.Create(request.Name, request.RegistrationCode, request.NumberOfParticipants,
            request.PaymentType,request.Details, id);
    }
    
    
    
    //Participant helper methods
    private static ErrorOr<Participant> ParticipantFromRequest(CreateParticipantRequest request)
    {
        return Participant.Create(request.FirstName, request.LastName, request.IdNumber, request.PaymentType,
            request.Details);
    }
    
    private static ErrorOr<Participant> ParticipantFromRequest(UpsertParticipantRequest request, Guid id)
    {
        return Participant.Create(request.FirstName, request.LastName, request.IdNumber,
            request.PaymentType, request.Details, id);
    }
    
    
}
using DAL;
using DAL.db;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nullam.Contracts.Corporation;
using Nullam.Contracts.Event;
using Nullam.Contracts.Participant;
using Nullam.Controllers;
using Xunit.Abstractions;

namespace NullamApiTests;

public class EventControllerTests
{
    private readonly EventController _controller;
    private readonly IEventRepository _service;
    private readonly NullamDbContext _dbContext;

    public EventControllerTests(ITestOutputHelper testOutputHelper)
    {
        var dbOptions = new DbContextOptionsBuilder<NullamDbContext>()
            .UseSqlite("Data source=C:\\Users\\Taavi\\RiderProjects\\Nullam\\Nullam\\Nullam\\Nullam.db").Options;
        _dbContext = new NullamDbContext(dbOptions);
        _service = new EventRepository(_dbContext);
        _controller = new EventController(_service);
    }

    [Fact]
    public void Get_All_When_Called_Returns_OK()
    {
        //Act
        var okResult = _controller.GetEvents();

        //Assert
        Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
    }

    [Fact]
    public void GetById_UnknownGuid_ReturnsObjectResult()
    {
        //Act
        var notFoundResult = _controller.GetEvent(Guid.NewGuid());

        //Assert
        Assert.IsType<ObjectResult>(notFoundResult);
    }

    [Fact]
    public void GetById_ExistingGuid_ReturnsOkResult()
    {
        //Arrange
        var testGuid = new Guid("83C71C81-2B0F-4ADC-9DA8-57A642FFC9EF");
        var testEvent = Event.Create("test", DateTime.Now, "string", "detail",
            new List<Participant>(), new List<Corporation>(), testGuid);

        _service.UpsertEvent(testEvent.Value);

        //Act
        var okResult = _controller.GetEvent(testGuid);

        //Assert
        Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
    }

    [Fact]
    public void GetById_ExistingGuid_ReturnsRightEvent()
    {
        //Arrange
        var testGuid = new Guid("83C71C81-2B0F-4ADC-9DA8-57A642FFC9EF");
        var testEvent = Event.Create("test", DateTime.Now, "string", "detail",
            new List<Participant>(), new List<Corporation>(), testGuid);

        _service.UpsertEvent(testEvent.Value);

        //Act
        var okResult = _controller.GetEvent(testGuid) as OkObjectResult;

        //Assert
        Assert.IsType<EventResponse>(okResult!.Value);
        Assert.Equal(testGuid, (okResult.Value as EventResponse).Id);
    }

    [Fact]
    public void Add_Object_ReturnsCreatedResponse()
    {
        // Arrange
        var request = new CreateEventRequest(
            "test",
            DateTime.Now,
            "string",
            "details",
            new List<Participant>(),
            new List<Corporation>()
        );

        // Act
        var createResponse = _controller.CreateEvent(request);

        // Assert
        Assert.IsType<CreatedAtActionResult>(createResponse);
    }
    
    [Fact]
    public void Add_Object_ResponseHasCreatedEvent()
    {
        // Arrange
        var request = new CreateEventRequest(
            "test",
            DateTime.Now,
            "string",
            "details",
            new List<Participant>(),
            new List<Corporation>()
        );

        // Act
        var createResponse = _controller.CreateEvent(request) as CreatedAtActionResult;
        var eventResponse = createResponse!.Value as EventResponse;

        // Assert
        Assert.IsType<EventResponse>(eventResponse);
        Assert.Equal("test", eventResponse!.Name);
    }
    
    [Fact]
    public void Remove_NotExistingGuidPassed_ReturnsObjectResult()
    {
        // Arrange
        var notExistingGuid = Guid.NewGuid();
        // Act
        var badResponse = _controller.DeleteEvent(notExistingGuid);
        // Assert
        Assert.IsType<ObjectResult>(badResponse);
    }
    
    [Fact]
    public void Remove_ExistingGuidPassed_ReturnsNoContentResult()
    {
        // Arrange
        var testGuid = new Guid("83C71C81-2B0F-4ADC-9DA8-57A642FFC9EF");
        var testEvent = Event.Create("test", DateTime.Now, "string", "detail",
            new List<Participant>(), new List<Corporation>(), testGuid);

        _service.UpsertEvent(testEvent.Value);
        // Act
        var noContentResponse = _controller.DeleteEvent(testGuid);
        // Assert
        Assert.IsType<NoContentResult>(noContentResponse);
    }
    
    [Fact]
    public void Remove_ExistingGuidPassed_RemovesOneItem()
    {
        // Arrange
        var testGuid = new Guid("83C71C81-2B0F-4ADC-9DA8-57A642FFC9EF");
        var testEvent = Event.Create("test", DateTime.Now, "string", "detail",
            new List<Participant>(), new List<Corporation>(), testGuid);

        _service.UpsertEvent(testEvent.Value);
        var amountOfEvents = _service.GetEvents().Value.Count;
        
        // Act
        _controller.DeleteEvent(testGuid);
        // Assert
        Assert.Equal(amountOfEvents - 1, _service.GetEvents().Value.Count);
    }
    
    [Fact]
    public void Upsert_ExistingGuidPassed_ReturnsNoContent()
    {
        // Arrange
        var testGuid = new Guid("83C71C81-2B0F-4ADC-9DA8-57A642FFC9EF");
        var testEventRequest = new UpsertEventRequest("test", DateTime.Now, "string", "detail",
            new List<Participant>(), new List<Corporation>());
        
        var testEvent = Event.Create("test", DateTime.Now, "string", "detail",
            new List<Participant>(), new List<Corporation>(), testGuid);

        _service.UpsertEvent(testEvent.Value);
        _dbContext.ChangeTracker.Clear();
        
        // Act
        var noContentResponse = _controller.UpsertEvent(testGuid, testEventRequest);
        // Assert
        Assert.IsType<NoContentResult>(noContentResponse);
    }
    
    [Fact]
    public void Upsert_NewGuidPassed_ReturnsNoContent()
    {
        // Arrange
        var testGuid = Guid.NewGuid();
        var testEventRequest = new UpsertEventRequest("test", DateTime.Now, "string", "detail",
            new List<Participant>(), new List<Corporation>());

        // Act
        var noContentResponse = _controller.UpsertEvent(testGuid, testEventRequest);
        // Assert
        Assert.IsType<CreatedAtActionResult>(noContentResponse);
    }
    
    [Fact]
    public void Add_ParticipantToEvent_ReturnsOkObjectResult()
    {
        // Arrange
        var testGuid = new Guid("83C71C81-2B0F-4ADC-9DA8-57A642FFC9EF");
        var testEvent = Event.Create("test", DateTime.Now, "string", "detail",
            new List<Participant>(), new List<Corporation>(), testGuid);

        _service.UpsertEvent(testEvent.Value);

        var participantRequest = new CreateParticipantRequest("first", "last",
            "39911250895", EPaymentType.Cash, "detail");

        // Act
        var response = _controller.AddParticipantToEvent(testGuid, participantRequest);
        // Assert
        Assert.IsType<OkObjectResult>(response);
    }
    
    [Fact]
    public void Add_CorporationToEvent_ReturnsOkObjectResult()
    {
        // Arrange
        var testGuid = new Guid("83C71C81-2B0F-4ADC-9DA8-57A642FFC9EF");
        var testEvent = Event.Create("test", DateTime.Now, "string", "detail",
            new List<Participant>(), new List<Corporation>(), testGuid);

        _service.UpsertEvent(testEvent.Value);

        var corporationRequest = new CreateCorporationRequest("company", "7000098585",
            35, EPaymentType.Cash, "detail");

        // Act
        var response = _controller.AddCorporationToEvent(testGuid, corporationRequest);
        // Assert
        Assert.IsType<OkObjectResult>(response);
    }
    
    [Fact]
    public void Delete_ParticipantFromEvent_ReturnsNoContentResult()
    {
        // Arrange
        var testGuid = new Guid("83C71C81-2B0F-4ADC-9DA8-57A642FFC9EF");
        var participantGuid = new Guid("81696435-6B05-4D3C-ABB2-F02F4A5C9907");
            
        var testEvent = Event.Create("test", DateTime.Now, "string", "detail",
            new List<Participant>(), new List<Corporation>(), testGuid);
        
        var participant = Participant.Create("first", "last",
            "39911259063", EPaymentType.Cash, "detail", participantGuid);

        _service.UpsertEvent(testEvent.Value);
        
        _service.UpsertParticipantInEvent(testEvent.Value, participant.Value);

        // Act
        var response = _controller.DeleteParticipantFromEvent(testGuid, participantGuid);
        // Assert
        Assert.IsType<NoContentResult>(response);
    }

    [Fact]
    public void Delete_CorporationFromEvent_ReturnsNoContentResult()
    {
        // Arrange
        var testGuid = new Guid("83C71C81-2B0F-4ADC-9DA8-57A642FFC9EF");
        var corpGuid = new Guid("81696435-6B05-4D3C-ABB2-F02F4A5C9907");
            
        var testEvent = Event.Create("test", DateTime.Now, "string", "detail",
            new List<Participant>(), new List<Corporation>(), testGuid);
        
        var corporation = Corporation.Create("company", "7000098585",
            35, EPaymentType.Cash, "detail", corpGuid);

        _service.UpsertEvent(testEvent.Value);
        
        _service.AddCorporationToEvent(testEvent.Value, corporation.Value);
        _service.UpsertCorpInEvent(testEvent.Value, corporation.Value);

        // Act
        var response = _controller.DeleteCorpFromEvent(testGuid, corpGuid);
        // Assert
        Assert.IsType<NoContentResult>(response);
    }
    
    [Fact]
    public void Upsert_EventParticipant_ReturnsNoContent()
    {
        // Arrange
        var testGuid = new Guid("83C71C91-2B0F-4ADC-9DA8-57A642FFC9EF");
        var participantGuid = new Guid("81696495-6B05-4D3C-ABB2-F02F4A5C9907");
            
        var testEvent = Event.Create("test", DateTime.Now, "string", "detail",
            new List<Participant>(), new List<Corporation>(), testGuid);
        
        var participant = Participant.Create("first", "last",
            "39911250963", EPaymentType.Cash, "detail", participantGuid);
        
        var participantRequest = new UpsertParticipantRequest("firstEdited", "lastEdited",
            "39911250963", EPaymentType.Cash, "detail");

        _service.UpsertEvent(testEvent.Value);
        
        _service.UpsertParticipantInEvent(testEvent.Value, participant.Value);

        // Act
        var response = _controller.UpsertEventParticipant(testGuid, participantGuid, participantRequest);
        // Assert
        Assert.IsType<NoContentResult>(response);
    }
    
    
    
    [Fact]
    public void Upsert_EventCorporation_ReturnsNoContentResult()
    {
        // Arrange
        var testGuid = new Guid("83C71C81-2B0F-4ADC-9DA8-57A642FFC9EF");
        var corpGuid = new Guid("81696435-6B05-4D3C-ABB2-F02F4A5C9907");
            
        var testEvent = Event.Create("test", DateTime.Now, "string", "detail",
            new List<Participant>(), new List<Corporation>(), testGuid);
        
        var corporation = Corporation.Create("company", "7000098585",
            35, EPaymentType.Cash, "detail", corpGuid);
        
        var corporationRequest = new UpsertCorporationRequest("companyEdited", "7000098585",
            35, EPaymentType.Cash, "detail");

        
        _service.UpsertEvent(testEvent.Value);

        _service.UpsertCorpInEvent(testEvent.Value, corporation.Value);

        // Act
        var response = _controller.UpsertCorporationToEvent(testGuid, corpGuid, corporationRequest);
        // Assert
        Assert.IsType<NoContentResult>(response);
    }

}
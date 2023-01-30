using Domain;

namespace Nullam.Contracts.Participant;

public record ParticipantResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string IdNumber,
    EPaymentType PaymentType,
    string Details,
    List<Domain.Event> Events);
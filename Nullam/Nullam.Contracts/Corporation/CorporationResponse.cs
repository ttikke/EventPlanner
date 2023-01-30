using Domain;

namespace Nullam.Contracts.Corporation;

public record CorporationResponse(
    Guid Id,
    string Name,
    string RegistrationCode,
    int NumberOfParticipants,
    EPaymentType PaymentType,
    string Details,
    List<Domain.Event> Events);
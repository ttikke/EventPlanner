using Domain;

namespace Nullam.Contracts.Corporation;

public record CreateCorporationRequest(
    string Name,
    string RegistrationCode,
    int NumberOfParticipants,
    EPaymentType PaymentType,
    string Details);

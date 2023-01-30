using Domain;

namespace Nullam.Contracts.Corporation;

public record UpsertCorporationRequest(
    string Name,
    string RegistrationCode,
    int NumberOfParticipants,
    EPaymentType PaymentType,
    string Details);
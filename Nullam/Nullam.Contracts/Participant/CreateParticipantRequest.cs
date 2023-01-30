using Domain;

namespace Nullam.Contracts.Participant;

public record CreateParticipantRequest(
    string FirstName,
    string LastName,
    string IdNumber,
    EPaymentType PaymentType,
    string Details);
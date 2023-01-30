using ErrorOr;


namespace ServiceErrors;

public static class Errors
{
    public static class Event
    {
        public static Error NameTooShort(int minLength)
        {
            return Error.Validation("Event.InvalidName",
                $"Event name must be at least {minLength} characters long.");
        }

        public static Error DetailsTooLong(int maxLength)
        {
            return Error.Validation("Event.InvalidDetails",
                $"Event details must not be longer than {maxLength} characters.");
        }

        public static Error NotFound => Error.NotFound("Event.NotFound", "Event was not Found");
    }
    
    
    

    public static class Participant
    {
        public static Error NameTooShort(int minLength)
        {
            return Error.Validation("Participant.InvalidName",
                $"Participant firstname and lastname must be at least {minLength} characters long.");
        }

        public static Error DetailsTooLong(int maxLength)
        {
            return Error.Validation("Participant.InvalidDetails",
                $"Participant details must not be longer than {maxLength} characters.");
        }

        public static Error NotFound => Error.NotFound("Participant.NotFound",
            "Participant was not Found");
    }
    
    
    
    
    public static class Corporation
    {
        public static Error NameTooShort(int minLength)
        {
            return Error.Validation("Corporation.InvalidName",
                $"Corporation name must be at least {minLength} characters long.");
        }

        public static Error DetailsTooLong(int maxLength)
        {
            return Error.Validation("Corporation.InvalidDetails",
                $"Corporation details must not be longer than {maxLength} characters.");
        }

        public static Error NotFound => Error.NotFound("Corporation.NotFound",
            "Corporation was not Found");
    }
}
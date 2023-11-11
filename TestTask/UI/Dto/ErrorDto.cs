namespace TestTask.UI.Dto;

public record ErrorDto<TCode>(
    TCode Code,
    string Message
);
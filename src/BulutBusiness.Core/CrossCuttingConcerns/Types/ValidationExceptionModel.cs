namespace BulutBusiness.Core.CrossCuttingConcerns.Types;

public class ValidationExceptionModel
{
    public string? Property { get; set; }
    public IEnumerable<string>? Errors { get; set; }
}
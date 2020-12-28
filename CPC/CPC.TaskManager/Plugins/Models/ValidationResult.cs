using System.Collections.Generic;
using System.Linq;

namespace CPC.TaskManager.Plugins
{
    public class ValidationResult
    {
        public bool Success => !Errors.Any();

        public List<ValidationError> Errors { get; set; } = new List<ValidationError>();
    }

    public class ValidationError
    {
        public string Field { get; set; }
        public string Reason { get; set; }
        public int? SegmentIndex { get; set; }
        public int? FieldIndex { get; set; }

        public static ValidationError EmptyField(string field) => new ValidationError() { Field = field, Reason = "The field cannot be empty." };

        public override string ToString() => $"{Field}: {Reason}";
    }
}

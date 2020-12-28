using System.Collections.Generic;

namespace CPC.TaskManager.Plugins
{
    public interface IHasValidation
    {
        void Validate(ICollection<ValidationError> errors);
    }
}
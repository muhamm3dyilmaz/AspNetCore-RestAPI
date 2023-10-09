using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public record BookDtoForInsertion : BookDtoForManipulation
    {
        [Required(ErrorMessage = "CategoryId is field required")]
        public int CategoryId { get; init; }
    }
}

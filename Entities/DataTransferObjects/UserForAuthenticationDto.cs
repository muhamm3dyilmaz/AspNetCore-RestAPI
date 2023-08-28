using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public record UserForAuthenticationDto
    {
        [Required(ErrorMessage = "Username is required field.")]
        public string? UserName { get; init; }

        [Required(ErrorMessage = "Password is required field.")]
        public string? Password { get; init; }
    }
}

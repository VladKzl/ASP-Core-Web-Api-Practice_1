using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects
{
    public record CompanyForUpdateDto()
    {
        [Required(ErrorMessage = "Company name is a required field.")]
        public string? Name { get; init; }
        [Required(ErrorMessage = "Company address is a required field.")]
        public string? Address { get; init; }
        [Required(ErrorMessage = "Company country is a required field.")]
        public string? Country { get; init; }
        IEnumerable<EmployeeForCreationDto> Employees { get; init; }
    }
}

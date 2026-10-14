using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace NZWalks.API.Models.DTO
{
    public class AddRegionRequestDTO
    {
        [Required]
        [MinLength(3,ErrorMessage = "Code has to be minimu of 3 charcaters")]
        [MaxLength(3, ErrorMessage="Code has to be a miaximum of 3 characters")]
        public string Code { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Code has to be a miaximum of 100 characters")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
        // The id cant be included in DTO because it is uniquely created
    }
}

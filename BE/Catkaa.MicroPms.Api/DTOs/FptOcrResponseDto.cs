using System.Collections.Generic;

namespace Catkaa.MicroPms.Api.DTOs
{
    public class FptOcrResponseDto
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public List<FptOcrDataDto> Data { get; set; } = new List<FptOcrDataDto>();
    }

    public class FptOcrDataDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Dob { get; set; } = string.Empty;
    }
}

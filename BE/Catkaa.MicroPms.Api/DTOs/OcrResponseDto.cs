namespace Catkaa.MicroPms.Api.DTOs
{
    public class OcrResponseDto
    {
        public string FullName { get; set; } = string.Empty;
        public string IdNumber { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public double Confidence { get; set; }
    }
}

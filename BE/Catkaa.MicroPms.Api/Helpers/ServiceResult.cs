namespace Catkaa.MicroPms.Api.Helpers
{
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        public static ServiceResult<T> Ok(string message, T? data = default)
        {
            return new ServiceResult<T> { Success = true, Message = message, Data = data };
        }

        public static ServiceResult<T> Fail(string message, T? data = default)
        {
            return new ServiceResult<T> { Success = false, Message = message, Data = data };
        }
    }
}

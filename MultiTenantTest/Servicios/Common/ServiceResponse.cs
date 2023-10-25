namespace MultiTenantTest.Servicios.Common
{
    public class ServiceResponse<T>
    {
        public bool Success { get; set; }
        public int CodeResult { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}

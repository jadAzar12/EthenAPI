namespace EthenAPI.ViewModels
{
    public class ServiceResult<T>
    {
        public T Result { get; set; }
        public string Error { get; set; }
        public bool ResourceNotFound { get; set; }
        public bool IsValid => string.IsNullOrEmpty(Error);

        public ServiceResult(T result, string error = null, bool resourceNotFound = false)
        {
            Result = result;
            Error = error;
            ResourceNotFound = resourceNotFound;
        }
    }
}

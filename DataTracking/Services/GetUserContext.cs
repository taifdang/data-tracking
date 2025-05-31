namespace DataTracking.Services
{
    public class GetUserContext
    {
        private readonly IHttpContextAccessor _context;
        public GetUserContext(IHttpContextAccessor context)
        {
            _context = context;
        }
        public string getSessionValue()
        {
            _context.HttpContext!.Items.TryGetValue("uid", out var sessionId);
            var sessionValue = sessionId?.ToString();
            return sessionValue ?? default!;                    
        }
    }
}

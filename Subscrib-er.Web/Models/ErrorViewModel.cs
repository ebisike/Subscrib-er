using System;

namespace Subscrib_er.Web.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }
        public Guid Id { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}

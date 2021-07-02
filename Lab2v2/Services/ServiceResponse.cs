using System;

namespace Lab2v2.Services
{
    public class ServiceResponse<TResponseOk, TResponseError>
    {
        public TResponseOk ResponseOk { get; set; }
        public TResponseError ResponseError { get; set; }
    }
}

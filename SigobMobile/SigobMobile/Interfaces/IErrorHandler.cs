namespace SigobMobile.Interfaces
{
    using System;
    /// <summary>
    /// Error Handler Interface
    /// </summary>
    public interface IErrorHandler
    {
        void HandleError(Exception ex);
    }
}

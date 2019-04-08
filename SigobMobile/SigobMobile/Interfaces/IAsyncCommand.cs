namespace SigobMobile.Interfaces
{
    using System.Threading.Tasks;
    using System.Windows.Input;

    /// <summary>
    /// Interface for ICommand Async
    /// </summary>
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync();
        bool CanExecute();
    }

    /// <summary>
    /// Interface for ICommand Async with Parameter
    /// </summary>
    public interface IAsyncCommand<T> : ICommand
    {
        Task ExecuteAsync(T parameter);
        bool CanExecute(T parameter);
    }
}

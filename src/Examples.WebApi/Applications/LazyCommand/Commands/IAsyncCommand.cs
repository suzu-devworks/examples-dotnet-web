using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Examples.WebApi.Applications.LazyCommand.Commands
{
    public interface IAsyncCommand
    {
        Task<IActionResult> ExecuteAsync();
    }

    public interface IAsyncCommand<T>
    {
        Task<IActionResult> ExecuteAsync(T parameter);
    }

    public interface IAsyncCommand<T1, T2>
    {
        Task<IActionResult> ExecuteAsync(T1 parameter1, T2 parameter2);
    }

    public interface IAsyncCommand<T1, T2, T3>
    {
        Task<IActionResult> ExecuteAsync(T1 parameter1, T2 parameter2, T3 parameter3);
    }

}
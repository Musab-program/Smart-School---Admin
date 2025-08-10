using SmartSchool.Core.Interfaces;

namespace SmartSchool.Core
{
    public interface IUnitOfWork : IDisposable
    {
        int Complete();
    }
}
using MediatR_Demo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatR_Demo.Domain.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IGenericRepo<Todo> TodoRepo { get; }

        Task SaveChanges(CancellationToken ct);
    }
}

using MediatR_Demo.Domain.Entities;
using MediatR_Demo.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatR_Demo.Infrastructure.Repositories
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly TodoDbContext _context;

        private IGenericRepo<Todo> _todoRepo;

        public UnitOfWork(TodoDbContext context)
        {
            _context = context;
        }

        public IGenericRepo<Todo> TodoRepo
        {
            get
            {
                if (this._todoRepo == null)
                    this._todoRepo
                        = new GenericRepo<Todo>(_context);
                return _todoRepo;
            }
        }

        public async Task SaveChanges(CancellationToken ct)
        {
            await _context.SaveChangesAsync(ct);
        }
    }
}

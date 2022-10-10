using AlgoriaCore.Application.Localization;
using AlgoriaCore.Domain.Exceptions;
using AlgoriaPersistence.Interfaces.Interfaces;
using MongoDB.Driver;
using System;

namespace AlgoriaPersistence.UOW
{
    public class MongoUnitOfWork : IMongoUnitOfWork
    {
        private readonly IMongoDBContext _context;
        private IClientSessionHandle session;

        public MongoUnitOfWork(IMongoDBContext context)
        {
            _context = context;

            if (_context.IsEnabled && _context.IsActive)
            {
                session = GetSession();
            }
        }

        public IClientSessionHandle GetSession()
        {
            if (session == null && _context.IsEnabled && _context.IsActive)
            {
                session = _context.Client.StartSession();
            }

            return session;
        }

        public void Begin()
        {
            session.StartTransaction();
        }

        public void Commit()
        {
            if (session.IsInTransaction)
            {
                session.CommitTransaction();
            }
        }

        public void Rollback()
        {
            if (session.IsInTransaction)
            {
                session.AbortTransaction();
            }
        }
    }
}

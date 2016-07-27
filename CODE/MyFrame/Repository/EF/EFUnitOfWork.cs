using MyFrame.Model.Unit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace MyFrame.Repository.EF
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private DbContext _dbContext;
        public DbContext DbContext
        {
            get
            {
                if (_dbContext == null)
                {
                    _dbContext = EFDbContextFactory.GetCurrentContext();
                }
                return _dbContext;
            }
        }
        private readonly IsolationLevel _isolationLevel;
        private DbContextTransaction _transaction;
        public EFUnitOfWork()
        {
            AutoCommit = true;
        }

        public EFUnitOfWork(IsolationLevel isolationLevel)
            : this()
        {
            _isolationLevel = isolationLevel;
        }


        public void Commit()
        {
            if (_transaction != null)
            {
                SaveChanges();
                _transaction.Commit();
            }
        }

        /// <summary>
        /// 是否自动提交，默认true
        /// </summary>
        public bool AutoCommit
        {
            get;
            set;
        }


        public void BeginTransaction()
        {
            //_transaction = DbContext.Database.BeginTransaction(_isolationLevel);
            _transaction = DbContext.Database.BeginTransaction();
        }

        public void Rollback()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                ClearUpTransaction();
            }
        }


        public int SaveChanges()
        {
            return DbContext.SaveChanges();
        }

        private void ClearUpTransaction()
        {
            _transaction = null;
        }

        private void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                if (_transaction != null)
                {
                    _transaction.Dispose();
                }
                DbContext.Dispose();
            }
            // get rid of unmanaged resources
        }
        ~EFUnitOfWork()
        {
            Dispose(false);
        }
    }
}

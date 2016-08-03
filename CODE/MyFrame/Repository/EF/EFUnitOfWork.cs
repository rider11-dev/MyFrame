using MyFrame.Infrastructure.Logger;
using MyFrame.Model.Unit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace MyFrame.Repository.EF
{
    public class EFUnitOfWork : IUnitOfWork, IDisposable
    {
        /// <summary>
        /// 数据上下文，使用属性依赖注入
        /// </summary>
        public DbContext DbContext { get; set; }
        private IsolationLevel _isolationLevel;
        private DbContextTransaction _transaction;
        ILogHelper<EFUnitOfWork> _logHelper;
        public EFUnitOfWork()
        {
            AutoCommit = true;
            _logHelper = LogHelperFactory.GetLogHelper<EFUnitOfWork>();
            _logHelper.LogInfo("EFUnitOfWork");
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

        private void Dispose(Boolean disposing = true)
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
            _logHelper.LogInfo("~EFUnitOfWork");
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace MyFrame.Core.UnitOfWork
{
    /// <summary>
    /// 工作单元
    /// 提供一个提交方法，它可以对调用层公开，为了减少连库次数
    /// </summary>
    public interface IUnitOfWork
    {
        DbContext DbContext { get; set; }
        void BeginTransaction();

        /// <summary>
        /// 将操作提交到数据库
        /// </summary>
        void Commit();

        void Rollback();

        int SaveChanges();

        bool AutoCommit { get; set; }
    }
}

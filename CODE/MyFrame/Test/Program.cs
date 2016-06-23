using MyFrame.Repository.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test;
using Test.dal;

namespace MyFrame.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            InitDbContext();
            UsersTest usersTest = new UsersTest();

            usersTest.Insert();
            usersTest.GetCount();

            Console.ReadKey();
        }

        static void InitDbContext()
        {
            EFDbContextFactory.DbContextProvider = new EFDbContextProviderRBAC();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using NUnit.Framework;
using SudokuApp.Core.BL;
using SudokuApp.Core.BL.FileLoaders;
using SudokuApp.Core.Interfaces;
using SudokuApp.Core.Interfaces.FileLoaders;

namespace SudokuApp.Core.Tests
{
    public abstract class TestBase
    {
        private IContainer _container;
        public IContainer Container
        {
            get
            {
                if (_container == null)
                    Build();

                return _container;
            }
        }

        [OneTimeSetUp]
        public void Build()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<SimpleSudokuSolver>().As<ISudokuSolver>();
            builder.RegisterType<SimpleSudokuGenerator>().As<ISudokuGenerator>();
            builder.RegisterType<SimpleSudokuComplexityEstimator>().As<ISudokuComplexityEstimator>();
            builder.RegisterType<TxtSudokuFileLoader>().As<ISudokuFileLoader>();

            //builder.RegisterAssemblyTypes(Assembly.Load("SudokuApp.Core"))
            //    .Where(t => t.IsClass)
            //    .AsImplementedInterfaces()
            //    .PropertiesAutowired();

            _container = builder.Build();

            var csl = new AutofacServiceLocator(_container);
            ServiceLocator.SetLocatorProvider(() => csl);
        }
    }
}

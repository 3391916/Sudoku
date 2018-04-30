using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using SudokuApp.Core.BL;
using SudokuApp.Core.BL.FileLoaders;
using SudokuApp.Core.Interfaces;
using SudokuApp.Core.Interfaces.FileLoaders;

namespace SudokuApp
{
    public static class IoCContainer
    {
        private static IContainer _baseContainer;
        public static IContainer BaseContainer
        {
            get
            {
                if (_baseContainer == null)
                    Build();

                return _baseContainer;
            }
        }

        public static void Build()
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

            _baseContainer = builder.Build();

            var csl = new AutofacServiceLocator(_baseContainer);
            ServiceLocator.SetLocatorProvider(() => csl);
        }

        public static TService Resolve<TService>()
        {
            return BaseContainer.Resolve<TService>();
        }
    }
}

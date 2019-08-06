using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace FX.Framework.FXDependencyInjection
{
    public static class DependencyInjectionHelper
    {
        public static void DenpendencyService(IServiceCollection service)
        {
            ///目录
            ///load 所有dll
            var AssemblyList = LoadAssembly();
            ///foreach dll中所有 type 
            var TypeList = LoadTypes(AssemblyList);
            ///判断是否为注入及 判断isInterface && class    if(a.IsInterface && b.IsClass)
            var dependencyinjectAttribute= LoadDependencyInjection(TypeList);
            ///判断注入类型， 注入   service.AddSingleton(a, b);
            Injection(dependencyinjectAttribute,service);
        }

        private static List<Assembly> LoadAssembly()
        {
            var result = new List<Assembly>();

            var basePath = System.AppContext.BaseDirectory;
            var dllList= Directory.GetFiles(basePath);
            foreach(var dll in dllList)
            {
                if (dll.EndsWith(".dll"))
                {
                    result.Add(Assembly.LoadFile(Path.Combine(basePath, dll)));
                }
            }
            return result;
        }

        private static List<Type> LoadTypes(List<Assembly> AssemblyList)
        {
            List<Type> result = new List<Type>();

            foreach(var assembly in AssemblyList)
            {
                var types= assembly.GetTypes();
                result.AddRange(types);
            }
            return result;
        }

        private static List<DependencyInjectionModel> LoadDependencyInjection(List<Type> typeList)
        {
            var result = new List<DependencyInjectionModel>();
            foreach(var type in typeList)
            {
                var attribute = GetDenpendencyInjection(type);
                if (attribute != null)
                {
                    result.Add(new DependencyInjectionModel
                    {
                        attribute= attribute,
                        type=type
                    });
                }
            }

            return result;
        }

        private static DependencyInjectionAttribute GetDenpendencyInjection(Type type)
        {
            var result = type.GetCustomAttribute<DependencyInjectionAttribute>();

            if(result!=null && result.Service.IsInterface && type.IsClass)
            {
                return result;
            }
            return null;
        }

        private static void Injection(List<DependencyInjectionModel> models, IServiceCollection service)
        {
            foreach(var m in models)
            {
                switch (m.attribute.Lifetime)
                {
                    case LiftType.Scoprd: service.AddScoped(m.attribute.Service, m.type);break;
                    case LiftType.Singleton:service.AddSingleton(m.attribute.Service, m.type);break;
                    case LiftType.Transient:service.AddTransient(m.attribute.Service, m.type); break;
                }
            }
        }
    }


}

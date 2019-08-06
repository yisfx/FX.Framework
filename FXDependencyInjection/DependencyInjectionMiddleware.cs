using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace FX.Framework.FXDependencyInjection
{
    public static class DependencyInjectionMiddleware
    {
        public static void UseDependencyInjectionMiddleware(this IServiceCollection service)
        {
            DependencyInjectionHelper.DenpendencyService(service);
        }
    }
}

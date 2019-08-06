using Microsoft.Extensions.DependencyInjection;
using System;

namespace FX.Framework.FXDependencyInjection
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple =true,Inherited =true)]
    public class DependencyInjectionAttribute:Attribute
    {
        public DependencyInjectionAttribute(Type service, LiftType Lifetime=LiftType.Transient, int Priority=1)
        {
            this.Service = service;

            this.Lifetime = Lifetime;

            this.Priority = Priority;
        }

        public Type Service { get; private set; }

        public LiftType Lifetime { get; set; }

        public int Priority { get; set; }
    }

    public enum LiftType
    {
        Transient=1,
        Scoprd=2,
        Singleton=3
    }
}

﻿using System.Data.Entity;
using Autofac;
using SampleArch.Model;
using SampleArch.Repository.Common;

namespace SampleArch.Modules
{


    public class EfModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new RepositoryModule());

            builder.RegisterType(typeof(SampleArchContext)).As(typeof(DbContext)).InstancePerLifetimeScope();
            builder.RegisterType(typeof(UnitOfWork)).As(typeof(IUnitOfWork)).InstancePerRequest();         

        }

    }
}
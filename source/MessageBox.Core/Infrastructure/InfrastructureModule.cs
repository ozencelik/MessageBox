﻿using Autofac;
using Module = Autofac.Module;
using System;
using System.Collections.Generic;
using System.Text;
using MessageBox.Data;
using MessageBox.Core.Services.Users;
using MessageBox.Core.Services.Messages;
using LogBox.Core.Services.Logs;
using ActivityLogBox.Core.Services.Logs;

namespace MessageBox.Core.Infrastructure
{
    public class InfrastructureModule : Module
    {
        #region Fields
        private bool _isDevelopment = false;
        #endregion

        #region Ctor
        public InfrastructureModule(bool isDevelopment)
        {
            _isDevelopment = isDevelopment;
        }
        #endregion

        #region Methods
        private void RegisterCommonDependencies(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(EfCoreRepository<>)).As(typeof(IRepository<>))
                .InstancePerLifetimeScope();

            builder.RegisterType<UserService>().As<IUserService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<BlockedUserService>().As<IBlockedUserService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<MessageService>().As<IMessageService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<LogService>().As<ILogService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ActivityLogService>().As<IActivityLogService>()
                .InstancePerLifetimeScope();
        }

        private void RegisterDevelopmentOnlyDependencies(ContainerBuilder builder)
        {
            // TODO: Add development only services
        }

        private void RegisterProductionOnlyDependencies(ContainerBuilder builder)
        {
            // TODO: Add production only services
        }

        protected override void Load(ContainerBuilder builder)
        {
            if (_isDevelopment)
            {
                RegisterDevelopmentOnlyDependencies(builder);
            }
            else
            {
                RegisterProductionOnlyDependencies(builder);
            }
            RegisterCommonDependencies(builder);
        }
        #endregion
    }
}
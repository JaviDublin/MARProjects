using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using App.Classes.BLL.Pooling.Models.Abstract;
using App.Classes.BLL.Pooling.Models;
using Mars.BLL.Pooling.Models;
using App.BLL.VehiclesAbroad.Models.Abstract;
using App.BLL.VehiclesAbroad.Models.Filters;
using App.Classes.DAL.Reservations.Abstract;
using App.Classes.DAL.Reservations;
using Mars.Pooling.Models;
using Mars.DAL.Pooling.Abstract;
using Mars.DAL.Pooling;
using Mars.Pooling.HTMLFactories.Abstract;
using Mars.Pooling.HTMLFactories;
using Mars.Entities.Pooling;
using App.Classes.DAL.Pooling.Abstract;
using App.Classes.DAL.Pooling;
using App.UserControls.Pooling;

namespace Mars.Pooling.Installer {
    public class PoolingInstaller:IWindsorInstaller {
        public void Install(IWindsorContainer container,IConfigurationStore store) {
            container.Register(
                Classes.FromThisAssembly().Where(Component.IsInNamespace("Mars.Pooling.Controllers")),
                Component.For<IButtonModel2>().ImplementedBy<ButtonModel2>().LifeStyle.Transient,
                Component.For<ITextFilterModel>().ImplementedBy<TextFilterModel>().LifeStyle.Transient,
                Component.For<IJavaScriptModel>().ImplementedBy<JavaScriptModel>().LifeStyle.Transient,
                Component.For<IHeadingModel>().ImplementedBy<HeadingModel>().LifeStyle.Transient,
                Component.For<IReservationLogRepository>().ImplementedBy<ReservationLogRepository>(),
                Component.For<LabelUpdateDBModel>().ImplementedBy<LabelUpdateDBModel>(),
                Component.For<IBrowserJavascriptRepository>().ImplementedBy<BrowserJavascriptRepository>(),
                Component.For<IBrowserParamsModel>().ImplementedBy<BrowserParamsModel>(),
                Component.For<IDateRangeRepository>().ImplementedBy<DateRangeRepository>().LifeStyle.Transient,
                Component.For<IDateRangeModel>().ImplementedBy<DateRangeModel>().LifeStyle.Transient,
                Component.For<ReservationDetailsModal>().LifeStyle.Transient,
                Component.For<AlertsBreakdownModel>().LifeStyle.Transient,
                Component.For<IOverdueActualRepository>().ImplementedBy<OverdueOpenTripsRepository>().LifeStyle.Transient.Named("OverdueOpenTripsRepository"),
                Component.For<IOverdueActualRepository>().ImplementedBy<OverdueCollectionsRepository>().LifeStyle.Transient.Named("OverdueCollectionsRepository"),
                Component.For<LabelStatusModel>().DependsOn("OverdueOpenTripsRepository").LifeStyle.Transient.Named("LabelODOpentripsModel"),
                Component.For<LabelStatusModel>().DependsOn(Dependency.OnComponent("repository","OverdueCollectionsRepository")).LifeStyle.Transient.Named("labelODCollectionsModel")
                );
        }
    }
}
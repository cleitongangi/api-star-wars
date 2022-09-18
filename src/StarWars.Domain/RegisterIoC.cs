using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarWars.Domain
{
    public static class RegisterIoC
    {
        public static void Register(IServiceCollection services)
        {
            //#region Registra todos os Validations
            //services.AddScoped<IUserAddValidation, UserAddValidation>();
            //services.AddScoped<ITeamUpdateValidation, TeamUpdateValidation>();
            //services.AddScoped<IPlayerUpdateValidation, PlayerUpdateValidation>();
            //services.AddScoped<ITransferListAddValidation, TransferListAddValidation>();
            //services.AddScoped<ITransferListCancelValidation, TransferListCancelValidation>();
            //services.AddScoped<ITransferListBuyValidation, TransferListBuyValidation>();
            //#endregion Registra todos os Validations

            //#region Registra todos os Serviços
            //var servicesAssembly = typeof(IUserAddValidation).Assembly;
            //var serviceRegistrations =
            //    from type in servicesAssembly.GetExportedTypes()
            //    where type.Namespace == "SoccerManager.Domain.Services"
            //    where type.GetInterfaces().Any()
            //    select new { Interface = type.GetInterfaces().Single(), Implementation = type };

            //foreach (var reg in serviceRegistrations)
            //    services.AddScoped(reg.Interface, reg.Implementation);
            //#endregion
        }
    }
}

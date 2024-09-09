using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Helpers;
using Talabat.Core.Repository;
using Talabat.Core.Services;
using Talabat.Repository;
using Talabat.Service;
using TalabatAPIProject.Error;

namespace TalabatAPIProject.Extension
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServises(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IResponseCasheService),typeof(ResponseCasheService));
            services.AddScoped(typeof(IPaymentServices), typeof(PaymentServices));
            services.AddScoped(typeof(IProductService), typeof(ProductService));
            services.AddScoped(typeof(IOrderServices), typeof(OrderServices));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            services.AddScoped(typeof(IGenaricRepository<>), typeof(GenaricRepository<>));
            //builder.Services.AddAutoMapper(M=>M.AddProfile(new MappingProfile()));
            services.AddAutoMapper(typeof(MappingProfile));
            services.Configure<ApiBehaviorOptions>(option =>
            {
                option.InvalidModelStateResponseFactory = (ActionContext) => {
                    var error = ActionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                    .SelectMany(p => p.Value.Errors)
                    .Select(E => E.ErrorMessage)
                    .ToArray();



                    var ValidationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = error
                    };
                    return new BadRequestObjectResult(ValidationErrorResponse);
                };

            });
            return services;
        }
    }
}

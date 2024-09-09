namespace TalabatAPIProject.Extension
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerService(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }
        public static WebApplication UseSwaggerMiddlware(this WebApplication web)
        {
            web.UseSwagger();
            web.UseSwaggerUI();
            return web;

        }  
    }
}

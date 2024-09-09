using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;
using Talabat.Repository.Identity;
using Talabat.Service;

namespace TalabatAPIProject.Extension
{
    public static class IdentityServicesExtension
    {
        public static IServiceCollection IdentityServices(this IServiceCollection services,IConfiguration configuration)
        {

             services.AddScoped(typeof(IAuthService), typeof(AuthService));
             services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                // options.Password.RequireUppercase = false;   // لا تحتاج الحروف الكبيرة
                // options.Password.RequireDigit = false;      // لا تحتاج إلى أرقام
                // options.Password.RequireLowercase = true;  // تحتاج إلى حروف صغيرة
                //  options.Password.RequiredLength = 6;      // الحد الأدنى لطول كلمة المرور
                // يمكنك تعديل المزيد من الخيارات هنا...
            }).AddEntityFrameworkStores<AppIdentityDbContext>();
            services.AddAuthentication(/*JwtBearerDefaults.AuthenticationScheme*/ option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;
             
            })
                .AddJwtBearer(option =>
            {
                option.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:Audience"],
                    ValidateIssuer=true,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidateIssuerSigningKey=true,
                    IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:secretkey"])),
                    ValidateLifetime=true,
                    ClockSkew=TimeSpan.FromDays(double.Parse(configuration["JWT:DurationInDay"]))
                 };
            });
            return services;
        }

    }
}

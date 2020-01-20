using System;
using Hiywin.Api.Authorize;
using Hiywin.Api.Models;
using Hiywin.BaseManager;
using Hiywin.Entities.Base;
using Hiywin.FrameManager;
using Hiywin.IBaseManager;
using Hiywin.IFrameManager;
using Hiywin.Models.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Hiywin.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var _secoretKey = Configuration["SecoretKey"];
            var _signingKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_secoretKey));
            //读取JWT配置
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuserOptions));
            services.Configure<JwtIssuserOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuserOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuserOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });
            //JwtBearer验证:
            services.AddSingleton<IJwtFactory, JwtFactory>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuserOptions.Issuer)];
                configureOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuserOptions.Issuer)],
                    ValidateAudience = true,
                    ValidAudience = jwtAppSettingOptions[nameof(JwtIssuserOptions.Audience)],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = _signingKey,
                    RequireExpirationTime = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                configureOptions.SaveToken = true;
            });

            //注入接口
            services.AddSingleton<ISqlConnModel, SqlConnModel>();
            /****** Hiywin.IBaseManager ******/
            services.AddSingleton<IInitialManager, InitialManager>();
            /****** Hiywin.IFrameManager *****/
            services.AddSingleton<IModuleManager, ModuleManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IInitialManager manager, ISqlConnModel conn)
        {
            conn.MysqlConn = Configuration["ConnectionStrings:MysqlConn"];
            conn.MssqlConn = Configuration["ConnectionStrings:MssqlConn"];

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //启用跨域
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
            });
            app.UseRouting();
            //请求错误提示配置
            app.UseErrorHandling();
            //使用认证授权
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //初始化
            manager.InitData(conn);
        }
    }
}

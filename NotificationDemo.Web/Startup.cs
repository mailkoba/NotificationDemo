using System;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NotificationDemo.DbContext;
using NotificationDemo.Migrations.Migrations;
using NotificationDemo.Service;
using NotificationDemo.Service.Dto;
using NotificationDemo.Service.Impls;
using NotificationDemo.Web.Filters;

namespace NotificationDemo.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            AppDomain.CurrentDomain.UnhandledException +=
                (sender, args) =>
                {
                    const string title = "Необработанное исключение!";
                    var ex = (Exception)args.ExceptionObject;

                    if (args.IsTerminating)
                    {
                        Serilog.Log.Fatal(ex, title);
                    }
                    else
                    {
                        Serilog.Log.Error(ex, title);
                    }
                };
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // logging
            services.AddSingleton(typeof(ILogger),
                provider => provider.GetRequiredService<ILoggerFactory>().CreateLogger("Default"));

            services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .SetIsOriginAllowed(_ => true);
                });
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.LoginPath = new PathString("/login");
                        options.AccessDeniedPath = new PathString("/login");
                        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                        options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
                        options.Cookie.HttpOnly = true;
                        options.Events = new CookieAuthenticationEvents
                        {
                            OnRedirectToLogin = context =>
                            {
                                context.Response.Clear();
                                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                return Task.CompletedTask;
                            },
                            OnRedirectToAccessDenied = context =>
                            {
                                if (!context.Request.Path.StartsWithSegments("/Api"))
                                {
                                    return Task.CompletedTask;
                                }

                                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                                return context.Response.WriteAsync(
                                    JsonConvert.SerializeObject(
                                        new ContainerDto<string>(
                                            "Недостаточно прав для выполнения действия!"),
                                        new JsonSerializerSettings
                                        {
                                            ContractResolver =
                                                new CamelCasePropertyNamesContractResolver()
                                        }),
                                    Encoding.UTF8);
                            }
                        };
                    });

            services.AddAuthorization();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IPushService, PushService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserContextService, UserContextService>();

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.EnableForHttps = true;
                options.MimeTypes = new[]
                {
                    // Default
                    "text/plain",
                    "text/css",
                    "application/javascript",
                    "text/html",
                    "application/xml",
                    "text/xml",
                    "application/json",
                    "text/json",
 
                    // Custom
                    "image/svg+xml",
                    "application/font-woff2"
                };
            });

            services.AddControllers(options =>
                {
                    options.Filters.Add<NotificationDemoAuthorizeFilter>();
                    options.Filters.Add<NotificationDemoExceptionFilterAttribute>();
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
                    options.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                    options.SerializerSettings.StringEscapeHandling = StringEscapeHandling.EscapeHtml;
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.Converters.Add(new IsoDateTimeConverter());
                });

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddDistributedMemoryCache();

            services.AddDbContext<NotificationDbContext>(
                options => options
                    .UseSqlServer(Configuration.GetConnectionString("Database"),
                        builder =>
                        {
                            builder.UseRelationalNulls()
                                //.MigrationsAssembly("NotificationDemo.Migrations.Migrations")
                                .MigrationsAssembly(typeof(InitialMigration).Assembly.FullName)
                                .MigrationsHistoryTable("VersionInfo");
                        }));

            //services.Configure<HstsOptions>(options =>
            //{
            //    options.IncludeSubDomains = true;
            //    options.MaxAge = TimeSpan.FromDays(365);
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            //app.UseCors("default");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
             //   app.UseHsts();
            }

     //       app.UseHttpsRedirection();

            //app.Use(async (context, next) =>
            //{
            //    context.Response.Headers.Add("X-Frame-Options", new[] { "SAMEORIGIN" });
            //    context.Response.Headers.Add("Expect-CT", new[] { "expect-ct: max-age=604800, report-uri=https://example.com" });
            //    context.Response.Headers.Add("X-XSS-Protection", new[] { "1; mode=block; report=https://example.com" });
            //    context.Response.Headers.Add("X-Content-Type-Options", new[] { "nosniff" });
            //    context.Response.Headers.Add("Referrer-Policy", new[] { "strict-origin-when-cross-origin" });
            //    context.Response.Headers.Add("Feature-Policy", new[] { "accelerometer 'none'; camera 'none'; geolocation 'self'; gyroscope 'none'; magnetometer 'none'; microphone 'none'; payment 'none'; usb 'none'" });
            //    context.Response.Headers.Add("Content-Security-Policy", new[] { "default-src 'self'; script-src 'self'; style-src 'self' *.msecnd.net; img-src 'self' data:; connect-src https: wss: 'self'; font-src 'self' c.s-microsoft.com; frame-src 'self'; form-action 'self'; upgrade-insecure-requests; report-uri https://example.com" });
            //    context.Response.Headers.Remove(HeaderNames.Server);
            //    context.Response.Headers.Remove("X-Powered-By");
            //    await next();
            //});

            app.UseResponseCompression();

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    if (env.IsDevelopment())
                    {
                        ctx.Context.Response.Headers[HeaderNames.CacheControl] = "no-cache, no-store, max-age=0";
                    }
                    else
                    {
                        const int durationInSeconds = 60 * 60; // 1 час
                        ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                            "public,max-age=" + durationInSeconds;
                    }
                }
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    // fix Spa startup bug
                    spa.Options.StartupTimeout = new TimeSpan(days: 0, hours: 0, minutes: 1, seconds: 30);
                    //spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });

            // применение миграций к БД
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<NotificationDbContext>();
            dbContext.Database.Migrate();
            dbContext.EnsureSeedInitialData();
        }
    }
}

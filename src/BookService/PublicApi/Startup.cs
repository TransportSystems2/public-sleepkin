using System.Collections.Generic;
using System.Text;
using AutoMapper;
using MediatR;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pillow.ApplicationCore.Interfaces;
using Pillow.ApplicationCore.Services;
using Pillow.Infrastructure.Data;
using Pillow.Infrastructure.Identity;
using Pillow.Infrastructure.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Pillow.ApplicationCore.Entities.BookAggregate;
using ApplicationCore.Interfaces;
using Infrastructure.Services;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Options;
using System.Net.Http;
using Pillow.ApplicationCore.Services.Subscriptions.Implementation;
using Pillow.ApplicationCore.Services.Subscriptions.Interfaces;
using Pillow.Infrastructure.Services;
using Pillow.PublicApi.BookEndpoints;

namespace Pillow.PublicApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BookContext>(c =>
                c.UseLazyLoadingProxies()
                    .UseNpgsql(Configuration.GetConnectionString("BookConnection")));

            // Add Identity DbContext
            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("IdentityConnection")));
            
            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<AppIdentityDbContext>()
                    .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            });

            services.AddScoped(typeof(IBookRepository), typeof(BookRepository));
            services.AddScoped(typeof(IAsyncRepository<>), typeof(BaseRepository<>));

            services.Configure<ApiSettings>(Configuration);
            services.Configure<JWTTokenOptions>(Configuration.GetSection("JWTToken"));
            services.AddSingleton<IUriComposer>(new UriComposer(Configuration.Get<ApiSettings>()));
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
            services.AddScoped<ITokenClaimsService, IdentityTokenClaimService>();
            services.AddScoped<IRefreshTokenFactory, RefreshTokenFactory>();
            services.AddScoped<ITokenValidator, TokenValidator>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();
            services.AddSingleton<IAudioFileGuide, AudioFileGuide>();
            services.AddSingleton<IContentTypeProvider, ContentTypeProvider>();

            services.AddScoped<IAppleAccessTokenFactory, AppleAccessTokenFactory>();
            services.Configure<PushNotificationOptions>(Configuration.GetSection("PushNotification"));
            services.AddHttpClient<IPushNotificationService, PushNotificationService>()
                .ConfigureHttpClient((sp, httpClient) =>
                {
                    var pushNotificationOptions = sp.GetRequiredService<IOptions<PushNotificationOptions>>().Value;
                    httpClient.BaseAddress = new Uri(pushNotificationOptions.BaseAddress);
                    httpClient.DefaultRequestHeaders.Add("apns-topic", pushNotificationOptions.BundleId);
                    httpClient.DefaultRequestVersion = new Version(2, 0);
                })
                .ConfigurePrimaryHttpMessageHandler(x => new HttpClientHandler {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                });

            services.Configure<AppStoreSubcriptionOptions>(Configuration.GetSection("AppStoreSubscription"));
            services.AddHttpClient<IAppStoreSubscriptionService, AppStoreSubscriptionService>()
                .ConfigureHttpClient((sp, httpClient) => 
                {
                    httpClient.DefaultRequestVersion = new Version(2, 0);
                })
                .ConfigurePrimaryHttpMessageHandler(x => new HttpClientHandler {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                });
            
            // Add memory cache services
            services.AddMemoryCache();

            services.Configure<TelegramBotOptions>(Configuration.GetSection("TelegramBot"));
            services.AddScoped<ITelegramBot, TelegramBot>();

            // https://stackoverflow.com/questions/46938248/asp-net-core-2-0-combining-cookies-and-bearer-authorization-for-the-same-endpoin
            var jWTTokenOptions = Configuration.GetSection("JWTToken").Get<JWTTokenOptions>();
            var key = Encoding.ASCII.GetBytes(jWTTokenOptions.SecretKey);
            services.AddAuthentication(config =>
            {
                //config.DefaultScheme = "smart";
                //config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                config.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    RequireExpirationTime = false,
                    ClockSkew = TimeSpan.Zero
                };

                x.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddControllers()
                .AddJsonOptions(options => 
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            services.AddMediatR(typeof(Book).Assembly);

            services.AddAutoMapper(typeof(Startup).Assembly);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                c.EnableAnnotations();
                c.SchemaFilter<CustomSchemaFilters>();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });

                foreach (var filePath in System.IO.Directory.GetFiles(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)), "*.xml"))
                {
                    c.IncludeXmlComments(filePath);
                }
            });

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.Configure<ListPaged.UserFreeBookLimitOptions>(Configuration.GetSection("UserFreeBookLimit"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            if (Configuration.GetValue<bool>("UseSwagger"))
            {
                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
                // specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
            }
        }
    }
}
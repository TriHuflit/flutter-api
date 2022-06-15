using Flutter.Backend.Common.Constains;
using Flutter.Backend.DAL.Contracts;
using Flutter.Backend.DAL.Implementations;
using Flutter.Backend.Service.Models.Mappers;
using Flutter.Backend.Service.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Flutter.Backend.Api
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

            services.AddCors(o => o.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            
            
            services.AddControllers()
            .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Flutter.Backend.Api", Version = "v1" });
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
            });
            string issuer = Configuration.GetValue<string>("Tokens:Issuer");
            string signingKey = Configuration.GetValue<string>("Tokens:Key");
            byte[] signingKeyBytes = System.Text.Encoding.UTF8.GetBytes(signingKey);

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = issuer,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = System.TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
                };
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            //register service and respository
            services.AddInterfaceServices();

            services.AddTransient<IBannerRepository, BannerRepository>();
            services.AddTransient<IProductRepository, ProductRespository>();
            services.AddTransient<ICategoryRepository, CaterogyRespository>();
            services.AddTransient<IBrandRepository, BrandRespository>();
            services.AddTransient<ITemplateSendMailRepository, TemplateSendMailRepository>();
            services.AddTransient<IClassifyProductRepository, ClassifyProductRepository>();
            services.AddTransient<IRoleRepository, RoleRespository>();
            services.AddTransient<IMessageRepository, MessageResResponsitory>();
            services.AddTransient<IAppUserRepository, AppUserRepository>();
            services.AddTransient<IOrderRepository, OrderRespository>();
            services.AddTransient<IVoucherRepository, VoucherRespository>();
            services.AddTransient<INewsRepository, NewsRepository>();
            // database,cloud,redis,and email
            services.AddHttpContextAccessor();
            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
            services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(Configuration.GetConnectionString("UrlConnection")));
            services.Configure<CloundinarySetting>(Configuration.GetSection(nameof(CloundinarySetting)));
            services.Configure<MailSettings>(Configuration.GetSection(nameof(MailSettings)));


            services.AddSingleton<ICloundinarySetting>(sp =>
               sp.GetRequiredService<IOptions<CloundinarySetting>>().Value);
            services.AddSingleton<IMailSettings>(sp =>
              sp.GetRequiredService<IOptions<MailSettings>>().Value);


        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Flutter.Backend.Api v1"));
            }

            app.UseCors();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

     
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EasyStory.API.Domain.Persistence.Contexts;
using EasyStory.API.Domain.Repositories;
using EasyStory.API.Domain.Services;
using EasyStory.API.Extensions;
using EasyStory.API.Persistence.Repositories;
using EasyStory.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace EasyStory.API
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
            //CORS configuration
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder => builder.AllowAnyOrigin());
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddDbContext<AppDbContext>(options =>
            {
                
                options.UseMySQL(Configuration.GetConnectionString("MySQLConnection"));
            });

            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IBookmarkRepository, BookmarkRepository>();
            services.AddScoped<IHashtagRepository, HashtagRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            services.AddScoped<IPostHashtagRepository,PostHashtagRepository>();
            services.AddScoped<IQualificationRepository, QualificationRepository>();

            services.AddRouting(options => options.LowercaseUrls = true);

            // Unit Of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IBookmarkService, BookmarkService>();
            services.AddScoped<IHashtagService, HashtagService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();
            services.AddScoped<IPostHashtagService, PostHashtagService>();
            services.AddScoped<IQualificationService, QualificationService>();

            services.AddAutoMapper(typeof(Startup));

            services.AddCustomSwagger();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            //CORS configuration
            app.UseCors(options =>
            {
                options.WithOrigins("htpp://localhost:8080");
                options.AllowAnyMethod();
                options.AllowAnyHeader();
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseCustomSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("https://localhost:44346/api-docs/v1/swagger.json", "My API V1");

            });
        }
    }
}

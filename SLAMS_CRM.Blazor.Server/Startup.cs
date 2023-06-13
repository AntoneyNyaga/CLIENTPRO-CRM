using DevExpress.AspNetCore.Reporting;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ApplicationBuilder;
using DevExpress.ExpressApp.Blazor.ApplicationBuilder;
using DevExpress.ExpressApp.Blazor.Services;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Server.Circuits;
using SLAMS_CRM.Blazor.Server.Services;

namespace SLAMS_CRM.Blazor.Server;

public class Startup
{
    public Startup(IConfiguration configuration) { Configuration = configuration; }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton(
            typeof(Microsoft.AspNetCore.SignalR.HubConnectionHandler<>),
            typeof(ProxyHubConnectionHandler<>));

        //hiding an exception
        services.ConfigureReportingServices(
            configurator =>
            {
                configurator.DisableCheckForCustomControllers();
            });
        // added template services
        services.AddScoped<TemplateService>();

        services.AddScoped<ContactService>();
        services.AddScoped<IContactService, ContactService>();

        // Ensure all persistent classes are added to XPDictionary
        XPDictionary dictionary = new ReflectionDictionary();
        dictionary.GetDataStoreSchema(typeof(Module.BusinessObjects.CustomerManagement.Contact).Assembly);

        // Configure your database connection string and XPO data store
        string connectionString = "XpoProvider=MySql;server=127.0.0.1;password=;port=3306;user id=root;database=slamscrmdb;Convert Zero Datetime=True;Connect Timeout=120;persist security info=true;Character Set=utf8";
        IDataStore dataStore = XpoDefault.GetConnectionProvider(connectionString, AutoCreateOption.DatabaseAndSchema);

        // Initialize ThreadSafeDataLayer
        XpoDefault.DataLayer = new ThreadSafeDataLayer(dictionary, dataStore);

        // Register IObjectSpaceProvider
        services.AddScoped<IObjectSpaceProvider>(
            provider =>
            {
                var objectSpaceProvider = new XPObjectSpaceProvider(provider, connectionString);
                return objectSpaceProvider;
            });

        // Register UnitOfWork
        services.AddScoped(
            provider =>
            {
                var session = new Session(XpoDefault.Session.DataLayer);
                var unitOfWork = new UnitOfWork((IDataLayer)session);

                return unitOfWork;
            });


        services.AddRazorPages();
        services.AddServerSideBlazor();
        services.AddHttpContextAccessor();
        services.AddScoped<CircuitHandler, CircuitHandlerProxy>();
        services.AddXaf(
            Configuration,
            builder =>
            {
                builder.UseApplication<SLAMS_CRMBlazorApplication>();
                builder.Modules
                    .AddAuditTrailXpo()
                    .AddCloningXpo()
                    .AddConditionalAppearance()
                    .AddDashboards(
                        options =>
                        {
                            options.DashboardDataType = typeof(DevExpress.Persistent.BaseImpl.DashboardData);
                        })
                    .AddOffice()
                    .AddReports(
                        options =>
                        {
                            options.EnableInplaceReports = true;
                            options.ReportDataType = typeof(DevExpress.Persistent.BaseImpl.ReportDataV2);
                            options.ReportStoreMode = DevExpress.ExpressApp.ReportsV2.ReportStoreModes.XML;
                            options.ShowAdditionalNavigation = true;
                        })
                    .AddValidation()
                    .Add<SLAMS_CRM.Module.SLAMS_CRMModule>()
                    .Add<SLAMS_CRMBlazorModule>();
                builder.ObjectSpaceProviders
                    .AddSecuredXpo(
                        (serviceProvider, options) =>
                        {
                            string connectionString = null;
                            if (Configuration.GetConnectionString("ConnectionString") != null)
                            {
                                connectionString = Configuration.GetConnectionString("ConnectionString");
                            }
#if EASYTEST
                    if(Configuration.GetConnectionString("EasyTestConnectionString") != null) {
                        connectionString = Configuration.GetConnectionString("EasyTestConnectionString");
                    }
#endif

                            ArgumentNullException.ThrowIfNull(connectionString);
                            options.ConnectionString = connectionString;
                            options.ThreadSafe = true;
                            options.UseSharedDataStoreProvider = true;
                        })
                    .AddNonPersistent();
                builder.Security
                    .UseIntegratedMode(
                        options =>
                        {
                            options.RoleType = typeof(PermissionPolicyRole);
                            // ApplicationUser descends from PermissionPolicyUser and supports the OAuth authentication. For more information, refer to the following topic: https://docs.devexpress.com/eXpressAppFramework/402197
                            // If your application uses PermissionPolicyUser or a custom user type, set the UserType property as follows:
                            options.UserType = typeof(SLAMS_CRM.Module.BusinessObjects.ApplicationUser);
                            // ApplicationUserLoginInfo is only necessary for applications that use the ApplicationUser user type.
                            // If you use PermissionPolicyUser or a custom user type, comment out the following line:
                            options.UserLoginInfoType = typeof(SLAMS_CRM.Module.BusinessObjects.ApplicationUserLoginInfo);
                            options.UseXpoPermissionsCaching();
                        })
                    .AddPasswordAuthentication(
                        options =>
                        {
                            options.IsSupportChangePassword = true;
                        });
            });
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(
                options =>
                {
                    options.LoginPath = "/LoginPage";
                });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. To change this for production scenarios, see: https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseRequestLocalization();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseXaf();
        app.UseEndpoints(
            endpoints =>
            {
                endpoints.MapXafEndpoints();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
                endpoints.MapControllers();
            });
    }
}

using Microsoft.Extensions.Logging;

using Data;
using Data.Models.Interfaces;
using Microsoft.Maui.Controls.PlatformConfiguration;

namespace MauiBlazorHybridApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            //builder.Services
            //    .AddOptions<BlogApiJsonDirectAccessSetting>()
            //    .Configure(options =>
            //    {
            //        options.DataPath = @"..\..\..\Data\";
            //        options.BlogPostsFolder = "Blogposts";
            //        options.TagsFolder = "Tags";
            //        options.CategoriesFolder = "Categories";
            //        options.CommentsFolder = "Comments";
            //    });

            builder.Services
                .AddOptions<BlogApiJsonDirectAccessSetting>()
                .Configure(options =>
                {
                    string dataPath = GetAppDataFolderPath();

                    options.DataPath = Path.Combine(dataPath, "Data");
                    options.BlogPostsFolder = "Blogposts";
                    options.TagsFolder = "Tags";
                    options.CategoriesFolder = "Categories";
                    options.CommentsFolder = "Comments";
                });

            builder.Services.AddScoped<IBlogApi, BlogApiJsonDirectAccess>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif
            // test paths
            //string path = GetAppDataFolderPath(); 

            return builder.Build();
        }


        private static string GetAppDataFolderPath()
        {
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            }
            else if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            }
            else if (DeviceInfo.Platform == DevicePlatform.iOS ||
                     DeviceInfo.Platform == DevicePlatform.macOS)
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            //else if (DeviceInfo.Platform == DevicePlatform.macOS)
            //{
            //    return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //}
            else
            {
                throw new PlatformNotSupportedException();
            }
        }
    }
}

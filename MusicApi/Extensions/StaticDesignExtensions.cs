namespace MusicApi.Design;

public static class StaticDesignExtensions
{
    public static WebApplication UseStaticDesign(this WebApplication app)
    {
        app.UseDefaultFiles();
        app.UseStaticFiles();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerUI(options =>
            {
                options.DocumentTitle = "Music API";
                options.InjectStylesheet("/css/swagger.css");
            });
        }

        return app;
    }
}

Imports System
Imports Microsoft.AspNetCore.Builder
Imports Microsoft.AspNetCore.Hosting
Imports Microsoft.Extensions.DependencyInjection

Module Program
    Sub Main(args As String())

        Dim builder = WebApplication.CreateBuilder(args)
        builder.Services.AddControllers()

        builder.Services.AddEndpointsApiExplorer()
        builder.Services.AddSwaggerGen()


        builder.Services.AddCors(Sub(options)
                                     options.AddDefaultPolicy(Sub(corsBuilder)
                                                                  corsBuilder.AllowAnyOrigin() _
                                                                 .AllowAnyHeader() _
                                                                 .AllowAnyMethod()
                                                              End Sub)
                                 End Sub)

        builder.WebHost.UseUrls("http://localhost:5000")

        Dim app = builder.Build()

        app.UseSwagger()
        app.UseSwaggerUI()


        app.UseCors()


        app.MapGet("/", Function() "Hello World!")

        app.MapControllers()

        app.Run()
        Console.WriteLine("Starting server!")
    End Sub
End Module
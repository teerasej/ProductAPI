Imports System
Imports Microsoft.AspNetCore.Builder
Imports Microsoft.AspNetCore.Cors.Infrastructure
Imports Microsoft.Extensions.DependencyInjection

Module Program
    Sub Main(args As String())

        Dim builder = WebApplication.CreateBuilder(args)
        builder.Services.AddControllers()

        builder.Services.AddEndpointsApiExplorer()
        builder.Services.AddSwaggerGen()

        ' เพิิ่มการทำงานของ Cors ที่อนุญาตให้ client สามารถส่ง request มาได้
        builder.Services.AddCors(Sub(options)
                                     options.AddDefaultPolicy(Sub(corsBuilder)
                                                                  corsBuilder.AllowAnyOrigin() _
                                                                 .AllowAnyHeader() _
                                                                 .AllowAnyMethod()
                                                              End Sub)
                                 End Sub)

        Dim app = builder.Build()

        app.UseSwagger()
        app.UseSwaggerUI()

        ' เปิดการทำงานของ CORS ใน App
        app.UseCors()


        app.MapGet("/", Function() "Hello World!")

        app.MapControllers()

        app.Run()
        Console.WriteLine("Starting server!")
    End Sub
End Module
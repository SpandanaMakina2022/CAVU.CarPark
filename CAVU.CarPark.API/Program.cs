using System.Globalization;
using CAVU.CarPark.Abstraction.Data;
using CAVU.CarPark.Abstraction.Service;
using CAVU.CarPark.Data;
using CAVU.CarPark.Service;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
    //.AddNewtonsoftJson(options =>
    //{
    //    var dateConverter = new Newtonsoft.Json.Converters.IsoDateTimeConverter
    //    {
    //        DateTimeFormat = "dd'-'MM'-'yyyy'T'HH':'mm"
    //    };

    //    options.SerializerSettings.Converters.Add(dateConverter);
    //    options.SerializerSettings.Culture = new CultureInfo("en-IE");
    //    options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
    //});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IBookingData, BookingData>();

builder.Services.AddSingleton<IBookingService, BookingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using JsonWebTokenAPI.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//INYECCI�N DE DEPENDENCIAS DEL SERVICIO AUTENTHICATIONSERVICE
builder.Services.AddScoped(typeof(IAuthenticationService), typeof(AuthenticationService));

//ENABLE JWT AUTHORIZATION
var privateKey = builder.Configuration.GetValue<string>("Authentication:JWT:Key");
builder.Services.AddAuthentication(option =>
{
    //SE DEFINE QUE EL ESQUEMA UTILIZADO SER� EL DEL TOKEN JWT
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>      //HABILITACI�N JWT EMPLEANDO EL ESQUEMA POR DEFECTO ANTERIOR
{
    options.RequireHttpsMetadata = false;   // NO SE REQUIERE HTTPS PARA LA VALIDACI�N JWT PORQUE ESTOY  EN ENTORNO DESARROLLO
    options.TokenValidationParameters = new TokenValidationParameters
    { 
        //PAR�METROS QUE SE VALIDARAN
        ValidateIssuer = false,                                                             //EMISOR DE LA PETICI�N
        ValidateAudience = false,                                                           //SERVIDOR DE ENTRADA
        ValidateIssuerSigningKey = true,                                                    //FIRMA
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKey)),
        ValidateLifetime = true,
        RequireExpirationTime = true,
        ClockSkew = TimeSpan.Zero
    };
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//SE A�ADE A LA CONFIGURACI�N DE SWAGGER LA DEFINICI�N DE USO DE BEARER TOKEN PARA INCLUIR COMO PAR�METRO DE LA CABECERA DE CADA SOLICITUD
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "JWT Authorization API" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.ApiKey,   //Par�metro de seguridad empleado como clave API
        Scheme = "Bearer",
        BearerFormat = "JWT",               //Formato del token JWT
        In = ParameterLocation.Header,      //Incluir� el valor en la cabecera
        Name = "Authorization",             //Clave del par�metro de la cabecera
        Description = "Cabecera autorizaci�n JWT. \r\n Introduce ['Bearer'] [un espacio] [Token]"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()  //Indico que es un requisito enviar el token, para poder ser validado por la aplicaci�n
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              }
          }, new string[] { }
          }
        });
});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "JWT Authorization API"));

    app.UseDeveloperExceptionPage();
}




//app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();
});

app.Run();

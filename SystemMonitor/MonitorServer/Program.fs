module Program

open Domain
open Falco
open Falco.Routing
open Falco.HostBuilder
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting


let configureServices (configuration: IConfiguration) (services: IServiceCollection) =

    let settings =
         { apiKey = configuration.GetValue("ApiKey")
           clientKey = configuration.GetValue("ClientKey") }

    // let mongoContext = DbContext(settings) :> IDbContext


    services
        // .AddSingleton<IDbContext>(mongoContext)
        .AddSingleton<ServerState>(ServerState.initial)
        .AddSingleton<Settings>(settings)
        .AddCors()
        .AddFalco()
    |> ignore


let configureCors (corsBuilder: CorsPolicyBuilder) : unit =
    corsBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
    |> ignore

let configureApp: IApplicationBuilder -> IApplicationBuilder =
    fun app ->
        app.UseCors(fun options -> configureCors options) |> ignore
        app.UseStaticFiles(StaticFileOptions(RequestPath = "/static" )) |> ignore 
        // app.UseAuthentication() |> ignore
        // app.UseAuthorization() |> ignore

        app

let configureWebHost (configuration: IConfiguration) (webHost: IHostBuilder) =
    webHost.ConfigureServices(configureServices configuration)

[<EntryPoint>]
let main args =
    let config = configuration [||] { required_json "appsettings.json" }

    webHost [||] {
        use_middleware configureApp
        host (configureWebHost config)

        endpoints (
            [ 
              get "/_version" (Response.ofPlainText "Hello world") ]
            @ Endpoints.handlers
        )

    }

    0

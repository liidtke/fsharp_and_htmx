module Security

open System
open System.IdentityModel.Tokens.Jwt
open System.Security.Claims
open System.Text
open Domain
open Microsoft.Extensions.Configuration
open Microsoft.IdentityModel.Tokens
open Falco.Security

type Token =
    { Token: string
      Type: string
      Expiration: DateTime }

type SecuritySettings =
    { Salt: string
      Iterations: int32
      JwtSecurityKey: string
      TokenExpirationTime: int32
      JwtIssuer: string
      JwtAudience: string }

type LoginRequest = { Email: string; Password: string }

let getClaims (user: User) =
    [ Claim("Email", user.email)
      Claim("UserId", user.id.ToString())
      Claim("Name", user.name)
      Claim(ClaimTypes.Role, if user.isVerified then "User" else "Anon")
      ]

let generateToken (config: SecuritySettings) (user: User) =
    let symmetricKey =
        config.JwtSecurityKey
        |> Encoding.UTF8.GetBytes
        |> SymmetricSecurityKey

    let credentials =
        SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256)

    let expiration =
        config.TokenExpirationTime
        |> DateTime.Now.AddHours

    let notBefore = DateTime.Now
    
    let claims = getClaims user

    let securityToken =
        JwtSecurityToken(config.JwtIssuer, config.JwtAudience, claims, System.Nullable(), expiration, credentials)

    let handler = JwtSecurityTokenHandler()
    let token = handler.WriteToken securityToken

    { Token = token
      Type = "Bearer"
      Expiration = securityToken.ValidTo }

let hashPassword (config: SecuritySettings) =
    let hash (password: string) =
        password
        |> Crypto.sha256 config.Iterations 32 config.Salt

    hash

let validateUserPassword (config: SecuritySettings) =
    let validate (user: User) (password: string) =
        let hashedPassword = password |> hashPassword config
        hashedPassword = user.password

    validate

// let login (db: IMongoContext) (config: SecuritySettings)  (request: LoginRequest) =
//     let generate (user: User) (valid: bool) =
//         match valid with
//         | true -> succeed (generateToken config user)
//         | false -> forbid "Usuário não existe ou senha incorreta"
//
//     let workflow (getUserEffect: string -> User Option) =
//         let userOption = getUserEffect request.Email
//
//         match userOption with
//         | Some user ->
//             validateUserPassword config user request.Password
//             |> generate user
//         | None -> forbid "Usuário não existe ou senha incorreta"
//
//     workflow <| Effects.User.getOne db

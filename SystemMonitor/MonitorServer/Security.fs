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


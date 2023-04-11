# ProductAPI 


## SSL Certificate on target machine 

The ASP.NET Core developer certificate is in an invalid state. To fix this issue, run 

```bash
dotnet dev-certs https --clean
dotnet dev-certs https'
```

to remove all existing ASP.NET Core development certificates and create a new untrusted developer certificate. 

## trust Certificate

On macOS or Windows, to trust the new certificate, run following code:

```
dotnet dev-certs https --trust
```



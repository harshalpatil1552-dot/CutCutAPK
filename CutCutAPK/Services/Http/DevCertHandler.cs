namespace CutCutAPK.Services.Http;

/// <summary>
/// The local CutCut.API "https" launch profile serves HTTPS using ASP.NET Core's self-signed dev
/// certificate, which the Android emulator (and most physical devices) don't trust out of the box
/// — there's no OS-level step to "trust" it the way `dotnet dev-certs https --trust` does on the
/// machine actually running the API. Skipping validation here is scoped to DEBUG builds only and
/// only ever talks to the loopback host ApiConstants.BaseUrl points at; Release builds never call
/// this and always validate certificates normally.
/// </summary>
public static class DevCertHandler
{
    public static HttpMessageHandler Create()
    {
        var handler = new HttpClientHandler();

#if DEBUG
        handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
#endif

        return handler;
    }
}

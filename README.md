public class KerberosDelegationDelegatingHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _contextAccessor;

    public KerberosDelegationDelegatingHandler(
        IHttpContextAccessor contextAccessor
    )
    {
        _contextAccessor = contextAccessor;
    }

    protected override HttpResponseMessage Send(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        if (_contextAccessor.HttpContext?.User?.Identity is not WindowsIdentity windowsIdentity)
            return base.Send(request, cancellationToken);
        return WindowsIdentity.RunImpersonated(
            windowsIdentity.AccessToken,
            () => base.Send(request, cancellationToken)
        );
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        if (_contextAccessor.HttpContext?.User?.Identity is not WindowsIdentity windowsIdentity)
            return base.SendAsync(request, cancellationToken);
        return WindowsIdentity.RunImpersonatedAsync(
            windowsIdentity.AccessToken,
            () => base.SendAsync(request, cancellationToken)
        );
    }
}

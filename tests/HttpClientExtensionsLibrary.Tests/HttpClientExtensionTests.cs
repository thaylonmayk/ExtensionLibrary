using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HttpClientExtensionsLibrary;
using Xunit;

namespace HttpClientExtensionsLibrary.Tests;

public class TestDelegatingHandler : HttpMessageHandler
{
    private int _invocationCount = 0;
    public int InvocationCount => _invocationCount;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _invocationCount++;
        if (_invocationCount < 2)
        {
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError));
        }
        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
    }
}

public class HttpClientExtensionTests
{
    [Fact]
    public async Task SendWithRetryAsync_ShouldRetryWithFreshRequest()
    {
        var handler = new TestDelegatingHandler();
        var client = new HttpClient(handler);

        var response = await client.SendWithRetryAsync(() => new HttpRequestMessage(HttpMethod.Get, "http://localhost/test"), retryCount: 3);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(2, handler.InvocationCount);
    }

    [Fact]
    public async Task SendWithRetryAsync_WithHttpRequestMessage_ShouldCloneRequestAndSucceed()
    {
        var handler = new TestDelegatingHandler();
        var client = new HttpClient(handler);
        using var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/test");

        var response = await client.SendWithRetryAsync(request, retryCount: 3);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(2, handler.InvocationCount);
    }

    [Fact]
    public async Task ExponentialBackoffRetryAsync_ShouldRetryAndSucceed()
    {
        var handler = new TestDelegatingHandler();
        var client = new HttpClient(handler);
        using var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/test");

        var response = await client.ExponentialBackoffRetryAsync(request, retryCount: 3, baseDelayMilliseconds: 10);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(2, handler.InvocationCount);
    }

    [Fact]
    public async Task SendWithRetryAsync_WithNullClient_ShouldThrowArgumentNullException()
    {
        HttpClient? nullClient = null;
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            nullClient.SendWithRetryAsync(() => new HttpRequestMessage(HttpMethod.Get, "http://localhost")));
    }

    [Fact]
    public async Task SendWithRetryAsync_WithNullFactory_ShouldThrowArgumentNullException()
    {
        var client = new HttpClient();
        await Assert.ThrowsAsync<ArgumentNullException>(() =>
            client.SendWithRetryAsync((Func<HttpRequestMessage>)null!));
    }

    [Fact]
    public void HasClaim_ShouldIdentifyJwtClaims()
    {
        var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var descriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
        {
            Subject = new System.Security.Claims.ClaimsIdentity(new[]
            {
                new System.Security.Claims.Claim("sub", "user-999"),
                new System.Security.Claims.Claim("role", "Manager")
            })
        };
        var tokenString = handler.CreateEncodedJwt(descriptor);

        Assert.True(HttpClientExtensions.HasClaim(tokenString, "sub", "user-999"));
        Assert.True(HttpClientExtensions.HasClaim(tokenString, "role", "Manager"));
        Assert.False(HttpClientExtensions.HasClaim(tokenString, "role", "Guest"));
    }
}

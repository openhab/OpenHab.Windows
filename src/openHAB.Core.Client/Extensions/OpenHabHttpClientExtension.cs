﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using openHAB.Core.Client.Options;

namespace openHAB.Core.Client.Extensions;

/// <summary>
/// Provides extension methods for adding OpenHAB HTTP clients to the service collection.
/// </summary>
public static class OpenHabHttpClientExtension
{
    /// <summary>
    /// Adds the OpenHAB HTTP clients to the specified service collection.
    /// </summary>
    /// <param name="services">The service collection to add the HTTP clients to.</param>
    public static void AddOpenHabHttpClients(this IServiceCollection services)
    {
        AddHttpClient(services, "local", options => options.LocalConnection);
        AddHttpClient(services, "remote", options => options.RemoteConnection);
    }

    private static void AddHttpClient(IServiceCollection services, string name, Func<ConnectionOptions, Connection.Models.Connection> getConnection)
    {
        services.AddHttpClient<OpenHABClient>(name, (sp, client) =>
        {
            var options = sp.GetRequiredService<IOptions<ConnectionOptions>>().Value;
            var connection = getConnection(options);
            ConfigureHttpClient(client, connection);
        }).ConfigurePrimaryHttpMessageHandler(sp =>
        {
            var options = sp.GetRequiredService<IOptions<ConnectionOptions>>().Value;
            var connection = getConnection(options);
            return CreateHttpClientHandler(connection);
        });
    }

    private static void ConfigureHttpClient(HttpClient client, Connection.Models.Connection connection)
    {
        client.BaseAddress = new Uri(connection.Url);

        if (!string.IsNullOrEmpty(connection.Username) && !string.IsNullOrEmpty(connection.Password))
        {
            string basicCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{connection.Username}:{connection.Password}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicCredentials);
        }

        //client.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    private static HttpClientHandler CreateHttpClientHandler(Connection.Models.Connection connection)
    {
        return new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (errors == SslPolicyErrors.None)
                {
                    return true;
                }

                bool result = true;
                if (errors.HasFlag(SslPolicyErrors.RemoteCertificateChainErrors))
                {
                    result &= connection.WillIgnoreSSLCertificate.GetValueOrDefault();
                }

                if (errors.HasFlag(SslPolicyErrors.RemoteCertificateNameMismatch))
                {
                    result &= connection.WillIgnoreSSLHostname.GetValueOrDefault();
                }

                if (errors.HasFlag(SslPolicyErrors.RemoteCertificateNotAvailable))
                {
                    result = false;
                }

                return result;
            }
        };
    }
}

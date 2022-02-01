﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using RX.Nyss.Common.Utils.DataContract;
using RX.Nyss.Common.Utils.Logging;
using static RX.Nyss.Common.Utils.DataContract.Result;

namespace RX.Nyss.Common.Services.StringsResources;

public interface IStringsResourcesService
{
    Task<StringsResourcesVault> GetStrings(string languageCode);

    Task<Result<IDictionary<string, StringResourceValue>>> GetStringsResources(string languageCode);

    Task<Result<IDictionary<string, string>>> GetEmailContentResources(string languageCode);

    Task<StringsBlob> GetStringsBlob();

    Task<StringsBlob> GetEmailContentBlob();

    Task<StringsBlob> GetSmsContentBlob();

    Task SaveStringsBlob(StringsBlob blob);

    Task SaveEmailContentsBlob(StringsBlob blob);

    Task SaveSmsContentsBlob(StringsBlob blob);

    Task<Result<IDictionary<string, string>>> GetSmsContentResources(string languageCode);
}

public class StringsResourcesService : IStringsResourcesService
{
    private readonly IGeneralBlobProvider _generalBlobProvider;

    private readonly ILoggerAdapter _loggerAdapter;

    public StringsResourcesService(
        IGeneralBlobProvider generalBlobProvider,
        ILoggerAdapter loggerAdapter)
    {
        _generalBlobProvider = generalBlobProvider;
        _loggerAdapter = loggerAdapter;
    }

    public async Task<StringsResourcesVault> GetStrings(string languageCode)
    {
        var result = await GetStringsResources(languageCode);

        return new StringsResourcesVault(result.Value);
    }

    public async Task<Result<IDictionary<string, StringResourceValue>>> GetStringsResources(string languageCode)
    {
        try
        {
            var stringBlob = await GetStringsBlob();

            var dictionary = stringBlob.Strings
                .Select(entry => new
                {
                    entry.Key,
                    Value = new StringResourceValue
                    {
                        Value = entry.GetTranslation(languageCode),
                        NeedsImprovement = entry.NeedsImprovement
                    }
                }).ToDictionary(x => x.Key, x => x.Value);

            return Success<IDictionary<string, StringResourceValue>>(dictionary);
        }
        catch (Exception exception)
        {
            _loggerAdapter.Error(exception, "There was a problem during fetching the strings resources");
            return Error<IDictionary<string, StringResourceValue>>(ResultKey.UnexpectedError);
        }
    }

    public async Task<Result<IDictionary<string, string>>> GetEmailContentResources(string languageCode)
    {
        try
        {
            var emailContentsBlob = await GetEmailContentBlob();

            var dictionary = emailContentsBlob.Strings
                .Select(entry => new
                {
                    entry.Key,
                    Value = entry.GetTranslation(languageCode)
                })
                .OrderBy(x => x.Key)
                .ToDictionary(x => x.Key, x => x.Value);

            return Success<IDictionary<string, string>>(dictionary);
        }
        catch (Exception exception)
        {
            _loggerAdapter.Error(exception, "There was a problem during fetching the email contents resources");
            return Error<IDictionary<string, string>>(ResultKey.UnexpectedError);
        }
    }

    public async Task<Result<IDictionary<string, string>>> GetSmsContentResources(string languageCode)
    {
        try
        {
            var smsContentBlob = await GetSmsContentBlob();

            var dictionary = smsContentBlob.Strings
                .Select(entry => new
                {
                    entry.Key,
                    Value = entry.GetTranslation(languageCode)
                })
                .OrderBy(x => x.Key)
                .ToDictionary(x => x.Key, x => x.Value);

            return Success<IDictionary<string, string>>(dictionary);
        }
        catch (Exception exception)
        {
            _loggerAdapter.Error(exception, "There was a problem during fetching the Sms contents resources");
            return Error<IDictionary<string, string>>(ResultKey.UnexpectedError);
        }
    }

    public async Task<StringsBlob> GetStringsBlob()
    {
        var blobValue = await _generalBlobProvider.GetStringsResources();

        return JsonSerializer.Deserialize<StringsBlob>(blobValue, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
    }

    public async Task SaveStringsBlob(StringsBlob blob)
    {
        var blobValue = JsonSerializer.Serialize(blob, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        await _generalBlobProvider.SaveStringsResources(blobValue);
    }

    public async Task SaveEmailContentsBlob(StringsBlob blob)
    {
        var blobValue = JsonSerializer.Serialize(blob, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        await _generalBlobProvider.SaveEmailContentResources(blobValue);
    }

    public async Task SaveSmsContentsBlob(StringsBlob blob)
    {
        var blobValue = JsonSerializer.Serialize(blob, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        await _generalBlobProvider.SaveSmsContentResources(blobValue);
    }

    public async Task<StringsBlob> GetEmailContentBlob()
    {
        var blobValue = await _generalBlobProvider.GetEmailContentResources();

        return JsonSerializer.Deserialize<StringsBlob>(blobValue, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
    }

    public async Task<StringsBlob> GetSmsContentBlob()
    {
        var blobValue = await _generalBlobProvider.GetSmsContentResources();

        return JsonSerializer.Deserialize<StringsBlob>(blobValue, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
    }
}
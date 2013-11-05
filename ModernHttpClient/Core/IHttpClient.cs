using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Authentication.ExtendedProtection;
using System.Threading;
using System.Threading.Tasks;

namespace Cheesebaron.MvxPlugins.ModernHttpClient
{
    public interface IHttpClient : IHttpMessageInvoker
    {
        Uri BaseAddress { get; set; }
        IHttpRequestHeaders DefaultRequestHeaders { get; }
        long MaxResponseContentBufferSize { get; set; }
        TimeSpan Timeout { get; set; }
        void CancelPendingRequests();

        Task<IHttpResponseMessage> DeleteAsync(string requestUri);
        Task<IHttpResponseMessage> DeleteAsync(string requestUri, CancellationToken cancellationToken);

        Task<IHttpResponseMessage> DeleteAsync(Uri requestUri);
        Task<IHttpResponseMessage> DeleteAsync(Uri requestUri, CancellationToken cancellationToken);

        Task<IHttpResponseMessage> GetAsync(string requestUri);
        Task<IHttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken);
        Task<IHttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption);
        Task<IHttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption,
            CancellationToken cancellationToken);

        Task<IHttpResponseMessage> GetAsync(Uri requestUri);
        Task<IHttpResponseMessage> GetAsync(Uri requestUri, CancellationToken cancellationToken);
        Task<IHttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption);
        Task<IHttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption,
            CancellationToken cancellationToken);

        Task<IHttpResponseMessage> PostAsync(string requestUri, IHttpContent content);
        Task<IHttpResponseMessage> PostAsync(string requestUri, IHttpContent content, 
            CancellationToken cancellationToken);

        Task<IHttpResponseMessage> PostAsync(Uri requestUri, IHttpContent content);
        Task<IHttpResponseMessage> PostAsync(Uri requestUri, IHttpContent content,
            CancellationToken cancellationToken);

        Task<IHttpResponseMessage> PutAsync(string requestUri, IHttpContent content);
        Task<IHttpResponseMessage> PutAsync(string requestUri, IHttpContent content,
            CancellationToken cancellationToken);

        Task<IHttpResponseMessage> PutAsync(Uri requestUri, IHttpContent content);
        Task<IHttpResponseMessage> PutAsync(Uri requestUri, IHttpContent content,
            CancellationToken cancellationToken);

        Task<IHttpResponseMessage> SendAsync(IHttpRequestMessage request);
        Task<IHttpResponseMessage> SendAsync(IHttpRequestMessage request, HttpCompletionOption completionOption);
        new Task<IHttpResponseMessage> SendAsync(IHttpRequestMessage request, CancellationToken cancellationToken);
        Task<IHttpResponseMessage> SendAsync(IHttpRequestMessage request, HttpCompletionOption completionOption,
            CancellationToken cancellationToken);

        Task<byte[]> GetByteArrayAsync(string requestUri);
        Task<byte[]> GetByteArrayAsync(Uri requestUri);
        Task<Stream> GetStreamAsync(string requestUri);
        Task<Stream> GetStreamAsync(Uri requestUri);
        Task<string> GetStringAsync(string requestUri);
        Task<string> GetStringAsync(Uri requestUri);

    }

    public interface IHttpMessageInvoker : IDisposable
    {
        Task<IHttpResponseMessage> SendAsync(IHttpRequestMessage request, CancellationToken cancellationToken);
    }

    public interface IHttpResponseMessage : IDisposable
    {
        IHttpContent Content { get; set; }
        IHttpResponseHeaders Headers { get; set; }
        bool IsSuccessStatusCode { get; }
        string ReasonPhrase { get; }
        IHttpRequestMessage RequestMessage { get; set; }
        HttpStatusCode StatusCode { get; set; }
        Version Version { get; set; }
    }

    public interface IHttpRequestMessage : IDisposable
    {
        IHttpContent Content { get; set; }
        IHttpRequestHeaders Headers { get; }
        IHttpMethod Method { get; set; }
        IDictionary<string, object> Properties { get; }
        Uri RequestUri { get; }
        Version Version { get; set; }
    }

    public interface IHttpMethod : IEquatable<IHttpMethod>
    {
        IHttpMethod Delete { get; }
        IHttpMethod Get { get; }
        IHttpMethod Head { get; }
        string Method { get; }
        IHttpMethod Options { get; }
        IHttpMethod Post { get; }
        IHttpMethod Put { get; }
        IHttpMethod Trace { get; }
    }

    public interface IHttpRequestHeaders
    {
        IHttpHeaderValueCollection<IMediaTypeWithQualityHeaderValue> Accept { get; }
        IHttpHeaderValueCollection<IStringWithQualityHeaderValue> AcceptCharset { get; }
        IHttpHeaderValueCollection<IStringWithQualityHeaderValue> AcceptEncoding { get; }
        IHttpHeaderValueCollection<IStringWithQualityHeaderValue> AcceptLanguage { get; }
        IAuthenticationHeaderValue Authorization { get; set; }
        ICacheControlHeaderValue CacheControl { get; set; }
        IHttpHeaderValueCollection<string> Connection { get; }
        bool? ConnectionClose { get; set; }
        DateTimeOffset? Date { get; set; }
        IHttpHeaderValueCollection<INameValueWithParametersHeaderValue> Expect { get; }
        bool? ExpectContinue { get; set; }
        string From { get; set; }
        string Host { get; set; }
        IHttpHeaderValueCollection<IEntityTagHeaderValue> IfMatch { get; }
        DateTimeOffset? IfModifiedSince { get; set; }
        IHttpHeaderValueCollection<IEntityTagHeaderValue> IfNoneMatch { get; }
        IRangeConditionHeaderValue IfRange { get; set; }
        DateTimeOffset? IfUnmodifiedSince { get; set; }
        int? MaxForwards { get; set; }
        IHttpHeaderValueCollection<INameValueHeaderValue> Pragma { get; }
        IAuthenticationHeaderValue ProxyAuthorization { get; set; }
        IRangeHeaderValue Range { get; set; }
        Uri Referrer { get; set; }
        IHttpHeaderValueCollection<ITransferCodingWithQualityHeaderValue> TE { get; }
        IHttpHeaderValueCollection<string> Trailer { get; }
        IHttpHeaderValueCollection<ITransferCodingHeaderValue> TransferEncoding { get; }
        bool? TransferEncodingChunked { get; }
        IHttpHeaderValueCollection<IProductHeaderValue> Upgrade { get; }
        IHttpHeaderValueCollection<IProductInfoHeaderValue> UserAgent { get; }
        IHttpHeaderValueCollection<IViaHeaderValue> Via { get; }
        IHttpHeaderValueCollection<IWarningHeaderValue> Warning { get; }
    }

    public interface ITransferCodingWithQualityHeaderValue : ITransferCodingHeaderValue
    {
        double? Quality { get; set; }
        new ITransferCodingWithQualityHeaderValue Parse(string input);
        bool TryParse(string input, out ITransferCodingWithQualityHeaderValue parsedValue);
    }

    public interface IRangeItemHeaderValue
    {
        long? From { get; }
        long? To { get; }
    }

    public interface IRangeHeaderValue
    {
        ICollection<IRangeItemHeaderValue> Ranges { get; }
        string Unit { get; set; }
        IRangeHeaderValue Parse(string input);
        bool TryParse(string input, out IRangeHeaderValue parsedValue);
    }

    public interface IRangeConditionHeaderValue
    {
        DateTimeOffset? Date { get; }
        IEntityTagHeaderValue EntityTag { get; }
        IRangeConditionHeaderValue Parse(string input);
        bool TryParse(string input, out IRangeConditionHeaderValue parsedValue);
    }

    public interface INameValueWithParametersHeaderValue
    {
        ICollection<INameValueHeaderValue> Parameters { get; }
        INameValueWithParametersHeaderValue Parse(string input);
        bool TryParse(string input, out INameValueWithParametersHeaderValue parsedValue);
    }

    public interface IStringWithQualityHeaderValue
    {
        double? Quality { get; }
        string Value { get; }
        IStringWithQualityHeaderValue Parse(string input);
        bool TryParse(string input, out IStringWithQualityHeaderValue parsedValue);
    }

    public interface IMediaTypeWithQualityHeaderValue : IMediaTypeHeaderValue
    {
        double? Quality { get; set; }
        new IMediaTypeWithQualityHeaderValue Parse(string input);
        bool TryParse(string input, out IMediaTypeWithQualityHeaderValue parsedValue);
    }

    public interface IHttpContent : IDisposable
    {
        Task CopyToAsync(Stream stream);
        Task CopyToAsync(Stream stream, ITransportContext context);
        Task LoadIntoBufferAsync();
        Task LoadIntoBufferAsync(long maxBufferSize);
        Task<Stream> ReadAsStreamAsync();
        Task<byte[]> ReadAsByteArrayAsync();
        Task<string> ReadAsStringAsync();
    }

    public interface IHttpResponseHeaders : IHttpHeaders
    {
        IHttpHeaderValueCollection<string> AcceptRanges { get; }
        TimeSpan? Age { get; set; }
        ICacheControlHeaderValue CacheControl { get; set; }
        IHttpHeaderValueCollection<string> Connection { get; }
        bool? ConnectionClose { get; set; }
        DateTimeOffset? Date { get; set; }
        IEntityTagHeaderValue ETag { get; set; }
        Uri Location { get; set; }
        IHttpHeaderValueCollection<INameValueHeaderValue> Pragma { get; }
        IHttpHeaderValueCollection<IAuthenticationHeaderValue> ProxyAuthenticate { get; }
        IRetryConditionHeaderValue RetryAfter { get; set; }
        IHttpHeaderValueCollection<IProductInfoHeaderValue> Server { get; }
        IHttpHeaderValueCollection<string> Trailer { get; }
        IHttpHeaderValueCollection<ITransferCodingHeaderValue> TransferEncoding { get; }
        bool? TransferEncodingChunked { get; set; }
        IHttpHeaderValueCollection<IProductHeaderValue> Upgrade { get; }
        IHttpHeaderValueCollection<string> Vary { get; }
        IHttpHeaderValueCollection<IViaHeaderValue> Via { get; }
        IHttpHeaderValueCollection<IWarningHeaderValue> Warning { get; }
        IHttpHeaderValueCollection<IAuthenticationHeaderValue> WwwAuthenticate { get; }
    }

    public interface IWarningHeaderValue
    {
        string Agent { get; }
        int Code { get; }
        DateTimeOffset? Date { get; }
        string Text { get; }
        IWarningHeaderValue Parse(string input);
        bool TryParse(string input, out IWarningHeaderValue parsedValue);
    }

    public interface IViaHeaderValue
    {
        string Comment { get; }
        string ProtocolName { get; }
        string ProtocolVersion { get; }
        string ReceivedBy { get; }
        IViaHeaderValue Parse(string input);
        bool TryParse(string input, out IViaHeaderValue parsedValue);
    }

    public interface ITransferCodingHeaderValue
    {
        ICollection<INameValueHeaderValue> Parameters { get; }
        string Value { get; }
        ITransferCodingHeaderValue Parse(string input);
        bool TryParse(string input, out ITransferCodingHeaderValue parsedValue);
    }

    public interface IProductInfoHeaderValue
    {
        string Comment { get; }
        IProductHeaderValue Product { get; }
        IProductInfoHeaderValue Parse(string input);
        bool TryParse(string input, out IProductInfoHeaderValue parsedValue);
    }

    public interface IProductHeaderValue
    {
        string Name { get; }
        string Version { get; }
        IProductHeaderValue Parse(string input);
        bool TryParse(string input, out IProductHeaderValue parsedValue);
    }

    public interface IRetryConditionHeaderValue
    {
        DateTimeOffset? Date { get; }
        TimeSpan? Delta { get; }
        IRetryConditionHeaderValue Parse(string input);
        bool TryParse(string input, out IRetryConditionHeaderValue parsedValue);
    }

    public interface IAuthenticationHeaderValue
    {
        string Parameter { get; }
        string Scheme { get; }
        IAuthenticationHeaderValue Parse(string input);
        bool TryParse(string input, out IAuthenticationHeaderValue parsedValue);
    }

    public interface IEntityTagHeaderValue
    {
        IEntityTagHeaderValue Any { get; }
        bool IsWeak { get; }
        string Tag { get; }
        IEntityTagHeaderValue Parse(string input);
        bool TryParse(string input, out IEntityTagHeaderValue parsedValue);
    }

    public interface IHttpContentHeaders : IHttpHeaders
    {
        ICollection<string> Allow { get; }
        ICollection<string> ContentEncoding { get; }
        IContentDispositionHeaderValue ContentDisposition { get; set; }
        ICollection<string> ContentLanguage { get; }
        long? ContentLength { get; set; }
        Uri ContentLocation { get; set; }
        byte[] ContentMD5 { get; set; }
        IContentRangeHeaderValue ContentRange { get; set; }
        IMediaTypeHeaderValue ContentType { get; set; }
        DateTimeOffset? Expires { get; set; }
        DateTimeOffset? LastModified { get; set; }
    }

    public interface IHttpHeaders : IEnumerable<KeyValuePair<string, IEnumerable<string>>>, IEnumerable
    {
        void Add(string name, string value);
        void Add(string name, IEnumerable<string> values);
        bool TryAddWithoutValidation(string name, string value);
        bool TryAddWithoutValidation(string name, IEnumerable<string> values);
        void Clear();
        bool Contains(string name);
        IEnumerable<string> GetValues(string name);
        bool Remove(string name);
        bool TryGetValues(string name, out IEnumerable<string> values);
    }

    public interface IContentDispositionHeaderValue
    {
        DateTimeOffset? CreationDate { get; set; }
        string DispositionType { get; set; }
        string FileName { get; set; }
        string FileNameStar { get; set; }
        DateTimeOffset? ModificationDate { get; set; }
        string Name { get; set; }
        ICollection<INameValueHeaderValue> Parameters { get; set; }
        DateTimeOffset? ReadDate { get; set; }
        long? Size { get; set; }
        IContentDispositionHeaderValue Parse(string input);
        bool TryParse(string input, out IContentDispositionHeaderValue parsedValue);
    }

    public interface INameValueHeaderValue
    {
        string Name { get; }
        string Value { get; set; }
        INameValueHeaderValue Parse(string input);
        bool TryParse(string input, out INameValueHeaderValue parsedValue);
    }

    public interface ICacheControlHeaderValue
    {
        ICollection<INameValueHeaderValue> Extensions { get; }
        TimeSpan? MaxAge { get; set; }
        bool MaxStale { get; set; }
        TimeSpan? MaxStaleLimit { get; set; }
        TimeSpan? MinFresh { get; set; }
        bool MustRevalidate { get; set; }
        bool NoCache { get; set; }
        ICollection<string> NoCacheHeaders { get; }
        bool NoStore { get; set; }
        bool NoTransform { get; set; }
        bool OnlyIfCached { get; set; }
        bool Private { get; set; }
        ICollection<string> PrivateHeaders { get; }
        bool ProxyRevalidate { get; set; }
        bool Public { get; set; }
        TimeSpan? SharedMaxAge { get; set; }
        ICacheControlHeaderValue Parse(string input);
        bool TryParse(string input, out ICacheControlHeaderValue parsedValue);
    }

    public interface IHttpHeaderValueCollection<T> : ICollection<T>, IEnumerable<T>, IEnumerable where T : class
    {
        void ParseAdd(string input);
        bool TryParseAdd(string input);
    }

    public interface IContentRangeHeaderValue
    {
        long? From { get; }
        bool HasLength { get; }
        bool HasRange { get; }
        long? Length { get; }
        long? To { get; }
        string Unit { get; set; }
        IContentRangeHeaderValue Parse(string input);
        bool TryParse(string input, out IContentRangeHeaderValue parsedValue);
    }

    public interface IMediaTypeHeaderValue
    {
        string CharSet { get; set; }
        string MediaType { get; set; }
        ICollection<INameValueHeaderValue> Parameters { get; }
        IMediaTypeHeaderValue Parse(string input);
        bool TryParse(string input, out IMediaTypeHeaderValue parsedValue);
    }

    public interface ITransportContext
    {
        IChannelBinding GetChannelBinding(ChannelBindingKind kind);
    }

    public interface IChannelBinding
    {
        int Size { get; }
    }
}

namespace System.Security.Authentication.ExtendedProtection
{
    public enum ChannelBindingKind
    {
        Unknown,
        Unique,
        Endpoint,
    }
}

namespace System.Net.Http
{
    public enum HttpCompletionOption
    {
        ResponseContentRead,
        ResponseHeadersRead,
    }
}

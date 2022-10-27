using Microsoft.Net.Http.Headers;

namespace PocMarten.Api.Aggregates.Helpdesk.Core.Http
{
    public static class ETagExtensions
    {
        public static int ToExpectedVersion(string? eTag)
        {
            if (eTag == null)
                throw new ArgumentNullException(nameof(eTag));

            var value = EntityTagHeaderValue.Parse(eTag).Tag.Value;

            return int.Parse(value.Substring(1, value.Length - 2));
        }
    }
}

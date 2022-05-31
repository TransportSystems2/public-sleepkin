using System;
using System.IO;
using Ardalis.GuardClauses;
using Pillow.ApplicationCore.Interfaces;

namespace Pillow.ApplicationCore.Services
{
    public sealed class UriComposer : IUriComposer
    {
        private readonly ApiSettings _apiSettings;

        public UriComposer(ApiSettings apiSettings) => _apiSettings = apiSettings;

        readonly static string homePath = (Environment.OSVersion.Platform == PlatformID.Unix ||
                   Environment.OSVersion.Platform == PlatformID.MacOSX)
            ? Environment.GetEnvironmentVariable("HOME")
            : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");

        public string ComposePicUri(string uriTemplate)
        {
            return uriTemplate.Replace("http://catalogbaseurltobereplaced", _apiSettings.CatalogBaseUrl);
        }

        public string ComposeTrackUri(string bookCode, string trackCode, string trackFormat)
        {
            return string.Format(_apiSettings.TrackUrlTemplate,
                bookCode,
                trackCode,
                trackFormat);
        }

        public string ComposeTrackFilePath(string trackCode, string format)
        {
            return Path.Combine(
                _apiSettings.TracksFolder.Replace("~", homePath),
                GetTrackPath(trackCode, format));
        }

        public string ComposeTempTrackFilePath(string trackCode, string format)
        {
            return Path.Combine(
                _apiSettings.TempTracksFolder.Replace("~", homePath),
                GetTrackPath(trackCode, format));
        }

        public string GetTrackPath(string trackCode, string format)
        {
            Guard.Against.NullOrEmpty(trackCode, nameof(trackCode));

            return $"{trackCode.Substring(0,2)}/{trackCode}.{format}";
        }
    }
}
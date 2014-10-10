using System;
using System.Collections;
using System.Collections.Generic;
using Xamarin;

namespace Cheesebaron.MvxPlugins.XamarinInsights
{
    public interface IAppInsights
    {
        void Identify(string uid, string key, string value);
        void Identify(string uid, IDictionary<string, string> table);
        void Report(Exception exception, string key, string value, ReportSeverity warningLevel = ReportSeverity.Warning);
        void Report(Exception exception, IDictionary extraData, ReportSeverity warningLevel = ReportSeverity.Warning);
        void Report(Exception exception = null, ReportSeverity warningLevel = ReportSeverity.Warning);
        void Track(string trackIdentifier, IDictionary<string, string> table = null);
        ITrackHandle TrackTime(string identifier, IDictionary<string, string> table = null);

        bool DisableCollection { get; set; }
        Insights.CollectionTypes DisableCollectionTypes { get; set; }
        bool DisableDataTransmission { get; set; }
        bool DisableExceptionCatching { get; set; }
        string ApiKey { get; set; }
    }
}
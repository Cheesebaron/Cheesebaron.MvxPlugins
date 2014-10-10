// // AppInsights.cs
// // Created 20141008T15:42
// // Author Tomasz Cielecki

using System;
using System.Collections;
using System.Collections.Generic;
using Xamarin;

namespace Cheesebaron.MvxPlugins.XamarinInsights
{
    public class AppInsights
        : IAppInsights
    {
        public void Identify(string uid, string key, string value)
        {
            Insights.Identify(uid, key, value);
        }

        public void Identify(string uid, IDictionary<string, string> table)
        {
            Insights.Identify(uid, table);
        }

        public void Report(Exception exception, string key, string value,
            ReportSeverity warningLevel = ReportSeverity.Warning)
        {
            Insights.Report(exception, key, value, warningLevel);
        }

        public void Report(Exception exception, IDictionary extraData,
            ReportSeverity warningLevel = ReportSeverity.Warning)
        {
            Insights.Report(exception, extraData, warningLevel);
        }

        public void Report(Exception exception = null, ReportSeverity warningLevel = ReportSeverity.Warning)
        {
            Insights.Report(exception, warningLevel);
        }

        public void Track(string trackIdentifier, IDictionary<string, string> table = null)
        {
            Insights.Track(trackIdentifier, table);
        }

        public ITrackHandle TrackTime(string identifier, IDictionary<string, string> table = null)
        {
            return Insights.TrackTime(identifier, table);
        }

        public bool DisableCollection
        {
            get { return Insights.DisableCollection; }
            set { Insights.DisableCollection = value; }
        }

        public Insights.CollectionTypes DisableCollectionTypes
        {
            get { return Insights.DisableCollectionTypes; }
            set { Insights.DisableCollectionTypes = value; }
        }

        public bool DisableDataTransmission
        {
            get { return Insights.DisableDataTransmission; }
            set { Insights.DisableDataTransmission = value; }
        }

        public bool DisableExceptionCatching
        {
            get { return Insights.DisableExceptionCatching; }
            set { Insights.DisableExceptionCatching = value; }
        }

        public string ApiKey { get; set; }
    }
}
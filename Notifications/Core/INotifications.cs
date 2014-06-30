/*
 * Copyright 2014 Tomasz Cielcki (@Cheesebaron)
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Threading.Tasks;

namespace Cheesebaron.MvxPlugins.Notifications
{
    public interface INotifications
    {
        string RegistrationId { get; }
        bool IsRegistered { get; }

        /// <summary>
        /// Register for notifications. (Will run Sync on iOS!)
        /// </summary>
        Task RegisterAsync();

        /// <summary>
        /// Unregister for notifications. (Will run Sync on iOS!)
        /// </summary>
        Task UnregisterAsync();

        /// <summary>
        /// Registered event, fires when a registration went well and on WP it also fires when Channel URI was updated. 
        /// Use this to notify web service or similar about the registration.
        /// </summary>
        event DidRegisterForNotificationsEventHandler Registered;
        event NotificationErrorEventHandler Error;
        event EventHandler Unregistered;

        //TODO add methods for scheduling local notifications
    }
}

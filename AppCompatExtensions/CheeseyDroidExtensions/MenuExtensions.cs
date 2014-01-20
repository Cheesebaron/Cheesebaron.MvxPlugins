//---------------------------------------------------------------------------------
// Copyright 2014 Tomasz Cielecki (tomasz@ostebaronen.dk)
// Licensed under the Apache License, Version 2.0 (the "License"); 
// You may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0 

// THIS CODE IS PROVIDED *AS IS* BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED, 
// INCLUDING WITHOUT LIMITATION ANY IMPLIED WARRANTIES OR 
// CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE, 
// MERCHANTABLITY OR NON-INFRINGEMENT. 

// See the Apache 2 License for the specific language governing 
// permissions and limitations under the License.
//---------------------------------------------------------------------------------

using Android.Content;
using Android.Graphics;
using Android.Views;

namespace CheeseyDroidExtensions
{
    public static class MenuExtensions
    {
        public static IMenuItem SetEnabled(this IMenuItem item, bool enabled, Context context, int iconId)
        {
            return item.SetEnabled(enabled, context, iconId, Color.Gray);
        }

        public static IMenuItem SetEnabled(this IMenuItem item, bool enabled, Context context, int iconId, Color disabledColor)
        {
            var resIcon = context.Resources.GetDrawable(iconId);

            if (!enabled)
                resIcon.Mutate().SetColorFilter(disabledColor, PorterDuff.Mode.SrcIn);

            item.SetEnabled(enabled);
            item.SetIcon(resIcon);

            return item;
        }
    }
}

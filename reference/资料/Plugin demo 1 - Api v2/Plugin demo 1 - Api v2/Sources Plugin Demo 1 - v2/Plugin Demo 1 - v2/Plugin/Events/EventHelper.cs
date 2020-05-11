using System;

namespace Plugin.Events
{
    public static class EventHelper
    {
        public static void Raise(this EventHandler eventHandler, object sender, EventArgs args)
        {
            if (eventHandler == null)
                return;
            eventHandler(sender, args);
        }
    }
}

#if NET35

#pragma warning disable CA1003 // Replace EventHandlerEx with generic EventHandler.

namespace System
{
    public delegate void EventHandler<in TEventArgs>(object sender, TEventArgs e);
}

#endif
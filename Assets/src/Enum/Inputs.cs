using System;

namespace Enums
{
    // Enum for detecting player's input device
    public enum eInputDevice
    {
        KeyboardMouse,
        Controller,
        Invalid
    };

    // Enum for detecting key event (Pressed, Released, or NULL)
    public enum eKeyEvent
    {
        Press,
        Release,
        Invalid
    }
}

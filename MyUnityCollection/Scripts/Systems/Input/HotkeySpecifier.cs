


namespace Muc.Input {

  using System;

  [System.Flags]
  public enum HotkeySpecifier : uint {

    None = 0,

    /// <summary> Require the secondary (right) mouse button to be pressed instead of the primary (left)  </summary>
    Secondary = 1 << 0,


    /// <summary>
    /// Static InputTypes will not be removed automatically.
    /// Static InputTypes will be ignored before any non-static InputTypes are settled.
    /// </summary>
    Persistent = 1 << 1,

    /// <summary> Priority InputTypes will be settled before any non-priority InputTypes </summary>
    Priority = 1 << 2,

    /// <summary> Require all of modifier 1,2,3 keys </summary>
    ControlAltShift = Control | Alt | Shift,
    /// <summary> Require all of modifier 1,2 keys </summary>
    ControlAlt = Control | Alt,
    /// <summary> Require all of modifier 1,3 keys </summary>
    ControlShift = Control | Shift,
    /// <summary> Require all of modifier 2,3 keys </summary>
    AltShift = Alt | Shift,

    /// <summary> Require modifier1 key </summary>
    Control = 1 << 3,
    /// <summary> Require modifier2 key </summary>
    Alt = 1 << 4,
    /// <summary> Require modifier3 key </summary>
    Shift = 1 << 5,


    /// <summary> Allow any of modifier 1,2,3 keys </summary>
    AllowControlAltShift = AllowControl | AllowAlt | AllowShift,
    /// <summary> Allow any of modifier 1,2 keys </summary>
    AllowControlAlt = AllowControl | AllowAlt,
    /// <summary> Allow any of modifier 1,3 keys </summary>
    AllowControlShift = AllowControl | AllowShift,
    /// <summary> Allow any of modifier 2,3 keys </summary>
    AllowAltShift = AllowAlt | AllowShift,

    /// <summary> Allow modifier1 key </summary>
    AllowControl = 1 << 6,
    /// <summary> Allow modifier2 key </summary>
    AllowAlt = 1 << 7,
    /// <summary> Allow modifier3 key </summary>
    AllowShift = 1 << 8,


  }
}
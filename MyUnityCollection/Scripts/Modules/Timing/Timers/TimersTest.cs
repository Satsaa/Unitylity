

namespace Muc.Timing {

  using System;
  using System.Collections.Generic;
  using Muc.Components.Values;
  using UnityEngine;
  using static Timers;

  public class TimersTest : MonoBehaviour {

    public int integer1;
    public int integer2;
    public int integer3;

    [Serializable]
    public struct Structy {
      public int integer;
      public Interval[] intervalArray;
    }

    [Serializable]
    public struct Classy {
      public int integer;
      public Structy nestedStructy;
      public Interval[] intervalArray;
    }

    [Serializable]
    public struct ClassyClassy {
      public int integer;
      public Classy nestedClassy;
    }

    public List<Timeout> timeoutList = new List<Timeout> { new Timeout(1), new Timeout(3) };

    public List<Structy> structies;
    public List<Classy> classies;
    public List<ClassyClassy> classiestClasses;

  }

}
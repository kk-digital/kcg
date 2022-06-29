using UnityEngine;

public class Utils
{

    public static void Assert(bool condition)
    {
        UnityEngine.Assertions.Assert.IsTrue(condition);

        if (!condition)
        {
             Application.Quit();
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class HighLighter : MonoBehaviour //монобех не нужен
{
    public void TurnLight(ILightable lightable, bool state)
    {
        lightable?.SwitchLight(state);
    }

    public void TurnAllLights(List<ILightable> lightables, bool state)
    {
        if (lightables == null) return;

        foreach (var lightable in lightables)
        {
            lightable.SwitchLight(state);

            if (lightable is Point point && point.Chip != null)
            {
                point.Chip.SwitchLight(state);
            }
        }
    }
}
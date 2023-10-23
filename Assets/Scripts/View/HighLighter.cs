using System.Collections.Generic;

public class HighLighter
{
    public void SwitchLights(List<ILightable> lightables, bool state)
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
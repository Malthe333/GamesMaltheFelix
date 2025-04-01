using UnityEngine;
using System.Collections.Generic;

public class GhostReplayer : MonoBehaviour
{
    public List<PlayerFrameData> playbackData;
    private int currentIndex = 0;
    // Der er en fejl med at den despawner efter noget tid lidt før eller efter. hm
    void Update()
    {
        if (currentIndex >= playbackData.Count)
        {
            Destroy(gameObject);
            return;
        }

        var data = playbackData[currentIndex];
        transform.position = data.position;
        transform.rotation = data.rotation;

        if (data.didAttack)
        {
            Debug.Log("Ghost Attack!");
            // Play ghost attack animation or VFX
        }

        currentIndex++;
    }
}

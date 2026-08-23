using UnityEngine;

public class EndTileController : MonoBehaviour
{
    /// This script is attached to each real endtile. It needs:
    /// 1. Animator - for showing hint after X time
    /// 2. A sprite to show after it was revealed (for example a hole in the correct color
    /// 3. A target that is required to be placed on top of this tile (Collider - isTrigger) - only the target will trigger the effect
    /// 
    /// When the condition will be met it will "remove" the target and reveal the real tile. If after X time the player will not bring the target to the tile it will play the animation (every X time)
    /// Will notify the GameManager that the condition was met, when all conditions are met, the GameManager will declare "Level Complete!"



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

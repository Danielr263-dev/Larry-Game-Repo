using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    void Start()
    {
        // Only set position if coming back from a battle
        if (PlayerPrefs.HasKey("RespawnX") && PlayerPrefs.HasKey("RespawnY"))
        {
            float respawnX = PlayerPrefs.GetFloat("RespawnX");
            float respawnY = PlayerPrefs.GetFloat("RespawnY");

            transform.position = new Vector2(respawnX, respawnY);

            // Clear saved position after respawning so it doesn't affect future scene loads
            PlayerPrefs.DeleteKey("RespawnX");
            PlayerPrefs.DeleteKey("RespawnY");
        }
    }
}

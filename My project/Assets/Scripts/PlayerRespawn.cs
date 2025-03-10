using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.HasKey("RespawnX") && PlayerPrefs.HasKey("RespawnY"))
        {
            float respawnX = PlayerPrefs.GetFloat("RespawnX");
            float respawnY = PlayerPrefs.GetFloat("RespawnY");

            transform.position = new Vector2(respawnX, respawnY);

            // Clear saved position after respawning to prevent issues
            PlayerPrefs.DeleteKey("RespawnX");
            PlayerPrefs.DeleteKey("RespawnY");
        }
    }
}

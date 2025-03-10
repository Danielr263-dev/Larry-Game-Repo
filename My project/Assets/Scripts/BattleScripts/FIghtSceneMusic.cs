using UnityEngine;

public class FightSceneManager : MonoBehaviour
{
    public AudioClip fightMusic; // Assign FightScene music in the Inspector

    void Start()
    {
        // Play FightScene music (MusicManager is auto-destroyed)
        if (fightMusic != null)
        {
            AudioSource.PlayClipAtPoint(fightMusic, Camera.main.transform.position, 0.5f);
        }
    }
}

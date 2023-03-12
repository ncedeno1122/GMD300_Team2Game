using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelEndTrigger : MonoBehaviour
{
    public UnityEvent m_LevelEndEvent;

    // + + + + | Collision Handling | + + + +

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        m_LevelEndEvent.Invoke();
        MMDebug.DebugLogTime($"{this.GetType().Name} - Invoked LevelEndEvent!");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TeleportToPosition : MonoBehaviour
{
    [SerializeField] WorldMoving world;

    [SerializeField] Transform target;

    [SerializeField] PlayerController playerController;

    [SerializeField] Image blackScreen;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            world.AllowedToMove = false; 
            playerController.ChangeAnimationTo(PlayerState.Idle);
            StartCoroutine(Teleport());
        }
    }

    public IEnumerator Teleport()
    {

        
        

        Vector2 worldTargetPosition = world.transform.position + (transform.position - target.position );

        blackScreen.DOFade(1f, 0.5f).OnComplete(() =>
        {
            world.transform.position = new Vector3(worldTargetPosition.x, world.transform.position.y, world.transform.position.z);
            playerController.ResetPosition();

            blackScreen.DOFade(0f, 0.5f).OnComplete(() =>
            {
                
                
                world.AllowedToMove = true;
                playerController.ChangeAnimationTo(PlayerState.Run);
            });
        });

        yield return null;
    }
}

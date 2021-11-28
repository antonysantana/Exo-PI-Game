using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastEnemy : MonoBehaviour
{
    Ray ray;
    RaycastHit hitInfo;

    public Transform raycastOrigin;
    public Transform raycastDestination;
    public ParticleSystem MuzzleFlash;
    public int amountDamage;

    public ParticleSystem hitFX;
    public Animator animShot;

    public void StartFiring()
    {
        ray.origin = raycastOrigin.position;
        ray.direction = raycastDestination.position - raycastOrigin.position;

        MuzzleFlash.Emit(1);
        //animShot.SetTrigger("Shot");

        if(Physics.Raycast(ray, out hitInfo))
        {
            hitFX.transform.position = hitInfo.point;
            hitFX.transform.forward = hitInfo.normal;
            hitFX.Emit(1);

            hitInfo.collider.gameObject.SendMessage("GetDamage", amountDamage, SendMessageOptions.DontRequireReceiver);

        }
    }
}

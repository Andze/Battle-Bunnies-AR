using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyBehaviour : MonoBehaviour
{
    public Texture2D eggTexture;

    private GameObject preSpawn;
    private GameObject postSpawn;
    private Animator bunnyAnimator;

    private void Awake()
    {
        preSpawn = transform.GetChild(0).gameObject;
        postSpawn = transform.GetChild(1).gameObject;
        bunnyAnimator = postSpawn.transform.GetComponentInChildren<Animator>();

        if (eggTexture != null) {
            for (int i = 0; i < preSpawn.transform.childCount; i++) {
                if (preSpawn.transform.GetChild(i).name.Contains("egg")) {
                    preSpawn.transform.GetChild(i).GetComponent<Renderer>().material.mainTexture = eggTexture;
                }
            }

            for (int i = 0; i < postSpawn.transform.childCount; i++) {
                if (postSpawn.transform.GetChild(i).name.Contains("egg")) {
                    postSpawn.transform.GetChild(i).GetComponent<Renderer>().material.mainTexture = eggTexture;
                }
            }
        }

        postSpawn.SetActive(false);
    }
    
    public void PlayAnimation(int animationIndex)
    {
        if (bunnyAnimator != null) {
            switch (animationIndex) {
                case 0:
                    preSpawn.SetActive(false);
                    postSpawn.SetActive(true);
                    bunnyAnimator.SetTrigger("bunny_spawn");
                    break;

                case 1:
                    bunnyAnimator.SetTrigger("bunny_attack");
                    break;

                case 2:
                    bunnyAnimator.SetTrigger("bunny_flinch");
                    break;
            }
        }
    }

    public void Die()
    {

    }
}

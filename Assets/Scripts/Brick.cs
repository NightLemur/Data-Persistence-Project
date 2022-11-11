using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Brick : MonoBehaviour
{
    public UnityEvent<int> onDestroyed;

    public int NumberOfHits; // The amount of hits it takes to destroy this brick
    public int PointValue; // The point value, per hit, to destroy this block

    AudioSource audioSource; // The Audio Source on this GameComponent

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Set the Audio Source volume, based on Options Save
        if (OptionsManager.instance.SavedData.SfxValue > 0)
            audioSource.volume = OptionsManager.instance.SavedData.SfxValue / 10f;
        else
            audioSource.volume = 0f;

        var renderer = GetComponentInChildren<Renderer>();

        MaterialPropertyBlock block = new MaterialPropertyBlock();
        switch (PointValue)
        {
            case 1 :
                block.SetColor("_BaseColor", Color.green);
                break;
            case 2:
                block.SetColor("_BaseColor", Color.yellow);
                break;
            case 5:
                block.SetColor("_BaseColor", Color.blue);
                break;
            default:
                block.SetColor("_BaseColor", Color.red);
                break;
        }
        renderer.SetPropertyBlock(block);
    }

    private void OnCollisionEnter(Collision other)
    {
        onDestroyed.Invoke(PointValue);
        NumberOfHits--;


        //slight delay to be sure the ball have time to bounce
        if (NumberOfHits <= 0)
            Destroy(gameObject, 0.2f);
        else
        {
            // Changing the color of the block to indicate hits left to go
            var renderer = GetComponentInChildren<Renderer>();
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            switch (NumberOfHits)
            {
                case 1:
                    block.SetColor("_BaseColor", Color.green);
                    break;
                case 2:
                    block.SetColor("_BaseColor", Color.yellow);
                    break;
                case 3:
                    block.SetColor("_BaseColor", Color.blue);
                    break;
            }
            renderer.SetPropertyBlock(block);
        }

        // Play the "Hit" Sound Effect
        audioSource.Play();
    }
}

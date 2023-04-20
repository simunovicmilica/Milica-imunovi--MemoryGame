using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCard : MonoBehaviour
{
    [SerializeField] private SceneController sceneController;
    [SerializeField] private GameObject cardBack;
    [SerializeField] private AudioClip clickSound;

    private int _id;

    public int Id
    {
        get { return _id; }
    }


    public void OnMouseDown()
    {
        if (cardBack.activeSelf && sceneController.canReveal)
        {
            cardBack.SetActive(false);
            sceneController.RevealCard(this);

            AudioSource.PlayClipAtPoint(clickSound, transform.position);
        }
    }

    public void Unreveal()
    {
        cardBack.SetActive(true);
    }

    public void ChangeSprite(int id, Sprite image)
    {
        _id = id;
        GetComponent<SpriteRenderer>().sprite = image;
    }

}

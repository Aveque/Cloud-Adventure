using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTutorialWithChild : MonoBehaviour
{
    private SpriteRenderer _sprite;
    private SpriteRenderer _childSprite;
    private bool onTutorial;
    private float Speed = 2f;
    private Color color;

    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _childSprite = GameObject.FindGameObjectWithTag("TutorialDot").GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        color = _sprite.color;
        if (onTutorial)
        {
            color.a += Speed * Time.deltaTime;
            color.a = Mathf.Clamp(color.a, 0, 1);
            _sprite.color = color;
            _childSprite.color = color;
        }
        else
        {
            color.a -= Speed * Time.deltaTime;
            color.a = Mathf.Clamp(color.a, 0, 1);
            _sprite.color = color;
            _childSprite.color = color;
        }
    }

    void OnTriggerEnter2D(Collider2D otherl)
    {
        if (otherl.CompareTag("Player"))
        {
            onTutorial = true;   
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            onTutorial = false;
        }
    }
}
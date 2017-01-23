using UnityEngine;
using System.Collections;

public class Line : MonoBehaviour
{
    public float Speed;

    private LineRenderer mesh;

    private float timeLeft = 2f;
    private bool lightsOn;

    private Color originalColor;
    private Color changedColor;

    private bool moving = false;
    private Vector3 moveTarget;
    private Transform animationTransform;

    void Start()
    {
        mesh = this.GetComponent<LineRenderer>();
        originalColor = new Color(22, 33, 51, 128);
        changedColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        lightsOn = true;

        animationTransform = this.GetComponentInChildren<Animator>().transform;

        //mesh.startColor = changedColor;
        //mesh.endColor = changedColor;
    }

    void Update()
    {
        if (moving)
        {
            animationTransform.position = Vector3.MoveTowards(animationTransform.position, moveTarget, Speed * Time.deltaTime);

            if (Vector3.Distance(animationTransform.position, moveTarget) < 0.1f)
            {
                animationTransform.position = this.transform.position;
                this.GetComponentInChildren<SpriteRenderer>().enabled = true;
            }
        }
    }

    public void MoveTowards(Vector3 target)
    {
        float angleBetweenVectors = Vector3.Angle(this.transform.position, target);

        if(animationTransform == null)
            animationTransform = this.GetComponentInChildren<Animator>().transform;

        animationTransform.Rotate(new Vector3(0, 0, 1), -angleBetweenVectors);

        moveTarget = target;
        moving = true;
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class ArcherAttack : MonoBehaviour
{
    public GameObject arrowPrefab;
    private GameObject arrowToLoad;
    public GameObject arrow;
    public Transform loadArrowParent,shootPoint;
    [SerializeField] float arrowSpeed = 30.0f;
    public Transform player;
    [SerializeField]GameObject splineToInst;
    GameObject splineObj;
    AudioSource weaponAs;
    [SerializeField]AudioClip shootSFX;
    SplineContainer SplineContainer;
    Vector3 arrowStartPos;
    private Spline spline;
    SplineAnimate splineAnimate;
    private void Start()
    {
        splineObj = Instantiate(splineToInst,Vector3.zero,Quaternion.identity);
        splineAnimate = arrow.GetComponent<SplineAnimate>();
        SplineContainer = splineObj.GetComponent<SplineContainer>();
        spline = SplineContainer.Spline;
        splineAnimate.Container = SplineContainer;
        splineAnimate.MaxSpeed = arrowSpeed;
        splineAnimate.enabled = false;
        weaponAs = GameObject.Find("Weapons").GetComponent<AudioSource>();
        arrowStartPos = arrow.transform.position;
        spline = SplineContainer.Spline;
        arrow.SetActive(false);
    }
    public void FireArrow()
    {
        // Clear any existing knots to reset the path
        spline.Clear();
        Destroy(arrowToLoad);

        // Get the positions in world space
        Vector3 shootPos = shootPoint.position;
        Vector3 playerPos = new Vector3(player.position.x, player.position.y, player.position.z);

        // Create knots at the enemy and player positions
        BezierKnot startKnot = new BezierKnot(shootPos);
        BezierKnot endKnot = new BezierKnot(playerPos);

        // Add the knots to the spline
        spline.Add(startKnot);
        spline.Add(endKnot);

        weaponAs.PlayOneShot(shootSFX);
        // Reactivate and reset the SplineAnimate component
        if (splineAnimate != null)
        {
            splineAnimate.NormalizedTime = 0;  // Start from the beginning
            splineAnimate.enabled = true;
            splineAnimate.Play();
        }

        // Activate the arrow and start the reset coroutine
        arrow.SetActive(true);
        StartCoroutine(resetArrow());
    }


    public void instantiateArrow()
    {
        // Instantiate arrow for load animation
        Vector3 direction = (player.position - shootPoint.transform.position).normalized;
        arrowToLoad = Instantiate(arrowPrefab, loadArrowParent.transform.position,Quaternion.LookRotation(direction),loadArrowParent);
        arrowToLoad.SetActive(true);
    }
    private IEnumerator resetArrow()
    {
        yield return new WaitForSeconds(3);

        // Get the SplineAnimate component
        if (splineAnimate != null)
        {
            // Stop the animation and reset progress
            splineAnimate.enabled = false;
            splineAnimate.NormalizedTime = 0;  // Reset the time to start from the beginning
        }

        // Reset arrow position to the initial position
        arrow.transform.position = arrowStartPos;

        // Deactivate the arrow
        arrow.SetActive(false);
    }


}

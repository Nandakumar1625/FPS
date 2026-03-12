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
    private Coroutine resetCoroutine;
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
        if (shootPoint == null || player == null)
        {
            Debug.LogError("ArcherAttack: ShootPoint or Player is null!");
            return;
        }

        Vector3 shootPos = shootPoint.position;
        Vector3 playerPos = player.position;

        // Check for NaN positions
        if (float.IsNaN(shootPos.x) || float.IsNaN(shootPos.y) || float.IsNaN(shootPos.z) ||
            float.IsNaN(playerPos.x) || float.IsNaN(playerPos.y) || float.IsNaN(playerPos.z))
        {
            Debug.LogError($"ArcherAttack: NaN detected in positions! ShootPos: {shootPos}, PlayerPos: {playerPos}");
            return;
        }

        // Check distance to avoid zero-length spline which causes NaN in evaluation
        if (Vector3.Distance(shootPos, playerPos) < 0.01f)
        {
            Debug.LogWarning($"ArcherAttack: Shoot position and player position are too close ({Vector3.Distance(shootPos, playerPos)}). Skipping fire to avoid NaN error.");
            return;
        }

        // Stop any currently running reset coroutine to prevent race conditions
        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
            resetCoroutine = null;
        }

        // Disable animation while modification happens
        if (splineAnimate != null)
        {
            splineAnimate.enabled = false;
        }

        // Create knots at the enemy and player positions
        BezierKnot startKnot = new BezierKnot(shootPos);
        BezierKnot endKnot = new BezierKnot(playerPos);

        // Add the knots to the spline
        spline.Add(startKnot);
        spline.Add(endKnot);

        weaponAs.PlayOneShot(shootSFX);
        
        // Activate the arrow and start the reset coroutine
        arrow.SetActive(true);
        resetCoroutine = StartCoroutine(resetArrow());

        // Start the animation with a frame delay to ensure spline is "cooked"
        StartCoroutine(StartSplineAnimation());
    }

    private IEnumerator StartSplineAnimation()
    {
        if (splineAnimate == null || spline == null || spline.Count < 2) yield break;

        // Ensure animation is disabled while we wait
        splineAnimate.enabled = false;

        // Wait for one frame to allow Unity's Spline system to finalize internal data
        yield return null;

        if (splineAnimate == null || spline == null || spline.Count < 2) yield break;

        // Force spline to update its length and data
        // Explicitly catching any internal Spline errors
        try 
        {
            splineAnimate.enabled = true;
            splineAnimate.NormalizedTime = 0;
            splineAnimate.Play();
        } 
        catch (System.Exception e) 
        {
            Debug.LogWarning($"ArcherAttack: Failed to start spline animation after delay: {e.Message}");
        }
    }



    public void instantiateArrow()
    {
        // Null checks for required fields
        if (loadArrowParent == null)
        {
            Debug.LogError("ArcherAttack: loadArrowParent is not assigned in the inspector!");
            return;
        }
        if (shootPoint == null || player == null) return;

        // Instantiate arrow for load animation
        Vector3 direction = (player.position - shootPoint.position).normalized;
        
        // Safety check for LookRotation to avoid 'Look rotation viewing vector is zero'
        Quaternion rotation = Quaternion.identity;
        if (direction.sqrMagnitude > 0.001f)
        {
            rotation = Quaternion.LookRotation(direction);
        }

        arrowToLoad = Instantiate(arrowPrefab, loadArrowParent.position, rotation, loadArrowParent);
        arrowToLoad.SetActive(true);
    }
    private IEnumerator resetArrow()
    {
        yield return new WaitForSeconds(3);

        // Get the SplineAnimate component
        if (splineAnimate != null)
        {
            // Stop the animation
            splineAnimate.enabled = false;

            // Only reset time if the spline is still valid (not cleared by another attack)
            if (spline != null && spline.Count >= 2)
            {
                splineAnimate.NormalizedTime = 0;
            }
        }

        // Reset arrow position to the initial position
        arrow.transform.position = arrowStartPos;

        // Deactivate the arrow
        arrow.SetActive(false);
    }


}

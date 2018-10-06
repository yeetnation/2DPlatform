using UnityEngine;
using System.Collections;
using Pathfinding;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyAI : MonoBehaviour
{

    // What to chase
    public Transform target;

    // How many times a each second we will update our path
    public float updateRate = 2f;

    // Caching
    private Seeker seeker;
    private Rigidbody2D rb;

    // The calculated path
    public Path path;

    // The AI's speed per second
    public float speed = 300f;
    public ForceMode2D fMode;

    [HideInInspector]
    public bool pathIsEnded = false;

    // The max distance from the AI to a waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 3;

    // The waypoint we are currently towards
    private int currentWaypoint = 0;

    private bool searchingForPlayer = false;

    private Transform enemySprite;
    private bool spriteRotationLeft = true;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        enemySprite = GetComponentInChildren<Transform>();
        if (target == null)
        {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }
        // Start a new path to the target position, return the result to the OnPathComplete method
        seeker.StartPath(transform.position, target.position, OnPathComplete);

        StartCoroutine(UpdatePath());
    }

    public IEnumerator SearchForPlayer()
    {
        GameObject sResult = GameObject.FindGameObjectWithTag("Player");
        if (sResult == null)
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(SearchForPlayer());
        }
        else
        {
            searchingForPlayer = false;
            target = sResult.transform;
            StartCoroutine(UpdatePath());
            yield return false;
        }
    }
    IEnumerator UpdatePath()
    {
        if (target == null)
        {
            //Debug.LogError("No Player found!");
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            yield return false;
        }
        else
        {
            seeker.StartPath(transform.position, target.position, OnPathComplete);
            yield return new WaitForSeconds(1 / updateRate);
            StartCoroutine(UpdatePath());
        }
    }
    private void Update()
    {
        if (target != null)
        {
            enemySprite = transform.GetChild(1);
            Vector3 diff = enemySprite.position - target.position;
            diff.Normalize();         // normalizing the vector. Meaning that the sum of the vector will be equal to 1
            float rotZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            if ((rotZ > -90 && rotZ < 90) && spriteRotationLeft == false)
            {
                enemySprite.rotation = Quaternion.Euler(0f, 0f, rotZ);
                enemySprite.localScale = new Vector3(enemySprite.localScale.x, enemySprite.localScale.y * -1, enemySprite.localScale.z);
                spriteRotationLeft = true;
            }
            else if ((rotZ > 90 || rotZ < -90) && spriteRotationLeft == true)
            {
                enemySprite.rotation = Quaternion.Euler(0f, 0f, rotZ);
                enemySprite.localScale = new Vector3(enemySprite.localScale.x, enemySprite.localScale.y * -1, enemySprite.localScale.z);
                spriteRotationLeft = false;
            }
            else
            {
                enemySprite.rotation = Quaternion.Euler(0f, 0f, rotZ);
            }
        }

    }
    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    void FixedUpdate()
    {
        if (target == null)
        {
            //Debug.LogError("No Player found!");
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }

        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            if (pathIsEnded)
                return;

            pathIsEnded = true;
            return;
        }
        pathIsEnded = false;

        // Direction to the next waypoint
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime;

        rb.AddForce(dir, fMode);

        if (Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]) <= nextWaypointDistance)
        {
            currentWaypoint++;
            return;
        }
    }


}

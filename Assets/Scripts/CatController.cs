using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CatController : MonoBehaviour
{
    enum PlayerState
    {
        Left,
        Center,
        Right
    }

    [SerializeField] private PlayerState playerState;
    // [SerializeField] private float threshold;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveDistance;
    [SerializeField] private List<Vector3> positions = new List<Vector3>();
    [SerializeField] private Animator anim;
    [SerializeField] private Transform collider;
    [Tooltip("How high should the collider get when jumping?")]
    [SerializeField] private float jumpHeight;
    [Tooltip("How much should the collider lower when sliding?")]
    [SerializeField] private float slideCollFinalPos;
    [SerializeField] private float jumpInterval;
    [SerializeField] private Ease jumpUpEase;
    [SerializeField] private Ease jumpDownEase;

    [SerializeField] private float slideHeight;
    [SerializeField] private float slideInterval;
    [SerializeField] private Ease slideDownEase;
    [SerializeField] private Ease slideUpEase;

    [Tooltip("How far should the finger go until the gesture is considered a swipe. Expressed as percentage of screen width")]
    [Range(0, 100)]
    [SerializeField] private float swipeThreshold;

    private bool isMoving;
    private Vector3 originalTouchPos;

    // Start is called before the first frame update
    void Start()
    {
        playerState = PlayerState.Center;

        // initialize the possible positions
        positions.Add(transform.position - new Vector3(moveDistance, 0, 0));
        positions.Add(transform.position);
        positions.Add(transform.position + new Vector3(moveDistance, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    bool GetLeftMovement()
    {

#if UNITY_EDITOR
        return Input.GetKeyDown(KeyCode.LeftArrow);
#elif UNITY_ANDROID
        // check number of fingers touching
        if (Input.touchCount > 0)
        {
            Touch firstTouch = Input.GetTouch(0);

            if (firstTouch.phase == TouchPhase.Began)
            {
                originalTouchPos = firstTouch.position;
            }
            else if (firstTouch.phase == TouchPhase.Moved)
            {

                Vector3 currentTouchPosition = firstTouch.position;

                // 
                float delta = currentTouchPosition.x - originalTouchPos.x;
                if (delta < 0)
                {
                    // finger is moving left
                    if (Mathf.Abs(delta) / Screen.width > swipeThreshold / 100f)
                    {
                        // this is indeed a swipe
                        return true;
                    }
                }

            }

        }
        return false;
#endif

    }

    bool GetRightMovement()
    {
#if UNITY_EDITOR
        return Input.GetKeyDown(KeyCode.RightArrow);
#elif UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch firstTouch = Input.GetTouch(0);

            if (firstTouch.phase == TouchPhase.Began)
            {
                originalTouchPos = firstTouch.position;
            }
            else if (firstTouch.phase == TouchPhase.Moved)
            {

                Vector3 currentTouchPosition = firstTouch.position;

                // 
                float delta = currentTouchPosition.x - originalTouchPos.x;
                if (delta > 0)
                {
                    // finger is moving right
                    if (Mathf.Abs(delta) / Screen.width > swipeThreshold / 100f)
                    {
                        // this is indeed a swipe
                        return true;
                    }
                }

            }

        }
        return false;
#endif
    }

    void Movement()
    {
        if (!isMoving)
        {

            // moving left
            if (SimpleInput.GetAxis("HorizontalMove") < 0 &&
                Mathf.Abs(SimpleInput.GetAxis("HorizontalMove")) > Mathf.Abs(SimpleInput.GetAxis("JumpAndSlide")))
            {
                if (!isMoving)
                {
                    if (playerState != PlayerState.Left)
                    {
                        StartCoroutine(MoveHorizontally(true));
                    }
                }
            }

            // moving right
            if (SimpleInput.GetAxis("HorizontalMove") > 0 &&
            Mathf.Abs(SimpleInput.GetAxis("HorizontalMove")) > Mathf.Abs(SimpleInput.GetAxis("JumpAndSlide")))
            {
                if (!isMoving)
                {
                    if (playerState != PlayerState.Right)
                    {
                        StartCoroutine(MoveHorizontally(false));
                    }
                }
            }

            // jumping
            if (SimpleInput.GetAxis("JumpAndSlide") > 0 &&
             Mathf.Abs(SimpleInput.GetAxis("JumpAndSlide")) > Mathf.Abs(SimpleInput.GetAxis("HorizontalMove")))
            {
                if (!isMoving)
                {
                    isMoving = true;
                    anim.ResetTrigger("Jump");

                    Vector3 origPos = collider.localPosition;
                    Vector3 targetPos = collider.localPosition + new Vector3(0, jumpHeight, 0);
                    collider.DOLocalMove(targetPos, jumpInterval).SetEase(jumpUpEase).OnComplete(
                        () =>
                        {
                            collider.DOLocalMove(origPos, jumpInterval).SetEase(jumpDownEase).OnComplete(
                                () =>
                                {
                                    isMoving = false;
                                }
                            );
                        }
                    );
                    anim.SetTrigger("Jump");
                }
            }

            // sliding
            if (SimpleInput.GetAxis("JumpAndSlide") < 0 &&
                Mathf.Abs(SimpleInput.GetAxis("JumpAndSlide")) > Mathf.Abs(SimpleInput.GetAxis("HorizontalMove")))
            {
                if (!isMoving)
                {
                    isMoving = true;
                    anim.ResetTrigger("Slide");

                    Vector3 origPos = collider.localPosition;
                    Vector3 targetPos = new Vector3(collider.localPosition.x, slideHeight, collider.localPosition.z);
                    collider.DOLocalMove(targetPos, slideInterval).SetEase(slideDownEase).OnComplete(
                        () =>
                        {
                            collider.DOLocalMove(origPos, slideInterval).SetEase(slideUpEase).OnComplete(
                                () =>
                                {
                                    isMoving = false;
                                }
                            );
                        }
                    );
                    anim.SetTrigger("Slide");
                }
            }

            //anim.SetTrigger("Slide");



        }
    }

    IEnumerator MoveHorizontally(bool direction)
    {
        isMoving = true;

        Vector3 targetPosition = Vector3.zero;

        // if player is moving towards the left
        if (direction == true)
        {
            if (playerState == PlayerState.Center)
            {
                targetPosition = positions[0];
                playerState = PlayerState.Left;
            }
            else if (playerState == PlayerState.Right)
            {
                targetPosition = positions[1];
                playerState = PlayerState.Center;
            }

        }
        // if player is moving towards the right
        else
        {
            if (playerState == PlayerState.Center)
            {
                targetPosition = positions[2];
                playerState = PlayerState.Right;
            }
            else if (playerState == PlayerState.Left)
            {
                targetPosition = positions[1];
                playerState = PlayerState.Center;
            }
        }

        float timer = 0;
        float distance = Vector3.Distance(transform.position, targetPosition);
        float moveInterval = distance / moveSpeed;

        // rotate player to face the direction they are moving. just for visual feedback.
        transform.DOLocalRotate(new Vector3(0, direction ? -90 : 90, 0), moveInterval / 2f).OnComplete(
            () =>
            {
                transform.DOLocalRotate(new Vector3(0, 0, 0), moveInterval / 2);
            }
        );

        // while (Vector3.Distance(transform.position, targetPosition) > threshold)
        while (timer <= moveInterval)
        {
            timer += Time.deltaTime;

            if (direction)
                transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0, 0);
            else
                transform.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);

            yield return null;
        }

        // clip the position to the final position
        transform.position = targetPosition;

        isMoving = false;
    }
}

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopManager : Singleton<ShopManager>, IPointerClickHandler
{
    public FrogSO[] frogPool = new FrogSO[8];

    public Action<bool> OnSetShopOnOff;
    public event Action OnShopOnOff;

    private FrogSO frogSO;
    private GameObject selectedFrogPrefab;
    private Vector3 prefabPos;

    private Camera cam;
    private Vector2 touchPos;

    private Animator animator;

    private Coroutine placeFrogCoroutine;
    private int number;

    public GameObject CancelPlaceingButton;

    private void Start()
    {
        //base.Awake();
        animator = GetComponentInParent<Animator>();

        cam = Camera.main;

        WheelManager.Instance.OnPlaceFrog += ShopManager_OnPlaceFrog;

        InputManager.Instance.OnTouchInput += pos =>
        {
            if (frogSO != null)
            {
                touchPos = pos;
            }
        };
        InputManager.Instance.OnTouchTap += (obj) => { OnSetShopOnOff(true); };
        InputManager.Instance.OnTouchPressStarted += Instance_OnTouchPressStarted;
        InputManager.Instance.OnTouchPressCanceled += Instance_OnTouchPressCanceled;

        OnSetShopOnOff = state => { animator.SetBool("OnStoreClick", state); };

        CancelPlaceingButton.GetComponent<Button>().onClick.AddListener(() => {Destroy(selectedFrogPrefab);
            selectedFrogPrefab = null;
            CancelPlaceingButton.SetActive(false);
        });
    }
    

    private void ShopManager_OnPlaceFrog(FrogSO obj, int num)
    {
        number = num;
        OnSetShopOnOff(true);

        frogSO = obj;
        selectedFrogPrefab = Instantiate(obj.prefab, new Vector3(6.2f, 0, -2.5f), Quaternion.identity);

        CancelPlaceingButton.SetActive(true);
        StopCoroutineTarget(placeFrogCoroutine);
        placeFrogCoroutine = StartCoroutine(PlaceFrog());
    }

    private IEnumerator PlaceFrog()
    {
        while (selectedFrogPrefab != null)
        {
            Vector3 targetWorldPosition = cam.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, 10f));
            selectedFrogPrefab.transform.position = targetWorldPosition + prefabPos;

            Color col = IsPlacable(selectedFrogPrefab.transform.position) ? Color.green : Color.red;
            selectedFrogPrefab.GetComponent<FrogBrain>().ChangeColor(col);
            yield return null;
        }
    }

    private void Instance_OnTouchPressStarted(Vector2 obj)
    {
        if (selectedFrogPrefab != null)
        {
            Vector3 targetWorldPosition = cam.ScreenToWorldPoint(new Vector3(obj.x, obj.y, 10f));
            prefabPos -= targetWorldPosition;
            StopCoroutineTarget(placeFrogCoroutine);
            placeFrogCoroutine = StartCoroutine(PlaceFrog());
        }
    }
    private void Instance_OnTouchPressCanceled(Vector2 obj)
    {
        if(selectedFrogPrefab != null)
        {
            prefabPos = selectedFrogPrefab.transform.position;
            StopCoroutineTarget(placeFrogCoroutine);
            if (IsPlacable(selectedFrogPrefab.transform.position))
            {
                GameManager.Instance.BugBitsChange(-frogSO.logicSO.cost * number);
                selectedFrogPrefab.GetComponent<FrogBrain>().SpawnFrog();
                frogSO = null;
                selectedFrogPrefab = null;
                prefabPos = Vector3.zero;
                WheelManager.Instance.SetPrice?.Invoke();
                CancelPlaceingButton.SetActive(false);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnSetShopOnOff(false);

        OnShopOnOff?.Invoke();
        if (selectedFrogPrefab != null)
        {
            StopCoroutineTarget(placeFrogCoroutine);
            Destroy(selectedFrogPrefab);
            selectedFrogPrefab = null;
            frogSO = null;
            CancelPlaceingButton.SetActive(false);
            prefabPos = Vector3.zero;
        }
    }

    private bool IsPlacable(Vector3 pos)
    {
        NavMeshHit navHit;

        //check if it is in a navmesh area
        if (NavMesh.SamplePosition(pos, out navHit, 0.1f, NavMesh.AllAreas))
        {
            //check if there are other game objects that would collide withion the same area
            Collider[] colliders = Physics.OverlapSphere(navHit.position, 0.2f);
            bool res;

            switch (navHit.mask)
            {
                case 1:
                    //ground frogs
                    res = !frogSO.logicSO.isWaterFrog && colliders.Length == 0;
                    return res;
                case 8:
                    //path for bugs
                    return false;
                case 16:
                    //water frogs
                    res = frogSO.logicSO.isWaterFrog && colliders.Length == 0;
                    return res;
                default:
                    return false;
            }
        }
        return false;
    }

    private void StopCoroutineTarget(Coroutine target)
    {
        if (target != null)
        {
            StopCoroutine(target);
        }
    }
} 

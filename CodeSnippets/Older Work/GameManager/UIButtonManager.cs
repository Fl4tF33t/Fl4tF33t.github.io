using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonManager : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    //instead of game object, this will be the scriptable object taht shows all the info of that frog
    [SerializeField] 
    protected FrogSO frogSO;

    //variables used to show info when holding down on a button fro a specified time
    private bool isPointerDown;
    private Coroutine holdDownCoroutine;
    [SerializeField]
    private float holdTimeDelay;

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        //This method is used to select a UI element
        //This method is where you implement the functionalionality of the button
        Debug.Log("Click");
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        //This method is used when the user presses down on the UI element
        //You can use a form of TimeScale to determine what is shown after the user presses down
        Debug.Log("Down");
        //Clears out any pre-existing coroutine
        if(holdDownCoroutine != null)
        {
            StopCoroutine(holdDownCoroutine);
        }

        //Sets the hold down is true, and starts a new coroutine
        isPointerDown = true;
        holdDownCoroutine = StartCoroutine(HoldDownDuration());
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        //This method is used when the user releases the UI element
        //You can use this method to reset the Ui element to its original state
        Debug.Log("Up");
        isPointerDown = false;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        //This method is used when the user exits the UI element
        //You can use this method to reset the Ui element to its original state
        Debug.Log("Exit");

        isPointerDown = false;
    }

    private IEnumerator HoldDownDuration()
    {
        yield return new WaitForSeconds(holdTimeDelay);
        while(isPointerDown)
        {
            ActivatedHoldDown();
            yield return null;
        }
        DeactivatedHoldDown();
    }

    protected virtual void ActivatedHoldDown() { }
    protected virtual void DeactivatedHoldDown() { }

    public virtual void OnBeginDrag(PointerEventData eventData) { }

    public virtual void OnDrag(PointerEventData eventData) { }

    public virtual void OnEndDrag(PointerEventData eventData) { }

}

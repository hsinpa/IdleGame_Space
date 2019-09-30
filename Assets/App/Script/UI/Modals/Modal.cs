using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using Utility;
/// <summary>
/// Class that implements the functionalities of modal windows.
/// </summary>
public class Modal : MonoBehaviour
{
    /// <summary>
    /// Reference to the canvas group.
    /// </summary>
    public CanvasGroup group { get { return GetComponent<CanvasGroup>(); } }

    /// <summary>
    /// Method called when the modal is requested to open.
    /// </summary>
    public virtual void Open() 
    {   
        float d = OnModalOpen();
        //Notify(d, N.Modal.Open);
        group.interactable = true;
        group.blocksRaycasts = true;
        Cull(false);
        transform.SetAsLastSibling();
		//transform.parent.Find("background").GetComponent<Image>().enabled = useBackground;
    }

    /// <summary>
    /// Method called when the modal is requested to close.
    /// </summary>
    public virtual void Close() 
    {
        OnModalClose();
		Cull(true);
        // Notify(d, N.Modal.Close);
        group.interactable = false;
        group.blocksRaycasts = false;
    }

    /// <summary>
    /// Method to be extended and implement the open behaviour, returning the duration of the transition.
    /// </summary>
    /// <returns></returns>
    virtual protected float OnModalOpen() {
		group.DOKill();
	    group.DOFade(1, 0.4f);

        return 0.3f;
    }

    /// <summary>
    /// Method to be extended and implement the close behaviour, returning the duration of the transition.
    /// </summary>
    /// <returns></returns>
    virtual protected float OnModalClose() {
		group.DOFade(0, 0.2f);
		return 0.2f;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="p_button"></param>
    virtual public void OnButtonClick(Button p_button)
    {

    }

    /// <summary>
    /// Move the ui away.
    /// </summary>
    /// <param name="p_flag"></param>
    public void Cull(bool p_flag)
    {
		//group.alpha = p_flag ? 0 : 1;
		RectTransform rec = GetComponent<RectTransform>();        
		rec.anchoredPosition = p_flag ? new Vector2(10000f, 0f) : Vector2.zero;
		
    }
}

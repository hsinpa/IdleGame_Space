using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/// <summary>
/// Class that implements the functionalities of modal windows.
/// </summary>
public class ModalView : MonoBehaviour
{

    /// <summary>
    /// List of modals.
    /// </summary>
    public List<Modal> modals;

    /// <summary>
    /// Currently open modal.
    /// </summary>
    public Modal current { get { return opened.Count <= 0 ? null : opened[opened.Count - 1]; } }

    /// <summary>
    /// List of opened modals.
    /// </summary>
    public List<Modal> opened;

    public float time;
    /// <summary>
    /// CTOR.
    /// </summary>
    void Awake()
    {

        Modal[] ml = GetComponentsInChildren<Modal>();
        modals = new List<Modal>(ml);
        //Activity.RunOnce(delegate () { foreach (Modal it in modals) it.Cull(true); }, 3f / 60f);
    }

    /// <summary>
    /// Searches and returns a modal by type.
    /// </summary>
    /// <param name="p_type"></param>
    /// <returns></returns>
    public Modal GetModal(ModalType p_type) { return modals.Find(delegate (Modal it) { return it.type == p_type; }); }

    /// <summary>
    /// Searches and returns a modal by type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="p_type"></param>
    /// <returns></returns>
    public T GetModal<T>() where T : Modal
    {
        return (T)modals.Find(delegate (Modal it) { return (it is T); });
    }

    /// <summary>
    /// Opens a given modal by type.
    /// </summary>
    /// <param name="p_type"></param>
    public void Open(ModalType p_type)
    {
        Modal m = GetModal(p_type);
        Open(m);
    }

    /// <summary>
    /// Opens a given modal.
    /// </summary>
    /// <param name="p_modal"></param>
    public void Open(Modal p_modal)
    {
        time = Time.time;


        if (opened.IndexOf(p_modal) < 0)
        {
            opened.Add(p_modal);
        }
        else
        {
            opened.Remove(p_modal);
            opened.Add(p_modal);
        }

        p_modal.Open();
    }

    /// <summary>
    /// Calls a dialog modal and expects the user input.
    /// </summary>
    /// <param name="p_title"></param>
    /// <param name="p_options"></param>
    //public void Dialog(string p_title, string p_message, System.Action<string> p_callback, params string[] p_options)
    //{
    //    DialogModal md = GetModal<DialogModal>();
    //    md.Set(p_title, p_message, p_callback, p_options);
    //    Open(md);
    //}


    /// <summary>
    /// Calls a dialog after a delay.
    /// </summary>
    /// <param name="p_delay"></param>
    /// <param name="p_title"></param>
    /// <param name="p_message"></param>
    /// <param name="p_callback"></param>
    /// <param name="p_options"></param>
    public void Dialog(float p_delay, string p_title, string p_message, System.Action<string> p_callback, params string[] p_options)
    {
		//GameSingletonView.SimpleCoroutine(delegate () { Dialog(p_title, p_message, p_callback, p_options); }, p_delay);
    }

    private int calculateTotalOpenTime(float pastTime) {
        return (int)(Time.time - pastTime);
    }

	public void CloseAll()  {
        
		foreach (Modal modal in opened) {
			modal.Close();
		}
		opened.Clear();
	}

    /// <summary>
    /// Closes the current modal.
    /// </summary>
    public void Close()
    {
        if(current)
        {
            Modal currentModal = current;
            opened.RemoveAt(opened.Count -1 );
            currentModal.Close();
        }
    }

    public bool Visible {
        set {
            if (current)
			{
				current.group.alpha = value ? 1 : 0;
				current.group.interactable = value ? true : false;
				current.group.blocksRaycasts = value ? true : false;
			}
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.NativePlugins;

public class Sharing : MonoBehaviour
{
    public string m_shareMessage;
    public string m_shareURL;
    public eShareOptions[] m_exludedOPtions;
    public GameObject sharePanel;

    void FinishedSharing(eShareResult result)
    {
        sharePanel.SetActive(true);
        Debug.Log("Finished sharing: " + result);
    }
    public void ShareURLUsingShareSheet()
    {
        ShareSheet _shareSheet = new ShareSheet();
        _shareSheet.Text = m_shareMessage;
        _shareSheet.URL = m_shareURL;

        _shareSheet.ExcludedShareOptions = m_exludedOPtions;

        NPBinding.UI.SetPopoverPointAtLastTouchPosition();
        NPBinding.Sharing.ShowView(_shareSheet, FinishedSharing);

    }

    public void ShareScreenShotUsingShareSheet()
    {
        sharePanel.SetActive(false);
        ShareSheet _shareSheet = new ShareSheet();
        _shareSheet.Text = m_shareMessage;

        _shareSheet.ExcludedShareOptions = m_exludedOPtions;

        _shareSheet.AttachScreenShot();

        NPBinding.UI.SetPopoverPointAtLastTouchPosition();
        NPBinding.Sharing.ShowView(_shareSheet, FinishedSharing);

    }


}

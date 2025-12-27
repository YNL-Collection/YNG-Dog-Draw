using UnityEngine;

public class PopupWin : PopupBase
{
    protected override void OnShow()
    {
        
    }

    public override void DestroyPopup()
    {
        Destroy(gameObject);
    }



    public void NextLevel()
    {
        
            LevelManager.Instance.LoadNextLevelByPopup();

            DestroyPopup();
        

    }
}

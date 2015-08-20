using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropMe : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
	public Image containerImage;
	public Image receivingImage;
	private Color normalColor;
	public Color highlightColor = Color.yellow;
    private string DropSpriteName;
    public bool IsWaitListVersion;

	public void OnEnable ()
	{
		if (containerImage != null)
			normalColor = containerImage.color;
	}
	
	public void OnDrop(PointerEventData data)
	{
		containerImage.color = normalColor;
		
		if (receivingImage == null){
			Debug.Log("Image dropped is null");
			return;
		}
		Debug.Log("Dropping onto waitlist/rental");
		Sprite dropSprite = GetDropSprite (data);
        Player tempPlayer = GameObject.Find("Player").GetComponent<Player>();
        
        if(IsWaitListVersion == false){
			bool ShipAlreadyRented = tempPlayer.GetRentedStatus();
			if (dropSprite != null && ShipAlreadyRented == false) { 
				receivingImage.overrideSprite = dropSprite;
				DropSpriteName = "Ship " + DropSpriteName.Substring(11);
				//tell the player which ship was rented
				//tempPlayer.SetShipRented(DropSpriteName);
				
				Debug.Log("Rented ship is: " + DropSpriteName);
				//tell the docking tracker which ship was rented
				GameObject.Find("DockingFunctions").GetComponent<ShipTracking>().ShipRented(DropSpriteName, false);
			}
        }else{
			bool ShipAlreadyWaitListed = tempPlayer.GetWaitListStatus();
			Debug.Log("WaitListing ship? " + ShipAlreadyWaitListed);
			if (dropSprite != null && ShipAlreadyWaitListed == false) { 
				receivingImage.overrideSprite = dropSprite;
				DropSpriteName = "Ship " + DropSpriteName.Substring(11);
				//tell the player which ship was rented
				//tempPlayer.SetShipRented(DropSpriteName);
				
				Debug.Log("WaitListed ship is: " + DropSpriteName);
				//tell the docking tracker which ship was rented
				GameObject.Find("DockingFunctions").GetComponent<ShipTracking>().ShipWaitListed(DropSpriteName);
			}
        }
    }

    public void SetSpriteDrop(Sprite ShipSprite)
    {
        containerImage.color = normalColor;
        Player tempPlayer = GameObject.Find("Player").GetComponent<Player>();
        bool ShipAlreadyRented = tempPlayer.GetRentedStatus();
        if (ShipSprite != null && ShipAlreadyRented == false)
        {
            receivingImage.overrideSprite = ShipSprite;
            DropSpriteName = "Ship " + DropSpriteName.Substring(11);

            Debug.Log("Rented ship is: " + DropSpriteName);
            //tell the docking tracker which ship was rented
            GameObject.Find("DockingFunctions").GetComponent<ShipTracking>().ShipRented(DropSpriteName, false);
        }
    }

    public void ResetImage()
    {
        receivingImage.overrideSprite = containerImage.sprite;
    }

	public void OnPointerEnter(PointerEventData data)
	{
		if (containerImage == null)
			return;
		
		Sprite dropSprite = GetDropSprite (data);
		if (dropSprite != null)
			containerImage.color = highlightColor;
	}

	public void OnPointerExit(PointerEventData data)
	{
		if (containerImage == null)
			return;
		
		containerImage.color = normalColor;
	}
	
	private Sprite GetDropSprite(PointerEventData data)
	{
		var originalObj = data.pointerDrag;
		if (originalObj == null)
			return null;

		var srcImage = originalObj.GetComponent<Image>();
		if (srcImage == null)
			return null;
        Debug.Log("Sprite grabbed is: " + srcImage.name.Substring(11));
        DropSpriteName = srcImage.name;
		return srcImage.sprite;
	}
}

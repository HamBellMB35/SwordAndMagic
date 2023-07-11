using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    
    [SerializeField] public Image imageBar;
    // Health Rombus Image here 
    [SerializeField] public float decreaseAmount = 0.1f;
    [SerializeField] public float refillAmount = 0.1f;
    [SerializeField] public float refillInterval = 2f;
    private float currentFillAmount;
    private bool isRefilling;

    private void Start()
    {
        SetFillAmount();
        StartCoroutine("RefillImage");
    }

    // Update is called once per frame
    void Update()
    {
      
       // StartRefill();
    }

     public void DecreaseHorizontalFill(Image image)
    {
        image.fillAmount -= decreaseAmount;

        image.fillAmount = Mathf.Clamp01(image.fillAmount);
    }

    private void SetFillAmount()
    {
        currentFillAmount = imageBar.fillAmount;
    }

    private IEnumerator RefillImage()
    {
        while (currentFillAmount < 1f)
        {
            SetFillAmount();
            currentFillAmount += refillAmount;

            currentFillAmount = Mathf.Clamp01(currentFillAmount);

            imageBar.fillAmount = currentFillAmount;

            yield return new WaitForSeconds(refillInterval);
            
        }

        isRefilling = false;
    }

    public void StartRefill()
    {
        if (!isRefilling)
        {
            isRefilling = true;
            StartCoroutine(RefillImage());
        }
    }
}

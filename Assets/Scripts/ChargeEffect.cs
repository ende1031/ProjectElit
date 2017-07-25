using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ChargeEffect : MonoBehaviour {

    public static ChargeEffect instance = null;

    public GameObject first_Canvas;
    public GameObject second_Canvas;
    public GameObject third_Canvas;
    public GameObject ssecond_Canvas;

    public Sprite fire_charge_effect;
    public Sprite water_charge_effect;
    public Sprite wind_charge_effect;
    public Sprite sand_charge_effect;
    public Sprite null_charge_effect;

    public Image first_charge_image;
    public Image second_charge_image;
    public Image third_charge_image;
    public Image ssecond_charge_image;

    public int charge_stat;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start ()
    {
        first_Canvas.SetActive(false);
        second_Canvas.SetActive(false);
        third_Canvas.SetActive(false);
        ssecond_Canvas.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, 0, 90) * Time.deltaTime);

    }

    public void Chargefirst()
    {
        first_Canvas.SetActive(true);

        charge_stat = 1;
    }

    public void Chargethird()
    {
        ssecond_Canvas.SetActive(false);

        first_Canvas.SetActive(true);
        second_Canvas.SetActive(true);
        third_Canvas.SetActive(true);

        charge_stat = 3;
    }
    
    public void ChargeSSecond()
    {
        ssecond_Canvas.SetActive(true);

        charge_stat = 2;
    }

    public void ChargeReset()
    {
        first_Canvas.SetActive(false);
        second_Canvas.SetActive(false);
        third_Canvas.SetActive(false);
        ssecond_Canvas.SetActive(false);

        charge_stat = 0;
    }

    public void ChangeE(Image E_image, int E_num)
    {
        if (E_num == 0)
        {
            E_image.sprite = fire_charge_effect;
        }

        else if (E_num == 1)
        {
            E_image.sprite = water_charge_effect;
        }

        else if (E_num == 2)
        {
            E_image.sprite = wind_charge_effect;
        }

        else if (E_num == 3)
        {
            E_image.sprite = sand_charge_effect;
        }

        else
        {
            E_image.sprite = null_charge_effect;
        }
    }
}
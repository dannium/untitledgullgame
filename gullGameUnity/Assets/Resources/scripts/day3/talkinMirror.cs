using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Cinemachine;
using System.Diagnostics.Contracts;
using UnityEngine.SceneManagement;

public class talkinMirror : MonoBehaviour
{
    public TextMeshProUGUI speechText;
    public TextMeshProUGUI charText;
    bool skippin = false;
    public Image charImg;
    bool typing;
    float charCd = 0;
    float switchCd = 0;
    AudioSource As;
    CinemachineCamera cam;
    Transform plr;
    float pitch;
    //menu stuff
    public Image fadinImg;
    private string lastChar;

    bool toShop = false;
    int index = 0;
    float typeSpeed = 0.5f;
    string text;
    int animFrame;
    Animator g;
    AudioClip hs;
    string[] otherNames = new string[] {"bob", "Mergie", "roland", "Panzer", "Philip", "Cross", 
        "big terry", "big terry (but louder)", "Big terry (but even louder)", "big Terry (but very very louder)",
        "Jon the 3", "bird", "jimster"}; //other name suggestions so technically all suggestions are in ts


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        plr = GameObject.FindGameObjectWithTag("plr").GetComponent<Transform>();
        g = plr.GetComponent<Animator>();
        cam = GameObject.Find("CinemachineCamera").GetComponent<CinemachineCamera>();

        typing = true;
        As = GameObject.FindWithTag("MainCamera").GetComponent<AudioSource>();
        string name = "George Washington";
        if(Random.Range(0, 5) == 2)
        {
            name = otherNames[Random.Range(0, otherNames.Length-1)];
        }
        text = /*Ok so I'm supposed to base this monolouge around Wurthering Heights but I've never read it so I kinda just had AI write it sorry...*/ "(raises the sack)\r\nYou know why I can't stop? Because he couldn't stop. Heathcliff waited years — built himself into something monstrous just to come back and find her already dead. All that becoming, for nothing. All that hunger, and the table was bare.\r\n(strike)\r\nShe said she was him. His soul, his self. And then she let him be destroyed anyway. You can survive someone leaving you. You cannot survive being someone's ghost while you're still breathing.\r\n(strike, quieter)\r\nThat's what you are. That's what I made you for. So I could finally hit the thing that's been living inside my chest since the moment I understood that love, in this house, in this family — was always just a pretty name for ruin.\r\n(looks at the clone, breathing hard)\r\nYou heal exactly like I do, don't you.\r\n(drops the sack)\r\nOf course you do.\r\nWe never learn.";
        UpdateText(name, 0.02f);
        hs = Resources.Load<AudioClip>("sfx/heavySymbol");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !skippin)
        {
            index = text.Length - 1;
            skippin = true;
        }
        switchCd -= Time.deltaTime;
        charCd -= Time.deltaTime;
        if (typing && charCd <= 0)
        {
            charCd = 0;
            NewChar();
        }
       

        if (toShop)
        {
            fadinImg.color = new Color(0.06f, 0.06f, 0.12f, Mathf.Lerp(fadinImg.color.a, 1, 3 * Time.deltaTime));
            if (fadinImg.color.a > 0.999f)
            {
                SceneManager.LoadScene("shop");
            }
        }

        animFrame = Mathf.RoundToInt(g.GetCurrentAnimatorStateInfo(0).normalizedTime * 40) % 40;
        if(animFrame == 27)
        {
            As.pitch = 1;
            As.PlayOneShot(hs);
            As.pitch = 0;
        }

    }

    void UpdateText(string newChar, float speed)
    {
        pitch = -0.25f;
        typing = true;
        lastChar = newChar;
        speechText.text = "";
        index = 0;
        switchCd = 999;
        As.clip = Resources.Load<AudioClip>("sfx/gullCaw");

        charText.text = newChar;

        charImg.sprite = Resources.Load<Sprite>("materials/day3/gwPixel");
        typeSpeed = speed;
    }

    void NewChar()
    {

        if (index < text.Length)
        {
            if (text[index].ToString() == " ")
            {
                charCd = 0;
            }
            else
            {
                As.pitch = Random.Range(0.6f + pitch, 0.8f + pitch);
                As.PlayOneShot(As.clip);
            }
            speechText.text += text[index];
            index++;
            charCd = typeSpeed;
            if (text[index - 1].ToString() == "." || text[index - 1].ToString() == "!" || text[index - 1].ToString() == "?" || text[index - 1].ToString() == "-")
            {
                charCd *= 4;
            }
        }
        else
        {
            toShop = true;
            typing = false;
            index = 0;
            switchCd = 0;
            g.speed = 0.1f;
        }
    }



}

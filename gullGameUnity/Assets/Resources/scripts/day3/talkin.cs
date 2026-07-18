//good luck reading my spaghetti code for this one lmao
//(shop script)

using System.Collections;
using System.Diagnostics.Contracts;
using TMPro;
using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class talkin : MonoBehaviour
{
    public TextMeshProUGUI speechText;
    public TextMeshProUGUI charText;
    public Image charImg;
    string text = "Died again, huh?";
    int index = 0;
    float typeSpeed;
    bool typing;
    float charCd = 0;
    float switchCd = 0;
    AudioSource As;
    public Animator theAnimator;
    public Animator thatAnimator;
    public Animator bananimator;
    bool tokensGiven = false;
    bool questTokensGiven = false;
    bool tokensGivenTheMan = false;
    bool onTheMan = true;
    bool inItemShop = true;
    bool banana;
    private GameObject crows;
    private GameObject pg;
    CinemachineCamera cam;
    TextMeshProUGUI uhave;
    Transform plr;
    float pitch;
    //menu stuff
    public Transform Items;
    public Transform Quests;
    public Transform Upgrades;
    Transform ctb;
    bool nextRound;
    public Image fadinImg;
    private string lastChar;
    bool canSwitch;

    private int[] buttonQuests = new int[3];
    string[] questNames = new string[] { "Beat 10 William Howard Tafts", "Last 10 waves", "Last 50 waves", "Beat 5 bosses", "Kill 20 animals"};
    int questAmount;
    static int[] questAmounts = new int[] { 50000, 10000, 30000, 5000, 7500 };

    private static int[][] upgradePrices = new int[][] {
    new int[] {50,100,200,500,1000,2000,4000,7500,10000,15000}, // reload time
    new int[] {20,40,80,160,320,640,1280,2560,5120,10240}, // rot speed
    new int[] {75,250,500,1000,2000,4000,6500,9000,12500,17500}, // damage
    new int[] {50,150,250,400,750,1000,2000,4000,8000,16000}  // cheese tokens
        };

    private static float[][] upgradeAmounts = new float[][] {
    new float[] {1,0.8f,0.6f,0.5f,0.4f,0.3f,0.2f,0.15f,0.1f,0.05f,0.01f}, // reload time
    new float[] {1,1.2f,1.4f,1.6f,1.8f,2f,2.1f,2.2f,2.3f,2.4f,2.5f}, // rot speed
    new float[] {1,1.2f,1.4f,1.6f,1.8f,2f,2.25f,2.5f,2.75f,3f,4f}, // damage
    new float[] {1,1.1f,1.2f,1.4f,1.6f,1.8f,2f,2.25f,2.5f,2.75f,3}  // cheese tokens
        };

    poof poof;

    bool startGuns = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        ctb = GameObject.Find("cheeseTokensBubble").transform;

        //PlayerPrefs.SetInt("cheeseTokens", 38425228);

        questAmount = PlayerPrefs.GetInt("questAmount");

        for(int i = 0; i < 3; i++)
        {
            buttonQuests[i] = Random.Range(0, 5);
            GameObject.Find("Q" + (i+1)).GetComponent<TextMeshProUGUI>().text = questNames[buttonQuests[i]];
            GameObject.Find("Qcost" + (i + 1)).GetComponent<TextMeshProUGUI>().text = "" + Mathf.RoundToInt(questAmounts[buttonQuests[i]] * upgradeAmounts[3][PlayerPrefs.GetInt("cheeseVar", 0)]);
        }


        banana = Random.Range(0, 2) == 1;
        if(banana)
        {
            GameObject.Find("theMan").SetActive(false);
        } else
        {
            GameObject.Find("banana").SetActive(false);
        }

        PlayerPrefs.SetInt("g0Owned", 1);
        uhave = GameObject.Find("uhave").gameObject.GetComponent<TextMeshProUGUI>();

        poof = GameObject.Find("scriptRunner").GetComponent<poof>();
        plr = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        cam  = GameObject.Find("CinemachineCamera").GetComponent<CinemachineCamera>();
        if (PlayerPrefs.GetInt("pgTokens") == 0) {
            GameObject.Find("purpleGuyShopPrefab").SetActive(false);
        }
        typeSpeed = 0.5f;
        typing = false;
        As = GameObject.FindWithTag("MainCamera").GetComponent<AudioSource>();

        string nukedText = "";
        if (PlayerPrefs.GetInt("nuked") == 1)
        {
            nukedText = " (maybe don't kill the rat that turns bullets into nukes)";
        }

        if(banana)
        {
            UpdateText("Good heavens, you appear to have met your demise so soon... One can only marvel at your staggering ineptitude" + nukedText, null, "TheMan", 0.075f, false);
        } else
        {
            UpdateText("Died already... Wow, you must suck!" + nukedText, null, "TheMan", 0.1f, false);
        }
        GameObject.Find("ctxt").GetComponent<TextMeshProUGUI>().text = "Cheese Tokens: " + PlayerPrefs.GetInt("cheeseTokens");
        updateButton(1, "reload", "Reload Time", "s");
        updateButton(2, "rotation", "Rotation Speed", "x");
        updateButton(3, "damage", "Damage", "x");
        updateButton(4, "cheese", "Cheese Tokens", "x");
        Quests.gameObject.SetActive(false);
        Upgrades.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(startGuns)
        {
            startGuns = false;
            reloadGuns();
            reloadGunEnabled();
            uhave.text = "(You have: " + PlayerPrefs.GetInt("steak") + ")";
        }

        switchCd -= Time.deltaTime;
        charCd -= Time.deltaTime;
        if (!typing && lastChar == "Crows")
        {
            //end crows
            tokensGiven = true;
            poof.Poofs(new Vector3(0, 1.05f, 2.74f), 0.5f);
            Destroy(crows);
            Resources.Load<Material>("materials/day3/theManEyes").SetTexture(Shader.PropertyToID("_texture"), Resources.Load<Sprite>("materials/day3/theManEye").texture);
            lastChar = "";
        }
        if (typing && charCd <= 0)
        {
            charCd = 0;
            NewChar();
        }
        if (!typing && !tokensGivenTheMan && canSwitch)
        {
            canSwitch = false;
            if (!questTokensGiven)
            {
                //quyest tokens:
                if (questAmount == 0)
                {
                    if (banana)
                    {
                        UpdateText("What did I eludicate regarding an excess of self-assurance, my dear boy? I rather suspected you'd find yourself quite unable to vanquish that particular quest.", null, "TheMan", 0.075f, false);
                    }
                    else
                    {
                        UpdateText("What'd I tell ya about being too cocky kid, I knew you wouldn't beat that quest.", null, "TheMan", 0.1f, false);
                    }
                }
                else
                {
                    if (banana)
                    {
                        UpdateText("For vanquishing the alotted quest, you have procured ", " cheese tokens.", "TheMan", 0.075f, false);
                    }
                    else
                    {
                        UpdateText("Yea yea, whatever, you beat the quest, want a medal? Beginners luck. Here's ", " cheese tokens", "TheMan", 0.1f, false);
                    }
                }

                questTokensGiven = true;

            }
            else
            {
                //main round tokens:
                tokensGivenTheMan = true;
                if (PlayerPrefs.GetInt("cheeseTokensNew") == 0)
                {
                    if (banana)
                    {
                        UpdateText("Good gracious, you haven't managed to procure even a sole cheese token? I dare say, one hopes fortune smiles upon you with considerably more generosity in your subsequent endeavours, young fowl.", null, "TheMan", 0.075f, false);
                    }
                    else
                    {
                        UpdateText("You didn't even earn a single cheese token? Better luck next time, kid.", null, "TheMan", 0.1f, false);
                    }
                }
                else
                {
                    if (banana)
                    {
                        UpdateText("Returning to the matter at hand, you have procured ", " cheese tokens.", "TheMan", 0.075f, false);
                    }
                    else
                    {
                        UpdateText("Anyways, here's ", " cheese tokens for beating enemies", "TheMan", 0.1f, false);
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            canSwitch = true;
            if(!typing && tokensGivenTheMan && !tokensGiven)
            {
                //ts crows
                if(crows == null) {
                    crows = Instantiate(Resources.Load<GameObject>("prefabs/day3/crows"));
                }

                poof.Poofs(new Vector3(0, 1.05f, 2.74f), 0.5f);
                Resources.Load<Material>("materials/day3/theManEyes").SetTexture(Shader.PropertyToID("_texture"), Resources.Load<Sprite>("materials/day3/theManDownEye").texture);

                if (PlayerPrefs.GetInt("bossesBeaten") == 0)
                {
                    UpdateText("Hey you didn't beat any bosses so no bonuses for you.", null, "Crows", 0.1f, true);
                }
                else if(PlayerPrefs.GetInt("bossesBeaten") == 1)
                {
                    UpdateText("Hey you beat ", " boss so here's a bonus.", "Crows", 0.1f, true);
                }
                else
                {
                    UpdateText("Hey you beat ", " bosses so here's a bonus.", "Crows", 0.1f, true);
                }
                PlayerPrefs.SetInt("waves", 0);
                switchCd = 0;
            }
            if(switchCd > 0)
            {
                Skip();
            } else if (!tokensGiven)
            {
                switchCd = 0.5f;
            }
            else if (switchCd <= 0 && tokensGiven) { 
                onTheMan = !onTheMan;
                if (onTheMan)
                {
                    if (banana)
                    {
                        cam.Follow = GameObject.Find("bananaCam").transform;
                    }
                    else
                    {
                        cam.Follow = GameObject.Find("theMan").transform;
                    }
                    if(banana)
                    {
                        UpdateText("Returning in such a miniscule period of time? Would you be so frightfully kind as to expedite the matter with the utmost haste?", null, "TheMan", 0.1f, false);
                    }
                    else
                    {
                        UpdateText("Back so soon? Could ya make it quick?", null, "TheMan", 0.1f, false);
                    }
                    Upgrades.gameObject.SetActive(false);
                    Items.gameObject.SetActive(inItemShop);
                    Quests.gameObject.SetActive(!inItemShop);
                }
                else
                {
                    cam.Follow = GameObject.Find("THATMAN").transform;
                    UpdateText("Hey! Welcome to my shop!", null, "THATMAN", 0.05f, false);
                    Upgrades.gameObject.SetActive(true);
                    Items.gameObject.SetActive(false);
                    Quests.gameObject.SetActive(false);
                }
            }
        }

        if(nextRound)
        {
            print(fadinImg.color.a);
            fadinImg.color = new Color(0, 0, 0, Mathf.Lerp(fadinImg.color.a, 2, 3 * Time.deltaTime));
            print(fadinImg.color.a);
            if (fadinImg.color.a > 0.9999f)
            {
                SceneManager.LoadScene("mainDay2");
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && onTheMan && tokensGiven)
        {
            inItemShop = !inItemShop;
            Items.gameObject.SetActive(inItemShop);
            Quests.gameObject.SetActive(!inItemShop);
            if (!inItemShop)
            {
                uhave.text = "(You have: " + PlayerPrefs.GetInt("steak") + ")";
            }
        }
        if (onTheMan)
        {
            plr.transform.position = Vector3.Lerp(plr.transform.position, new Vector3(-0.7f, 1.1f, 1.52f), 0.02f);
        }
        else
        {
            plr.transform.position = Vector3.Lerp(plr.transform.position, new Vector3(-18f, 1.1f, 1.52f), 0.02f);
        }
    }

    void UpdateText(string newSpeech, string char2, string newChar, float speed, bool useWaves)
    {
        typing = true;
        lastChar = newChar;
        speechText.text = "";
        text = "";
        index = 0;
        switchCd = 999;
        As.clip = Resources.Load<AudioClip>("sfx/ip");

        charText.text = newChar;

        switch (newChar)
        {
            case "TheMan":
                if (banana)
                {
                    charImg.sprite = Resources.Load<Sprite>("materials/day3/bananaPixel");
                    As.clip = Resources.Load<AudioClip>("sfx/oi");
                    bananimator.SetBool("talking", true);
                    pitch = 0.5f;
                    charText.text = "Fancy Banana";
                }
                else
                {
                    charImg.sprite = Resources.Load<Sprite>("materials/day3/TheManPixel");
                    theAnimator.speed = 40;
                    pitch = 0;
                }
            break;
            case "THATMAN":
                charImg.sprite = Resources.Load<Sprite>("materials/day3/THATMANPixel");
                thatAnimator.speed = 80;
                pitch = 0.5f;
            break;
            case "George Washington":
                charImg.sprite = Resources.Load<Sprite>("materials/day3/gwPixel");
            break;
            case "Crows":
                charImg.sprite = Resources.Load<Sprite>("materials/day3/crowPixelated");
                As.clip = Resources.Load <AudioClip>("sfx/caw");
                pitch = 0.3f;
                break;
        }
        if(char2 == null)
        {
            text = newSpeech;
        } else
        {
            if (useWaves)
            {
                text = newSpeech + PlayerPrefs.GetInt("bossesBeaten") + char2;
                PlayerPrefs.SetInt("cheeseTokens", PlayerPrefs.GetInt("cheeseTokens") + PlayerPrefs.GetInt("bossesBeaten") * 5);
            }
            else
            {
                if(questTokensGiven)
                {
                    text = newSpeech + PlayerPrefs.GetInt("cheeseTokensNew") + char2;
                    PlayerPrefs.SetInt("cheeseTokens", PlayerPrefs.GetInt("cheeseTokens") + PlayerPrefs.GetInt("cheeseTokensNew"));
                    PlayerPrefs.SetInt("cheeseTokensNew", 0);
                }
                else
                {
                    text = newSpeech + questAmount + char2;
                    PlayerPrefs.SetInt("cheeseTokens", PlayerPrefs.GetInt("cheeseTokens") + questAmount);
                }
            }

            StartCoroutine(ChangeCTText());
        }
        typeSpeed = speed;
    }

    void NewChar()
    {

        if(index < text.Length)
        {
            if(text[index].ToString() == " ")
            {
                charCd = 0;
            } else
            {
                As.pitch = Random.Range(0.6f+pitch, 0.8f+pitch);
                As.PlayOneShot(As.clip);
            }
            speechText.text += text[index];
            index++;
            charCd = typeSpeed;
            if(text[index-1].ToString() == "." || text[index - 1].ToString() == "!" || text[index - 1].ToString() == "?" || text[index - 1].ToString() == "-")
            {
                charCd *= 4;
            }
        } else
        {
            typing = false;
            index = 0;
            theAnimator.speed = 1;
            bananimator.SetBool("talking", false);
            //bananimator.SetFloat("AnimTime", 0f);
            thatAnimator.speed = 2;
            switchCd = 0;
        }
    }

    void Skip()
    {
        speechText.text = text;
        typing = false;
        index = 0;
        theAnimator.speed = 1;
        bananimator.SetBool("talking", false);
        thatAnimator.speed = 2;
        switchCd = 0;
        canSwitch = false;
    }
    
    public void quest(int buttonNum)
    {
        int questNum = buttonQuests[buttonNum-1];

        PlayerPrefs.SetInt("quest", questNum);
        //0 = beat 10 whts, 1 = last to wave 10, 2 = last to wave 50, 3 = beat 5 bosses, 4 = kill 20 animals

        string[] boughtTexts;
        if (banana) {
            boughtTexts = new string[] { "I do wish you the very best of fortune in that endeavour.", "How terribly amusing; your confidence in your own abilities is quite something to behold...", "It was to be proclaimed that hubris proved the fowl's undoing... or words to that effect" };
        } else
        {
            boughtTexts = new string[] { "Yea, good luck with that...", "It's funny how good you think you are...", "Confidence killed the gull... or something" };
        }
        UpdateText(boughtTexts[Random.Range(0, boughtTexts.Length)], null, "TheMan", 0.1f, false);

        for (int i = 1; i < 4; i++)
        {
            Image buttonImg = GameObject.Find("Qb" + i).GetComponent<Image>();
            buttonImg.color = new Vector4(0, 0, 0, (float)(i == buttonNum ? 255 : 180) / (float)255);
        }
    }

    void successfulUpgrade()
    {
        string[] boughtTexts = new string[] { "MONEY!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!", "THANKS!" };
        UpdateText(boughtTexts[Random.Range(0, boughtTexts.Length)], null, "THATMAN", 0.05f, false);
    }


    private void updateButton(byte num, string varName, string fullName, string varSuffix)
    {
        int upgradeIndex = PlayerPrefs.GetInt(varName + "Var");
        int cost;
        //print(cost + "of upgradePrices[" + upgradeIndex + "]");
        if (upgradeIndex < 10)
        {
            cost = upgradePrices[num - 1][upgradeIndex];
            GameObject.Find("utxt" + num.ToString()).GetComponent<TextMeshProUGUI>().text = fullName + "\n" + upgradeAmounts[num - 1][upgradeIndex] + varSuffix + " >> " + upgradeAmounts[num - 1][upgradeIndex + 1] + varSuffix;
            GameObject.Find("ucostTxt" + num.ToString()).GetComponent<TextMeshProUGUI>().text = "" + cost;
        }
        else
        {
            GameObject.Find("utxt" + num.ToString()).GetComponent<TextMeshProUGUI>().text = fullName + "\n" + upgradeAmounts[num - 1][upgradeIndex] + varSuffix + " (Max)";
            GameObject.Find("ucostTxt" + num.ToString()).GetComponent<TextMeshProUGUI>().text = "";
            GameObject.Find("uctImg" + num.ToString()).SetActive(false);
            //print("yurt, " + GameObject.Find("ctImg" + num.ToString()).GetComponent<Image>().enabled);

        }
        //print("test = " + GameObject.Find("ucostTxt" + num.ToString()).GetComponent<TextMeshProUGUI>().text);
    }

    void buyUpgrade(byte num, string varName, string fullName, string varSuffix)
    {
        int upgradeIndex = PlayerPrefs.GetInt(varName + "Var");
        int cost = upgradeIndex < 10 ? upgradePrices[num - 1][upgradeIndex] : 0;

        if (PlayerPrefs.GetInt("cheeseTokens") >= cost && upgradeIndex < 10)
        {
            PlayerPrefs.SetInt("cheeseTokens", PlayerPrefs.GetInt("cheeseTokens") - cost);
            StartCoroutine(ChangeCTText());

            string[] boughtTexts = new string[] { "CHEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEESE", "THANKS", "HEY MAN GOOD LUCK OUT THERE YOU'RE DOING GOOD FENDING OFF AGAINST THOSE SLIMES THAT WERE FORMED BY FUSING YOUR DEAD RELAT- oh sorry that struck a nerve huh? i was told to add horrific lore that'll change someone perspective of life or something and I'm not really sure- ok I'll just stop talking now" };
            UpdateText(boughtTexts[Random.Range(0, boughtTexts.Length)], null, "THATMAN", 0.05f, false);
            PlayerPrefs.SetInt(varName + "Var", upgradeIndex + 1);
            updateButton(num, varName, fullName, varSuffix);
            print( upgradeAmounts[num-1][PlayerPrefs.GetInt(varName + "Var")] );
        } else if(PlayerPrefs.GetInt("cheeseTokens") < cost)
        {
            string[] cantBuyTexts = new string[] {"HEY MAN YOU DON'T HAVE ENOUGH SORRY", "SORRY NOT ENOUGH BRO", "GET UR MONEY UP"};
            UpdateText(cantBuyTexts[Random.Range(0, cantBuyTexts.Length)], null, "THATMAN", 0.05f, false);
        } else
        {
            UpdateText("YOU'RE ALREADY MAXED OUT DUDE", null, "THATMAN", 0.05f, false);
        }
    }

    public void buyReload()
    {
        buyUpgrade(1, "reload", "Reload Time", "s");
    }

    public void buyRotation()
    {
        buyUpgrade(2, "rotation", "Rotation Speed", "x");
    }

    public void buyDamage()
    {
        buyUpgrade(3, "damage", "Damage", "x");
    }

    public void buyCheese()
    {
        buyUpgrade(4, "cheese", "Cheese Tokens", "x");
    }

    void buyItem()
    {
        StartCoroutine(ChangeCTText());
        string[] boughtTexts;
        if(banana)
        {
            boughtTexts = new string[] { "It has been an absolute and utter delight conducting affairs with you, my dear fellow!", "Now then, I shall kindly ask you to vacate the premises with all due haste." };

        } else
        {
            boughtTexts = new string[] { "Pleasure scam- doin buisness with ya!", "Alright now get out." };
        }
        UpdateText(boughtTexts[Random.Range(0, boughtTexts.Length)], null, "TheMan", banana ? 0.075f : 0.1f, false);
    }

    void cantBuyItem()
    {
        string[] boughtTexts;
        if (banana)
        {
            boughtTexts = new string[] { "It appears as though you have not accured the legal tender to be able to acquire this good.", "I say, how tremendously droll! It would appear, you impoverished wretch, that your coffers are regrettably insufficient for such an undertaking!" };
        } else
        {
            boughtTexts = new string[] { "You tryna scam me? You don't have enough...", "Too poor bud", "...", "Is that all ya got?", "HAH! Good one, brokie" };
        }
        UpdateText(boughtTexts[Random.Range(0, boughtTexts.Length)], null, "TheMan", banana ? 0.075f : 0.1f, false);
    }


    public void buySteak()
    {
        if (tokensGiven)
        {
            if (PlayerPrefs.GetInt("cheeseTokens") >= 3842)
            {
                PlayerPrefs.SetInt("steak", PlayerPrefs.GetInt("steak") + 1);
                PlayerPrefs.SetInt("cheeseTokens", PlayerPrefs.GetInt("cheeseTokens") - 3842);
                uhave.text = "(You have: " + PlayerPrefs.GetInt("steak") + ")";
                buyItem();
            }
            else
            {
                cantBuyItem();
            }
        }
    }

    public void NextRound()
    {
        nextRound = tokensGiven; //goes to next round if tokens given
        if(nextRound == true)
        {
            fadinImg.gameObject.SetActive(true);
        }
    }

    public void cheeeeese()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("cheeseTokens", 38425228);
        PlayerPrefs.SetInt("steak", 0);
        StartCoroutine(ChangeCTText());
    }

    public void gunClick(int type)
    {
        if (tokensGiven)
        {

            string owned = "g" + type + "Owned";
            if (PlayerPrefs.GetInt(owned) == 0)
            {
                int[] costs = { 0, 500, 1000, 10000, 100000 };
                if (PlayerPrefs.GetInt("cheeseTokens") >= costs[type])
                {
                    PlayerPrefs.SetInt(owned, 1);
                    PlayerPrefs.SetInt("cheeseTokens", PlayerPrefs.GetInt("cheeseTokens") - costs[type]);
                    StartCoroutine(ChangeCTText());
                    buyItem();
                }
                else
                {
                    cantBuyItem();
                }
            }
            else
            {
                PlayerPrefs.SetInt("gunType", type);
            }

            reloadGuns();
            reloadGunEnabled();
        }
    }

    void reloadGuns()
    {
        string[] costs = { "0", "500", "1k", "10k", "100k"};
        for (int i = 1; i < 5; i++)
        {
            GameObject.Find("ctImg" + i).GetComponent<Image>().enabled = (PlayerPrefs.GetInt("g" + i + "Owned") == 0);
            //print("gun " + i + " owned : " + (PlayerPrefs.GetInt("g" + i + "Owned") == 1));
        }

        for (int i = 0; i < 5; i++)
        {
            GameObject.Find("costTxt" + i).GetComponent<TextMeshProUGUI>().text = (PlayerPrefs.GetInt("g" + i + "Owned") == 0) ? costs[i] : (PlayerPrefs.GetInt("gunType") == i) ? "(Equipped)" : "";
            //print("gun " + i + " equipped: " + (PlayerPrefs.GetInt("gunType") == i));
        }
    }

    void reloadGunEnabled()
    {
        int gunType = PlayerPrefs.GetInt("gunType");
        GameObject.Find("gunA").GetComponent<SkinnedMeshRenderer>().enabled = (gunType == 0);
        GameObject.Find("gunC").GetComponent<MeshRenderer>().enabled = (gunType == 1);
        GameObject.Find("gunR").GetComponent<MeshRenderer>().enabled = (gunType == 2);
        GameObject.Find("gunS").GetComponent<MeshRenderer>().enabled = (gunType == 3);
        GameObject.Find("gunT").GetComponent<MeshRenderer>().enabled = (gunType == 4);
    }
  
    IEnumerator ChangeCTText()
    {
        Vector3 size = new Vector3(1.7f, 1.19f, 1.19f);
        float currentSize = ctb.localScale.x/1.7f;
        GameObject.Find("ctxt").GetComponent<TextMeshProUGUI>().text = "Cheese Tokens: " + PlayerPrefs.GetInt("cheeseTokens");
        float time = 0;
        while(time < 0.3f)
        {
            time += Time.deltaTime;
            currentSize = Mathf.Lerp(currentSize, 1.25f, 10*Time.deltaTime);
            ctb.localScale = currentSize * size;
            yield return new WaitForEndOfFrame();
        }
        while (time < 0.6f)
        {
            time += Time.deltaTime;
            currentSize = Mathf.Lerp(currentSize, 1f, 10*Time.deltaTime);
            ctb.localScale = currentSize * size;
            yield return new WaitForEndOfFrame();
        }
        ctb.localScale = size;
        yield return null;
    }
}

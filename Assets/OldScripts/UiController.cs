using UnityEngine;
using System.Collections;
using UnityEngine.UI;
 
using System;
using System.Linq;
public class UiController : MonoBehaviour {

    [SerializeField]
    GameObject lobbybt, myproductbt, lobby, myproduct,sendBt,feedBack,DeleteBT,sureBT,ShowError,fbBT,layoutPrefab,feedbackLobby,Loadingscene,ContactTel;
    GameObject[] imptexts;
    [SerializeField]
     Text[] Informations;
    [SerializeField]
    InputField[] inputs;
    [SerializeField] Image ProductImage;
    [SerializeField]
    Sprite NoImage;
    string error;
    [SerializeField]
    Toggle DispToggle;
    Texture2D textureOfImage,savedImage;
    public static string facebookID;
    public GameObject layoutGroup;
   
   
	// Use this for initialization
	void Start () {
        Loadingscene.SetActive(true);
        StartCoroutine(Connection.GetInfo(feedbackLobby,Loadingscene));
        myproduct.SetActive(false);
        lobby.SetActive(true);
        StartCoroutine(lateStart());
        facebookID = " ";
       
        
	}
   
	
	// Update is called once per frame
    void Update()
    {
        
        LayoutElement[] layouts= FindObjectsOfType<LayoutElement>() as LayoutElement[];
        foreach (LayoutElement le in layouts)
        {
            le.preferredHeight = (70 * Camera.main.pixelHeight) / 330;
        }
        fbBT.GetComponent<Button>().interactable = facebookID == " " ? true : false;
        error = "";
        if (PlayerPrefs.HasKey("MYID"))
       {
           sendBt.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Atualizar";
           DeleteBT.SetActive(true);
       }
        else
        {
            sendBt.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Enviar";
            DeleteBT.SetActive(false);
        }
        imptexts = GameObject.FindGameObjectsWithTag("ImpText");
        int control =0;
        foreach (GameObject g in imptexts)
        {
            if (string.IsNullOrEmpty( g.GetComponent<Text>().text.Trim()))
            {
                control++;
            }
        }

        if (inputs[2].text.Length != 11 && inputs[2].text.Length >0)
        {
            error = "Número de celular incorreto, use (dd+num)";
        }
        else if (inputs[3].text.Length<=4)
        {
            if (inputs[3].text.Length == 4)
            {
                int a = int.Parse((inputs[3].text[0].ToString()));
                int b = int.Parse((inputs[3].text[1].ToString()));
                int c = int.Parse((inputs[3].text[2].ToString()));
                int d = int.Parse((inputs[3].text[3].ToString()));

                if ((a == 1 || a == 2 || a == 3) && (b == 0) && (c == 0) && (d == 1 || d == 2 || d == 3 || d == 4))
                {
                    if (inputs[4].text.Length <= 5 && !inputs[4].text.Contains('.'))
                    {
                        error = "O preço está incorreto use o modelo (RR.CC)";
                    }
                }
                else error = "Insira uma turma valida";
            }
            else 
            error = "A turma deve conter 4 Números";
        }
        else if (inputs[4].text.Length <= 5  && !inputs[4].text.Contains('.'))
        {                    
                  error ="O preço está incorreto use o modelo (RR.CC)";            
        }
            
        if (control != 0)
        {
            sendBt.SetActive(false);
        }
        else sendBt.SetActive(true);

        if(ProductImage.sprite == null)
        {
            ProductImage.sprite = NoImage;
        }
	
	}
    public void lobBt()
    {
        myproduct.SetActive(false);
        lobby.SetActive(true);
        if (UnityEngine.Random.Range(0, 4) == 0)
        {
            AdManager.Instance.ShowVideo();
        }
    }
    public void myprodBt()
    {
        myproduct.SetActive(true);
        lobby.SetActive(false);
        if (UnityEngine.Random.Range(0, 4) == 0)
        {
            AdManager.Instance.ShowVideo();
        }
    }
    public void SendBT()
    {
        try
        {
            if (error == "")
            {
                string disp = DispToggle.isOn ? "1":"2";
                feedBack.SetActive(true);
                if (!PlayerPrefs.HasKey("MYID"))
                {
                    if (textureOfImage == null) textureOfImage = ProductImage.sprite.texture;
                    StartCoroutine(Connection.SendDb(Informations[0].text, Connection.ConvertBase64(textureOfImage), Informations[1].text, Informations[2].text, Informations[3].text, Informations[4].text,facebookID,disp, feedBack));
                }
                else
                {
                    if (textureOfImage == null) textureOfImage = savedImage;
                    Debug.Log("jacadastro");
                    StartCoroutine(Connection.UpdateDB(PlayerPrefs.GetInt("MYID"), Informations[0].text, Connection.ConvertBase64(textureOfImage), Informations[1].text, Informations[2].text, Informations[3].text, Informations[4].text, facebookID, disp, feedBack));
                }
            }
            else
            {
                ShowError.SetActive(true);
                ShowError.transform.FindChild("Text").GetComponent<Text>().text = error;
            }
         
        }
        catch (Exception e )
        {
            Camera.main.GetComponent<NativeToolkitExample>().OnShowAlertPress(e.Message);
        }
        if (UnityEngine.Random.Range(0, 4) == 0)
        {
            AdManager.Instance.ShowVideo();
        }
    }
    public void LoadTx(string path)
    {
        StartCoroutine(load(path,ProductImage));
    }
      
    IEnumerator load(string s,Image sp)
  {
      WWW www = new WWW("file://" + s);

      yield return www;
      sp.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));
      textureOfImage = www.texture;

  }
    IEnumerator lateStart()
    {
        yield return new WaitForEndOfFrame();
        StartCoroutine(video());
        if (PlayerPrefs.HasKey("MYID"))
        {
           inputs[0].text = PlayerPrefs.GetString("Produto");
           inputs[1].text = PlayerPrefs.GetString("vend");
           inputs[2].text = PlayerPrefs.GetString("wpp");
           inputs[3].text = PlayerPrefs.GetString("turma");
           inputs[4].text = PlayerPrefs.GetString("preco");
           facebookID = PlayerPrefs.GetString("fbid");
           DispToggle.isOn = PlayerPrefs.GetString("disp") == "1" ? true : false;
           byte[] b = Convert.FromBase64String(PlayerPrefs.GetString("img"));
           savedImage= new Texture2D(1, 1);
           savedImage.LoadImage(b);
           ProductImage.sprite = Sprite.Create(savedImage, new Rect(0, 0, savedImage.width, savedImage.height), new Vector2(0.5f, 0.5f));
          
        }

    }
     public void DeleteBTpress()
    {
        sureBT.SetActive(true);
        if (UnityEngine.Random.Range(0, 4) == 0)
        {
            AdManager.Instance.ShowVideo();
        }
    }
    public void DeleteYes()
     {
         Debug.Log("AQUI");
         StartCoroutine(Connection.Delete(PlayerPrefs.GetInt("MYID"),sureBT));
     }
    public void DeleteNo()
    {
        sureBT.SetActive(false);
    }
    public void CleanInputs()
    {
        foreach(InputField i in inputs)
        {
            i.text = "";
        }
        ProductImage.sprite = NoImage;
    }
    public void ErrorOK()
    {
        ShowError.SetActive(false);
    }
    public void CreateLobby(string[,] infs)
    {
      
        GameObject[] gameobjects = new GameObject[infs.GetLength(0)];

        for( int i =0;i<infs.GetLength(0);i++)
        {
            gameobjects[i]= Instantiate(layoutPrefab)as GameObject;
            gameobjects[i].transform.SetParent(layoutGroup.transform);
            gameobjects[i].transform.localScale = new Vector3(1, 1, 1);
        }
        for (int i = 0; i < infs.GetLength(0); i++)
        {
           // for (int j = 0; j < infs.GetLength(1); j++)
            {
                Texture2D tx = new Texture2D(2,1);
                tx.LoadImage(Convert.FromBase64String(infs[i, 0]), false);
                string cellPhone= infs[i,3];
                string fbID = infs[i, 6];
                
                gameobjects[i].transform.FindChild("Image").GetComponent<Image>().sprite = Sprite.Create(tx,new Rect(0,0,tx.width,tx.height),new Vector2(0.5f,0.5f));
                gameobjects[i].transform.FindChild("prod").GetComponent<Text>().text = infs[i, 1];
                gameobjects[i].transform.FindChild("vend").GetComponent<Text>().text = infs[i, 2];
                gameobjects[i].transform.FindChild("turma").GetComponent<Text>().text = infs[i, 4];
                gameobjects[i].transform.FindChild("preco").GetComponent<Text>().text = "R$:"+infs[i, 5];
                gameobjects[i].transform.FindChild("disp").GetComponent<Text>().text = infs[i, 7] == "1"? "Disponível":"Indisponível";
                gameobjects[i].transform.FindChild("disp").GetComponent<Text>().color = infs[i, 7] == "1" ? Color.green : Color.red;
                if (cellPhone.Length==11)
                {
                gameobjects[i].transform.FindChild("wpp").GetComponent<Button>().onClick.AddListener(delegate {CallWpp((cellPhone));});
                }
                else  gameobjects[i].transform.FindChild("wpp").gameObject.SetActive(false);
                if (fbID!=" "&& fbID.Length>5)
                {
                    gameobjects[i].transform.FindChild("fb").GetComponent<Button>().onClick.AddListener(delegate { OpenFb(fbID); });
                }
                else gameobjects[i].transform.FindChild("fb").gameObject.SetActive(false);
            }
        }
    }
            void CallWpp(string number)
            {
                ContactTel.transform.Find("Text").GetComponent<Text>().text = number;
                ContactTel.SetActive(true);                
            }
            void OpenFb(string fbID)
            {
                Application.OpenURL("https://www.facebook.com/" + fbID);
          
            }
            public void getinf()
            {
                StartCoroutine(Connection.GetInfo(feedbackLobby,Loadingscene));
            }
            public void close()
            {
                ContactTel.SetActive(false);
                ContactTel.transform.Find("Text").GetComponent<Text>().text = "";
            }
    IEnumerator video()
            {
              while(true)
              {
                  yield return new WaitForSeconds(50);
                  if (lobby.activeSelf)
                  {
                      if (UnityEngine.Random.Range(0, 4) == 0)
                      {
                          AdManager.Instance.ShowVideo();
                      }
                  }
              }
            }
    }
  
   

    


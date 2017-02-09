using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
public class Manager : MonoBehaviour
{
    [SerializeField]
    GameObject Products, Login, lpart, rpart, loginbt, regbt, loginerror, GoLogin, infs,sendinfs,sendImage,editBt;
    [SerializeField]
    InputField reguser, regpass, loginuser, loginpass;
    [SerializeField]
    Text userfeedback;
    [SerializeField]
    InputField[] inputs;
    [SerializeField]
    Image UserPhoto;
    [SerializeField]
    Button fbBT;
    public static string facebookID;
    bool usernameOK;
    string ImagePath;
    Texture2D UserTx, ProdTx;
    // Use this for initialization    
    void Start()
    {
        facebookID = PlayerPrefs.HasKey("fbID") ? PlayerPrefs.GetString("fbID") : "";
        usernameOK = false;
        TryLogin();
    }

    // Update is called once per frame
    void Update()
    {
        if (reguser.text.Length != 0 && regpass.text.Length != 0 && usernameOK)
        {
            regbt.SetActive(true);
        }
        else regbt.SetActive(false);

        if (loginuser.text.Length != 0 && loginpass.text.Length != 0)
        {
            loginbt.SetActive(true);
        }
        else loginbt.SetActive(false);
        fbBT.interactable = PlayerPrefs.HasKey("fbID") ? false :true ;
    }
    public void GoToProducts()
    {
        Products.SetActive(true);
        Login.SetActive(false);
    }
    public void GoToLogin()
    {
        Products.SetActive(false);
        Login.SetActive(true);
    }
    public void Loginpart()
    {
        lpart.SetActive(true);
        rpart.SetActive(false);
    }
    public void Registerpart()
    {
        rpart.SetActive(true);
        lpart.SetActive(false);
    }
    public void RegBt()
    {
        string login = reguser.text;
        string pass = regpass.text;
        StartCoroutine(Register(login, pass));
    }
    public void LoginBt()
    {
        string login = loginuser.text;
        string pass = loginpass.text;
        StartCoroutine(Logindb(login, pass));
    }
    public void checkLogin()
    {
        StartCoroutine(CheckLogin(reguser.text));
    }
    public void regEdit()
    {
        usernameOK = false;
    }
    public void close(GameObject gb)
    {
        gb.SetActive(false);
    }
    void SaveUsername(string user)
    {
        PlayerPrefs.SetString("user", user);
        TryLogin();
    }
    void TryLogin()
    {
        if (PlayerPrefs.HasKey("user"))
        {
            GoLogin.SetActive(false);
            infs.SetActive(true);
            StartCoroutine(GetInfos(PlayerPrefs.GetString("user")));
        }
        else
        {
            GoLogin.SetActive(true);
            infs.SetActive(false);
        }
    }
   public  void edit()
    {
        sendImage.SetActive(true);
        sendinfs.SetActive(true);
        editBt.SetActive(false);
        fbBT.gameObject.SetActive(true);
       foreach(InputField i in inputs)
       {
           i.enabled = true;
           i.gameObject.GetComponent<Image>().enabled = true;
           i.transform.GetChild(1).gameObject.SetActive(true);
       }
       
    }
    public void send()
   {
       sendImage.SetActive(false);
       sendinfs.SetActive(false);
       editBt.SetActive(true);
       fbBT.gameObject.SetActive(false);
       foreach (InputField i in inputs)
       {
           i.enabled = false;
           i.gameObject.GetComponent<Image>().enabled = false;
           i.transform.GetChild(1).gameObject.SetActive(false);
       }
   }
    public void atualizeBt()
    {
        if (UserTx != null)
        {
            StartCoroutine(AtualizeInfos(PlayerPrefs.GetString("user"), inputs[0].text, ConvertBase64(UserTx), inputs[1].text, inputs[2].text, PlayerPrefs.GetString("fbID")));
        }
        else 
            StartCoroutine(AtualizeInfos(PlayerPrefs.GetString("user"), inputs[0].text, ConvertBase64(UserPhoto.sprite.texture), inputs[1].text, inputs[2].text, PlayerPrefs.GetString("fbID")));
        
    }

    public void LoadTx(string path)
    {
        StartCoroutine(load(path, UserPhoto));
    }
    IEnumerator load(string s, Image sp)
    {
        WWW www = new WWW("file://" + s);
        Camera.main.GetComponent<NativeToolkitExample>().console.text = "CHEGOU AQUI";
        yield return www;
        sp.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));
        UserTx = www.texture;

    }


    public static string ConvertBase64(Texture2D tex)
    {
        if (tex.height * tex.width > 65536)
        {
            TextureScale.Bilinear(tex, 256, 256);
        }
        try
        {
            string s = Convert.ToBase64String(tex.EncodeToPNG());
            return s;
        }
        catch (Exception e)
        {
            Camera.main.GetComponent<NativeToolkitExample>().OnShowAlertPress(e.Message);
            return null;
        }

    }
    IEnumerator Register(string login, string senha)
    {

        WWWForm fm = new WWWForm();
        fm.AddField("servID", 31);
        fm.AddField("login", login);
        fm.AddField("senha", senha);

        WWW www = new WWW("http://navefood.pe.hu/Services.php", fm);
        yield return www;
        Debug.Log(www.text);
        if (www.text.Length == 5)
        {
            SaveUsername(login);
        }
        else Debug.Log("Erro");
    }
    IEnumerator Logindb(string login, string senha)
    {

        WWWForm fm = new WWWForm();
        fm.AddField("servID", 37);
        fm.AddField("login", login);
        fm.AddField("senha", senha);

        WWW www = new WWW("http://navefood.pe.hu/Services.php", fm);
        yield return www;
        Debug.Log(www.text);
        if (www.text.Length == 5)
        {
            SaveUsername(login);
        }
        else { loginerror.SetActive(true); };

    }
    IEnumerator CheckLogin(string login)
    {
        WWWForm fm = new WWWForm();
        fm.AddField("servID", 49);
        fm.AddField("login", login);

        WWW www = new WWW("http://navefood.pe.hu/Services.php", fm);
        yield return www;
        Debug.Log(www.text);
        if (www.text.Length == 5)
        {
            usernameOK = true;
            userfeedback.text = "";
        }
        else
        {
            usernameOK = false;
            userfeedback.text = "login ja cadastrado";
        }
    }
    IEnumerator GetInfos(string login)
    {
        WWWForm fm = new WWWForm();
        fm.AddField("servID", 65);
        fm.AddField("login", login);

        WWW www = new WWW("http://navefood.pe.hu/Services.php", fm);
        yield return www;
        Debug.Log(www.text);
        string[] splited = www.text.Split('|');
        Debug.Log(splited.Length);
        for (int i =0;i<inputs.Length;i++)
        {
            inputs[i].text = splited[i];
        }
        if (splited[3]=="")
        {
            Debug.Log("sem Imagem");
        }
        else
        {
          Texture2D tx= new Texture2D(1,1);
          try
          {
              tx.LoadImage(Convert.FromBase64String(splited[3]));
              UserTx= tx;
          }
          catch { Debug.Log("Error ao converter"); }
          UserPhoto.sprite = Sprite.Create(tx, new Rect(0, 0, tx.width, tx.height), new Vector2(0.5f, 0.5f));
        }
        if (splited[4].Length == 2)
        {
            facebookID = "";
        }
        else facebookID = splited[4];
        send();

    }
    IEnumerator AtualizeInfos(string login,string nome,string foto,string turma,string cel,string fbid)

    {
        WWWForm fm = new WWWForm();
        fm.AddField("servID", 92);
        fm.AddField("login", login);
        fm.AddField("nome", nome);
        fm.AddField("foto", foto);
        fm.AddField("turma", turma);
        fm.AddField("cel", cel);
        fm.AddField("fbid", fbid);

        WWW www = new WWW("http://navefood.pe.hu/Services.php", fm);
        yield return www;
        Debug.Log(www.text);
        if (www.text.Length==5)
        {
            send();
        }
    }
}

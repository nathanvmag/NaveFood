using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;
using UnityEngine.UI;


public class Connection : MonoBehaviour {
    
    private static int ID;
    
	// Use this for initialization
    void Start()
    {
        ID = PlayerPrefs.HasKey("MYID") ? PlayerPrefs.GetInt("MYID") : 0;
       
       
    }
        
	
	// Update is called once per frame
	void Update () {
	
	}
   static public IEnumerator GetInfo(GameObject feedback)
    {
        feedback.SetActive(true);
        feedback.transform.GetChild(0).GetComponent<Text>().text = "Atualizando...";
        WWWForm fm = new WWWForm();
        fm.AddField("servID", 920);
        WWW www = new WWW("http://navefood.pe.hu/Services.php", fm);
        yield return www;
        foreach (Transform t in Camera.main.GetComponent<UiController>().layoutGroup.transform)
        {
            Destroy(t.gameObject);
            Debug.Log("hey");
        }
        string[,] infs = new string [ www.text.Split(';').Length - 1, www.text.Split(';')[2].Split('|').Length ];
        for (int i=0;i<infs.GetLength(0);i++)
        {
            for (int j = 0;j<infs.GetLength(1);j++)
            {
                infs[i, j] = www.text.Split(';')[i].Split('|')[j];
            }
        }
      if (infs[0,3] != null)
      {
          Camera.main.GetComponent<UiController>().CreateLobby(infs);
          feedback.transform.GetChild(0).GetComponent<Text>().text = "Suscesso";
          yield return new WaitForSeconds(0.8f);
          feedback.SetActive(false);
      }
      else
      {
          feedback.transform.GetChild(0).GetComponent<Text>().text = "Falha";
          yield return new WaitForSeconds(0.8f);
          feedback.SetActive(false);
          yield return new WaitForSeconds(25);
          Camera.main.GetComponent<UiController>().getinf();        
          
      }
     
   }
      
       

    
    static public IEnumerator SendDb(string prod, string img, string vend, string wpp, string turma, string preco,string fbid,string disp ,GameObject feedback)
    {
        feedback.transform.GetChild(0).GetComponent<Text>().text = "Enviando...";
     //   Camera.main.GetComponent<NativeToolkitExample>().OnShowAlertPress("Enviando ao banco de dados "+ img.Length);
        WWWForm fm = new WWWForm();
        fm.AddField("Produto", prod);
        fm.AddField("img", img);
        fm.AddField("vend",vend);
        fm.AddField("wpp", wpp);
        fm.AddField("turma", turma);
        fm.AddField("preco", preco);
        fm.AddField("servID", 21);
        fm.AddField("fbid", fbid);
        fm.AddField("disp", disp);

        WWW www = new WWW("http://navefood.pe.hu/Services.php", fm);
        yield return www;
        Debug.Log(www.text);
        if (int.TryParse(www.text,out ID))
        {
            LocalSave(ID, prod, img, vend, wpp, turma, preco,fbid,disp);
            feedback.transform.GetChild(0).GetComponent<Text>().text = "Suscesso";
            Camera.main.GetComponent<UiController>().getinf();
             yield return new WaitForSeconds(0.8f);
        }
      
        else {
            
            feedback.transform.GetChild(0).GetComponent<Text>().text = "Falhou";
            yield return new WaitForSeconds(0.8f);
        };
        feedback.SetActive(false);
        feedback.transform.GetChild(0).GetComponent<Text>().text = "";
        
    }
    static public IEnumerator Delete(int id,GameObject suremenu)
    {
        WWWForm fm = new WWWForm();
        fm.AddField("servID", 212);
        fm.AddField("id", id);
        WWW www = new WWW("http://navefood.pe.hu/Services.php", fm);
        yield return www;
        Debug.Log(www.text);
        suremenu.SetActive(false);
        PlayerPrefs.DeleteAll();
        UiController.facebookID = " ";
        Camera.main.GetComponent<UiController>().CleanInputs();
        Camera.main.GetComponent<UiController>().getinf();
    }
    static public IEnumerator UpdateDB(int id, string prod, string img, string vend, string wpp, string turma, string preco, string fbid, string disp, GameObject feedback)
   {
       feedback.transform.GetChild(0).GetComponent<Text>().text = "Enviando...";
      // Camera.main.GetComponent<NativeToolkitExample>().OnShowAlertPress("Enviando ao banco de dados " + img.Length);
       WWWForm fm = new WWWForm();
       fm.AddField("Produto", prod);
       fm.AddField("img", img);
       fm.AddField("vend", vend);
       fm.AddField("wpp", wpp);
       fm.AddField("turma", turma);
       fm.AddField("preco", preco);
       fm.AddField("servID", 315);
       fm.AddField("id", id);
       fm.AddField("fbid", fbid);
       fm.AddField("disp", disp);
       WWW www = new WWW("http://navefood.pe.hu/Services.php", fm);
       yield return www;
       Debug.Log(www.text);
       if (www.text.Length==9)
       {
           feedback.transform.GetChild(0).GetComponent<Text>().text = "Suscesso";
           Camera.main.GetComponent<UiController>().getinf();
           yield return new WaitForSeconds(0.8f);
       }
       else
       {
           feedback.transform.GetChild(0).GetComponent<Text>().text = "Falhou";
           yield return new WaitForSeconds(0.8f);
       }
       LocalSave(PlayerPrefs.GetInt("MYID"), prod, img, vend, wpp, turma, preco,fbid,disp);
       feedback.SetActive(false);
       feedback.transform.GetChild(0).GetComponent<Text>().text = "";
   }
  
    static void LocalSave(int id,string prod, string img, string vend, string wpp, string turma, string preco,string fbid,string disp)
   {
       PlayerPrefs.SetInt("MYID", id);
       PlayerPrefs.SetString("Produto", prod);
       PlayerPrefs.SetString("img", img);
       PlayerPrefs.SetString("vend", vend);
       PlayerPrefs.SetString("wpp", wpp);
       PlayerPrefs.SetString("turma", turma);
       PlayerPrefs.SetString("preco", preco);
       PlayerPrefs.SetString("disp", disp);
       PlayerPrefs.SetString("fbid", fbid);
       Debug.Log("salvoCOMSUCESSO");
   }
    public static string ConvertBase64(Texture2D tex)
    {
        if (tex.height * tex.width > 262144)
        {
            TextureScale.Bilinear(tex, 512, 512);
        }
        try { string s = Convert.ToBase64String(tex.EncodeToPNG());
        return s;
        }
        catch (Exception e)
        {
            Camera.main.GetComponent<NativeToolkitExample>().OnShowAlertPress(e.Message);
            return null;
        }
        
    }
   
    
   
}

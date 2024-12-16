using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;
using System.Text;
using System;
using System.Text.RegularExpressions;

public class LoginScene : MonoBehaviour
{
    public GameObject sceneFadeInOut;

    public InputField inputUsername;
    public InputField inputPassword;
    public InputField inputLicense;
    public Text message;

    private MySqlConnection connection;
    private string serverName = "82.157.177.70";
    private string dbName = "FoxLogin";	
    private string userName = "admin";		
    private string sqlPassword = "123456";		
    private string port = "3306";			

    void Start()
    {
        string connectionString = "Server=" + serverName + ";Database=" + dbName + ";Uid=" + userName
                                  + ";Pwd=" + sqlPassword + ";Port=" + port + ";SslMode=None";
        connection = new MySqlConnection(connectionString);
        connection.Open();
        Debug.Log("连接数据库成功");
    }

    public void Register()
    {
        //sceneFadeInOut.GetComponentInChildren<SceneFadeInOut>().NextScene();
        string username = inputUsername.text;
        string password = inputPassword.text;
        string license = inputLicense.text;
        string licenseQuery = "SELECT COUNT(*) FROM license WHERE licensehash = @license AND usetime < 10 AND expireddate > NOW()";
        string userQuery = "SELECT COUNT(*) FROM userpasswd WHERE username = @username";
        string registerQuery = "INSERT INTO userpasswd(username, password, createtime, archive) VALUES (@username, @password, @createtime, 0)";
        string licenseAddQuery = "UPDATE license SET usetime = (SELECT x.usetime FROM (SELECT usetime FROM license WHERE licensehash = @license AND usetime < 10 AND expireddate > NOW()) x) + 1 WHERE licensehash = @license";

        if(username == "")
        {
            message.text = "用户名不能为空";
            return;
        }
        else if(password == "")
        {
            message.text = "密码不能为空";
            return;
        }
        else if(license == "")
        {
            message.text = "注册时许可证不能为空";
            return;
        }
        MySqlCommand cmd = new MySqlCommand(userQuery, connection);
          
        cmd.Parameters.AddWithValue("@username", username);
        int count = Convert.ToInt32(cmd.ExecuteScalar());
        if (count > 0)
        {
              message.text = "用户已存在，请更换用户名或直接登录";
              return;
        } 

        cmd = new MySqlCommand(licenseQuery, connection);
        cmd.Parameters.AddWithValue("@license", HashPassword(license));
        count = Convert.ToInt32(cmd.ExecuteScalar());
        if (count == 0)
        {
            message.text = "不合法的许可证";
            return;
        }

        if(!judgePwd(password))
        {
            message.text = "密码需至少包含大写、小写、数字、特殊符号中的三种且长度大于8";
            return;
        }

        try
        {
            cmd = new MySqlCommand(registerQuery, connection);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", HashPassword(password));
            cmd.Parameters.AddWithValue("@createtime", DateTime.Now);
            count = cmd.ExecuteNonQuery();
            if (count > 0)
            {
                message.text = "注册成功，请登录";
                cmd = new MySqlCommand(licenseAddQuery, connection);
                cmd.Parameters.AddWithValue("@license", HashPassword(license));
                int error = cmd.ExecuteNonQuery();
                if (error == 0)
                {
                    Debug.Log("error when update license usetime");
                }
            }
            else
            {
                message.text = "注册失败，请重新尝试";
            }
        }
        catch 
        {
            message.text = "用户已存在";
            return;
        }        
    }

    public void Login()
    {
        string username = inputUsername.text;
        string password = inputPassword.text;
        string query = "SELECT COUNT(*) FROM userpasswd WHERE username=@username AND password=@password";        

        if (username == "")
        {
            message.text = "用户名不能为空";
            return;
        }
        else if (password == "")
        {
            message.text = "密码不能为空";
            return;
        }

        MySqlCommand cmd = new MySqlCommand(query, connection);
        cmd.Parameters.AddWithValue("@username", username);
        cmd.Parameters.AddWithValue("@password", HashPassword(password));
        int count = Convert.ToInt32(cmd.ExecuteScalar());
        if (count > 0)
        {
            message.text = "登陆成功";
            sceneFadeInOut.GetComponentInChildren<SceneFadeInOut>().NextScene();
        }
        else
        {
            message.text = "用户名或密码错误";
        }
    }

    public void Return()
    {
        SceneManager.LoadScene("BeginScene");
    }

    private string HashPassword(string password)
    {
        SHA256Managed crypt = new SHA256Managed();
        StringBuilder hash = new StringBuilder();
        byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(password));
        foreach (byte theByte in crypto)
        {
            hash.Append(theByte.ToString("x2"));
        }
        return hash.ToString();
    }

    public void createLicense()
    {
        RandomNumberGenerator random = RandomNumberGenerator.Create();
        byte[] bytes = new byte[8];
        random.GetBytes(bytes);
        StringBuilder ret = new StringBuilder();
        foreach (byte b in bytes)
        {
            ret.AppendFormat("{0:x2}", b);
        }
        string lic = ret.ToString();
        Debug.Log("GenerateLicense: " + lic);
        string licenseInsertQuery = "INSERT INTO license(licensehash, usetime, expireddate) VALUES (@licensehash, @usetime, @expireddate)";
        MySqlCommand cmd = new MySqlCommand(licenseInsertQuery, connection);
        cmd.Parameters.AddWithValue("@licensehash", HashPassword(lic));
        cmd.Parameters.AddWithValue("@usetime", 0);
        cmd.Parameters.AddWithValue("@expireddate", Convert.ToDateTime("2025/05/24 00:00:00"));
        int count = cmd.ExecuteNonQuery();
    }

    private bool judgePwd(string password)
    {
        var reg_val = 0;
        var pw_txt = password;
        var reg = @"[*0-9]";//数字
        if (Regex.IsMatch(pw_txt, reg))
        {
            reg_val += 1;
        }
        reg = @"[*a-z]";//小写字母
        if (Regex.IsMatch(pw_txt, reg))
        {
            reg_val += 1;
        }

        reg = @"[A-Z ]";//大写字母
        if (Regex.IsMatch(pw_txt, reg))
        {
            reg_val += 1;
        }
        reg = @"[\W_!@#$%^&`~()-+=]";//特殊字符
        if (Regex.IsMatch(pw_txt, reg))
        {
            reg_val += 1;
        }
        if (reg_val < 3)
        {
            return false;
        }else if(password.Length < 8)
        {
            return false;
        }else
        {
            return true;
        }
    }
}

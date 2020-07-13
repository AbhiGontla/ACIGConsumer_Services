using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ACIGConsumer.Models;
using Core.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Services.Interfaces;

namespace ACIGConsumer.Controllers
{
    public class LoginController : Controller
    {
        private  ICustomerService _customerService;
        private readonly IOptions<ApplConfig> appSettings;
      
        public LoginController(ICustomerService customerService, IOptions<ApplConfig> _config)
        {
            _customerService = customerService;
            appSettings = _config;
        }
        public IActionResult Index()
        {
            if (TempData["userdetails"] != null)
            {
                TempData["userdetails"] = null;
            }

                return View();
        }
        public IActionResult sendsms()
        {
            var otp = GenerateRandomNo();
            string url = appSettings.Value.SmsConfig.url;
            string uname = appSettings.Value.SmsConfig.userName;
            string pwd = appSettings.Value.SmsConfig.password;
            string sender = appSettings.Value.SmsConfig.senderName;
            string mobilenumber = "966508095931";
            var request = (HttpWebRequest)WebRequest.Create(url);
            var postData =
            "UserName="+uname+"&Password="+pwd+"&MessageType=text&Recipients="+mobilenumber+"&SenderName="+sender+"&MessageText = "+otp+"";
            var data = Encoding.ASCII.GetBytes(postData);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            if (response.StatusCode.ToString() == "OK")
            {
                return Json(new { success = true, responseText = "Sending OTP Success.", sentotp = otp });
              
            }
            else
            {
                return Json(new { success = false, responseText = "Sending OTP Failed." });
            }
        }
     
        //Generate RandomNo
        public int GenerateRandomNo()
        {
            Random _rdm = new Random();
            int _min = 1000;
            int _max = 9999;
            return _rdm.Next(_min, _max);
        }

        [HttpGet]
        public IActionResult ValidateUser(string nid, string pin)
        {            
            Registration Item = _customerService.ValidateCustomer(nid, pin);

            //second request, get value marking it from deletion
            TempData["userdetails"] = Item.Iqama_NationalID;
            //later on decide to keep it
            TempData.Keep("userdetails");

            if (Item == null)
            {
                return Json(new { success = false, responseText = "Login Failed." }); 
            }
            else
            {
                return Json(new { success = true, responseText = "Login Success." });
            }
        }
       
        public IActionResult RegisterUser(string nid,string dob,string enterpin, string confirmpin)
        {
            string status = "false";
            try
            {
                Registration _userdetails = new Registration();
                _userdetails.Iqama_NationalID = nid;
                _userdetails.DOB = dob;
                _userdetails.CreatePin = enterpin;
                _userdetails.ConfirmPin = confirmpin;
                _customerService.Insert(_userdetails);
                status = "true";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (status == "false")
            {
                return Json(new { success = false, responseText = "User Registration Failed." });
            }
            else
            {
                return Json(new { success = true, responseText = "User Registration Success." });
            }
            
        }
    }
}
